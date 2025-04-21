--create database ProductDB

use [ProductDB]

-------------------------------------------Create Table-------------------------------------------------------

create table Product (Id int Primary key identity(1,1), Name nvarchar(20), Price decimal, Description nvarchar(100), IsSold bit);

--alter table Product set IsSold not null
ALTER TABLE Product
ALTER COLUMN Name NVARCHAR(100);

CREATE TABLE ErrorLog (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ErrorMessage NVARCHAR(MAX),
    StackTrace NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
);

--Select * from ErrorLog

--------------------------------------------Create Procedure----------------------------------------------------

CREATE PROCEDURE GetAllProducts
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Name,
        Price,
        Description,
        IsSold
    FROM 
        Product;
END;

CREATE PROCEDURE GetPaginatedProducts
    @Page INT,
    @PageSize INT,
    @Search NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    -- Total count
    SELECT COUNT(*) 
    FROM Product
    WHERE 
        (@Search IS NULL OR @Search = '' 
         OR Name LIKE '%' + @Search + '%'
         OR Description LIKE '%' + @Search + '%');

    -- Paged Result
    SELECT 
        Id,
        Name,
        Price,
        Description,
        IsSold
    FROM Product
    WHERE 
        (@Search IS NULL OR @Search = '' 
         OR Name LIKE '%' + @Search + '%'
         OR Description LIKE '%' + @Search + '%')
    ORDER BY Id
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;


CREATE PROCEDURE GetByIdProduct
@Id int
AS
BEGIN

SET NOCOUNT ON;
SELECT TOP(1) * FROM Product
	WHERE Id = @Id
END

CREATE PROCEDURE AddProduct
    @Name NVARCHAR(40),
    @Price DECIMAL(18, 0),
    @Description NVARCHAR(100),
    @IsSold BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Product (Name, Price, Description, IsSold)
    VALUES (@Name, @Price, @Description, @IsSold);
END;

CREATE PROCEDURE UpdateProduct
    @Id INT,
    @Name NVARCHAR(40),
    @Price DECIMAL(18, 0),
    @Description NVARCHAR(100),
    @IsSold BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Product
    SET 
        Name = @Name,
        Price = @Price,
        Description = @Description,
        IsSold = @IsSold
    WHERE 
        Id = @Id;
END;

CREATE PROCEDURE DeleteProduct
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Product
    WHERE Id = @Id;
END;


ALTER PROCEDURE AddProduct
    @Name NVARCHAR(100), 
    @Price DECIMAL(18, 0),
    @Description NVARCHAR(100),
    @IsSold BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Product (Name, Price, Description, IsSold)
    VALUES (@Name, @Price, @Description, @IsSold);
END;



--------------------------------------------Data Modification-------------------------------------------------------------

EXEC GetAllProducts

EXEC GetPaginatedProducts 1, 10, 'm'

EXEC AddProduct @Name = 'iPhone 14', @Price = 79999, @Description = 'Latest Apple smartphone', @IsSold = 0;
EXEC AddProduct @Name = 'Samsung Galaxy S23', @Price = 69999, @Description = 'Flagship Samsung device', @IsSold = 0;
EXEC AddProduct @Name = 'OnePlus 11', @Price = 59999, @Description = 'High performance OnePlus phone', @IsSold = 1;
EXEC AddProduct @Name = 'MacBook Pro M2', @Price = 149999, @Description = 'Apple laptop with M2 chip', @IsSold = 0;
EXEC AddProduct @Name = 'Dell XPS 13', @Price = 109999, @Description = 'Premium ultrabook from Dell', @IsSold = 1;
EXEC AddProduct @Name = 'iPad Air', @Price = 54999, @Description = 'Lightweight iPad with A14 chip', @IsSold = 0;
EXEC AddProduct @Name = 'Sony WH-1000XM5', @Price = 29999, @Description = 'Noise cancelling wireless headphones', @IsSold = 0;
EXEC AddProduct @Name = 'Canon EOS 1500D', @Price = 39999, @Description = 'Entry-level DSLR camera', @IsSold = 0;
EXEC AddProduct @Name = 'Asus ROG Phone 6', @Price = 69999, @Description = 'Gaming phone with high refresh rate', @IsSold = 1;
EXEC AddProduct @Name = 'HP Spectre x360', @Price = 124999, @Description = 'Convertible premium laptop', @IsSold = 0;
EXEC AddProduct @Name = 'Google Pixel 7', @Price = 64999, @Description = 'Google flagship with great camera', @IsSold = 0;
EXEC AddProduct @Name = 'Lenovo Legion 5', @Price = 84999, @Description = 'Gaming laptop with Ryzen processor', @IsSold = 0;
EXEC AddProduct @Name = 'AirPods Pro 2', @Price = 24999, @Description = 'Apple noise-cancelling earbuds', @IsSold = 1;
EXEC AddProduct @Name = 'Samsung Galaxy Tab S8', @Price = 72999, @Description = 'High-end Android tablet', @IsSold = 0;
EXEC AddProduct @Name = 'Microsoft Surface Laptop 5', @Price = 134999, @Description = 'Sleek productivity laptop', @IsSold = 0;
EXEC AddProduct @Name = 'Realme GT Neo 3', @Price = 36999, @Description = 'Fast charging midrange phone', @IsSold = 1;
EXEC AddProduct @Name = 'DJI Mini 3 Pro', @Price = 78999, @Description = 'Compact drone with 4K video', @IsSold = 0;
EXEC AddProduct @Name = 'Xbox Series X', @Price = 49999, @Description = 'Next-gen gaming console from Microsoft', @IsSold = 0;
EXEC AddProduct @Name = 'PlayStation 5', @Price = 49999, @Description = 'Sony next-gen gaming console', @IsSold = 0;
EXEC AddProduct @Name = 'LG OLED TV 55"', @Price = 129999, @Description = 'Premium 4K OLED television', @IsSold = 1;
