using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

#region TABLA
/*
USE [ORCAFIT]
GO

DROP TABLE[dbo].[COMENTARIOS]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMENTARIOS](

[USER_NO][int] NOT NULL,

[USERNAME] [nvarchar](100) NOT NULL,

[RUTINA_NO] [int] NOT NULL,

[COMENTARIO_NO] [int] NOT NULL,

[COMENTARIO_TEXTO] [nvarchar](1000) NOT NULL,

[FECHA] [date] NOT NULL,
CONSTRAINT[PK_COMENTARIOS] PRIMARY KEY CLUSTERED 
(

    [COMENTARIO_NO] ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO
*/
#endregion

namespace orcafit.Models
{
    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("USER_NO")]
        public int IdUser { get; set; }
        [Column("USERNAME")]
        public string Username { get; set; }
        [Column("PASSWORD")]
        public string Password { get; set; }
        [Column("ROLE")]
        public string Role { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
    }
}
