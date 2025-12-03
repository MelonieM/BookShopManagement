# Book Shop Management System

A comprehensive, production-ready desktop application for managing bookstore operations, built with WPF (Windows Presentation Foundation) and SQL Server.


## Project Overview

The Book Shop Management System is a full-featured desktop application designed for small to medium-sized bookstores. It provides complete inventory management, sales processing, customer relationship management, and business analytics with secure user authentication and role-based access control.

## Team members
A00314636, Melonie Matho M


## Key Features

### 1. Authentication & Security
- User Login System : Secure authentication with password hashing
- Role-Based Access Control: Admin, Manager, and Cashier roles
- Session Management: Track logged-in users and login history
- Logout Functionality: Secure session termination

### 2. Dashboard
- Real-time statistics overview
- Today's revenue tracking
- Total books and customers count
- Low stock alerts
- Recent sales display
- Quick action buttons

### 3. Book Management
- Add, edit, and delete books
- ISBN, title, author, price, category tracking
- Stock quantity management
- Publisher and publication year
- Advanced search functionality
- Bulk stock updates
- Real-time inventory tracking

### 4. Customer Management
- Customer registration and profiles
- Contact information management
- Email and address tracking
- Purchase history viewing
- Customer relationship tracking

### 5. Sales Processing 
- Interactive shopping cart
- Add/remove items with quantity control
- Automatic stock deduction
- Discount application (percentage-based)
- Payment methods
- Customer assignment (optional)
- Real-time total calculation
- Transaction recording

### 6. Reports & Analytics
- Today's Sales Report
- Date Range Reports
- Low Stock Alerts
- Top Selling Books
- Revenue Summary 


### 7. User Management (Admin Only)
- Add and edit users
- Assign roles (Admin/Manager/Cashier)
- Activate/deactivate accounts
- Password management
- View login history


## Technology 

Framework : .NET 6.0+ / WPF 
Language  : C# 10.0 
Database  : SQL Server LocalDB 
Data Access: Microsoft.Data.SqlClient 
UI Framework : WPF (Windows Presentation Foundation) 
Architecture : Three-Tier (Presentation, Business, Data) 
Security : MD5 Password Hashing (demo - upgrade to bcrypt) 



## Installation & Setup

Prerequisites

- Windows 10/11 (64-bit)
- Visual Studio 2022/ Insider (Community or higher)
- SQL Server (Express or LocalDB)
- SQL Server Management Studio (SSMS) - Optional but recommended
- .NET 6.0 SDK or higher

#### Step 1: Clone or Download

Clone the repository (if using Git)
git clone https://github.com/yourusername/BookShopManagement.git


#### Step 2: Install Dependencies

1. Open Visual Studio 2022
2. Open the solution: `BookShopManagement.sln
3. Install NuGet package:
   
   Tools → NuGet Package Manager → Manage NuGet Packages
   Search: Microsoft.Data.SqlClient
   Install
   

#### Step 3: Create Database

1. Open SQL Server Management Studio (SSMS)
2. Connect to**: `(localdb)\projectModels` or your SQL Server instance
3. Create Database**:
   sql
   CREATE DATABASE BookShopDB;
   GO
   

#### Step 4: Run Database Scripts

Execute these scripts in order:

1. Create Core Tables (Books, Customers, Sales, SaleItems)
2. Create Login Tables (Users, LoginHistory)
3. Insert Sample Data

sql
USE BookShopDB;
GO

Run all table creation scripts provided in the project
See /Database/CreateTables.sql for complete script


#### Step 5: Update Connection String

Open "Data/DatabaseConnection.cs" and update:

csharp
private static string connectionString = 
    "Server=(localdb)\\projectModels;Database=BookShopDB;Integrated Security=true;";


#### Step 6: Build and Run

1. Clean Solution: Build → Clean Solution
2. Rebuild Solution: Ctrl + Shift + B
3. Run: 'F5' or Start


## Recommended Upgrades for Production
- Upgrade to bcrypt or Argon2 password hashing
- Implement HTTPS for network communication
- Add account lockout after failed login attempts
- Implement password complexity requirements
- Add two-factor authentication (2FA)
- Use environment variables for connection strings



## Future Enhancements

## Planned Features
- Barcode Scanning
- Receipt Printing
- Data Export
- Email Notifications
- Cloud Backup
- Multi-language Support
- Dark Mode
- Supplier Management
## Technical Improvements
- Migrate to .NET 8
- Add **Integration Tests
- Implement Logging



## Acknowledgments

- Microsoft- .NET Framework and WPF
- SQL Server - Robust database management
- Visual Studio - Excellent IDE
- Stack Overflow Community - Problem-solving assistance
- GitHub - Version control and collaboration
- Claude.ai - Assistant

