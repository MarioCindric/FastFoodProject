using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastFood.DAL.DataModel;
using System.Threading.Tasks;
using FastFoodIdentity.DAL;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class StatistikaController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public enum PeriodFilter
        {
            Tjedan,
            Mjesec,
            Godina,
            Sve
        }
        public StatistikaController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Top 5 jela

        [HttpGet("top5")]
        public async Task<IActionResult> TopPet([FromQuery] int? mjesec, [FromQuery] int? godina, [FromQuery] string kategorija)
        {
            /*
             1. filtrira po vrijednosti, da ima value
             2. Dohvaća vezano jelo, i kategoriju
             3. Dohvaca vezanu narudzbu 
             4. filtar ako ima datum kreiranja*/
            var query = _dbContext.NarudzbeJela
                .Where(nj => nj.JeloId.HasValue)
                .Include(nj => nj.Jelo).ThenInclude(j => j.Kategorija)
                .Include(nj => nj.Narudzba)
                .Where(nj => nj.Narudzba.DatumKreiranja.HasValue);

            /*var query = _dbContext.NarudzbeJela
                .Where(nj => nj.JeloId.HasValue)
                .Include(nj => nj.Jelo).ThenInclude(j => j.Kategorija)
                .Include(nj => nj.Narudzba)
                .Where(nj => nj.Narudzba.DatumKreiranja.HasValue &&
                 (nj.Narudzba.StatusNarudzbe == "Završeno" || nj.Narudzba.StatusNarudzbe == "Na dostavi"));
                */

            // Oba filtera, ako oba imaju vrijednost, filtrira po mjesecu i godini
            if (mjesec.HasValue && godina.HasValue)
            {
                if (mjesec < 1 || mjesec > 12)
                    return BadRequest("Neispravan mjesec.");
                if (godina < 2000 || godina > DateTime.Today.Year)
                    return BadRequest("Neispravna godina.");

                // Prvi i zadnji dan mjeseca
                var odDatuma = new DateTime(godina.Value, mjesec.Value, 1);
                var doDatuma = odDatuma.AddMonths(1).AddDays(-1);

                // Narudzbe unutar tog mjeseca
                query = query.Where(nj =>
                    nj.Narudzba.DatumKreiranja.Value.Date >= odDatuma &&
                    nj.Narudzba.DatumKreiranja.Value.Date <= doDatuma);
            }

            // Ako je zadana kategorija, a ne sve, onda filtrira po nazivu, filter po kategoriji je neovisan
            if (!string.IsNullOrWhiteSpace(kategorija) &&
                !string.Equals(kategorija, "sve", StringComparison.OrdinalIgnoreCase))
            {
                // Filter po kategoriji. povezano jela, definirana kategorija, kategorija ima naziv, usporeduba iz baze s unosom
                query = query.Where(nj =>
                    nj.Jelo != null &&
                    nj.Jelo.Kategorija != null &&
                    nj.Jelo.Kategorija.Naziv != null &&
                    kategorija.ToLower() == nj.Jelo.Kategorija.Naziv.ToLower());
            }

            // Grupiranje po idu jela i nazivu jela
            // Vrača key jela, naziv, te količinu
            var top5 = await query
                .GroupBy(nj => new { nj.JeloId, nj.Jelo.Naziv })
                .Select(g => new
                {
                    g.Key.JeloId,
                    NazivJela = g.Key.Naziv,
                    Ukupno = g.Sum(x => x.Kolicina ?? 0)
                })
                .OrderByDescending(x => x.Ukupno)
                .Take(5)
                .ToListAsync();

            return Ok(top5);
        }







        [HttpGet("BrojNarudzbi")]
        public async Task<IActionResult> NarudzbePoVremenu([FromQuery] int? mjesec, [FromQuery] int? godina)
        {
            var danas = DateTime.Today;

            // Ako ima vrijednost, koristim godinu, ako nema onda koristim trenutnu godinu, isto za mjesec
            var filterGodina = godina ?? danas.Year;
            var filterMjesec = mjesec ?? danas.Month;

            if (mjesec.HasValue && (mjesec < 1 || mjesec > 12))
                return BadRequest("Neispravan mjesec.");
            if (godina.HasValue && (godina < 2000 || godina > danas.Year))
                return BadRequest("Neispravna godina.");

            // Indeks dana u tjednu, nedelja je 0, ponedeljak 1...
            var danTjedna = (int)danas.DayOfWeek;

            // Npr utorak - 1 = ponedeljak
            var pocetakTjedna = danas.AddDays(danTjedna == 0 ? -6 : -(danTjedna - 1));

            // Narudžbe koje imaju datum kreiranja, u biti nebitno kad sve imaju po defaultu
            var query = _dbContext.Narudzbe.AsQueryable().Where(o => o.DatumKreiranja.HasValue);

            //var query = _dbContext.Narudzbe
            //   .Where(o => o.DatumKreiranja.HasValue &&
            //   (o.StatusNarudzbe == "Završeno" || o.StatusNarudzbe == "Na dostavi"));


            var sve = await query.CountAsync();
            var godinaUkupno = await query.CountAsync(o => o.DatumKreiranja.Value.Year == filterGodina);
            var mjesecUkupno = await query.CountAsync(o => o.DatumKreiranja.Value.Year == filterGodina && o.DatumKreiranja.Value.Month == filterMjesec);
            // Od ponedeljka do x dana
            var tjedan = await query.CountAsync(o => o.DatumKreiranja.Value.Date >= pocetakTjedna && o.DatumKreiranja.Value.Date <= danas);
            // danas
            var dan = await query.CountAsync(o => o.DatumKreiranja.Value.Date == danas);

            return Ok(new
            {
                Dan = dan,
                Tjedan = tjedan,
                Mjesec = mjesecUkupno,
                Godina = godinaUkupno,
                Sveukupno = sve
            });
        }


        [HttpGet("heatmap")]
        public IActionResult HeatmapNarudzbi([FromQuery] string period, [FromQuery] DateTime? od = null, [FromQuery] DateTime? doDatuma = null)
        {
            var danas = DateTime.Today;
            DateTime? odDatuma = null;
            DateTime? doDatumaKrajnji = null;

            if (od.HasValue && doDatuma.HasValue)
            {
                odDatuma = od.Value.Date;
                doDatumaKrajnji = doDatuma.Value.Date.AddDays(1); // uključuje cijeli zadnji dan

                if (odDatuma == doDatumaKrajnji.Value.AddDays(-1))
                    doDatumaKrajnji = odDatuma.Value.AddDays(1);
            }
            else if (period?.ToLower() == "tjedan")
            {
                int danTjedna = (int)danas.DayOfWeek;
                odDatuma = danTjedna == 0 ? danas.AddDays(-6) : danas.AddDays(-(danTjedna - 1));
            }
            else if (period?.ToLower() == "mjesec")
                odDatuma = new DateTime(danas.Year, danas.Month, 1);
            else if (period?.ToLower() == "godina")
                odDatuma = new DateTime(danas.Year, 1, 1);
            else if (period?.ToLower() == "dan")
                odDatuma = danas;

            var query = _dbContext.Narudzbe
                .Where(n => n.VrijemeDostave.HasValue);
            query = query.Where(n => n.VrijemeDostave.Value >= new DateTime(2020, 1, 1));


            if (odDatuma.HasValue)
                query = query.Where(n => n.VrijemeDostave.Value >= odDatuma.Value);

            if (doDatumaKrajnji.HasValue)
                query = query.Where(n => n.VrijemeDostave.Value < doDatumaKrajnji.Value);

            var podaci = query
                .AsEnumerable()
                .Where(n => (int)n.VrijemeDostave.Value.DayOfWeek >= 1 && (int)n.VrijemeDostave.Value.DayOfWeek <= 6)
                .GroupBy(n => new
                {
                    Dan = (int)n.VrijemeDostave.Value.DayOfWeek,
                    Interval = n.VrijemeDostave.Value.Hour * 2 + (n.VrijemeDostave.Value.Minute < 30 ? 0 : 1)
                })
                .Select(g => new
                {
                    dan = g.Key.Dan,
                    interval = g.Key.Interval,
                    broj = g.Count()
                })
                .ToList();

            return Ok(podaci);
        }


        [HttpGet("promet")]
        public async Task<IActionResult> Promet([FromQuery] int? mjesec, [FromQuery] int? godina)
        {
            // Sve koje imaju datum kreiranja
            //var query = _dbContext.Narudzbe
            //    .Where(n => n.DatumKreiranja.HasValue);

            var query = _dbContext.Narudzbe
                .Where(n => n.DatumKreiranja.HasValue &&
                (n.StatusNarudzbe == "Završeno" || n.StatusNarudzbe == "Na dostavi"));
                

            if (mjesec.HasValue && godina.HasValue)
            {
                if (mjesec < 1 || mjesec > 12)
                    return BadRequest("Neispravan mjesec.");
                if (godina < 2000 || godina > DateTime.Today.Year)
                    return BadRequest("Neispravna godina.");

                // Od prvog dana mjeseca do zadnjeg
                var od = new DateTime(godina.Value, mjesec.Value, 1);
                var doD = od.AddMonths(1).AddDays(-1);

                // Narudzbe unutar mjeseca
                query = query.Where(n =>
                    n.DatumKreiranja.Value.Date >= od &&
                    n.DatumKreiranja.Value.Date <= doD);

                var ukupno = await query.SumAsync(n => n.UkupnaCijena);

                // Vraća objekt
                return Ok(new[] {
                new {
                    Godina = godina.Value,
                    Mjesec = mjesec.Value,
                    Ukupno = ukupno
                }
        });
            }

            // Filter samo po godini
            if (godina.HasValue)
            {
                if (godina < 2000 || godina > DateTime.Today.Year)
                    return BadRequest("Neispravna godina.");

                var od = new DateTime(godina.Value, 1, 1);
                var doD = od.AddYears(1).AddDays(-1);

                query = query.Where(n =>
                    n.DatumKreiranja.Value.Date >= od &&
                    n.DatumKreiranja.Value.Date <= doD);

                var ukupno = await query.SumAsync(n => n.UkupnaCijena);

                // Lista s jednim elementom, drugačije ne radi
                return Ok(new[] {
                new {
                    Godina = godina.Value,
                    Mjesec = 0,
                    Ukupno = ukupno
                }
        });
            }

            // Ako je oboje "sve"
            var ukupnoSve = await query.SumAsync(n => n.UkupnaCijena);

            return Ok(new[] {
            new {
                Godina = 0,
                Mjesec = 0,
                Ukupno = ukupnoSve
            }
    });
        }


    }
}
