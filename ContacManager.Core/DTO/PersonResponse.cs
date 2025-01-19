using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Enum;


namespace ContacManager.Core.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public Guid? CountryId { get; set; }

        public string? Country { get; set; }
        public string? Address { get; set; }

        public bool RecieveNewsLetter { get; set; }

        public double? Age { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(PersonResponse))
                return false;
            PersonResponse personResponse = (PersonResponse)obj;
            return PersonID == personResponse.PersonID && Country == personResponse.Country &&
                PersonName == personResponse.PersonName && Email == personResponse.Email && DateOfBirth == personResponse.DateOfBirth &&
                Gender == personResponse.Gender && CountryId == personResponse.CountryId && Address == personResponse.Address
                && RecieveNewsLetter == personResponse.RecieveNewsLetter;
        }



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"Person ID:{PersonID},Person Name : {PersonName}, Email : {Email}, Date of Birth :{DateOfBirth?.ToString("dd MMM yyyy")}" +
                $",Gender : {Gender} , Country ID :{CountryId}, Country:{Country}, Address : {Address}," +
                $" Receive News Letter :{RecieveNewsLetter}";
        }
        public PersonUpdateRequest ToPersonUdpateRequest() => new PersonUpdateRequest
        {
            PersonName = PersonName,
            PersonID = PersonID,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = (GenderOptions)System.Enum.Parse(typeof(GenderOptions), Gender, true),
            Address = Address,
            CountryId = CountryId,
            RecieveNewsLetter = RecieveNewsLetter,
        };


    }
    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                PersonID = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                RecieveNewsLetter = person.RecieveNewsLetter,
                Age = person.DateOfBirth != null ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Country = person.Country?.CountryName
            };
        }

        /*   public static PersonResponse ConvertPersonToPersonResponse(this Person person)
           {

               PersonResponse personResponse = person.ToPersonResponse();
               personResponse.Country = countriesService.GetCountryByCountryId(personResponse.CountryId)?.CountryName;
               personResponse.Country = person.Country?.CountryName;
               return personResponse;
           }*/
    }

}
