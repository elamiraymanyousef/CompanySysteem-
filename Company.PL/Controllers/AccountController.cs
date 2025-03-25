using Company.DAL.Models;
using Company.PL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) 
        {
           _userManager = userManager;
           _signInManager = signInManager;
        }

        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDTOs model)
        {
            if (ModelState.IsValid) 
                {

                var user= await _userManager.FindByNameAsync(model.UserName);

                if(user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);

                    if( user is null)
                    {
                        // Manual Maping 
                         user = new AppUser
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {

                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SinUp !!");

            }
            return View();
        }

        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email); 

                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // SignIn 
                       var result =await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMy, false);
                       if (result.Succeeded)
                        {
                        return RedirectToAction(nameof(HomeController.Index), "Home");

                        }
                        
                    }

                }
                ModelState.AddModelError("", "Invalid LogIn !!");
            }

            return View(model);
        }
        #endregion

        #region SignOut
        public new  async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }


        #endregion


    }
}
