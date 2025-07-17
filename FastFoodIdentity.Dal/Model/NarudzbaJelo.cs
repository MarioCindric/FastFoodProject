using System;
using System.Collections.Generic;

namespace FastFood.DAL.DataModel
{
    public class NarudzbaJelo
    {
        public int Id { get; set; }
        public int? NarudzbaId { get; set; }
        public int? JeloId { get; set; }
        public int? Kolicina { get; set; }
        public string Napomena { get; set; }
        public decimal Cijena { get; set; }
        public  Jelo Jelo { get; set; }
        public  Narudzba Narudzba { get; set; }
    }
}
