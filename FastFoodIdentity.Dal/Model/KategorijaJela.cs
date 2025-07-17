using System;
using System.Collections.Generic;

namespace FastFood.DAL.DataModel
{
    public class KategorijaJela
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public ICollection<Jelo> Jelo { get; set; }
    }
}
