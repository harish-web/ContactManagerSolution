using ContacManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContacManager.Core.Domain.RepositoryContracts
{
    public interface IPersonsRepository
    {
        public Task<Person> AddPerson(Person person);
        public Task<List<Person>> GetAllPerson();
        public Task<Person?> GetPersonById(Guid personID);

        public Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        public Task<bool?> DeletePersonByPersonId(Guid PersondID);

        public Task<int> UpdatePerson();

    }
}
