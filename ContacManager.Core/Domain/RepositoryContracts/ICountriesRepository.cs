using ContacManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContacManager.Core.Domain.RepositoryContracts
{
    public interface ICountriesRepository
    {
        public Task<Country> AddCountry(Country country);
        public Task<List<Country>> GetAllCountries();
        public Task<Country?> GetcountryByCountryId(Guid countryId);
        public Task<Country?> GetCountryByCountryName(string countryName);


    }
}
