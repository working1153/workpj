using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApp2.Data;
using MyWebApp2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace MyWebApp2.Controllers
{
    public class RoletestController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoletestController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET:Roletest
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.UserRole = "Guest";
            //ViewData["Roles"] = _context.Roles.ToList();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var UserRole = await _context.UserRoles.FirstOrDefaultAsync(i => i.UserId == user.Id);
            if(UserRole != null)
            {
                var role = _context.Roles.FirstOrDefault(i => i.Id== UserRole.RoleId);
                
                ViewBag.UserRole = role.Name;
            }
            return View();
        }

        // GET:Roletest/AddRole

        public IActionResult AddRole()
        {
            ViewData["Roles"] = _context.Roles.ToList();
            return View();
        }

        
        [HttpPost]
        // POST: Jobs/AddRole
        public async Task<IActionResult> AddRoleConfirm(String RoleName)
        {
            var exist = await _context.Roles.FirstOrDefaultAsync(r => r.Name == RoleName);
            if (exist == null)
            {
                var Role = new IdentityRole();
                Role.Name = RoleName;
                Role.NormalizedName = StringNormalizationExtensions.Normalize(Role.Name);
                _context.Roles.Add(Role);
                //await _context.SaveChanges();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AddRole));
        }

        // GET:Roletest/AddRole2User

        public async Task<IActionResult> AddRole2User()
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var rstats = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == user.Id);
            if (rstats == null)
            {
                ViewBag.usr = "You don't have role now.";
            }
            else
            {
                List<IdentityRole> ur = _context.Roles.Where(r => r.Id == rstats.RoleId).ToList();
                ViewBag.usr = "Your role is " + ur[0];
            }


            //var test = await _context.Users.FirstOrDefaultAsync(r => r.UserName == user.UserName);
            //List<IdentityUser> a = _context.Users.ToList();
            //IdentityUser k = a[0];

            /* get the admin roles
            List<IdentityRole> a = _context.Roles.Where(r => r.Name == "admin").ToList();
            IdentityRole k = a[0];
            */

            List<IdentityRole> rs = _context.Roles.ToList();

            //string[] myst = { "car1", "car2", "car3", "car4"};
            List<string> r = new List<string>();
            foreach (var role in rs)
            {
                r.Add(role.Name);
            }
            ViewData["Roles"] = r;

            return View(); //view("filename", )
        }

        // GET:Roletest/AddRole2UserConfirm
        [HttpPost]
        public async Task<IActionResult> AddRole2UserConfirm(String rolename)
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var rstats = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == user.Id);
            if (rstats == null) //if empty
            {
                await _userManager.AddToRoleAsync(user, rolename);
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
