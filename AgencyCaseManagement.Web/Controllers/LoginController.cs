using Microsoft.AspNetCore.Mvc;

namespace AgencyCaseManagement.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
