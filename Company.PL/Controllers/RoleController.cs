using Company.DAL.Models;
using Company.PL.DTOs;
using Company.PL.HelperImage;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole>roleManager, UserManager<AppUser>userManager)
        {
            _roleManager = roleManager;
           _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? SearchName)
        {
            IEnumerable<RoleToReturnDTO> roles;

            if (string.IsNullOrEmpty(SearchName))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDTO()
                {
                   Id =U.Id,
                   Name = U.Name,

                });
            }
            else
            {
                // for search by name
                roles = _roleManager.Roles.Select(U => new RoleToReturnDTO()
                {
                    Id = U.Id,
                    Name = U.Name,

                }).Where(R => R.Name.ToLower().Contains(SearchName.ToLower()));
            }


            return View(roles);
        }




        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Create()
        {
            var roles = _roleManager.Roles.Select(r => new RoleToReturnDTO
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            ViewData["Role"] = roles;

            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDTO roleToReturnDTO)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(roleToReturnDTO.Name);
                if (role == null)
                {
                    role = new IdentityRole()
                    {
                        Name = roleToReturnDTO.Name,
                    };

                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            // لو في خطأ، نرجع نفس البيانات عشان الفيو يشتغل صح
            ViewData["Role"] = _roleManager.Roles.Select(r => new RoleToReturnDTO
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return View(roleToReturnDTO);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewStat = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new { StatusCode = 404, Message = $"Role with id : {id} not found" });
            var dto =  new RoleToReturnDTO()
            {

                Id = role.Id,
                Name = role.Name,
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
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleToReturnDTO roleToReturnDTO)
        {
            if (ModelState.IsValid)
            {
                if (id != roleToReturnDTO.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Invalid Operation");

                var result01 = await _roleManager.FindByNameAsync(roleToReturnDTO.Name);
                if(result01 is  null)
                {
                    role.Name = roleToReturnDTO.Name;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("","Invalid Operation");

            }
            return View(roleToReturnDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleToReturnDTO roleToReturnDTO)
        {

            if (ModelState.IsValid)
            {
                if (id != roleToReturnDTO.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Invalid Operation");

              
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
                ModelState.AddModelError("", "Invalid Operation");

            }
            return View(roleToReturnDTO);
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers( string roleId)
        {
           var role =await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var usersIdRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                     UserId = user.Id,
                     UserName = user.UserName,


                };

                if( await _userManager.IsInRoleAsync(user,role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected= false;
                }

                usersIdRole.Add(userInRole);

            }
            return View(usersIdRole);
                
        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            if(ModelState.IsValid)
            {
                foreach (var userVM in users)
                {
                    var user = await _userManager.FindByIdAsync(userVM.UserId);
                    if (user is not null)
                    {
                        if (userVM.IsSelected)
                        {
                            if (userVM.IsSelected &&!await _userManager.IsInRoleAsync(user, role.Name))
                            {
                                await _userManager.AddToRoleAsync(user, role.Name);
                            }
                        }
                        else
                        {
                            if (!userVM.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                            {
                                await _userManager.RemoveFromRoleAsync(user, role.Name);
                            }
                        }
                    }

                   
                }
                return RedirectToAction(nameof(Index)); // أو رجع للـ Role Details
            }
            return View(users); // أو رجع للـ Role Details
        }

    }


}
