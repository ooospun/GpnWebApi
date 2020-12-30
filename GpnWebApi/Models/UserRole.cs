using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace GpnWebApi.Models
{
    public partial class UserRole
    {
        public Guid Urid { get; set; }
        public Guid UrUid { get; set; }
        public Guid UrRid { get; set; }

        public virtual Role UrR { get; set; }
        [JsonIgnore]
        public virtual User UrU { get; set; }
    }
}
