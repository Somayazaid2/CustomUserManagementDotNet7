using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserController> logger;

        public UserController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, ILogger<UserController> logger)
        {
            _userManager  = userManager;
            _roleManager = roleManager;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.Select(user => new UserViewModel {
                Id = user.Id ,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Roles =  _userManager.GetRolesAsync(user).Result
            }).ToList();
            logger.LogInformation("here is users :",users);
            return View(users);
        }

        public async Task<IActionResult> Add()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.Name,
            }).ToListAsync();
            var viewModel = new AddUserViewModel
            {
                
                Roles = roles
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(!model.Roles.Any(r=> r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please Select at Least One Role");
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null) 
            {
                ModelState.AddModelError("Email", "Email is already exists");
                return View(model);
            }
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "UserName is already exists");
                return View(model);
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            //CreateUser();
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r=>r.IsSelected).Select(r=>r.RoleName));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileFormViewModel
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if(userWithSameEmail != null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This email is already assigned to another user");
                return View(model); 
            }
            var userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);
            if (userWithSameUserName != null && userWithSameUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This UserName is already assigned to another user");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            await _userManager.UpdateAsync(user);


            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user  = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(roles => new RoleViewModel { RoleId = roles.Id, RoleName = roles.Name, IsSelected = _userManager.IsInRoleAsync(user, roles.Name).Result }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
            }
            

            return RedirectToAction(nameof(Index));
        }
    }
}
