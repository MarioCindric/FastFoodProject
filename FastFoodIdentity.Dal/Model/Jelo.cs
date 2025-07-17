using System;
using System.Collections.Generic;

namespace FastFood.DAL.DataModel
{
    public class Jelo
    {
   
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public decimal Cijena { get; set; }
        public bool Dostupno { get; set; }
        public string SlikaUrl { get; set; }
        public int Popust { get; set; }
        public int? KategorijaId { get; set; }

        public  KategorijaJela Kategorija { get; set; }
        public  ICollection<NarudzbaJelo> NarudzbaJelo { get; set; }
    }
}
