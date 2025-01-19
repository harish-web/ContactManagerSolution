using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Domain.RepositoryContracts;
using ContactManager.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Infrastructure.Repository
{
    public class EntityCorePersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext db;

        public EntityCorePersonsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<Person> AddPerson(Person person)
        {

            await db.Persons.AddAsync(person);
            await db.SaveChangesAsync();
            return person;
        }

        public async Task<bool?> DeletePersonByPersonId(Guid PersondID)
        {

            db.Persons.RemoveRange(db.Persons.Where(p => p.PersonId == PersondID));
            int rowDeleted = await db.SaveChangesAsync();


            return rowDeleted > 0;

        }

        public async Task<List<Person>> GetAllPerson()
        {
            return await db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid personID)
        {
            return await db.Persons.Include("Country").Where(temp => temp.PersonId == personID).FirstOrDefaultAsync();
        }

        public async Task<int> UpdatePerson()
        {
            return await db.SaveChangesAsync();

        }
    }
}
