using Microsoft.AspNetCore.Mvc;

namespace S.A_Esquadrias.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
