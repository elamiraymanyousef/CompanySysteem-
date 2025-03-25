using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class SignUpDTOs
    {
        [Required(ErrorMessage = " User Name Is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " First Name Is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " Last Name Is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password dose'nt match the password !!")]
        public string ConfirmPassword { get; set; }


        // [Required(ErrorMessage = "Password is required")]
        // [DataType(DataType.Password)]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        // public string Password { get; set; }

        // [Required(ErrorMessage = "Confirm Password is required")]
        // [DataType(DataType.Password)]
        // [Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match the password!")]
        // public string ConfirmPassword { get; set; }



        [Required(ErrorMessage = " Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }
        public  bool IsAgree { get; set; }  

    }
}
