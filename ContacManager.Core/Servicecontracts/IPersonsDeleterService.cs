namespace ContacManager.Core.Servicecontracts
{
    public interface IPersonsDeleterService
    {

        public Task<bool> DeletePerson(Guid personID);



    }
}
