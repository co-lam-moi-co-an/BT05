namespace BT05.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNum { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        public int FacultyId { get; set; }

        public int LevelId { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual Level Level { get; set; }
    }
}
