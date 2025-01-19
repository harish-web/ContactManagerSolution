using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContacManager.Core.Domain.Entities
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }
        [StringLength(40)]
        public string? PersonName { get; set; }
        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; } //fixed length datatyp

        [StringLength(10)]

        public string? Gender { get; set; }

        //Unique Identiffie

        public Guid? CountryId { get; set; }


        //Navigation Property since we have class name ,by default null and use Include property to load
        //the contry when loading persons Object eagerly load .no need to call country service separtely
        [ForeignKey(nameof(CountryId))]

        public virtual Country? Country { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        //bit
        public bool RecieveNewsLetter { get; set; }

        //public double age { get; set; }

        public string? TIN { get; set; }

    }
}
