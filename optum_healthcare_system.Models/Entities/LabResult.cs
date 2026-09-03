using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace optum_healthcare_system.Models.Entities
{
    [Table("Tbl_Optum_HealthCare_LabResults")]
    public class LabResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        [Required]
        [MaxLength(200)]
        public string TestName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string ResultValue { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string Unit { get; set; } = string.Empty;
        [Required]
        public DateTime ObservedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // LOGGING DATA

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
        [MaxLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedOn { get; set; }

    }
}
