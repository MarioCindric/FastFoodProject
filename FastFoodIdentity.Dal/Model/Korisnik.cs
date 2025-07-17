using System;
using System.Collections.Generic;

namespace FastFood.DAL.DataModel
{
    public class Korisnik
    {
  
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Rola { get; set; }

        public  ICollection<Adresa> Adresa { get; set; }
        public  ICollection<KreditnaKartica> KreditnaKartica { get; set; }
        public  ICollection<Narudzba> Narudzba { get; set; }
    }
}
