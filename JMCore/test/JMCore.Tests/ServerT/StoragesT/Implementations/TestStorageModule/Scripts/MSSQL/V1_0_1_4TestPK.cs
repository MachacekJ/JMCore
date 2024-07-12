// using JMCore.Server.DB.Abstract;
//
// // ReSharper disable InconsistentNaming
//
// namespace JMCore.Tests.ServerT.DbT.TestDBContext.Scripts;
//
// public class V1_0_1_4TestPK : DbVersionScriptsBase
// {
//     public override Version Version => new("1.0.0.4");
//
//     public override List<string> AllScripts
//     {
//         get
//         {
//             List<string> l = new()
//             {
//                 @"
// CREATE TABLE [dbo].[TestPKGuid](
//     [Id] [uniqueidentifier],
//     [Name] [nvarchar](20)
//     CONSTRAINT [PK_TestPKGuid] PRIMARY KEY CLUSTERED ([Id] ASC)
// )
// GO
// CREATE TABLE [dbo].[TestPKString](
//     [Id] [nvarchar](50),
//     [Name] [nvarchar](20)
//     CONSTRAINT [PK_TestPKString] PRIMARY KEY CLUSTERED ([Id] ASC)
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