using optum_healthcare_system.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace optum_healthcare_system.Repository
{
    public static class DataSeeder
    {
        public static void seedData(AppDbContext context)
        {
            if (context.LabResults.Any()) return;

            var patient1 = Guid.Parse("2A536E9F-FD3F-474C-BFB2-464479FACAC5");
            var patient2 = Guid.Parse("5ED42DF3-5246-437F-A47A-64DD172A0E68");
            var patient3 = Guid.Parse("4F342198-7839-49E9-9B65-80F26DF84723");

            context.LabResults.AddRange(
                        new LabResult
                        {
                            Id = Guid.NewGuid(),
                            PatientId = patient3,
                            TestName = "Blood Glucose",
                            ResultValue = "95",
                            Unit = "mg/dL",
                            ObservedDate = DateTime.Now.AddDays(-10)
                        },
                        new LabResult
                        {
                            Id = Guid.NewGuid(),
                            PatientId = patient1,
                            TestName = "Hemoglobin",
                            ResultValue = "14.2",
                            Unit = "g/dL",
                            ObservedDate = DateTime.Now.AddDays(-5)
                        },
                        new LabResult
                        {
                            Id = Guid.NewGuid(),
                            PatientId = patient2,
                            TestName = "Covid Test",
                            ResultValue = "Negative",
                            Unit = "",
                            ObservedDate = DateTime.Now.AddDays(-7)
                        }
            );
            context.SaveChanges();

        }
    }
}
