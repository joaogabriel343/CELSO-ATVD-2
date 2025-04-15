    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

    [HttpGet]
    public IActionResult Register(string role)
    {
        ViewBag.Role = role;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, string role)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(user, role);

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        ViewBag.Role = role;
        return View(model);
    }


    [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

    [HttpPost]
    public async Task<IActionResult> Login(RegisterViewModel model, string returnUrl = "/Home/Index")
    {
        ModelState.Remove(nameof(RegisterViewModel.ConfirmPassword));

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Aluno"))
                {
                    return RedirectToAction("Index", "Treino");
                }
                else if (roles.Contains("Personal"))
                {
                    return RedirectToAction("Index", "Treino");
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Conta bloqueada.");
            }
            else if (result.RequiresTwoFactor)
            {
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            }
        }

        return View(model);
    }


    [Authorize(Roles = "Personal")]
        public class TreinosController : Controller
        {
            public IActionResult CriarTreino()
            {
                return View();
            }

            public IActionResult EditarTreino(int id)
            {
                return View();
            }

            public IActionResult DeletarTreino(int id)
            {
                return View();
            }

            public IActionResult DetalharTreino(int id)
            {
                return View();
            }
        }

        [Authorize(Roles = "Aluno")]
        public class TreinosAlunoController : Controller
        {
            public IActionResult MeusTreinos()
            {
                return View();
            }
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }