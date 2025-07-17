using FastFood.DAL.DataModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastFoodIdentity.DAL.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Adresa Adresa { get; set; }
        public KreditnaKartica Kartica { get; set; }
        public ICollection<Narudzba> Narudzbe { get; set; }
    }
}
