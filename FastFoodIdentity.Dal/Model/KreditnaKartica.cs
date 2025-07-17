using FastFoodIdentity.DAL.Model;
using System;
using System.Collections.Generic;

namespace FastFood.DAL.DataModel
{
    public class KreditnaKartica
    {
        public int Id { get; set; }
        public string KorisnikId { get; set; }
        public ApplicationUser Korisnik { get; set; }
        public string BrojKartice { get; set; }
        public string DatumIsteka { get; set; }
        public string SigurnosniKod { get; set; }

    }
}
