using System;
namespace WebApi.Models.NewAccess
{
    public class NewAccessUsersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
    }
}
