// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace JMCore.Server.DataStorages.Memory.BasicStructure;
//
// public class JMMemoryDbContextConfiguration(IServiceCollection services, string cn) : JMDbContextConfiguration(cn)
// {
//   public void AddJMMemoryDbContext<TInterface, TDbContext>()
//     where TDbContext : DbContext, TInterface
//     where TInterface : class
//   {
//     // _serviceCollection.AddDbContext<TDbContext>(opt => opt.UseSqlServer(ConnectionString));
//     services.AddDbContext<TDbContext>(opt => opt.UseInMemoryDatabase(ConnectionString));
//    // AddDbScoped<TInterface, TDbContext>();
//   }
// }