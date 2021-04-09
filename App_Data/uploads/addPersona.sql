USE [ExpedienteDigital]
GO

INSERT INTO [dbo].[Persona]
           ([cedula]
           ,[nombre]
           ,[apellido1]
           ,[apellido2]
           ,[username]
           ,[password]
           ,[categoria]
           ,[rol]
           ,[telefono]
           ,[celular]
           ,[puesto]
           ,[correoElectronico]
           ,[departamento])
     VALUES
           ('115400482'
           ,'Mariela'
           ,'Bolanos'
           ,'Delgado'
           ,'mariela@gmail.com'
           ,'1234'
           ,'hola'
           ,1
           ,'88588055'
           ,'22368486'
           ,'Gerente'
           ,'mariela2@gmail.com'
           ,'hola2')
GO