using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContacManager.Core.DTO
{
    public class SignInDTO
    {
        [Required(ErrorMessage ="Email can not be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email {  get; set; }

        [Required(ErrorMessage ="Password filed can't be empty")]
        [DataType(DataType.Password)]
        public string? Password  { get; set; }
    }
}
