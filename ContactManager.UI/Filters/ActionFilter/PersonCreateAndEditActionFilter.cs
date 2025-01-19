using ContacManager.Core.DTO;
using ContacManager.Core.Servicecontracts;
using ContactManager.UI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ActionFilter
{
    public class PersonCreateAndEditActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesGetterService countriesGetterService;

        public PersonCreateAndEditActionFilter(ICountriesGetterService countriesGetterService)
        {
            this.countriesGetterService = countriesGetterService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is PersonsController personsController)
            {


                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await countriesGetterService.GetAllCountries();
                    personsController.ViewBag.Countries = countries;

                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();
                    //return View();
                    context.Result = personsController.View(context.ActionArguments["personAddRequest"]);
                    
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }


          
        }
    }
}
