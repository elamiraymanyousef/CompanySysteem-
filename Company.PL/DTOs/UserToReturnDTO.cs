namespace Company.PL.DTOs
{
    public class UserToReturnDTO
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public string Email { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
