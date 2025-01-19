using System.ComponentModel.DataAnnotations;

namespace ContacManager.Core.Domain.Entities
{
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }

        [StringLength(30)]
        public string? CountryName { get; set; } = string.Empty;


        //public ICollection<Person> Persons { get; set;}
        //navigation property in runtime load person object having particular country id ,no need of loadingj
        //all the person .this is the way it is working...

        public override string ToString()
        {
            return $"CountryName ={CountryName}, CountyId ={CountryId}";
        }
    }
}
