using ContacManager.Core.DTO;

namespace ContacManager.Core.Servicecontracts
{
    public interface ICountriesGetterService
    {


        Task<List<CountryResponse>> GetAllCountries();

        Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);



    }
}
