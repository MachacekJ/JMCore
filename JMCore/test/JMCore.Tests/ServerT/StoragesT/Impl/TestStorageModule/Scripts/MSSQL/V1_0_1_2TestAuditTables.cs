// using JMCore.Server.DB.Abstract;
//
// // ReSharper disable InconsistentNaming
//
// namespace JMCore.Tests.ServerT.DbT.TestDBContext.Scripts;
//
// public class V1_0_1_2TestAuditTables : DbVersionScriptsBase
// {
//     public override Version Version => new("1.0.0.2");
//
//     public override List<string> AllScripts
//     {
//         get
//         {
//             List<string> l = new()
//             {
//                 @"
// CREATE TABLE [dbo].[TestAttributeAudit](
//     [Id]                 [int] NOT NULL IDENTITY(1,1),
//     [Name]               [nvarchar](50) NULL,
//     [NotAuditableColumn] [nvarchar](50) NULL,
//     [Created]            [datetime2] NULL
//     CONSTRAINT [PK_TestAttributeAudit] PRIMARY KEY CLUSTERED ([Id] ASC)
// )
// GO
//
// CREATE TABLE [dbo].[TestManualAudit](
//     [Id]                 [int] NOT NULL IDENTITY(1,1),
//     [Name]               [nvarchar](50) NULL,
//     [NotAuditableColumn] [nvarchar](50) NULL,
//     [Created]            [datetime2] NULL
//     CONSTRAINT [PK_TestManualAudit] PRIMARY KEY CLUSTERED ([Id] ASC)
// )
// GO"
//             };
//
//
//             return l;
//         }
//     }
// }