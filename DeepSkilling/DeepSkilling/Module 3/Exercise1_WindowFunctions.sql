-- ===============================================================================
--                 Exercise 1: Ranking and Window Functions
-- ===============================================================================
-- Goal: Use ROW_NUMBER(), RANK(), DENSE_RANK(), OVER(), and PARTITION BY.
-- Scenario: Find the top 3 most expensive products in each category.
-- ===============================================================================

-- 1. Create Schema
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(100),
    Region VARCHAR(50)
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Price DECIMAL(10, 2)
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerID INT,
    OrderDate DATE,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- 2. Insert Sample Data (with tied prices to demonstrate different ranking functions)
INSERT INTO Customers (CustomerID, Name, Region) VALUES
(1, 'Alice', 'North'),
(2, 'Bob', 'South'),
(3, 'Charlie', 'East'),
(4, 'David', 'West');

INSERT INTO Products (ProductID, ProductName, Category, Price) VALUES
-- Electronics
(1, 'Laptop Dell XPS', 'Electronics', 1500.00),
(2, 'Apple iPhone 15', 'Electronics', 1200.00),
(3, 'Samsung Galaxy S24', 'Electronics', 1200.00), -- Tie in Price
(4, 'Sony Headphones', 'Electronics', 350.00),
(5, 'Mechanical Keyboard', 'Electronics', 150.00),
-- Home & Kitchen
(6, 'Robot Vacuum', 'Home & Kitchen', 450.00),
(7, 'Air Fryer XL', 'Home & Kitchen', 180.00),
(8, 'Instant Pot Duo', 'Home & Kitchen', 120.00),
(9, 'Slow Cooker', 'Home & Kitchen', 120.00), -- Tie in Price
(10, 'Electric Kettle', 'Home & Kitchen', 50.00),
-- Footwear
(11, 'Nike Air Max', 'Footwear', 180.00),
(12, 'Adidas Ultraboost', 'Footwear', 180.00), -- Tie in Price
(13, 'Puma Running Shoes', 'Footwear', 120.00),
(14, 'Reebok Trainers', 'Footwear', 100.00),
(15, 'Slippers', 'Footwear', 30.00);

-- 3. Steps 1-3: Display Products with ROW_NUMBER(), RANK(), and DENSE_RANK()
-- This query demonstrates how each function handles ties in price per category.
SELECT 
    Category,
    ProductName,
    Price,
    ROW_NUMBER() OVER (PARTITION BY Category ORDER BY Price DESC) AS RowNumber,
    RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS RankVal,
    DENSE_RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS DenseRankVal
FROM Products;

-- 4. Find the top 3 most expensive products in each category using DENSE_RANK()
-- (Using CTE to filter results)
WITH RankedProducts AS (
    SELECT 
        Category,
        ProductName,
        Price,
        DENSE_RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS PriceRank
    FROM Products
)
SELECT 
    Category,
    ProductName,
    Price,
    PriceRank
FROM RankedProducts
WHERE PriceRank <= 3
ORDER BY Category, PriceRank;
