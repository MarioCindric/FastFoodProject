using FastFood.DAL.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodIdentity.DAL.Model
{
    public class OcjenaNarudzbe
    {
        public int Id { get; set; }
        public int NarudzbaId { get; set; }
        public Narudzba Narudzba { get; set; }
        public int Ocjena { get; set; }
        public string Komentar { get; set; }
    }
}
