using ContacManager.Core.Domain.Entities;
using ContacManager.Core.DTO;
using ContacManager.Core.Enum;
using ContacManager.Core.Servicecontracts;
using CRUDOperation.Filters.ActionFilter;
using CRUDOperation.Filters.AuthorizationFilter;
using CRUDOperation.Filters.ExceptionFilters;
using CRUDOperation.Filters.ResourceFilter;
using CRUDOperation.Filters.ResultFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Rotativa.AspNetCore;


namespace ContactManager.UI.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResponseHeaderActionfilter), Arguments = new object[] { "X-Custom-Key-controller", "Custom-Value-Controller", 3 }, Order = 3)]
    // [TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(AlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonsUpdaterService personsUpdaterService;
        private readonly IPersonsAdderService personsAdderService;
        private readonly IPersonsDeleterService personsDeleterService;
        private readonly IPersonsGetterService personsGetterService;
        private readonly IPersonsSorterService personsSorterService;

        private readonly ICountriesAdderService countriesAdderService;
        private readonly ICountriesGetterService countriesGetterService;
        private readonly ICountriesUploadFromExcelService countriesUploadFromExcelService;


        public PersonsController
            (IPersonsAdderService personsAdderService,
            IPersonsGetterService personsGetterService,
            IPersonsSorterService personsSorterService,
            IPersonsUpdaterService personsUpdaterService,
            IPersonsDeleterService personsDeleterService,
            ICountriesAdderService countriesAdderService,
            ICountriesGetterService countriesGetterService,
            ICountriesUploadFromExcelService countriesUploadFromExcelService)
        {
            this.personsAdderService = personsAdderService;
            this.personsGetterService = personsGetterService;
            this.personsSorterService = personsSorterService;
            this.personsUpdaterService = personsUpdaterService;
            this.personsDeleterService = personsDeleterService;
            this.countriesAdderService = countriesAdderService;
            this.countriesGetterService = countriesGetterService;
            this.countriesUploadFromExcelService = countriesUploadFromExcelService;
        }

        // [Route("persons/index")]//since added [Route("[controller]")] in controller level
        [Route("index")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter), Order = 4)]
        [TypeFilter(typeof(ResponseHeaderActionfilter), Arguments = new object[] { "X-Custom-Key", "Custom-Value", 1 }, Order = 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [TypeFilter(typeof(SkipFilter))]

        public async Task<IActionResult> Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>
            {
                 { nameof(Person.PersonName),"Person Name" },
                 { nameof(Person.Email),"Email" },
                 { nameof(Person.DateOfBirth),"Date of Birth" },
                 { nameof(Person.Gender),"Gender" },
                 { nameof(Person.CountryId),"Country" },
                 { nameof(Person.Address),"Address" }
            };
            //Search Person
            //List<PersonResponse> persons =  personsService.GetAllPersons();
            List<PersonResponse> persons = await personsGetterService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSeachString = searchString;
            ViewBag.CurrentSearchBy = searchBy;

            //Sort Person
            List<PersonResponse> sortedPerson = personsSorterService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            //return View(persons);
            return View(sortedPerson);
        }
        //[Route("persons/create")]//since added [Route("[controller]")] in controller level
        [Route("create")]
        [HttpGet]

        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }
        [Route("create")]//since added [Route("[controller]")] in controller level
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]
        [TypeFilter(typeof(FeatureDisableResourceFilter), Arguments = new object[] { false })]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            //Error checking is moved to Filters 
            await personsAdderService.AddPerson(personAddRequest);

            return RedirectToAction("Index", "Persons");
        }
        [HttpGet]
        [Route("[action]/{personid}")]//since added [Route("[controller]")] in controller level
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid personid)
        {
            PersonResponse? personResponse = await personsGetterService.GetPersonByPersionID(personid);
            if (personResponse == null)
                return RedirectToAction("Index");

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUdpateRequest();
            List<CountryResponse> countries = await countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries;
            //ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName ,Value = temp.CountryID.ToString() });
            return View(personUpdateRequest);
        }
        [HttpPost]
        [Route("[action]/{PersonID}")]//since added [Route("[controller]")] in controller level
        [TypeFilter(typeof(PersonCreateAndEditActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        [TypeFilter(typeof(AlwaysRunResultFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest? personAddRequest)
        {

            if (ModelState.IsValid)
            {
                // personAddRequest.PersonID = Guid.NewGuid();//exception testing 
                PersonResponse? personResponse = await personsUpdaterService.UpdatePerson(personAddRequest);
                return RedirectToAction("Index");
            }

            else
            {
                //Implemented in filter logic we can remove it...
                List<CountryResponse> countries = await countriesGetterService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                return View(personAddRequest);
            }
        }

        [HttpGet]
        [Route("[action]/{personid}")]//since added [Route("[controller]")] in controller level
        public async Task<IActionResult> Delete(Guid? personid)
        {
            PersonResponse? personResponse = await personsGetterService.GetPersonByPersionID(personid);
            if (personResponse == null)
                return RedirectToAction("Index");
            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]//since added [Route("[controller]")] in controller level
        public async Task<IActionResult> Delete(PersonResponse personResponse)
        {
            PersonResponse? personToDelete = await personsGetterService.GetPersonByPersionID(personResponse.PersonID);
            if (personToDelete == null)
                return RedirectToAction("Index");
            await personsDeleterService.DeletePerson(personToDelete.PersonID);
            return RedirectToAction("Index");
        }
        [Route("[action]")]
        public async Task<IActionResult> PersonsPdf()
        {
            List<PersonResponse> personsResponse = await personsGetterService.GetAllPersons();
            return new ViewAsPdf(personsResponse, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }
        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await personsGetterService.GetPersonsCSV();
            return File(memoryStream, "application/octe-stream", "persons.csv");
        }
        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await personsGetterService.GetPesonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }


    }
}
