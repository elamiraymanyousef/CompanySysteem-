using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.DTOs;
using Company.PL.HelperImage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Configuration;

namespace Company.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController( UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? SearchName)
        {
            IEnumerable<UserToReturnDTO> users;

            if (string.IsNullOrEmpty(SearchName))
            {
              users = _userManager.Users.Select(U => new UserToReturnDTO()
               {
                   Id = U.Id,
                   UserName = U.UserName,
                   FirstName = U.FirstName,
                   LastName = U.LastName,
                   Email = U.Email,
                   Roles = _userManager.GetRolesAsync(U).Result

               });
            }
            else
            {
                // for search by name
                users = _userManager.Users.Select(U => new UserToReturnDTO()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result

                }).Where(U => U.FirstName.ToLower().Contains(SearchName.ToLower()));
            }


            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewStat = "Details")
        {
            if (id is null) return BadRequest( "Invalid Id");
            var usre = await _userManager.FindByIdAsync(id);
            if (usre is null)
                return NotFound(new { StatusCode = 404, Message = $"Employee with id : {id} not found" });
            var dto = new UserToReturnDTO() {
            Id = usre.Id,
            UserName= usre.UserName, 
            FirstName = usre.FirstName,
            LastName= usre.LastName,
            Email = usre.Email,
            Roles =_userManager.GetRolesAsync(usre).Result
            
            };
            return View(viewStat, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserToReturnDTO  userToReturnDTO)
        {
            if (ModelState.IsValid)
            {
                if (id != userToReturnDTO.Id) return BadRequest("Invalid Operation");
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return BadRequest("Invalid Operation");
                user.UserName = userToReturnDTO.UserName;
                user.FirstName = userToReturnDTO.FirstName;
                user.LastName = userToReturnDTO.LastName;
                user.Email = userToReturnDTO.Email;

                var result= await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(userToReturnDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string? id,  UserToReturnDTO userToReturnDTO)
        {

            if (ModelState.IsValid)
            {

                if (id != userToReturnDTO.Id) return BadRequest("Invalid Operation");
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return BadRequest("Invalid Operation");
                user.UserName = userToReturnDTO.UserName;
                user.FirstName = userToReturnDTO.FirstName;
                user.LastName = userToReturnDTO.LastName;
                user.Email = userToReturnDTO.Email;

                var result = await _userManager.DeleteAsync(user);
 
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
           return View(userToReturnDTO);
        }
    }
}

    

