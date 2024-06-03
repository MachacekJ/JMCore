// using JMCore.Server.Configuration.DB;
// using JMCore.Server.StorageModules.DB.Audit;
// using JMCore.Tests.ServerT.DbT.DbContexts.AuditStructureT;
// using JMCore.Tests.ServerT.DbT.TestDBContext;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace JMCore.TestsIntegrations.ServerT.DbT.AuditStructureT;
//
// public class AuditStructureBaseT : DbBaseT
// {
//     protected AuditDbContext AuditDbContext = null!;
//     protected TestBasicDbContext TestBasicDbContext = null!;
//
//
//     protected override void RegisterServices(ServiceCollection sc)
//     {
//         base.RegisterServices(sc);
//         sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
//         sc.AddJMDb(string.Format(ConnectionString, TestData.TestName), (o) =>
//         {
//             o.AuditStructure = true;
//             o.AddJMDbContext<ITestBasicDbContext, TestBasicDbContext>();
//         });
//     }
//
//     protected override async Task GetServicesAsync(IServiceProvider sp)
//     {
//         await base.GetServicesAsync(sp);
//         await sp.ConfigureJMDbAsync();
//         AuditDbContext = sp.GetService<AuditDbContext>() ?? throw new ArgumentException($"{nameof(AuditDbContext)} is null.");
//         TestBasicDbContext = sp.GetService<TestBasicDbContext>() ?? throw new ArgumentException($"{nameof(TestBasicDbContext)} is null.");
//     }
// }