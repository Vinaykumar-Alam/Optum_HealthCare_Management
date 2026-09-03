namespace optum_healthcare_system.Models.DTOs
{
    public class LabResultResponseDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string ResultValue { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public DateTime ObservedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
