using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Model
{
    public class OcjenaNarudzbaDto
    {
        public int Id { get; set; }
        public int NarudzbaId { get; set; }
        public int Ocjena { get; set; }
        public string Komentar { get; set; }
    }
}
