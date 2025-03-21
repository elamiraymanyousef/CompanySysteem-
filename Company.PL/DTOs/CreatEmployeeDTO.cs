using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class CreatEmployeeDTO
    {
        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }
        [Range(10,80)]
        public int? Age { get; set; }
        [DataType(DataType.EmailAddress,ErrorMessage ="Email is not Valid ")]
        public string Email { get; set; }
        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsACtive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }

        [Display(Name ="Department")]
        public int? DepartmentId { get; set; }

        //public string? EmoloyeeType { get; set; }
        public string? Department { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }

    }
}
