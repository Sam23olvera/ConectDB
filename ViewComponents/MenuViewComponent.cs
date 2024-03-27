using ConectDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConectDB.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(UsuarioModel model)
        {
            return View(model);
        }
    }
}