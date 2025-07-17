using FastFoodIdentity.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Model
{
    public class AdresaDto
    {
        public int Id { get; set; }
        public string KorisnikId { get; set; }
        public string Grad { get; set; }
        public string Ulica { get; set; }
        public string BrojStana { get; set; }
        public string Kat { get; set; }
    }
}
