using Microsoft.EntityFrameworkCore;
using optum_healthcare_system.Models.DTOs;
using optum_healthcare_system.Models.Entities;
using optum_healthcare_system.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace optum_healthcare_system.Repository.Repository
{
    public class LabResultRepository : ILabResultRepository
    {
        private readonly AppDbContext _appDbContext;

        public LabResultRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        /// <summary>
        /// we have Id passed from the controller, and we are fetching the record from the database using that Id.
        /// The query here is using FirstOrDefaultAsync which will return the first record that matches the condition or null if no record is found.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// The method returns a Task of LabResultResponseDto. If a record is found, it maps the LabResult entity to LabResultResponseDto and returns it. If no record is found, it returns null.
        /// </returns>
        /// <SqlSyntax>
        /// SELECT TOP 1 * FROM Tbl_Optum_HealthCare_LabResults WHERE Id = @Id
        /// </SqlSyntax>
        public async Task<LabResultResponseDto> GetTestByIdAsync(Guid Id)
        {
            var record = await _appDbContext.LabResults.FirstOrDefaultAsync(e => e.Id == Id && e.IsActive);
            return record == null ? null : MapToResponse(record);
        }

        /// <summary>
        /// This method retrieves a paginated list of lab test results for a specific patient based on the provided patientId. It constructs a query to filter the LabResults table for records that match the given patientId and are marked as active. The method then counts the total number of matching records, applies pagination using Skip and Take, orders the results by ObservedDate, and maps the resulting LabResult entities to LabResultResponseDto objects. Finally, it returns a PaginationDto containing the paginated data, total count, current page number, and page size.
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// the method returns a Task of PaginationDto<LabResultResponseDto>, which contains the paginated list of lab test results for the specified patient, along with pagination metadata such as total count, current page number, and page size.
        /// </returns>
        /// <SqlSyntax>
        /// SELECT * FROM Tbl_Optum_HealthCare_LabResults WHERE PatientId = @patientId AND IsActive = 1 ORDER BY ObservedDate OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY
        /// </SqlSyntax>
        public async Task<PaginationDto<LabResultResponseDto>> GetTestByPatientIdAsync(Guid patientId,int page,int pageSize)
        {
            var query=_appDbContext.LabResults.Where(w=>w.PatientId == patientId && w.IsActive);
            var totalCount = await query.CountAsync();
            var data=await query.OrderBy(o=>o.ObservedDate)
                                .Skip((page-1)*pageSize)
                                .Take(pageSize)
                                .Select(s => MapToResponse(s)).ToListAsync();
            return new PaginationDto<LabResultResponseDto>
            {
                Data = data,
                TotalCount = totalCount,
                pageNumber = page,
                pageSize = pageSize
            };
        }

        /// <summary>
        /// This method creates a new lab test result entry in the database based on the provided CreateLabResultDto. It constructs a new LabResult entity, populates its properties with the data from the DTO, and adds it to the LabResults table. After saving the changes to the database, it maps the newly created LabResult entity to a LabResultResponseDto and returns it.
        /// </summary>
        /// <param name="createLabResultDto"></param>
        /// <returns>
        /// the method returns a Task of LabResultResponseDto, which represents the newly created lab test result entry. If the creation is successful, the response DTO contains the details of the new entry; otherwise, it may return null or throw an exception based on the implementation.
        /// </returns>
        /// <SqlSyntax>
        /// INSERT INTO Tbl_Optum_HealthCare_LabResults (PatientId, TestName, ResultValue, Unit, ObservedDate, CreatedBy, IsActive,CreatedOn , UpdatedBy,UpdatedOn) VALUES (@patientId, @testName, @resultValue, @unit, @observedDate, @createdBy,1,GETUTCDATE(),NULL,NULL)
        /// </SqlSyntax>
        public async Task<LabResultResponseDto> CreateTestAsync(CreateLabResultDto createLabResultDto)
        {
            var entry = new LabResult
            {
                PatientId = createLabResultDto.PatientId,
                TestName = createLabResultDto.TestName,
                ResultValue = createLabResultDto.ResultValue,
                Unit = createLabResultDto.Unit,
                ObservedDate = createLabResultDto.ObservedDate,
                CreatedBy = "OH Employee"
            };
            _appDbContext.LabResults.Add(entry);
            await _appDbContext.SaveChangesAsync();
            return MapToResponse(entry);
        }

        /// <summary>
        /// This method updates an existing lab test result entry in the database based on the provided Id and UpdateLabResult DTO. It first retrieves the existing LabResult entity from the database using the provided Id. If the record is found, it updates its properties with the new values from the UpdateLabResult DTO, sets the UpdatedOn timestamp to the current UTC time, and marks the UpdatedBy field. After saving the changes to the database, it maps the updated LabResult entity to a LabResultResponseDto and returns it.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateLabResult"></param>
        /// <returns>
        /// the method returns a Task of LabResultResponseDto, which represents the updated lab test result entry. If the update is successful, the response DTO contains the details of the updated entry; otherwise, it may return null or throw an exception based on the implementation.
        /// </returns>
        /// <SqlSyntax>
        /// UPDATE Tbl_Optum_HealthCare_LabResults SET TestName = @testName, ResultValue = @resultValue, Unit = @unit, ObservedDate = @observedDate, UpdatedOn = GETUTCDATE(), UpdatedBy = @updatedBy WHERE Id = @id
        /// </SqlSyntax>
        public async Task<LabResultResponseDto> UpdateTestAsync(Guid Id, UpdateLabResultDto updateLabResult)
        {
            var record=await _appDbContext.LabResults.FirstOrDefaultAsync(e => e.Id == Id && e.IsActive);
            if (record == null) return null;
            record.TestName = updateLabResult.TestName;
            record.ResultValue= updateLabResult.ResultValue;
            record.Unit = updateLabResult.Unit;
            record.ObservedDate = updateLabResult.ObservedDate;
            record.UpdatedOn = DateTime.UtcNow;
            record.UpdatedBy = "OH Manager";

            await _appDbContext.SaveChangesAsync();
            return MapToResponse(record);
        }


        /// <summary>
        /// This method deletes a lab test result entry from the database based on the provided Id. It first retrieves the existing LabResult entity from the database using the provided Id. If the record is found, it removes the entity from the LabResults table and saves the changes to the database. The method returns a boolean indicating whether the deletion was successful.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// A boolean indicating whether the deletion was successful.
        /// </returns>
        /// <SqlSyntax>
        /// --> HARD DELETE
        /// DELETE FROM Tbl_Optum_HealthCare_LabResults WHERE Id = @id 
        /// -- SOFT DELETE
        /// UPDATE Tbl_Optum_HealthCare_LabResults SET IsActive = 0, UpdatedOn = GETUTCDATE(), UpdatedBy = @updatedBy WHERE Id = @id
        /// </SqlSyntax>
        public async Task<bool> DeleteTestByIdAsync(Guid Id)
        {
            //This approach we use to hard delete - which deletes the whole record from the database.
            var record = await _appDbContext.LabResults.FirstOrDefaultAsync(e => e.Id == Id);
            if (record != null)
            {
                _appDbContext.LabResults.Remove(record);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;

            //we can also have soft delete approach - which just marks the record as inactive
            //and keeps it in the database for historical purposes.
            //var record = await _appDbContext.LabResults.FirstOrDefaultAsync(e => e.Id == Id);
            //if (record == null) return false;
            //record.IsActive = false;
            //record.UpdatedOn = DateTime.UtcNow();
            //record.UpdatedBy = "OH Manager";
            //await _appDbContext.SaveChangesAsync();
            //return true;
        }

        /// <summary>
        /// This private method maps a LabResult entity to a LabResultResponseDto. It takes a LabResult object as input and creates a new LabResultResponseDto object, populating its properties with the corresponding values from the LabResult entity. This mapping is useful for transforming data from the database model to a format suitable for API responses.
        /// </summary>
        /// <param name="labResult"></param>
        /// <returns>
        /// A LabResultResponseDto object populated with the values from the LabResult entity.
        /// </returns>
        private static LabResultResponseDto MapToResponse(LabResult labResult)
        {
            return new LabResultResponseDto
            {
                Id = labResult.Id,
                PatientId = labResult.PatientId,
                TestName = labResult.TestName,
                ResultValue = labResult.ResultValue,
                Unit = labResult.Unit,
                ObservedDate = labResult.ObservedDate,
                IsActive = labResult.IsActive,
                CreatedOn = labResult.CreatedOn,

            };
        }
    }
}
