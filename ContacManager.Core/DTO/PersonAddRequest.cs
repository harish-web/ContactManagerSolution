using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Enum;
using System.ComponentModel.DataAnnotations;


namespace ContacManager.Core.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person name cannot be empty")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email cannot be empty")]
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }

        public Guid? CountryId { get; set; }
        public string? Address { get; set; }

        public bool RecieveNewsLetter { get; set; }
        //Assigning PeronsAddrequstObject to Person object
        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                RecieveNewsLetter = RecieveNewsLetter
            };
        }
    }
}
