-- ============================================
-- Database Verification Script
-- Run this in SQL Server Management Studio
-- ============================================

USE BookShopDB;
GO

PRINT '========================================';
PRINT 'CHECKING DATABASE SETUP';
PRINT '========================================';
PRINT '';

-- 1. Check if Users table exists
IF OBJECT_ID('Users', 'U') IS NOT NULL
    PRINT '✓ Users table EXISTS'
ELSE
BEGIN
    PRINT '✗ Users table DOES NOT EXIST - Please create it first!'
    PRINT 'Run the table creation script first.'
END
GO

-- 2. Check if LoginHistory table exists
IF OBJECT_ID('LoginHistory', 'U') IS NOT NULL
    PRINT '✓ LoginHistory table EXISTS'
ELSE
    PRINT '✗ LoginHistory table DOES NOT EXIST'
GO

PRINT '';
PRINT '========================================';
PRINT 'CHECKING DEFAULT USERS';
PRINT '========================================';
PRINT '';

-- 3. Check user count
DECLARE @UserCount INT;
SELECT @UserCount = COUNT(*) FROM Users;
PRINT 'Total Users: ' + CAST(@UserCount AS VARCHAR(10));
PRINT '';

-- 4. Display all users
IF EXISTS (SELECT * FROM Users)
BEGIN
    PRINT '--- USER LIST ---';
    SELECT 
        UserID,
        Username,
        FullName,
        Role,
        CASE WHEN IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status,
        PasswordHash
    FROM Users
    ORDER BY Role, Username;
    PRINT '';
END
ELSE
BEGIN
    PRINT '⚠️ NO USERS FOUND!';
    PRINT 'Creating default users now...';
    PRINT '';
    
    -- Create default users if they don't exist
    INSERT INTO Users (Username, PasswordHash, FullName, Role, IsActive)
    VALUES 
    ('admin', '0192023a7bbd73250516f069df18b500', 'System Administrator', 'Admin', 1),
    ('manager', '1d0258c2440a8d19e716292b231e3190', 'Store Manager', 'Manager', 1),
    ('cashier', '25f9e794323b453885f5181f1b624d0b', 'Cashier User', 'Cashier', 1);
    
    PRINT '✓ Default users created!';
    PRINT '';
    
    SELECT 
        UserID,
        Username,
        FullName,
        Role,
        'Active' AS Status
    FROM Users
    ORDER BY Role, Username;
END

PRINT '';
PRINT '========================================';
PRINT 'PASSWORD HASH VERIFICATION';
PRINT '========================================';
PRINT '';
PRINT 'Checking admin user password hash...';

DECLARE @AdminHash NVARCHAR(256);
SELECT @AdminHash = PasswordHash FROM Users WHERE Username = 'admin';

IF @AdminHash IS NOT NULL
BEGIN
    PRINT 'Admin PasswordHash: ' + @AdminHash;
    
    -- The correct hash for "admin123" using MD5
    IF @AdminHash = '0192023a7bbd73250516f069df18b500'
        PRINT '✓ Admin password hash is CORRECT for password: admin123';
    ELSE IF @AdminHash = 'e99a18c428cb38d5f260853678922e03'
        PRINT '✓ Admin password hash is valid (alternative hash)';
    ELSE
        PRINT '⚠️ Admin password hash does not match expected value!';
END
ELSE
    PRINT '✗ Admin user not found!';

PRINT '';
PRINT '========================================';
PRINT 'TEST CREDENTIALS';
PRINT '========================================';
PRINT '';
PRINT 'Try logging in with:';
PRINT '';
PRINT '  Username: admin';
PRINT '  Password: admin123';
PRINT '';
PRINT '  Username: manager';
PRINT '  Password: manager123';
PRINT '';
PRINT '  Username: cashier';
PRINT '  Password: cashier123';
PRINT '';

PRINT '========================================';
PRINT 'VERIFICATION COMPLETE';
PRINT '========================================';

-- Show connection string info
PRINT '';
PRINT 'Your connection string should be:';
PRINT 'Server=(localdb)\projectModels;Database=BookShopDB;Integrated Security=true;';