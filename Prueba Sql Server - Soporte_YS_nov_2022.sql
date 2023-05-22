-- Crear tabla PRODUCTOS
CREATE TABLE PRODUCTOS (
  CODIGO_PRODUCTO INT PRIMARY KEY NOT NULL,
  DESC_PRODUCTO VARCHAR(100)
);

-- Crear tabla DEPARTAMENTO
CREATE TABLE DEPARTAMENTO (
  CÓDIGO_DPTO INT PRIMARY KEY NOT NULL,
  DESCRIPCIÓN_DPTO VARCHAR(100)
);

-- Crear tabla CIUDAD
CREATE TABLE CIUDAD (
  COD_CIUDAD INT PRIMARY KEY NOT NULL,
  CODIGO_DPTO INT NOT NULL,
  DESCRIPCION_CIUDAD VARCHAR(100),
  FOREIGN KEY (CODIGO_DPTO) REFERENCES DEPARTAMENTO (CÓDIGO_DPTO)
);

-- Crear tabla USUARIOS
CREATE TABLE USUARIOS (
  CODIGO_USUARIO INT PRIMARY KEY NOT NULL,
  USUARIO VARCHAR(50),
  CLAVE VARCHAR(50)
);

-- Crear tabla CLIENTES
CREATE TABLE CLIENTES (
  ID_CLIENTE INT PRIMARY KEY NOT NULL,
  TIPO_IDENTIFICACION VARCHAR(50),
  NUMERO_IDENTIFICACION VARCHAR(50),
  NOMBRES VARCHAR(100),
  APELLIDOS VARCHAR(100),
  CARGO VARCHAR(50),
  MAIL VARCHAR(100),
  DIRECCION VARCHAR(200),
  TELEFONO VARCHAR(20),
  CODIGO_CIUDAD INT NOT NULL,
  CODIGO_USUARIO INT,
  FOREIGN KEY (CODIGO_CIUDAD) REFERENCES CIUDAD (COD_CIUDAD),
  FOREIGN KEY (CODIGO_USUARIO) REFERENCES USUARIOS (CODIGO_USUARIO)
);

-- Crear tabla VENTAS
CREATE TABLE VENTAS (
  ID_CLIENTE INT PRIMARY KEY NOT NULL,
  COD_CIUDAD INT NOT NULL,
  CODIGO_PRODUCTO INT NOT NULL,
  FECHA_VENTA DATE,
  VALOR_VENTA FLOAT,
  CANTIDAD_PRODUCTO INT,
  FOREIGN KEY (ID_CLIENTE) REFERENCES CLIENTES (ID_CLIENTE),
  FOREIGN KEY (COD_CIUDAD) REFERENCES CIUDAD (COD_CIUDAD),
  FOREIGN KEY (CODIGO_PRODUCTO) REFERENCES PRODUCTOS (CODIGO_PRODUCTO)
);

--INSERT TABLA PRODUCTOS--
INSERT INTO PRODUCTOS (CODIGO_PRODUCTO, DESC_PRODUCTO)
VALUES (1, 'Producto 1'),
       (2, 'Producto 2'),
       (3, 'Producto 3'),
       (4, 'Producto 4'),
       (5, 'Producto 5'),
       (6, 'Producto 6'),
       (7, 'Producto 7'),
       (8, 'Producto 8'),
       (9, 'Producto 9'),
       (10, 'Producto 10');

--INSERT DEPARTAMENTO--
INSERT INTO DEPARTAMENTO (CÓDIGO_DPTO, DESCRIPCIÓN_DPTO)
VALUES (1, 'Departamento 1'),
       (2, 'Departamento 2'),
       (3, 'Departamento 3'),
       (4, 'Departamento 4'),
       (5, 'Departamento 5'),
       (6, 'Departamento 6'),
       (7, 'Departamento 7'),
       (8, 'Departamento 8'),
       (9, 'Departamento 9'),
       (10, 'Departamento 10');

--INSERT TABLA CIUDAD--

INSERT INTO CIUDAD (COD_CIUDAD, CODIGO_DPTO, DESCRIPCION_CIUDAD)
VALUES (1, 1, 'Ciudad 1'),
       (2, 2, 'Ciudad 2'),
       (3, 3, 'Ciudad 3'),
       (4, 4, 'Ciudad 4'),
       (5, 5, 'Ciudad 5'),
       (6, 6, 'Ciudad 6'),
       (7, 7, 'Ciudad 7'),
       (8, 8, 'Ciudad 8'),
       (9, 9, 'Ciudad 9'),
       (10, 10, 'Ciudad 10');

--INSERT TABLA USUARIOS--
INSERT INTO USUARIOS (CODIGO_USUARIO, USUARIO, CLAVE)
VALUES (1, 'Usuario1', 'Clave1'),
       (2, 'Usuario2', 'Clave2'),
       (3, 'Usuario3', 'Clave3'),
       (4, 'Usuario4', 'Clave4'),
       (5, 'Usuario5', 'Clave5'),
       (6, 'Usuario6', 'Clave6'),
       (7, 'Usuario7', 'Clave7'),
       (8, 'Usuario8', 'Clave8'),
       (9, 'Usuario9', 'Clave9'),
       (10, 'Usuario10', 'Clave10');

--INSERT TABLA CLIENTES--
INSERT INTO CLIENTES (ID_CLIENTE, TIPO_IDENTIFICACION, NUMERO_IDENTIFICACION, NOMBRES, APELLIDOS, CARGO, MAIL, DIRECCION, TELEFONO, CODIGO_CIUDAD, CODIGO_USUARIO)
VALUES (1, 'Tipo1', '1234567890', 'Nombres 1', 'Apellidos 1', 'Cargo 1', 'correo1@ejemplo.com', 'Dirección 1', '1234567890', 1,1),
       (2, 'Tipo2', '2345678901', 'Nombres 2', 'Apellidos 2', 'Cargo 2', 'correo2@ejemplo.com', 'Dirección 2', '2345678901', 2,2),
       (3, 'Tipo3', '3456789012', 'Nombres 3', 'Apellidos 3', 'Cargo 3', 'correo3@ejemplo.com', 'Dirección 3', '3456789012', 3,3),
       (4, 'Tipo4', '4567890123', 'Nombres 4', 'Apellidos 4', 'Cargo 4', 'correo4@ejemplo.com', 'Dirección 4', '4567890123', 4,4),
       (5, 'Tipo5', '5678901234', 'Nombres 5', 'Apellidos 5', 'Cargo 5', 'correo5@ejemplo.com', 'Dirección 5', '5678901234', 5,5),
       (6, 'Tipo6', '6789012345', 'Nombres 6', 'Apellidos 6', 'Cargo 6', 'correo6@ejemplo.com', 'Dirección 6', '6789012345', 6,6),
       (7, 'Tipo7', '7890123456', 'Nombres 7', 'Apellidos 7', 'Cargo 7', 'correo7@ejemplo.com', 'Dirección 7', '7890123456', 7,7),
       (8, 'Tipo8', '8901234567', 'Nombres 8', 'Apellidos 8', 'Cargo 8', 'correo8@ejemplo.com', 'Dirección 8', '8901234567', 8,8),
       (9, 'Tipo9', '9012345678', 'Nombres 9', 'Apellidos 9', 'Cargo 9', 'correo9@ejemplo.com', 'Dirección 9', '9012345678', 9,9),
       (10, 'CEDULA DE CIUDADANIA', '1234567890', 'Oscar', 'Castelblanco', 'desarrollador', 'oscard.castelblanco@ejemplo.com', 'Calle 87 #94L-09', '3102902233', 10,10);


--INSERT TABLA VENTAS--

INSERT INTO VENTAS (ID_CLIENTE, COD_CIUDAD, CODIGO_PRODUCTO, FECHA_VENTA, VALOR_VENTA, CANTIDAD_PRODUCTO)
VALUES (1, 1, 1, '2010-10-01', 100000, 5),
       (2, 2, 2, '2010-10-24', 200000, 10),
       (3, 3, 3, '2010-11-15', 300000, 8),
       (4, 4, 4, '2010-12-30', 400000, 12),
       (5, 5, 5, '2023-05-05', 555112, 3),
       (6, 6, 6, '2023-06-06', 67911, 7),
       (7, 7, 7, '2023-07-07', 45661500.00, 2),
       (8, 8, 8, '2023-08-08', 456561, 9),
       (9, 9, 9, '2023-09-09', 912121, 6),
       (10, 10, 10, '2023-10-10', 14884351515, 4);

delete from VENTAS


--CONSULTA PUNTO B--

SELECT c.ID_CLIENTE,
  c.NOMBRES AS NOMBRE,
  c.APELLIDOS AS APELLIDO,
  c.DIRECCION,
  ci.DESCRIPCION_CIUDAD AS DESCRIPCION_DE_LA_CIUDAD,
  u.USUARIO AS NOMBRE_DEL_USUARIO,
  c.NUMERO_IDENTIFICACION
  FROM CLIENTES c
INNER JOIN CIUDAD ci ON c.CODIGO_CIUDAD = ci.COD_CIUDAD
LEFT JOIN USUARIOS u ON c.CODIGO_USUARIO = u.CODIGO_USUARIO
WHERE c.TIPO_IDENTIFICACION = 'CEDULA DE CIUDADANIA' AND c.NUMERO_IDENTIFICACION = '1234567890'

--CONSULTA PUNTO C--
SELECT COUNT(DISTINCT v.ID_CLIENTE) AS CANTIDAD_CLIENTES
FROM VENTAS v
WHERE v.VALOR_VENTA > 30000000;

--PUNTO D--

SELECT
  c.ID_CLIENTE,
  c.NOMBRES AS NOMBRE_CLIENTE,
  c.APELLIDOS AS APELLIDO_CLIENTE,
  v.FECHA_VENTA,
  p.DESC_PRODUCTO AS DESCRIPCION_DEL_PRODUCTO,
  v.VALOR_VENTA
FROM
  CLIENTES c
  INNER JOIN VENTAS v ON c.ID_CLIENTE = v.ID_CLIENTE
  INNER JOIN PRODUCTOS p ON v.CODIGO_PRODUCTO = p.CODIGO_PRODUCTO
WHERE
  v.FECHA_VENTA BETWEEN '2010-10-01' AND '2010-12-31';

  -- Agregar columna "CLASE" a la tabla CLIENTES
ALTER TABLE CLIENTES
ADD CLASE VARCHAR(10);

-- Actualizar la columna "CLASE" según las ventas
UPDATE CLIENTES
SET CLASE = CASE
    WHEN ID_CLIENTE IN (
        SELECT ID_CLIENTE FROM VENTAS WHERE VALOR_VENTA >= 200000
    ) THEN 'ALTO'
    WHEN ID_CLIENTE IN (
        SELECT ID_CLIENTE FROM VENTAS WHERE VALOR_VENTA >= 100000 AND VALOR_VENTA < 200000
    ) THEN 'MEDIO'
    ELSE 'BAJO'
END;


CREATE PROCEDURE EliminarClienteBajo
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Obtener el cliente "BAJO" a eliminar
    DECLARE @ClienteBajo TABLE (
        ID_CLIENTE INT,
        CODIGO_CIUDAD INT,
        CODIGO_USUARIO INT
    );

    -- Insertar el cliente "BAJO" a eliminar en la tabla temporal
    INSERT INTO @ClienteBajo (ID_CLIENTE, CODIGO_CIUDAD, CODIGO_USUARIO)
    SELECT ID_CLIENTE, CODIGO_CIUDAD, CODIGO_USUARIO
    FROM CLIENTES
    WHERE CLASE = 'BAJO';

    -- Verificar si hay clientes clasificados como "BAJO"
    IF EXISTS (SELECT 1 FROM @ClienteBajo)
    BEGIN
        -- Obtener el siguiente consecutivo para el ID_CLIENTE
        DECLARE @NuevoClienteID INT;
        SELECT @NuevoClienteID = ISNULL(MAX(ID_CLIENTE), 0) + 1
        FROM CLIENTES;

        -- Crear un nuevo cliente con datos vacíos y clase "ELIMINADO"
        INSERT INTO CLIENTES (ID_CLIENTE, TIPO_IDENTIFICACION, NUMERO_IDENTIFICACION, NOMBRES, APELLIDOS, CARGO, MAIL, DIRECCION, TELEFONO, CODIGO_CIUDAD, CODIGO_USUARIO, CLASE)
        SELECT @NuevoClienteID, '', '', '', '', '', '', '', '', CODIGO_CIUDAD, CODIGO_USUARIO, 'ELIMINADO'
        FROM @ClienteBajo;

        -- Actualizar las ventas del cliente "BAJO" por el nuevo cliente creado
        UPDATE VENTAS
        SET ID_CLIENTE = @NuevoClienteID
        WHERE ID_CLIENTE IN (
            SELECT ID_CLIENTE FROM @ClienteBajo
        );

        -- Eliminar el cliente "BAJO"
        DELETE FROM CLIENTES
        WHERE ID_CLIENTE IN (
            SELECT ID_CLIENTE FROM @ClienteBajo
        );
    END;
END;

EXEC EliminarClienteBajo;

SELECT * FROM CLIENTES