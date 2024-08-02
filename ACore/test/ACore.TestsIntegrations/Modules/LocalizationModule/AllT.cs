// using System.Reflection;
// using FluentAssertions;
// using ACore.Localizer;
// using ACore.Server.StorageModules.DB.DbContexts.LocalizeStructure.Models;
// using ACore.Tests.ServerT.LocalizeT;
// using Microsoft.EntityFrameworkCore;
// using Xunit;
//
// namespace ACore.TestsIntegrations.ServerT.DbT.LocalizeStructureT;
//
// /// <summary>
// /// More test is in <see cref="LocalizeTableT"/>
// /// </summary>
// public class AllT : LocalizeStructureBaseT
// {
//     [Fact]
//     public async Task Ok()
//     {
//         var method = MethodBase.GetCurrentMethod();
//         await RunTestAsync(method, async () =>
//         {
//             await Db.SyncResXItemAsync(new LocalizationEntity()
//             {
//                 ContextId = "aa",
//                 Lcid = 1033,
//                 Scope = LocalizationScopeEnum.Client,
//                 Translation = "aa",
//                 MsgId = "key"
//             });
//             var res = await Db.Localizations.SingleAsync(l=>l.MsgId == "key");
//             res.MsgId.Should().Be("key");
//
//             await Db.ChangeTranslationAsync(res.Id, "newTrans");
//             
//             res = await Db.Localizations.SingleAsync(l=>l.MsgId == "key");
//             res.Translation.Should().Be("newTrans");
//         });
//     }
// }