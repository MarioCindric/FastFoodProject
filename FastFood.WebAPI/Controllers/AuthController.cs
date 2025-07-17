using FastFoodIdentity.DAL;
using FastFoodIdentity.DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FastFood.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
       
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
         
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("mojipodaci")]
        public async Task<IActionResult> GetMojiPodaci()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var dto = new
            {
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName
            };
            return Ok(dto);
        }

        //[Authorize]
        [HttpGet("svi-korisnici")]
        public async Task<IActionResult> DohvatiSveKorisnike()
        {
            var korisnici = await _userManager.Users.ToListAsync();

            var rezultat = korisnici.Select(k => new
            {
                k.Id,
                k.UserName,
                k.Email,
                k.FirstName,
                k.LastName
            });

            return Ok(rezultat);
        }

        //[Authorize]
        [HttpGet("zaposlenici")]
        public async Task<IActionResult> DohvatiZaposlenike()
        {
            var korisnici = await _userManager.Users.ToListAsync();
            var rezultat = new List<object>();

            foreach (var k in korisnici)
            {
                var role = await _userManager.GetRolesAsync(k);
                if (role.Contains("Zaposlenik") || role.Contains("Admin"))
                {
                    rezultat.Add(new
                    {
                        k.Id,
                        k.UserName,
                        k.Email,
                        k.FirstName,
                        k.LastName,
                        Role = role.FirstOrDefault()
                    });
                }
            }

            return Ok(rezultat);
        }

        //[Authorize]
        [HttpGet("samoKorisnici")]
        public async Task<IActionResult> DohvatiSamoKorisnike()
        {
            var korisnici = await _userManager.Users.ToListAsync();
            var rezultat = new List<object>();

            foreach (var k in korisnici)
            {
                var role = await _userManager.GetRolesAsync(k);
                if (role.Contains("User"))
                {
                    rezultat.Add(new
                    {
                        k.Id,
                        k.UserName,
                        k.Email,
                        k.FirstName,
                        k.LastName,
                        Role = role.FirstOrDefault()
                    });
                }
            }

            return Ok(rezultat);
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid registration data.");
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest("User registration failed: " + string.Join(", ", createResult.Errors));
            }

            var role = await _roleManager.FindByIdAsync(model.Rola);
            if (role == null)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest("Role does not exist.");
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest("Failed to add user to role: " + string.Join(", ", addToRoleResult.Errors));
            }

            return Ok("User registered successfully!");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(userRoles.Select(role => new Claim("role", role)));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpDelete("obrisi/{id}")]
        public async Task<IActionResult> ObrisiZaposlenika(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Korisnik ne postoji.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest("Greška pri brisanju korisnika: " + string.Join(", ", result.Errors));

            return Ok("Zaposlenik obrisan.");
        }

        [Authorize]
        [HttpPut("uredi/{id}")]
        public async Task<IActionResult> UrediZaposlenika(string id, [FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Neispravni podaci");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Korisnik ne postoji");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Username;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Greška pri ažuriranju: " + string.Join(", ", result.Errors));

            return Ok("Zaposlenik ažuriran");
        }

        [Authorize]
        [HttpPut("uredi/korisnik/{id}")]
        public async Task<IActionResult> UrediKorisnika(string id, [FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Neispravni podaci");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Korisnik ne postoji");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Greška pri ažuriranju: " + string.Join(", ", result.Errors));

            return Ok("Zaposlenik ažuriran");
        }

    }
}
