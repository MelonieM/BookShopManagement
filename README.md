*Project Overview*
The Book Shop Management System is a full-featured desktop application designed for small to medium-sized bookstores. It provides complete inventory management, sales processing, customer relationship management, and business analytics with secure user authentication and role-based access control.

*Technology Stack*
Framework  | .NET 6.0+ WPF
Language  | C#10.0 
Database  | SQL Server LocalDB
Data Access  | Microsoft.Data.SqlClient
UI Framewor  | kWPF (Windows Presentation Foundation)
Architecture  | Three-Tier (Presentation, Business, Data)
Security  | MD5 Password Hashing (demo - upgrade to bcrypt)

Key Features
*Authentication & Security*
User Login System - Secure authentication with password hashing
Role-Based Access Control - Admin, Manager, and Cashier roles
Session Management - Track logged-in users and login history
Password Protection - MD5 hashing (upgradable to bcrypt/Argon2)
Logout Functionality - Secure session termination

*Dashboard*
Real-time statistics overview
Today's revenue tracking
Total books and customers count
Low stock alerts
Recent sales display
Quick action buttons

*Book Management*
Add, edit, and delete books
ISBN, title, author, price, category tracking
Stock quantity management
Publisher and publication year
Advanced search functionality
Bulk stock updates
Real-time inventory tracking

*Customer Management*
Customer registration and profiles
Contact information management
Email and address tracking
Purchase history viewing
Customer relationship tracking

*Sales Processing*
Interactive shopping cart
Add/remove items with quantity control
Automatic stock deduction
Discount application (percentage-based)
Payment method
Customer assignment (optional)
Real-time total calculation
Transaction recording

*Reports & Analytics*
Today's Sales Report - Current day transactions
Date Range Reports - Custom period analysis
Low Stock Alerts - Inventory warnings (below 10 units)
Top Selling Books - Best performers ranking
Revenue Summary - Financial analytics
Exportable data views

*User Management (Admin Only)*
Add and edit users
Assign roles (Admin/Manager/Cashier)
Activate/deactivate accounts
Password management
View login history
