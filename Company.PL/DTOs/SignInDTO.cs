using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class SignInDTO
    {
        [Required(ErrorMessage = " Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool RememberMy { get; set; }

    }
}
