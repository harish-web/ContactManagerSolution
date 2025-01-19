using ContacManager.Core.Servicecontracts;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesUploadFromExcelService countriesUploadFromExcelService;
        public CountriesController(ICountriesUploadFromExcelService countriesUploadFromExcelService)
        {
            this.countriesUploadFromExcelService = countriesUploadFromExcelService;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadFromExcel(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                ViewBag.Errormessage = "Please select xl file";
                return View();
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "UnSupported File Type";
                return View();
            }
            int count = await countriesUploadFromExcelService!.UploadFromExcelFile(formFile);
            if (count == 0)
            {
                ViewBag.Errormessage = "zero country loaded";
                return View();
            }
            ViewBag.count = count;
            ViewBag.message = count + "  Countries gets uploaded to database";

            return View();
        }
    }
}
