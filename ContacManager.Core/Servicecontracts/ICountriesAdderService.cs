using ContacManager.Core.DTO;

namespace ContacManager.Core.Servicecontracts
{
    public interface ICountriesAdderService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest countryAddRequest);

    }
}
