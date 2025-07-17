using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Model
{
    public class NarudzbaJeloDto
    {
        public int Id { get; set; }
        public int? JeloId { get; set; }
        public int? NarudzbaId { get; set; }
        public int? Kolicina { get; set; }
        public decimal Cijena { get; set; }
        public string Napomena { get; set; }
        public string NazivJela {  get; set; }
    }
}
