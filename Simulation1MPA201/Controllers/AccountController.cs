using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simulation1MPA201.Models;
using Simulation1MPA201.ViewModels.Account;

namespace Simulation1MPA201.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _role;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> role)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _role = role;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var isExistEmail = await _userManager.FindByEmailAsync(vm.Email);
        if (isExistEmail != null)
        {
            return NotFound();
        }

        var isExistName = await _userManager.FindByNameAsync(vm.UserName);
        if (isExistName != null)
        {
            return NotFound();
        }

        AppUser newUser = new AppUser()
        {
            Email = vm.Email,
            UserName = vm.UserName,

        };

        var result = await _userManager.CreateAsync(newUser, vm.Password);
        await _userManager.AddToRoleAsync(newUser, "Member");
        if (!result.Succeeded)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
                return View(vm);
            }
        }

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = await _userManager.FindByEmailAsync(vm.Email);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.CheckPasswordAsync(user, vm.Password);
        if (!result)
        {
            ModelState.AddModelError("", "Email or password is wrong");
            return View(vm);
        }

        await _signInManager.SignInAsync(user, false);

        return RedirectToAction("Index", "Home");

    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }


    //public async Task<IActionResult> CreateRoles()
    //{
    //    var adminRole = new IdentityRole()
    //    {
    //        Name = "Admin",
    //    };

    //    var memberRole = new IdentityRole()
    //    {
    //        Name = "Member"
    //    };

    //    var result=await _role.CreateAsync(adminRole);
    //    if (!result.Succeeded)
    //    {
    //        return BadRequest();
    //    }

    //    result=await _role.CreateAsync(memberRole);
    //    if (!result.Succeeded)
    //    {
    //        return BadRequest();
    //    }
    //    return Ok("Created");
    //}

}
