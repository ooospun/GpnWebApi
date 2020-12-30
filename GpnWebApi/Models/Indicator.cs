using System;
using System.Collections.Generic;

#nullable disable

namespace GpnWebApi.Models
{
    public partial class Indicator
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
    }
}
