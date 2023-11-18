using ASP_SPD_111.Models;
using ASP_SPD_111.Models.Home;
using ASP_SPD_111.Services.Hash;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace ASP_SPD_111.Controllers
{
    public class HomeController : Controller
    {
        // залежність (від служби) заявляється як private readonly поле
        private readonly IHashService _hashService;  // DIP - тип залежності - це інтерфейс
        private readonly ILogger<HomeController> _logger;

        // конструктор зазначає необхідні залежності, їх передає - Resolver (Injector)
        public HomeController(ILogger<HomeController> logger, IHashService hashService)
        {
            _logger = logger;
            _hashService = hashService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Razor()
        {
            ViewData["formController"] = "Hello from Controller";
            return View();
        }
        public ViewResult Transfer()
        {
            TransferFormModel? formModel;

            if (HttpContext.Session.Keys.Contains("formModel"))
            {
                // Є збережені у сесії дані - відновлюємо їх та видаляємо з сесії
                formModel = JsonSerializer.Deserialize<TransferFormModel>(
                    HttpContext.Session.GetString("formModel")!
                );
                HttpContext.Session.Remove("formModel");
            }
            else formModel = null;

            TransferViewModel model = new()
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                ControllerName = this.GetType().Name,
                FormModel = formModel
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult ProcessTransferForm(TransferFormModel? formModel)
        {
            // модель у параметрах автоматично збирається з даних, що
            // передаються у запиті.
            if(formModel != null)
            {
                // зберігаємо у сесії серіалізований об'єкт formModel під
                // іменем "formModel"
                HttpContext.Session.SetString(
                    "formModel",
                    JsonSerializer.Serialize(formModel)
                );
            }
            return RedirectToAction(nameof(Transfer));
        }
        public ViewResult Ioc()
        {
            // використовуємо сервіс
            ViewData["hash"] = _hashService.HexString("123");
            ViewData["objHash"] = _hashService.GetHashCode();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
/* Д.З. Створити сервіс валідації даних
 * - інтерфейс IValidationService
 * - реалізацію MyValidationService
 * У рамках сервісу реалізувати методи валідації
 * імені (IsNameValid), телефону та/або E-mail
 * Зареєструвати як Singleton, інжектувати до контролера
 * 
 * Додати до форми поле введення телефону та/або E-mail,
 * і доповнити виведення даних щодо вмісту форми 
 * повідомленнями про валідність:
 * Ім'я: Тестун - валідне
 * Прізвище: 123 - не валідне
 * Email:  testun_ukr.net - не валідне
 * ** поля різної валідності зробити різним стилем
 */
