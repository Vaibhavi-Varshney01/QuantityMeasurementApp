IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QuantityMeasurementDB')
BEGIN
CREATE DATABASE QuantityMeasurementDB;
END
GO

USE QuantityMeasurementDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'QuantityMeasurements')
BEGIN
CREATE TABLE QuantityMeasurements
(
Id INT IDENTITY(1,1) PRIMARY KEY,

    OperationType NVARCHAR(50) NOT NULL,
    MeasurementType NVARCHAR(50) NOT NULL,

    Operand1Value FLOAT NULL,
    Operand1Unit NVARCHAR(20) NULL,

    Operand2Value FLOAT NULL,
    Operand2Unit NVARCHAR(20) NULL,

    ResultValue FLOAT NULL,
    ResultUnit NVARCHAR(20) NULL,

    BooleanResult BIT NULL,
    NumericResult FLOAT NULL,

    HasError BIT NOT NULL DEFAULT 0,
    ErrorMessage NVARCHAR(500) NULL,

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'QuantityMeasurementHistory')
BEGIN
CREATE TABLE QuantityMeasurementHistory
(
Id INT IDENTITY(1,1) PRIMARY KEY,

    MeasurementId INT NOT NULL,
    Action NVARCHAR(50) NOT NULL,

    ActionTime DATETIME NOT NULL DEFAULT GETDATE(),
    Note NVARCHAR(500) NULL,

    CONSTRAINT FK_QuantityMeasurement
    FOREIGN KEY (MeasurementId)
    REFERENCES QuantityMeasurements(Id)
    ON DELETE CASCADE
);

END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_operation_type')
CREATE INDEX idx_operation_type
ON QuantityMeasurements(OperationType);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_measurement_type')
CREATE INDEX idx_measurement_type
ON QuantityMeasurements(MeasurementType);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_created_at')
CREATE INDEX idx_created_at
ON QuantityMeasurements(CreatedAt);
GO

CREATE OR ALTER PROCEDURE sp_SaveMeasurement
(
@OperationType NVARCHAR(50),
@MeasurementType NVARCHAR(50),

@Operand1Value FLOAT = NULL,
@Operand1Unit NVARCHAR(20) = NULL,

@Operand2Value FLOAT = NULL,
@Operand2Unit NVARCHAR(20) = NULL,

@ResultValue FLOAT = NULL,
@ResultUnit NVARCHAR(20) = NULL,

@BooleanResult BIT = NULL,
@NumericResult FLOAT = NULL,

@HasError BIT,
@ErrorMessage NVARCHAR(500) = NULL

)
AS
BEGIN

INSERT INTO QuantityMeasurements
(
OperationType,
MeasurementType,
Operand1Value,
Operand1Unit,
Operand2Value,
Operand2Unit,
ResultValue,
ResultUnit,
BooleanResult,
NumericResult,
HasError,
ErrorMessage
)

VALUES
(
@OperationType,
@MeasurementType,
@Operand1Value,
@Operand1Unit,
@Operand2Value,
@Operand2Unit,
@ResultValue,
@ResultUnit,
@BooleanResult,
@NumericResult,
@HasError,
@ErrorMessage
);

END
GO

CREATE OR ALTER PROCEDURE sp_SaveHistory
(
@MeasurementId INT,
@Action NVARCHAR(50),
@Note NVARCHAR(500)
)
AS
BEGIN

INSERT INTO QuantityMeasurementHistory
(
MeasurementId,
Action,
Note
)

VALUES
(
@MeasurementId,
@Action,
@Note
);

END
GO

CREATE OR ALTER PROCEDURE sp_GetAllMeasurements
AS
BEGIN

SELECT *
FROM QuantityMeasurements
ORDER BY CreatedAt DESC;

END
GO

CREATE OR ALTER PROCEDURE sp_GetMeasurementCount
AS
BEGIN

SELECT COUNT(*) AS TotalMeasurements
FROM QuantityMeasurements;

END
GO

CREATE OR ALTER PROCEDURE sp_DeleteAllMeasurements
AS
BEGIN

DELETE FROM QuantityMeasurementHistory;
DELETE FROM QuantityMeasurements;

END
GO

SELECT * FROM QuantityMeasurements;

SELECT COUNT(*) FROM QuantityMeasurements;