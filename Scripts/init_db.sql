-- =============================================================
-- Script de Inicialización – Base de Datos Inventario
-- =============================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Inventario')
BEGIN
    CREATE DATABASE Inventario;
END
GO

USE Inventario;
GO

-- ── Tabla Producto ────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Producto' AND xtype='U')
BEGIN
    CREATE TABLE Producto (
        Id           INT              PRIMARY KEY IDENTITY(1,1),
        Nombre       NVARCHAR(100)    NOT NULL,
        Descripcion  NVARCHAR(255)    NULL,
        Stock        INT              NOT NULL DEFAULT 0,
        Precio       DECIMAL(18, 2)   NOT NULL,
        FechaCreacion DATETIME        DEFAULT GETDATE()
    );
END
GO

-- ── Tabla Usuario ─────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuario' AND xtype='U')
BEGIN
    CREATE TABLE Usuario (
        Id             INT             PRIMARY KEY IDENTITY(1,1),
        NombreUsuario  NVARCHAR(50)    NOT NULL,
        PasswordHash   NVARCHAR(MAX)   NOT NULL,
        Rol            NVARCHAR(20)    DEFAULT 'User',
        CONSTRAINT UQ_Usuario_NombreUsuario UNIQUE (NombreUsuario)
    );
END
GO

-- ── Datos de prueba (opcional) ────────────────────────────────
-- Nota: la contraseña 'Admin123!' está hasheada con BCrypt
-- Puedes generar tu propio hash usando el endpoint POST /api/auth/register

INSERT INTO Producto (Nombre, Descripcion, Stock, Precio) VALUES
    (N'Laptop Dell XPS 15',   N'Laptop de alto rendimiento con pantalla OLED', 25,  1299.99),
    (N'Mouse Logitech MX',    N'Mouse inalámbrico ergonómico',                  80,    89.90),
    (N'Teclado Mecánico HyperX', N'Teclado con switches Cherry MX Red',         40,   129.99),
    (N'Monitor LG 27"',       N'Monitor 4K IPS con HDR',                        15,   449.00),
    (N'Auriculares Sony WH-1000XM5', N'Cancelación de ruido activa',            30,   349.99);
GO

PRINT 'Base de datos Inventario inicializada correctamente.';
GO
