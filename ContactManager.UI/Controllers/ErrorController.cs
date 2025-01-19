using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        /*[Route("/Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            
           if(exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error!= null)
            {
                ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            }

            return View();
        }*/
        [Route("/Error/{code?}")]
        public IActionResult Error(int? code)
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
            }
            ViewBag.StatusCode = code ?? 500;
            return View(); // Use a shared error view.
        }
    }
}