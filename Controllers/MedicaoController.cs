using Microsoft.AspNetCore.Mvc;

namespace S.A_Esquadrias.Controllers
{
    public class MedicaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
