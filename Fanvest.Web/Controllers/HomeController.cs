using Microsoft.AspNetCore.Mvc;
namespace Fanvest.Web.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual IActionResult Index() => View();
    }
}