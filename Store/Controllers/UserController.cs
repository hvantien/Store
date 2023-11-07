using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Action để hiển thị danh sách người dùng
        public IActionResult Index()
        {
            var users = _userManager.Users;
            return View(users);
        }

        // Action để hiển thị form tạo người dùng mới
        public IActionResult Create()
        {
            return View();
        }

        // Action để xử lý việc tạo người dùng mới
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                FullName = model.FullName,
                Email = model.Email,
                NormalizedEmail = model.Email,
                NormalizedUserName = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // Action để xóa người dùng
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            // Xử lý khi xóa không thành công
            return RedirectToAction("Index");
        }
    }
}
