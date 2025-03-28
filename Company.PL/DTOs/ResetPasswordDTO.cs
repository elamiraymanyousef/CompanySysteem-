using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }



        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dose'nt match the password !!")]
        public string ConfirmPassword { get; set; }

    }
}
