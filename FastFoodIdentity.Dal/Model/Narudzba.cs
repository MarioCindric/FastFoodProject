using FastFoodIdentity.DAL.Model;
using System;
using System.Collections.Generic;

namespace FastFood.DAL.DataModel
{
    public class Narudzba
    {
        public int Id { get; set; }
        public string KorisnikId { get; set; }
        public ApplicationUser Korisnik { get; set; }
        public decimal UkupnaCijena { get; set; }
        public string NacinDostave { get; set; }
        public string NacinPlacanja { get; set; }
        public string StatusNarudzbe { get; set; }
        public DateTime? DatumKreiranja { get; set; }
        public DateTime? VrijemeDostave { get; set; }
        public string Grad { get; set; }
        public string Ulica { get; set; }
        public string KucniBroj { get; set; }
        public string Stan { get; set; }
        public string Kat { get; set; }
        public string BrojKartice { get; set; }
        public string DatumIsteka { get; set; }
        public string SigurnosniKod { get; set; }
        public string Napomena { get; set; }
        public string ZaposlenikImePrezime { get; set; }




        public ICollection<NarudzbaJelo> NarudzbaJelo { get; set; }
    }
}
