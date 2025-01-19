
using ContacManager.Core.DTO;


namespace ContacManager.Core.Servicecontracts
{
    public interface IPersonsUpdaterService
    {
        public Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    }
}
