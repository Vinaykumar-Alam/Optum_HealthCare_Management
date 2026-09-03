-- FILTERING: Get lab results for a specific patient
SELECT Id, PatientId, TestName, ResultValue, Unit, ObservedDate
FROM [dbo].[Tbl_Optum_HealthCare_LabResults]
WHERE PatientId = PatientID
ORDER BY ObservedDate DESC;
GO

-- AGGREGATION: Count of lab tests per patient
SELECT 
    PatientId,
    COUNT(*) AS TotalTests
FROM [dbo].[Tbl_Optum_HealthCare_LabResults]
GROUP BY PatientId;


-- MOST RECENT: Get the most recent lab result for each patient
SELECT 
    PatientId,
    TestName,
    ResultValue,
    Unit,
    ObservedDate
FROM [dbo].[Tbl_Optum_HealthCare_LabResults] lr
WHERE ObservedDate = (
    SELECT MAX(ObservedDate) 
    FROM [dbo].[Tbl_Optum_HealthCare_LabResults] 
    WHERE PatientId = lr.PatientId
)
ORDER BY PatientId;

