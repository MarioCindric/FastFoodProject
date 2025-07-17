using FastFood.DAL.DataModel;
using FastFood.Repository.Common;
using FastFood.Service.Common;
using FastFood.Service;
using FastFoodRepository.Automapper;
using FastFoodRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FastFoodIdentity.DAL;
using FastFoodIdentity.DAL.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FastFood.Repository;


namespace FastFood.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("FastFoodWeb_DBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddScoped<IRepositoryMappingService, RepositoryMappingService>();

            services.AddScoped<IAdresaRepository, AdresaRepository>();
            services.AddScoped<IAdresa, AdresaService>();

            services.AddScoped<IJeloRepository, JeloRepository>();
            services.AddScoped<IJelo, JeloService>();

            services.AddScoped<IKategorijaJeloRepository, KategorijaJeloRepository>();
            services.AddScoped<IKategorijaJelo, KategorijaJeloService>();

            services.AddScoped<INarudzbaJeloRepository, NarudzbaJeloRepository>();
            services.AddScoped<INarudzbaJeloService, NarudzbaJeloService>();

            services.AddScoped<INarudzbaRepository, NarudzbaRepository>();
            services.AddScoped<INarudzba, NarudzbaService>();
            services.AddScoped<INarudzbaRepository, NarudzbaRepository>();
            services.AddScoped<INarudzba, NarudzbaService>();
            services.AddScoped<INarudzbaRepository, NarudzbaRepository>();
            services.AddScoped<INarudzba, NarudzbaService>();

            services.AddScoped<IOcjenaJelaService, OcjenaJelaService>();
            services.AddScoped<IOcjenaJelaRepository, OcjenaJelaRepository>();



            services.AddScoped<IKreditnaKarticaRepository, KreditnaKarticaRepository>();
            services.AddScoped<IKreditnaKartica, KreditnaKarticaService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseCors("AllowSpecificOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
