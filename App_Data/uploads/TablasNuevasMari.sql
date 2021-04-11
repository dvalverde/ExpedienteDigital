/****** Object:  Table [dbo].[ObtencionVegetal]    Script Date: 11/12/2020 5:25:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObtencionVegetal](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ObtencionVegetal] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DocumentoObtencionVegetal]    Script Date: 11/12/2020 5:25:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentoObtencionVegetal](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[FileName] [varchar](max) NULL,
	[FileStreamCol] [varbinary](max) FILESTREAM  DEFAULT 0x,
	[id_obtencion_vegetal] [int] NOT NULL,
 CONSTRAINT [PK_DocumentoObtencionVegetal] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] FILESTREAM_ON [AtestadosDeProfesores],
UNIQUE NONCLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] FILESTREAM_ON [AtestadosDeProfesores]
GO

/****** Object:  Table [dbo].[PersonaXObtencionVegetal]    Script Date: 11/12/2020 5:25:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonaXObtencionVegetal](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[id_persona] [int] NOT NULL,
	[id_obtencion_vegetal] [int] NOT NULL,
 CONSTRAINT [PK_PersonaXObtencionVegetal] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DocumentoObtencionVegetal]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoObtencionVegetal_ObtencionVegetal] FOREIGN KEY([id_obtencion_vegetal])
REFERENCES [dbo].[ObtencionVegetal] ([ID])
GO
ALTER TABLE [dbo].[DocumentoObtencionVegetal] CHECK CONSTRAINT [FK_DocumentoObtencionVegetal_ObtencionVegetal]
GO

ALTER TABLE [dbo].[PersonaXObtencionVegetal]  WITH CHECK ADD  CONSTRAINT [FK_PersonaXObtencionVegetal_ObtencionVegetal] FOREIGN KEY([id_obtencion_vegetal])
REFERENCES [dbo].[ObtencionVegetal] ([ID])
GO
ALTER TABLE [dbo].[PersonaXObtencionVegetal] CHECK CONSTRAINT [FK_PersonaXObtencionVegetal_ObtencionVegetal]
GO
ALTER TABLE [dbo].[PersonaXObtencionVegetal]  WITH CHECK ADD  CONSTRAINT [FK_PersonaXObtencionVegetal_Persona] FOREIGN KEY([id_persona])
REFERENCES [dbo].[Persona] ([ID])
GO
ALTER TABLE [dbo].[PersonaXObtencionVegetal] CHECK CONSTRAINT [FK_PersonaXObtencionVegetal_Persona]
GO

/****** Object:  Table [dbo].[Designacion]    Script Date: 11/12/2020 5:25:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designacion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Designacion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DocumentoDesignacion]    Script Date: 11/12/2020 5:25:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentoDesignacion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[FileName] [varchar](max) NULL,
	[FileStreamCol] [varbinary](max) FILESTREAM  DEFAULT 0x,
	[id_designacion] [int] NOT NULL,
 CONSTRAINT [PK_DocumentoDesignacion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] FILESTREAM_ON [AtestadosDeProfesores],
UNIQUE NONCLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] FILESTREAM_ON [AtestadosDeProfesores]
GO

/****** Object:  Table [dbo].[PersonaXDesignacion]    Script Date: 11/12/2020 5:25:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonaXDesignacion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[id_persona] [int] NOT NULL,
	[id_designacion] [int] NOT NULL,
 CONSTRAINT [PK_PersonaXDesignacion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DocumentoDesignacion]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoDesignacion_Designacion] FOREIGN KEY([id_designacion])
REFERENCES [dbo].[Designacion] ([ID])
GO
ALTER TABLE [dbo].[DocumentoDesignacion] CHECK CONSTRAINT [FK_DocumentoDesignacion_Designacion]
GO

ALTER TABLE [dbo].[PersonaXDesignacion]  WITH CHECK ADD  CONSTRAINT [FK_PersonaXDesignacion_Designacion] FOREIGN KEY([id_designacion])
REFERENCES [dbo].[Designacion] ([ID])
GO
ALTER TABLE [dbo].[PersonaXDesignacion] CHECK CONSTRAINT [FK_PersonaXDesignacion_Designacion]
GO
ALTER TABLE [dbo].[PersonaXDesignacion]  WITH CHECK ADD  CONSTRAINT [FK_PersonaXDesignacion_Persona] FOREIGN KEY([id_persona])
REFERENCES [dbo].[Persona] ([ID])
GO
ALTER TABLE [dbo].[PersonaXDesignacion] CHECK CONSTRAINT [FK_PersonaXDesignacion_Persona]
GO