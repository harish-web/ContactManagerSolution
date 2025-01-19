using Azure.Identity;
using ContacManager.Core.Domain.IdentityEntities;
using ContacManager.Core.DTO;
using ContacManager.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Data;


namespace ContactManager.UI.Controllers
{
    [Route("[controller]/[action]")]
   // [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [Authorize("NotLoggedIn")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Authorize("NotLoggedIn")]
        [ValidateAntiForgeryToken]///Post method Register globally with filters
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e=>e.ErrorMessage);   
                return View(registerDTO);
            }
            var user = new ApplicationUser
            {
              PersonName = registerDTO.PersonName,
              Email = registerDTO.Email,
              UserName = registerDTO.Email,
              PhoneNumber = registerDTO.Phone,
              
              
            };
            IdentityResult identityResult = await userManager.CreateAsync(user,registerDTO.Password);
            if(identityResult.Succeeded)
            {
                
                var role = new ApplicationRole 
                {
                    Name = registerDTO.UserRoleType.ToString()
                };

                if (await roleManager.FindByNameAsync(role.Name) is null)
                    await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(user, role.Name);

                if (role.Name != UserRoleOptions.User.ToString())
                {
                    role.Name = UserRoleOptions.User.ToString();
                    if (await roleManager.FindByNameAsync(role.Name) is null)
                    {
                        await roleManager.CreateAsync(role);
                    }
                    await userManager.AddToRoleAsync(user, role.Name);
                }
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index","Persons");
            }
            foreach (var result in identityResult.Errors)
            {
                ModelState.AddModelError("Register",result.Description);
            }
            return View(registerDTO);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Persons");
        }
        [Authorize("NotLoggedIn")]
        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            return View();
        }
        [Authorize("NotLoggedIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDTO signInDTO,string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                return View(signInDTO);
            }
            else
            {

              Microsoft.AspNetCore.Identity.SignInResult result =  await  signInManager.PasswordSignInAsync(signInDTO.Email, signInDTO.Password, isPersistent: false,false);
                
                
                
                if(result.Succeeded)
                {
                    var applicationUser = await userManager.FindByEmailAsync(signInDTO.Email);

                    if (applicationUser != null)
                    {
                      if(await userManager.IsInRoleAsync(applicationUser,"Admin"))
                        {
                          return RedirectToAction("Index", "Home" ,new { area = "Admin"});
                        }
                    }

                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return LocalRedirect(ReturnUrl);
                        }
                        return RedirectToAction("Index", "Persons");
                        
                    }
                }
                ModelState.AddModelError("Login", "Invalid Email or Password");
                return View(signInDTO);
            }

           
        }
       
        [AllowAnonymous]
        [AcceptVerbs("get", "post")]
        public async Task<IActionResult> IsEmailInUse(String email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);//Ajax call need this line when immediate validation from jquery  .So returning Json is important
            }
            else
            {
                return Json($"Email  {email}  is alreaday in use");//Ajax call need this line when immediate validation from jquery
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
