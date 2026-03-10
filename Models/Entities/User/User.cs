using System.ComponentModel.DataAnnotations;

namespace Oliva.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is a required filed!")]
        [MaxLength(200, ErrorMessage = "Name is too long, 200 characters are required!")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [StringLength(11, MinimumLength = 11, 
            ErrorMessage = "CPF must have exactly 11 characters.")]
        public string? Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Password is a required filed!")]
        [MinLength(8)]
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}