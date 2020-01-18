using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ServerTest.Db
{
    public class User
    {
        protected User()
        {
            
        }
        public User(string login, string password)
        {
            Login = login ?? throw new ArgumentNullException(nameof(login));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        [Key]
        public string Login { get; private set; }
        [Required]
        public string Password { get; set; }
    }
}