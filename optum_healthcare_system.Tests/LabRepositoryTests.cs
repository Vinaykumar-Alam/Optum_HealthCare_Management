using optum_healthcare_system.Repository;
using Microsoft.EntityFrameworkCore;
using optum_healthcare_system.Repository.Repository;
using optum_healthcare_system.Models.DTOs;
using FluentAssertions;

namespace optum_healthcare_system.Tests
{
    public class LabRepositoryTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=labresults.db")
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CreateTestAsync_ShouldReturnCreatedLabResult()
        {
            // Arranging
            using var context = CreateDbContext();
            var repo = new LabResultRepository(context);

            var dto = new CreateLabResultDto
            {
                PatientId = Guid.NewGuid(),
                TestName = "Covid Test",
                ResultValue = "Negative",
                Unit = "N/A",
                ObservedDate = DateTime.UtcNow
            };

            //Acting
            var result = await repo.CreateTestAsync(dto);

            //Asserting
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.TestName.Should().Be("Covid Test");
            result.ResultValue.Should().Be("Negative");
            result.Unit.Should().Be("N/A");
            result.PatientId.Should().Be(dto.PatientId);
            result.ObservedDate.Should().Be(dto.ObservedDate);
        }

        [Fact]
        public async Task GetTestByPatientIdAsync_ShouldReturnLabResultsByPatient()
        {
            //Arranging
            using var context = CreateDbContext();
            var repository=new LabResultRepository(context);
            var patientId = Guid.NewGuid();
            for(int i = 1; i <=5; i++)
            {
                await repository.CreateTestAsync(new CreateLabResultDto
                {
                    PatientId = patientId,
                    TestName = $"Test {i}",
                    ResultValue = "Positive",
                    Unit = "mg/dL",
                    ObservedDate = DateTime.UtcNow.AddDays(-i)
                });
            }
            //Acting
            var result =await repository.GetTestByPatientIdAsync(patientId, 1, 3);

            //Asserting 
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(3);
            result.TotalCount.Should().Be(5);
            result.TotalPages.Should().Be(2);
            result.hasNextPage.Should().BeTrue();
            result.hasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteLabResult_ShouldReturnFalse_WhenNoIdExists () {
            //Arranging
            using var context= CreateDbContext();
            var repo = new LabResultRepository(context);

            //Acting
            var result=await repo.DeleteTestByIdAsync(Guid.NewGuid());

            //Asserting
            result.Should().BeFalse();
        }
        [Fact]
        public async Task DeleteLabResult_ShouldReturnTrue_WhenIdExists()
        {
            //Arranging
            using var context = CreateDbContext();
            var repo = new LabResultRepository(context);
            var labResult = await repo.CreateTestAsync(new CreateLabResultDto
            {
                PatientId = Guid.NewGuid(),
                TestName = "Covid Test",
                ResultValue = "Negative",
                Unit = "N/A",
                ObservedDate = DateTime.UtcNow
            });

            //Acting
            var result = await repo.DeleteTestByIdAsync(labResult.Id);

            //Asserting
            result.Should().BeTrue();
        }
    }
}
