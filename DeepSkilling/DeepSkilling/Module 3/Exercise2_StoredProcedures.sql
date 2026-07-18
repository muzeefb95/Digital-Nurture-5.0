-- ===============================================================================
--                 Exercise 2: Stored Procedures (Exercise 1 & 5)
-- ===============================================================================
-- Goal: Create stored procedures to insert, retrieve, and aggregate employee data.
-- ===============================================================================

-- 1. Create Schema
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100)
);

CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY, -- Using Identity for auto-increment
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT,
    Salary DECIMAL(10,2),
    JoinDate DATE,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- 2. Insert Sample Data
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES 
(1, 'HR'), 
(2, 'Finance'), 
(3, 'IT'), 
(4, 'Marketing');

INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES 
('John', 'Doe', 1, 5000.00, '2020-01-15'), 
('Jane', 'Smith', 2, 6000.00, '2019-03-22'),
('Michael', 'Johnson', 3, 7000.00, '2018-07-30'), 
('Emily', 'Davis', 4, 5500.00, '2021-11-05');

GO -- Using GO batch terminator as standard in SQL Server

-- ===============================================================================
-- Stored Procedure 1: Insert Employee
-- ===============================================================================
CREATE PROCEDURE sp_InsertEmployee
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @DepartmentID INT,
    @Salary DECIMAL(10,2),
    @JoinDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate)
    VALUES (@FirstName, @LastName, @DepartmentID, @Salary, @JoinDate);
END;
GO

-- ===============================================================================
-- Stored Procedure 2: Retrieve Employees by Department (with Salary - Exercise 2)
-- ===============================================================================
CREATE PROCEDURE sp_GetEmployeesByDepartment
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        EmployeeID,
        FirstName,
        LastName,
        DepartmentID,
        Salary, -- Salary column included as per Exercise 2 modification
        JoinDate
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- ===============================================================================
-- Stored Procedure 3: Get Employee Count in Department (Exercise 5)
-- ===============================================================================
CREATE PROCEDURE sp_GetEmployeeCountByDepartment
    @DepartmentID INT,
    @EmployeeCount INT OUTPUT -- Using OUTPUT parameter for returning data
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @EmployeeCount = COUNT(*)
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- ===============================================================================
-- Execution Demonstrations / Tests
-- ===============================================================================

-- A. Execute sp_InsertEmployee to insert a new IT employee
EXEC sp_InsertEmployee 'David', 'Miller', 3, 8000.00, '2023-05-10';

-- B. Execute sp_GetEmployeesByDepartment to retrieve IT department employees (ID: 3)
EXEC sp_GetEmployeesByDepartment @DepartmentID = 3;

-- C. Execute sp_GetEmployeeCountByDepartment to get count of IT department
DECLARE @Count INT;
EXEC sp_GetEmployeeCountByDepartment @DepartmentID = 3, @EmployeeCount = @Count OUTPUT;
SELECT @Count AS 'Total Employees in IT';
