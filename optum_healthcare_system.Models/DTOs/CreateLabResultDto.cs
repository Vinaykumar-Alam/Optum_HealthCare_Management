using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace optum_healthcare_system.Models.DTOs
{
    public class CreateLabResultDto
    {
        [Required]
        public Guid PatientId { get; set; }
        [Required]
        public string TestName { get; set; } = string.Empty;
        [Required]
        public string ResultValue { get; set; } = string.Empty;
        [Required]
        public string Unit { get; set; } = string.Empty;
        [Required]
        public DateTime ObservedDate { get; set; }

    }
}
