using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Enum;
using System.ComponentModel.DataAnnotations;


namespace ContacManager.Core.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Person Id cannot be empty or null")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person name cannot be empty")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }

        public Guid? CountryId { get; set; }
        public string? Address { get; set; }

        public bool RecieveNewsLetter { get; set; }

        public Person ToPerson()
        {
            return new Person
            {
                PersonId = PersonID,
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
