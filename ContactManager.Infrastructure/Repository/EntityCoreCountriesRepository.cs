using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Domain.RepositoryContracts;
using ContactManager.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;



namespace ContactManager.Infrastructure.Repository
{
    public class EntityCoreCountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext db;

        public EntityCoreCountriesRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<Country> AddCountry(Country country)
        {
            await db.Countries.AddAsync(country);
            await db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await db.Countries.ToListAsync();
        }

        public async Task<Country?> GetcountryByCountryId(Guid countryId)
        {
            return await db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == countryId);
        }

        public Task<Country?> GetCountryByCountryName(string countryName)
        {
            return db.Countries.FirstOrDefaultAsync(country => country.CountryName == countryName);
        }
    }
}
