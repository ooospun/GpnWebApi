using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace GpnWebApi.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Rid { get; set; }
        public string Rname { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
