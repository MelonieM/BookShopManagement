USE BookShopDB;

-- Test each user
DECLARE @testPassword NVARCHAR(50);
DECLARE @testHash NVARCHAR(256);

-- Test manager
SET @testPassword = 'manager123';
SET @testHash = '1d0258c2440a8d19e716292b231e3190';

SELECT 'MANAGER TEST' AS Test;
SELECT * FROM Users 
WHERE Username = 'manager' 
AND PasswordHash = @testHash;

-- Test cashier
SET @testPassword = 'cashier123';
SET @testHash = '25f9e794323b453885f5181f1b624d0b';

SELECT 'CASHIER TEST' AS Test;
SELECT * FROM Users 
WHERE Username = 'cashier' 
AND PasswordHash = @testHash;