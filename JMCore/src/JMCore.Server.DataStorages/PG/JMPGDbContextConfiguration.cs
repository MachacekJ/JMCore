// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace JMCore.Server.DataStorages.PG;
//
// public class JMPGDbContextConfiguration(IServiceCollection services, string cn) : JMDbContextConfiguration(cn)
// {
//   public void AddJMPostgresDbContext<TInterface, TDbContext>()
//     where TDbContext : DbContext, TInterface
//     where TInterface : class
//   {
//     // _serviceCollection.AddDbContext<TDbContext>(opt => opt.UseSqlServer(ConnectionString));
//     services.AddDbContext<TDbContext>(opt => opt.UseNpgsql(ConnectionString));
//    // AddDbScoped<TInterface, TDbContext>();
//   }
// }