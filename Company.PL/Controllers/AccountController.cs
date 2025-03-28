using Company.DAL.Models;
using Company.PL.DTOs;
using Company.PL.HelperImage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
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
        //security module
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

        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> SendRestePasswordUrl( ForgetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate Token
                     var token= await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Create URL
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);
                    
                    //Create Email 
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    // send Email
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {


                        //Check Your Email Inbox
                        return RedirectToAction("CheckYourInbox");
                    }

                }

            }
            ModelState.AddModelError("", "Invalid Reset Password !!");
            return View("ForgetPassword",model);
            //return View();
        }


        #endregion

        #region Check Your Inbox

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword( string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {

            if(ModelState.IsValid)
            {
                var email = TempData["email"] as string ;
                var token = TempData["token"] as string ;


                if( email is not null && token is not null)
                {
                    var user =await _userManager.FindByEmailAsync(email);
                    if (user is not null)
                    {
                        var result =await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
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
                else
                {
                    return BadRequest("Invalid opeeration !! ");
                }

            }
            return View();
        }

        #endregion


    }
}
