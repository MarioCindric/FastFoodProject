using FastFoodIdentity.DAL.Model;
using System;
using System.Collections.Generic;

namespace FastFood.DAL.DataModel
{
    public class Adresa
    {
        public int Id { get; set; }
        public string KorisnikId { get; set; }
        public ApplicationUser Korisnik { get; set; }
        public string Grad { get; set; }
        public string Ulica { get; set; }
        public string BrojStana { get; set; }
        public string Kat { get; set; }

    }
}
