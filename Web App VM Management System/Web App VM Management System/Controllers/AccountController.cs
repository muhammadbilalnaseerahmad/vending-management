using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_App_VM_Management_System.dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace Web_App_VM_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<IdentityUser> signInManager)
        {
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register data)
        {
            var user = new IdentityUser { UserName = data.Name,Email=data.Email, PasswordHash = data.Password ,PhoneNumber=data.PhoneNumber};
            if (data.Password != data.ConfirmPassword)
            {
                return RedirectToAction("Register", new { message = "Password & confirm Password are not same!", color = "red" });
            }
            var result = await _userManager.CreateAsync(user, data.Password);
            
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(data.Role).Result)
                {
                    _roleManager.CreateAsync(new IdentityRole(data.Role)).Wait();
                }
                await _userManager.AddToRoleAsync(user, data.Role);
                //ViewData["RegistrationSuccess"] = true; // Set the flag for successful registration
                return RedirectToAction("Login", new { message = "Successfully Registered!", color = "green" });
            }
            return RedirectToAction("Register", new { message = result.Errors.FirstOrDefault().Description, color = "red" });

        }
        public async Task<bool> CheckUserRole(string roleName)
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently authenticated user
            var isInRole =  await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                return true;
            }
            return false;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login data)
        {
            var user = await _userManager.FindByNameAsync(data.UserName);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, data.Password);
            if (user == null || !isPasswordValid)
            {
                return RedirectToAction("Login",new {message="Invalid Username or Password!",color="red"});
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
            //var isInRole = await _userManager.IsInRoleAsync(user, roleName);

            //    List<IdentityUser> users = await _userManager.Users.ToListAsync();

            //    var user = await _userManager.FindByNameAsync(data.Email);

            //    var userRoles = await _userManager.GetRolesAsync(user);

            //    if (user != null && await _userManager.CheckPasswordAsync(user, data.Password))
            //    {
            //        // User authenticated successfully

            //        // Create a list of claims for the user
            //        var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User's ID
            //    new Claim(ClaimTypes.Name, user.UserName), // User's username
            //    new Claim(ClaimTypes.Role, "Admin"), // Assign the "Admin" role to the user
            //    // You can add more claims as needed
            //};

            //        // Create a security key
            //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("xecretKeywqejane"));

            //        // Create signing credentials
            //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //        // Create a JWT token
            //        var tokenDescriptor = new SecurityTokenDescriptor
            //        {
            //            Subject = new ClaimsIdentity(claims),
            //            Expires = DateTime.Now.AddDays(5),
            //            SigningCredentials = creds
            //        };

            //        // Create a token handler
            //        var tokenHandler = new JwtSecurityTokenHandler();
            //        var Createtoken = tokenHandler.CreateToken(tokenDescriptor);
            //        var token = tokenHandler.WriteToken(Createtoken);

            //        // Redirect to a success page or return the token as needed
            //        // For example, redirect to the home page:
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        // Incorrect username or password
            //        ViewData["LoginSuccess"] = false;
            //        return View();
            //    }



        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Sign the user out
            await _signInManager.SignOutAsync();

            // Redirect to the home page or a specific page after logout
            return RedirectToAction("Index","Home",new { message = "Please login to continue!", color="green" });
        }
        [HttpGet]
        public IActionResult Register(string message="",string color="")
        {
            ViewBag.roles = _roleManager.Roles.ToList();
            ViewBag.message = message;
            ViewBag.color = color;
            return View();
        }

        [HttpGet]
        public IActionResult Login(string message="",string color="")
        {
            ViewBag.message=message;
            ViewBag.color = color;
            return View();
        }

    }
}
