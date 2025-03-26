using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class ForgetPasswordDTO
    {


        [Required(ErrorMessage = " Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
