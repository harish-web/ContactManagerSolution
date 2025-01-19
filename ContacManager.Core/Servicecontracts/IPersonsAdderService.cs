using ContacManager.Core.DTO;


namespace ContacManager.Core.Servicecontracts
{
    public interface IPersonsAdderService
    {
        public Task<PersonResponse> AddPerson(PersonAddRequest personAddRequest);

    }
}
