using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Domain.IdentityEntities;
using ContactManager.Infrastructure.DbContext.DbEntitiesUtil;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;



namespace ContactManager.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Country>().HasData(new {CountryId = Guid.NewGuid(),CountryName ="SampleCounry"});



            new CountryDBEntityConfiguration().Configure(modelBuilder.Entity<Country>());
            new PersonDBEntityConfiguration().Configure(modelBuilder.Entity<Person>());


            /*            modelBuilder.Entity<Person>(entity =>
                        {
                            entity.HasOne<Country>(p => p.Country) // Navigation property in Person pointing to Country
                                  .WithMany(c => c.Persons)        // Navigation property in Country pointing to Person collection
                                  .HasForeignKey(p => p.CountryId); // Foreign key in Person table
                        });*/
        }






        public List<Person> Sp_GetAllPersons()
        {
            //Databse.FromSqlRaw(String sql, parama)
            return Persons.FromSqlRaw("execute GetAllPersons").ToList();
        }

        public void Sp_InsertPerson(Person person)
        {
            SqlParameter[] sp = {
                new ("@PersonId",person.PersonId),
                new ("@PersonName",person.PersonName),
                new ("@Email",person.Email),
                new ("@DateOfBirth",person.DateOfBirth),
                new ("@Gender",person.Gender),
                new ("@CountryId",person.CountryId),
                new ("@Address",person.Address),
                new ("@RecieveNewsLetter",person.RecieveNewsLetter),
                new ("@TIN",person.TIN)

            };
            //Databse.ExecuteSqlRaw(String sql, parma)
            Database.ExecuteSqlRawAsync("execute InsertPerson @PersonID,@PersonName,@Email,@DateOfBirth,@Gender,@CountryId," +
                "@Address,@RecieveNewsLetter,@TIN", sp);
        }







    }
}
