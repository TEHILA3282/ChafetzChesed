using System;
using System.ComponentModel.DataAnnotations;

namespace ChafetzChesed.DAL.Entities
{
    public class Registration
    {
        [Key]
        [StringLength(9)]
        public string ID { get; set; } = null!; // Primary Key - נדרש

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [StringLength(15)]
        public string? LandlineNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string? PersonalStatus { get; set; }

        [StringLength(100)]
        public string? Street { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(10)]
        public string? HouseNumber { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }

        [StringLength(10)]
        public string? RegistrationStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StatusUpdatedAt { get; set; }
    }
}
