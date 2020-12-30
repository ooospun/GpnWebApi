using System;
using System.Collections.Generic;

#nullable disable

namespace GpnWebApi.Models
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Uid { get; set; }
        public string Uemail { get; set; }
        public string Upassword { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
