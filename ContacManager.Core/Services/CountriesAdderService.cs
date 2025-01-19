using ContacManager.Core.DTO;
using ContacManager.Core.Domain.RepositoryContracts;
using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Servicecontracts;

namespace ContacManager.Core.Services
{
    public class CountriesAdderService : ICountriesAdderService
    {
        private readonly ICountriesRepository countriesReposiory;

        //List<Country> _countries;
        //public CountriesService(bool initialize = true)
        public CountriesAdderService(ICountriesRepository countriesReposiory)
        {
            this.countriesReposiory = countriesReposiory;


            #region ForMockData
            /* _countries = new List<Country>();
             if (initialize)
             {

              _countries.AddRange(new List<Country>
              {
                 new Country
                 {
                     CountryId = Guid.Parse("B06B9935-3366-41E4-9253-8C5B5B023DA7"),
                     CountryName = "India"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("78D56FC0-15E8-42B3-B8AE-4527A22BF2DB"),
                     CountryName = "USA"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("9E6C9DE3-FCF0-4D3C-B4C9-839B1FA4A347"),
                     CountryName = "China"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("DD7D144F-E206-4B06-AC53-4B315C5795BC"),
                     CountryName = "UK"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("7F8372F5-5C22-4A43-8E0F-9BE28C69FAB3"),
                     CountryName = "Russia"
                 },
              });

             }*/

            #endregion


        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(CountryAddRequest));
            }
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentNullException(nameof(CountryAddRequest.CountryName));
            }

            //if (await countriesReposiory.Countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).CountAsync() > 0)
            if (await countriesReposiory.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentNullException("Country name already exist");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryId = Guid.NewGuid();
            //_countries.Add(country);
            //countriesReposiory.Countries.Add(country);//prvious in memory
            await countriesReposiory.AddCountry(country);
            //await countriesReposiory.SaveChangesAsync();
            return country.ToCountryResponse();
        }


    }
}
