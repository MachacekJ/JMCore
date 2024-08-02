// using ACore.Server.DB.Abstract;
//
// // ReSharper disable InconsistentNaming
//
// namespace ACore.Tests.ServerT.DbT.TestDBContext.Scripts;
//
// public class V1_0_1_3TestAuditTypes : DbVersionScriptsBase
// {
//     public override Version Version => new("1.0.0.3");
//
//     public override List<string> AllScripts
//     {
//         get
//         {
//             List<string> l = new()
//             {
//                 @"
// CREATE TABLE [dbo].[TestValueType](
//     [Id]                 [int] NOT NULL IDENTITY(1,1),
//     [IntNotNull]         [int] NOT NULL,
//     [IntNull]            [int] NULL,
//     [BigIntNotNull]      [bigint] NOT NULL,
//     [BigIntNull]         [bigint] NULL,
//     [Bit2]               [bit] NOT NULL,
//     [Char2]              [char](10) NOT NULL,
//     [Date2]              [date] NOT NULL,
//     [DateTime2]          [datetime2] NOT NULL,
//     [Decimal2]           DECIMAL(19,4) NOT NULL,
//     [NChar2]             [nchar](10) NOT NULL,
//     [NVarChar2]          [nvarchar](10) NOT NULL,
//     [SmallDateTime2]     [smalldatetime] NOT NULL,
//     [SmallInt2]          [smallint] NOT NULL,
//     [TinyInt2]           [tinyint] NOT NULL,
//     [Guid2]              [uniqueidentifier] NOT NULL,
//     [VarBinary2]         [varbinary](max) NOT NULL,
//     [VarChar2]           [varchar](100) NOT NULL
//     CONSTRAINT [PK_TestValueType] PRIMARY KEY CLUSTERED ([Id] ASC)
// )
// GO
// "
//             };
//
//
//             return l;
//         }
//     }
// }