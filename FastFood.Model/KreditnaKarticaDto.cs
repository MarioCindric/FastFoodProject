using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Model
{
    public class KreditnaKarticaDto
    {
        public int Id { get; set; }
        public string KorisnikId { get; set; }
        public string BrojKartice { get; set; }
        public string DatumIsteka { get; set; }
        public string SigurnosniKod {  get; set; }

    }
}
