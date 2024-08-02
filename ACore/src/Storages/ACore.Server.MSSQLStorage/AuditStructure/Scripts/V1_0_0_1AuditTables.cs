using ACore.Server.Storages.Base.EF;

namespace ACore.Server.MSSQLStorage.AuditStructure.Scripts
{
    // ReSharper disable once InconsistentNaming
    public class V1_0_0_1AuditTables : DbVersionScriptsBase
    {
        public override Version Version => new("1.0.0.1");

        public override List<string> AllScripts =>
            new()
            {
                @"
CREATE TABLE [dbo].[Audit] (
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [AuditTableId]       INT           NOT NULL,
    [PKValue]            BIGINT        NULL,
    [PKValueString]      NVARCHAR(450)  NULL,
    [AuditUserId]        INT           NULL,
    [DateTime]           DATETIME2 (7) NOT NULL,
    [EntityState]        INT       NOT NULL,
    CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE TABLE [dbo].[AuditColumn] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [AuditTableId]  INT   NOT NULL,
    [ColumnName]    VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_AuditColumn] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE TABLE [dbo].[AuditTable] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [TableName]  VARCHAR (255) NOT NULL,
    [SchemaName] VARCHAR (255) NULL,
    CONSTRAINT [PK_AuditTable] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[AuditUser] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserId]   NVARCHAR (450) NOT NULL,
    [UserName] NVARCHAR (256) NULL,
    CONSTRAINT [PK_AuditUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[AuditValue] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [AuditId]        BIGINT         NOT NULL,
    [AuditColumnId]  INT            NOT NULL,
    [OldValueString] NVARCHAR (MAX) NULL,
    [NewValueString] NVARCHAR (MAX) NULL,
    [OldValueInt]    INT            NULL,
    [NewValueInt]    INT            NULL,
    [OldValueLong]   BIGINT         NULL,
    [NewValueLong]   BIGINT         NULL,
    [OldValueBool]   BIT            NULL,
    [NewValueBool]   BIT            NULL,
    [OldValueGuid]   uniqueidentifier   NULL,
    [NewValueGuid]   uniqueidentifier   NULL
    CONSTRAINT [PK_AuditValue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
ALTER TABLE [dbo].[Audit]
    ADD CONSTRAINT [FK_Audit_AuditUser] FOREIGN KEY ([AuditUserId]) REFERENCES [dbo].[AuditUser] ([Id]);

GO
ALTER TABLE [dbo].[Audit]
    ADD CONSTRAINT [FK_Audit_AuditTable] FOREIGN KEY ([AuditTableId]) REFERENCES [dbo].[AuditTable] ([Id]);

GO
ALTER TABLE [dbo].[AuditValue]
    ADD CONSTRAINT [FK_AuditValue_AuditColumn] FOREIGN KEY ([AuditColumnId]) REFERENCES [dbo].[AuditColumn] ([Id]);

GO
ALTER TABLE [dbo].[AuditValue]
    ADD CONSTRAINT [FK_AuditValue_Audit] FOREIGN KEY ([AuditId]) REFERENCES [dbo].[Audit] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;

GO

ALTER TABLE [dbo].[AuditColumn]
    ADD CONSTRAINT [FK_AuditColumn_AuditTable] FOREIGN KEY ([AuditTableId]) REFERENCES [dbo].[AuditTable] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;
GO

CREATE UNIQUE INDEX [IX_AuditUser_UserId] ON [dbo].[AuditUser]
(
	[UserId] ASC
)
GO

CREATE UNIQUE INDEX [IX_AuditTable_NameSchema] ON [dbo].[AuditTable]
(
	[TableName] ASC,
	[SchemaName] ASC
)
GO

CREATE UNIQUE INDEX [IX_AuditColumn_AuditTableIdName] ON [dbo].[AuditColumn]
(
    [AuditTableId] ASC,
	[ColumnName] ASC
)
GO

ALTER TABLE [dbo].[Audit] WITH CHECK CHECK CONSTRAINT [FK_Audit_AuditUser];

ALTER TABLE [dbo].[Audit] WITH CHECK CHECK CONSTRAINT [FK_Audit_AuditTable];

ALTER TABLE [dbo].[AuditValue] WITH CHECK CHECK CONSTRAINT [FK_AuditValue_AuditColumn];

ALTER TABLE [dbo].[AuditValue] WITH CHECK CHECK CONSTRAINT [FK_AuditValue_Audit];

GO

CREATE VIEW [dbo].[vw_Audit]
AS
SELECT av.Id,
       att.TableName,
       a.PKValue,
       a.PKValueString,
	   au.UserName,
	   a.EntityState,
	   a.DateTime,
	   ac.ColumnName,
	   av.OldValueString,
	   av.NewValueString, 
       av.OldValueInt,
	   av.NewValueInt,
       av.[OldValueLong],
       av.[NewValueLong],
	   av.OldValueBool,
	   av.NewValueBool,
	   av.OldValueGuid,
	   av.NewValueGuid
FROM   dbo.Audit a INNER JOIN
       dbo.AuditTable att ON a.AuditTableId = att.Id INNER JOIN
       dbo.AuditUser au ON a.AuditUserId = au.Id INNER JOIN
       dbo.AuditValue av ON a.Id = av.AuditId INNER JOIN
       dbo.AuditColumn ac ON av.AuditColumnId = ac.Id
GO

"
            };
    }
}

