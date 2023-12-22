using ASP_SPD_111.Data;
using ASP_SPD_111.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace ASP_SPD_111.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Profile(String? id)
        {
            UserProfileViewModel model = new();
            if(id == null)   // спроба доступу до "свого" кабінету
            {
                // перевіряємо автентифікацію
                if (HttpContext.User.Identity?.IsAuthenticated ?? false)
                {
                    model.IsPersonal = true;
                    // шукаємо дані користувача за Claim
                    String sid = HttpContext
                        .User
                        .Claims
                        .First(claim => claim.Type == ClaimTypes.Sid)
                        .Value;
                    // Шукаємо у контексті даних (у БД)
                    model.User = _dataContext
                        .Users
                        .Find(Guid.Parse(sid));
                }
                else  // спроба доступу без входу у систему
                {
                    model.IsPersonal = false;
                    model.User = null;
                }
            }
            else  // вказано id - доступ до "чужого" профілю
            {
                model.IsPersonal = false;
                model.User = _dataContext
                    .Users
                    .FirstOrDefault(u => u.Login == id);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProfile(String newName, String newEmail)
        {
			// перевіряємо автентифікацію
			if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
				// шукаємо дані користувача за Claim
				String sid = HttpContext
					.User
					.Claims
					.First(claim => claim.Type == ClaimTypes.Sid)
					.Value;
				// Шукаємо у контексті даних (у БД)
				var user = _dataContext
					.Users
					.Find(Guid.Parse(sid));
                if(user != null)
                {
					// Вносимо зміни
                    user.Name = newName;
                    user.Email = newEmail;
                    await _dataContext.SaveChangesAsync();
					return Json(new { status = 200 });
				}
			}
            // сюди потрапляємо або якщо не автентифікований, або не знайдений
			return Json(new { status = 401 });
        }

        [HttpDelete]
		public async Task<JsonResult> DeleteProfile()
        {
            var user = this.GetAuthUser();
            if(user == null) { return Json(new { status = 401 }); }
            // _dataContext.Users.Remove(user);  // повне видалення - порушення зв'язків даних
            user.DeleteDt = DateTime.Now;  // встановлюємо "ознаку" видалення
                                           // за вимогами законодавства видаляємо персональні дані (https://gdpr-info.eu/art-17-gdpr/)
            user.Name = "";
            user.Email = "";
            if (user.Avatar != null) {  // якщо є - видаляємо файл аватарки
                String dir = Directory.GetCurrentDirectory();
                String avatarFileName = Path.Combine(dir, "wwwroot", "avatars", user.Avatar);
                if (System.IO.File.Exists(avatarFileName))
                {
					System.IO.File.Delete(avatarFileName);
				}
                user.Avatar = null;
            }
			user.Login = "";
            user.PasswordDk = "";
            user.PasswordSalt = "";

			await _dataContext.SaveChangesAsync();

			return Json(new { status = 200 });
		}

        private Data.Entities.User? GetAuthUser()
        {
            // перевіряємо автентифікацію
            if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                // шукаємо дані користувача за Claim
                String sid = HttpContext
                    .User
                    .Claims
                    .First(claim => claim.Type == ClaimTypes.Sid)
                    .Value;
                // Шукаємо у контексті даних (у БД)
                return _dataContext
                    .Users
                    .Find(Guid.Parse(sid));
            }
            return null;
        }
	}
}
/* Д.З. Завершити процедуру видалення користувача
 * - додати кнопку (посилання)
 * - реалізувати повідомлення-підтвердження про незворотність дії
 * - провести випробування
 * - перевірити, що персональні дані видаляються повністю
 *    (включно із файлами)
 *    а рядок таблиці у БД залишається (з технічними даними - id, datetime)
 *    
 * ** додате "м'яке" видалення - дані залишаються, але акаунт вважається
 * видаленим/зупиненим. !! Змінити алгоритми входу до системи, утримання
 * авторизації тощо.
 */
