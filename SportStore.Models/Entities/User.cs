using System;
using System.Collections.Generic;
using System.Text;

namespace SportStore.Models.Entities
{
    public class User : IEntityBase
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
