namespace Oliva.Models
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}