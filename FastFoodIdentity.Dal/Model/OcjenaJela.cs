using FastFood.DAL.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodIdentity.DAL.Model
{
    public class OcjenaJela
    {
        public int Id { get; set; }
        public int JeloId { get; set; }
        public Jelo Jelo { get; set; }
        public int Ocjena { get; set; }
        public string KorisnikId { get; set; }


    }
}
