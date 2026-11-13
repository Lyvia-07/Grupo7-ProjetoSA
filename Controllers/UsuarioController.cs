using Microsoft.AspNetCore.Mvc;

namespace S.A_Esquadrias.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
