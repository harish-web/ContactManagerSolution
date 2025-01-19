using ContacManager.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManager.Infrastructure.DbContext.DbEntitiesUtil
{
    internal class PersonDBEntityConfiguration : IEntityTypeConfiguration<Person>
    {


        public void Configure(EntityTypeBuilder<Person> builder)
        {
            //builder.HasData(new { CountryId = Guid.NewGuid(), CountryName = "SampleCounry" });
            string personsJson = File.ReadAllText("persons.json");

            List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach (Person person in persons)
            {
                builder.HasData(person);
            }
            builder.Property(temp => temp.TIN)
            .HasColumnType("varchar(80)")
            .HasColumnName("TIN");
            builder.HasIndex(temp => temp.TIN).IsUnique();

        }
    }
}