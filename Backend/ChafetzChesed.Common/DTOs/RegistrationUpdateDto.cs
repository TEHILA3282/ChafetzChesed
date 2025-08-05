using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChafetzChesed.Common.DTOs
{
    public class RegistrationUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LandlineNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PersonalStatus { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? HouseNumber { get; set; }
    }
}
