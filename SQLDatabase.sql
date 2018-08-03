--Database
CREATE DATABASE Pharmacy;

--Tables : Medicine, Prescriptions, Orders
CREATE TABLE Medicines(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name VARCHAR(256) NOT NULL UNIQUE, 
	Manufacturer VARCHAR(256) NOT NULL, 
	Price DECIMAL(18,2) NOT NULL, 
	Amount INT NOT NULL, 
	WithPrescription BIT NOT NULL
);

CREATE TABLE Prescriptions(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CustomerName VARCHAR(256) NOT NULL,  
	Pesel VARCHAR(11) NOT NULL UNIQUE, 
	PrescriptionNumber INT NOT NULL UNIQUE
);

CREATE TABLE Orders(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	PrescriptionId INT NULL,
	MedicineId INT NOT NULL,
	Date DATETIME NOT NULL,
	Amount INT NOT NULL
);

--Foreign keys
ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_Prescriptions FOREIGN KEY (PrescriptionId)
REFERENCES Prescriptions (Id);

ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_Medicines FOREIGN KEY (MedicineId)
REFERENCES Medicines (Id);