using optum_healthcare_system.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace optum_healthcare_system.Repository.Interfaces
{
    public interface ILabResultRepository
    {
        Task<LabResultResponseDto> GetTestByIdAsync(Guid Id);
        Task<PaginationDto<LabResultResponseDto>> GetTestByPatientIdAsync(Guid patientId,int page,int pageSize);
        Task<LabResultResponseDto> CreateTestAsync(CreateLabResultDto createLabResultDto);
        Task<LabResultResponseDto> UpdateTestAsync(Guid Id, UpdateLabResultDto updateLabResult);
        Task<bool> DeleteTestByIdAsync(Guid Id);
    }
}
