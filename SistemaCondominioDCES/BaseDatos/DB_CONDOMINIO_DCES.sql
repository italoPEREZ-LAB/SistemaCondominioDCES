USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'DB_CONDOMINIO_DCES')
BEGIN
    ALTER DATABASE DB_CONDOMINIO_DCES SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DB_CONDOMINIO_DCES;
END
GO

CREATE DATABASE DB_CONDOMINIO_DCES;
GO

USE DB_CONDOMINIO_DCES;
GO


--Roles
CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL
);
GO

--Condominios
CREATE TABLE Condominios (
    IdCondominio INT IDENTITY(1,1) PRIMARY KEY,
    NombreCondominio VARCHAR(100) NOT NULL,
    Direccion VARCHAR(200) NULL,
    Estado BIT DEFAULT 1 NOT NULL
);
GO

--Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    IdRol INT NOT NULL,
    IdCondominio INT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    DNI VARCHAR(20) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    Estado BIT DEFAULT 1 NOT NULL,
    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol),
    FOREIGN KEY (IdCondominio) REFERENCES Condominios(IdCondominio)
);
GO

--Torres
CREATE TABLE Torres (
    IdTorre INT IDENTITY(1,1) PRIMARY KEY,
    NombreTorre VARCHAR(100) NOT NULL
);
GO

--Departamentos
CREATE TABLE Departamentos (
    IdDepartamento INT IDENTITY(1,1) PRIMARY KEY,
    IdTorre INT NOT NULL,
    Numero VARCHAR(20) NOT NULL,
    Piso INT NOT NULL,
    Estado BIT DEFAULT 1 NOT NULL,
    FOREIGN KEY (IdTorre) REFERENCES Torres(IdTorre)
);
GO

--Propietarios
CREATE TABLE Propietarios (
    IdPropietario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    DNI VARCHAR(20) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Correo VARCHAR(100) NOT NULL,
    Torre VARCHAR(100) NOT NULL,
    NumeroDepartamento VARCHAR(20) NOT NULL,
    Estado BIT DEFAULT 1 NOT NULL
);
GO

--AreasComunes
CREATE TABLE AreasComunes (
    IdAreaComun INT IDENTITY(1,1) PRIMARY KEY,
    NombreArea VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    Capacidad INT NOT NULL DEFAULT 0,
    Estado BIT DEFAULT 1 NOT NULL
);
GO

--Recibos
CREATE TABLE Recibos (
    IdRecibo INT IDENTITY(1,1) PRIMARY KEY,
    IdDepartamento INT NOT NULL,
    Concepto VARCHAR(200) NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    FechaEmision DATE NOT NULL,
    FechaVencimiento DATE NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Pendiente' NOT NULL,
    FOREIGN KEY (IdDepartamento) REFERENCES Departamentos(IdDepartamento)
);
GO

--Pagos
CREATE TABLE Pagos (
    IdPago INT IDENTITY(1,1) PRIMARY KEY,
    IdRecibo INT NOT NULL,
    MontoPagado DECIMAL(10,2) NOT NULL,
    FechaPago DATETIME DEFAULT GETDATE() NOT NULL,
    MetodoPago VARCHAR(50) NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Aprobado' NOT NULL,
    FOREIGN KEY (IdRecibo) REFERENCES Recibos(IdRecibo)
);
GO

--Reservas
CREATE TABLE Reservas (
    IdReserva INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdAreaComun INT NOT NULL,
    FechaReserva DATE NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Confirmado' NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (IdAreaComun) REFERENCES AreasComunes(IdAreaComun)
);
GO

--Incidencias
CREATE TABLE Incidencias (
    IdIncidencia INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Titulo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
    FechaReporte DATETIME DEFAULT GETDATE() NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Abierto' NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO

CREATE TABLE Comunicados (
    IdComunicado INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(200) NOT NULL,
    Contenido VARCHAR(MAX) NOT NULL,
    FechaPublicacion DATETIME DEFAULT GETDATE() NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Activo' NOT NULL
);

-- =======================================================
-- DATOS DE PRUEBA
-- =======================================================

-- Roles
INSERT INTO Roles (NombreRol) VALUES ('Administrador');  -- IdRol = 1
INSERT INTO Roles (NombreRol) VALUES ('Residente');       -- IdRol = 2
INSERT INTO Roles (NombreRol) VALUES ('Propietario');     -- IdRol = 3
GO

-- Condominios
INSERT INTO Condominios (NombreCondominio, Direccion, Estado) 
VALUES ('Condominio Bella Vista', 'Av. El Sol 123, Lima', 1);
GO

-- =======================================================
-- USUARIOS: Administrador + Propietario de prueba
-- =======================================================

-- Usuario Administrador (IdRol=1)
INSERT INTO Usuarios (IdRol, IdCondominio, Nombre, Apellido, DNI, Telefono, Correo, PasswordHash, Estado)
VALUES (1, 1, 'Administrador', 'DCES', '12345678', '987654321', 'admin@condominio.com', 'admin123', 1);

-- Usuario Propietario (IdRol=3) - Puede ver pagos, gestionar reservas, ingresar incidencias
INSERT INTO Usuarios (IdRol, IdCondominio, Nombre, Apellido, DNI, Telefono, Correo, PasswordHash, Estado)
VALUES (3, 1, 'Juan', 'Perez', '87654321', '912345678', 'juan.perez@condominio.com', 'propietario123', 1);
GO

INSERT INTO Propietarios
(Nombre, Apellido, DNI, Telefono, Correo, Torre, NumeroDepartamento, Estado)
VALUES
('Italo', 'Navarro', '87654321', '912345678',
'italo.navarro@condominio.com', 'Torre A', '101', 1);
GO

INSERT INTO Usuarios
(IdRol, IdCondominio, Nombre, Apellido, DNI, Telefono, Correo, PasswordHash, Estado)
VALUES
(3, 1, 'Italo', 'Navarro', '87654321', '912345678',
'italo.navarro@condominio.com', 'propietario123', 1);
GO

-- Torres
INSERT INTO Torres (NombreTorre) VALUES ('Torre A');
INSERT INTO Torres (NombreTorre) VALUES ('Torre B');
INSERT INTO Torres (NombreTorre) VALUES ('Torre C');
GO

-- Departamentos
INSERT INTO Departamentos (IdTorre, Numero, Piso, Estado) VALUES (1, '101', 1, 1);
INSERT INTO Departamentos (IdTorre, Numero, Piso, Estado) VALUES (1, '102', 1, 1);
INSERT INTO Departamentos (IdTorre, Numero, Piso, Estado) VALUES (2, '201', 2, 1);
INSERT INTO Departamentos (IdTorre, Numero, Piso, Estado) VALUES (3, '301', 3, 1);
GO

-- Propietarios
INSERT INTO Propietarios (Nombre, Apellido, DNI, Telefono, Correo, Torre, NumeroDepartamento, Estado)
VALUES ('Juan', 'Perez', '87654321', '912345678', 'juan.perez@email.com', 'Torre A', '101', 1);
INSERT INTO Propietarios (Nombre, Apellido, DNI, Telefono, Correo, Torre, NumeroDepartamento, Estado)
VALUES ('Maria', 'Lopez', '45678912', '934567890', 'maria.lopez@email.com', 'Torre B', '201', 1);
GO

-- Áreas Comunes
INSERT INTO AreasComunes (NombreArea, Descripcion, Capacidad, Estado)
VALUES ('Piscina Principal', 'Piscina temperada para residentes', 20, 1);
INSERT INTO AreasComunes (NombreArea, Descripcion, Capacidad, Estado)
VALUES ('Salón de Eventos', 'Salón multiusos en el primer piso', 50, 1);
INSERT INTO AreasComunes (NombreArea, Descripcion, Capacidad, Estado)
VALUES ('Gimnasio', 'Gimnasio equipado disponible 24/7', 15, 1);
INSERT INTO AreasComunes (NombreArea, Descripcion, Capacidad, Estado)
VALUES ('Terraza BBQ', 'Área de parrilla en la azotea', 30, 1);
GO

-- Recibos (varios departamentos)
INSERT INTO Recibos (IdDepartamento, Concepto, Monto, FechaEmision, FechaVencimiento, Estado)
VALUES (1, 'Mantenimiento Junio 2026', 150.00, '2026-06-01', '2026-06-15', 'Pendiente');

INSERT INTO Recibos (IdDepartamento, Concepto, Monto, FechaEmision, FechaVencimiento, Estado)
VALUES (2, 'Mantenimiento Junio 2026', 150.00, '2026-06-01', '2026-06-15', 'Pendiente');

INSERT INTO Recibos (IdDepartamento, Concepto, Monto, FechaEmision, FechaVencimiento, Estado)
VALUES (3, 'Mantenimiento Junio 2026', 180.00, '2026-06-01', '2026-06-15', 'Pendiente');

INSERT INTO Recibos (IdDepartamento, Concepto, Monto, FechaEmision, FechaVencimiento, Estado)
VALUES (1, 'Agua Mayo 2026', 45.00, '2026-05-01', '2026-05-20', 'Pagado');
GO

-- Pago de prueba (sobre el recibo de agua ya pagado)
INSERT INTO Pagos (IdRecibo, MontoPagado, FechaPago, MetodoPago, Estado)
VALUES (4, 45.00, GETDATE(), 'Transferencia', 'Aprobado');
GO

-- Reservas de prueba
-- Reserva del Administrador (IdUsuario=1)
INSERT INTO Reservas (IdUsuario, IdAreaComun, FechaReserva, HoraInicio, HoraFin, Estado)
VALUES (1, 2, '2026-06-15', '18:00:00', '22:00:00', 'Confirmado');

-- Reserva del Propietario (IdUsuario=2)
INSERT INTO Reservas (IdUsuario, IdAreaComun, FechaReserva, HoraInicio, HoraFin, Estado)
VALUES (2, 1, '2026-06-20', '09:00:00', '12:00:00', 'Confirmado');
GO

-- Incidencia de prueba del Propietario
INSERT INTO Incidencias (IdUsuario, Titulo, Descripcion, FechaReporte, Estado)
VALUES (1, 'Foco quemado en pasadizo', 'El foco del pasillo del piso 2 de la Torre A está quemado.', GETDATE(), 'Abierto');

INSERT INTO Incidencias (IdUsuario, Titulo, Descripcion, FechaReporte, Estado)
VALUES (2, 'Goteras en el techo', 'Hay goteras en el techo del departamento 101 Torre A, urge revisión.', GETDATE(), 'Abierto');
GO

SELECT 'Base de datos DB_CONDOMINIO_DCES creada exitosamente' AS Resultado;
SELECT u.Nombre, u.Correo, r.NombreRol FROM Usuarios u INNER JOIN Roles r ON u.IdRol = r.IdRol;
GO


SELECT * FROM Propietarios;

USE DB_CONDOMINIO_DCES;
GO

SELECT *
FROM Usuarios
WHERE Correo = 'italo.navarro@condominio.com';

SELECT IdUsuario,
       Nombre,
       Apellido,
       Correo,
       PasswordHash
FROM Usuarios;