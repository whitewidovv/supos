DROP TABLE "Categories"; 
CREATE TABLE "Categories" (
	"Id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Name" TEXT NULL,
	"Icon" TEXT NULL
	);  


DROP TABLE "Products"; 
CREATE TABLE "Products"(
	"Id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	"CategoryId" INTEGER NOT NULL,
	"Name" TEXT NULL,
	"Icon" TEXT NULL,
	"DefaultTaxId" INTEGER NULL, 
	"Price" REAL NULL
	);  

	   
DROP TABLE "Taxes"; 
CREATE TABLE "Taxes"(
	"Id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Name" TEXT NULL,
	"Rate" REAL NULL,
	"Overrideable" INTEGER NOT NULL DEFAULT 1);  

	  
DROP TABLE "Customers"; 
CREATE TABLE "Customers"(
	"Id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Name" TEXT NULL
	);  

	  
DROP TABLE "Orders"; 
CREATE TABLE "Orders"(
	"Id" TEXT PRIMARY KEY NOT NULL,
	"CustomerId" INTEGER NOT NULL,
	"PaymentId" INTEGER NULL,
	"TaxId" INTEGER NULL
	);  

	
DROP TABLE "OrderDetails"; 
CREATE TABLE "OrderDetails"(
	"Id" TEXT PRIMARY KEY NOT NULL,
	"OrderId" TEXT NULL,
	"ProductId" INTEGER NULL,
	"Price" REAL NULL,
	"Quantity" INTEGER NULL,
	"TaxId" INTEGER NULL
	);  

	 
DROP TABLE "Payments"; 
CREATE TABLE "Payments"(
	"Id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	"Name" TEXT,
	"Allowed" INTEGER NOT NULL DEFAULT 1
	);  

DROP TABLE "Meta";
CREATE TABLE "Meta" (
    "Id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "Property" TEXT,
    "Value" TEXT
	);
