using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace loginapp.Models
{
    
    public partial class Users
    {
        public Users()
        {
            Passwords = new HashSet<Passwords>();
            Sessions = new HashSet<Sessions>();
        }

        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserPassword { get; set; }

        public virtual ICollection<Passwords> Passwords { get; set; }
        public virtual ICollection<Sessions> Sessions { get; set; }
    }
}
