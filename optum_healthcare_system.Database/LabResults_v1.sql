
/*====================================================================
    Table: Tbl_Optum_HealthCare_LabResults

    Purpose:
        Stores Lab Results for patients in the healthcare system.

    Description:
        - Represents Labresults (patients).
        - Links each patient to taken Undergone Lab tests.

====================================================================*/

CREATE TABLE [dbo].[Tbl_Optum_HealthCare_LabResults](
	-- Unique identifier for each user (sequential GUID)
	Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	-- Unique identifier for the patient (foreign key) - to maintain relation with patient table
	PatientId UNIQUEIDENTIFIER NOT NULL,
	-- Name of the lab test performed
	TestName NVARCHAR(200) NOT NULL,
	-- Name of the result
	ResultValue NVARCHAR(100) NOT NULL,
	-- Unit of measurement
	Unit NVARCHAR(10),
	-- Date when the test was observed
	ObservedDate DATETIME2 NOT NULL,
	-- Timestamp when the record was created
	CreatedOn Datetime2 NOT NULL DEFAULT GETUTCDATE(),
	-- used to soft delete the record
	IsActive BIT NOT NULL DEFAULT 1,

	----This is the log data which can be used to track the changes made to the record

	-- User who created the record
	CreatedBy NVARCHAR(100) NOT NULL,
	-- Timestamp when the record was last updated
	UpdatedOn Datetime2 NULL,
	-- User who last updated the record
	UpdatedBy NVARCHAR(100) NULL,

	-- Primary Key constraint on Id column
	CONSTRAINT PK_LabResults PRIMARY KEY(Id)
);
GO

CREATE INDEX IX_LabResults_PatientId ON [dbo].[Tbl_Optum_HealthCare_LabResults](PatientId);
GO

CREATE INDEX IX_LabResults_ObservedDate ON [dbo].[Tbl_Optum_HealthCare_LabResults](ObservedDate);
GO


