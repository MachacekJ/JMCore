// using ACore.Server.DB.Abstract;
//
// namespace ACore.Tests.ServerT.DbT.TestDBContext.Scripts;
//
// // ReSharper disable once InconsistentNaming
// public class V1_0_1_1TestTable : DbVersionScriptsBase
// {
//     public override Version Version => new ("1.0.0.1");
//
//     public override List<string> AllScripts
//     {
//         get
//         {
//
//             List<string> l = new()
//             {
//                 @"
// CREATE TABLE [dbo].[Test](
//     [Id]                 [int] NOT NULL IDENTITY(1,1),
//     [Name]               [nvarchar](50) NULL,
//     [Created]            [datetime2] NULL
//     CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED ([Id] ASC)
// )
// GO"
//             };
//
//
//             return l;
//         }
//     }
// }