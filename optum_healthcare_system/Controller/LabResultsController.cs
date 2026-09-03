using Microsoft.AspNetCore.Mvc;
using optum_healthcare_system.Models.DTOs;
using optum_healthcare_system.Models;
using optum_healthcare_system.Repository.Interfaces;

namespace optum_healthcare_system.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class LabResultsController : ControllerBase
    {
        private readonly ILabResultRepository _labResultRepository;
        public LabResultsController(ILabResultRepository labResultRepository)
        {
            _labResultRepository = labResultRepository;
        }
        /// <summary>
        /// Get a lab result by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Record matching with the ID.
        /// </returns>
        [HttpGet("{id:guid}", Name = "GetLabResultById")]
        public async Task<IActionResult> GetTestByIdAsync(Guid id)
        {
            var result = await _labResultRepository.GetTestByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = MessageConstants.NotFoundMessage });
            }
            return Ok(result);
        }

        /// <summary>
        /// Get lab results by patient ID with pagination.
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// A list of lab results for the specified patient.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetTestByPatientIdAsync([FromQuery] Guid patientId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page < 1)
            {
                return BadRequest(new { message = MessageConstants.MinPageNumberMessage });
            }
            if (pageSize < 1 || pageSize > 50)
            {
                return BadRequest(new { message = MessageConstants.MinPageSizeMessage });
            }
            var result = await _labResultRepository.GetTestByPatientIdAsync(patientId, page, pageSize);
            if (!result.Data.Any())
            {
                return NotFound(new { message = MessageConstants.NotFoundMessage });
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new lab result.
        /// </summary>
        /// <param name="labResult"></param>
        /// <returns>
        /// The created lab result.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateLabResult([FromBody] CreateLabResultDto labResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedLabResult = await _labResultRepository.CreateTestAsync(labResult);
            return CreatedAtRoute("GetLabResultById", new { id = addedLabResult.Id }, addedLabResult);
        }

        /// <summary>
        /// Update an existing lab result.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="labResult"></param>
        /// <returns>
        /// The updated lab result.
        /// </returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateLabResult(Guid id, [FromBody] UpdateLabResultDto labResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedLabResult = await _labResultRepository.UpdateTestAsync(id, labResult);
            if (updatedLabResult == null)
            {
                return NotFound(new { message = MessageConstants.NotFoundMessage });
            }
            return Ok(updatedLabResult);
        }
        
        /// <summary>
        /// Delete a lab result by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLabResult(Guid id)
        {
            var isDeleted = await _labResultRepository.DeleteTestByIdAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = MessageConstants.NotFoundMessage });
            }
            return NoContent();
        }
    }
}
