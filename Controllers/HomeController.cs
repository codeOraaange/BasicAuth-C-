using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using basicauth.Models;

//tambah
using Microsoft.AspNetCore.Authorization;

namespace basicauth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            //tambah
            AppDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _logger = logger;
            //tambah
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //menandakan harus login
        [Authorize]

        public IActionResult Privacy()
        {
            return View();
        }

        //tambah untuk tampil page
        public IActionResult Login()
        {
            return View();
        }

        //tambah untuk proses
        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user!=null)
            {
                var result = await _signInManager
                                    .PasswordSignInAsync(user, password, false, false);
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }  

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                var signInResult = await _signInManager
                    .PasswordSignInAsync(user, password, false, false);

                if(signInResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
