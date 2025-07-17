using FastFood.DAL.DataModel;
using FastFoodIdentity.DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FastFoodIdentity.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Jelo> Jela { get; set; }
        public DbSet<KategorijaJela> Kategorije { get; set; }
        public DbSet<Adresa> Adrese { get; set; }
        public DbSet<KreditnaKartica> Kartice { get; set; }
        public DbSet<Narudzba> Narudzbe { get; set; }
        public DbSet<NarudzbaJelo> NarudzbeJela { get; set; }
        public DbSet<OcjenaNarudzbe> OcjeneNarudzbi { get; set; }
        public DbSet<OcjenaJela> OcjenaJela { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Data Source=DESKTOP-1HPUI1T\\SQLEXPRESS01;Initial Catalog=FastFoodWebs;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=True");

            }
        }
    }


    
}
