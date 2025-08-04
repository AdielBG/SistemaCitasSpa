CREATE DATABASE SpaDB;
GO
USE SpaDB;
GO

CREATE TABLE Paciente (
    PacienteID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(15),
    Correo NVARCHAR(100),
    FechaNacimiento DATE
);

GO

CREATE TABLE Servicio (
    ServicioID INT PRIMARY KEY IDENTITY(1,1),
    NombreServicio NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    DuracionEnMinutos INT NOT NULL CHECK (DuracionEnMinutos > 0),
    Precio DECIMAL(10,2) NOT NULL CHECK (Precio >= 0)
);

GO

CREATE TABLE Terapeuta (
    TerapeutaID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Especialidad NVARCHAR(100),
    Telefono NVARCHAR(15),
    Correo NVARCHAR(100)
);

GO

CREATE TABLE Cita (
    CitaID INT PRIMARY KEY IDENTITY(1,1),
    PacienteID INT NOT NULL,
    ServicioID INT NOT NULL,
    TerapeutaID INT NOT NULL,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    
    -- Campos calculados no se almacenan físicamente; se manejan en el sistema

    CONSTRAINT FK_Cita_Paciente FOREIGN KEY (PacienteID) REFERENCES Paciente(PacienteID),
    CONSTRAINT FK_Cita_Servicio FOREIGN KEY (ServicioID) REFERENCES Servicio(ServicioID),
    CONSTRAINT FK_Cita_Terapeuta FOREIGN KEY (TerapeutaID) REFERENCES Terapeuta(TerapeutaID)
);