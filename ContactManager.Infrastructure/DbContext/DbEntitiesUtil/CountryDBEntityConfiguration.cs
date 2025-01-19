using ContacManager.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ContactManager.Infrastructure.DbContext.DbEntitiesUtil
{
    public class CountryDBEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            //builder.HasData(new { CountryId = Guid.NewGuid(), CountryName = "SampleCounry" });
            string countriesJson = File.ReadAllText("countries.json");

            List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);






            foreach (Country country in countries)
            {
                builder.HasData(country);
            }

        }
    }
}
