// using ACore.Server.Modules.AuditModule.Configuration;
// using ACore.Server.Modules.AuditModule.Extensions;
// using ACore.Server.Modules.SettingModule.Storage.SQL.Models;
// using ACore.Server.Storages.Models;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
//
// namespace ACore.Server.Storages.EF.ChangeTracker;
//
// public abstract class AuditChangeTrackerDbContext: DbContextBase
// {
//   private readonly IAuditConfiguration? _auditConfiguration;
//   
//   public abstract override StorageTypeDefinition StorageDefinition { get; }
//   
//   /// <summary>
//   /// This base class implements audit functionality and calls db <see cref="SaveChangesAsync"/> method.
//   /// Use <see cref="SaveChanges"/> only in special cases.
//   /// </summary>
//   protected AuditChangeTrackerDbContext(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger, IAuditConfiguration? auditConfiguration = null) 
//     : base(options, mediator, logger)
//   {
//     _auditConfiguration = auditConfiguration;
//     
//   }
//   
//   public DbSet<SettingEntity> Settings { get; set; }
//
//
//   #region Audit
//
//   public override int SaveChanges(bool acceptAllChangesOnSuccess)
//   {
//     Logger.LogError($"Don't use {nameof(SaveChanges)} in EF. Use {nameof(SaveChangesAsync)}.");
//     if (!IsAuditEnabledAsync().Result)
//     {
//       return SaveChangesAsync(CancellationToken.None).Result;
//     }
//
//     var entityAudits = OnBeforeSaveChangesAsync(ChangeTracker).Result;
//     var result = base.SaveChanges(acceptAllChangesOnSuccess);
//     OnAfterSaveChangesAsync(entityAudits).Wait();
//     return result;
//   }
//
//   public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
//   {
//     if (!await IsAuditEnabledAsync())
//       return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
//
//     var entityAudits = await OnBeforeSaveChangesAsync(ChangeTracker);
//     var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
//     await OnAfterSaveChangesAsync(entityAudits);
//
//     return result;
//   }
//
//
//   private async Task<IEnumerable<AuditEntry>> OnBeforeSaveChangesAsync(Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker changeTracker)
//   {
//     changeTracker.DetectChanges();
//     var auditEntries = (from entry in changeTracker.Entries()
//       where entry.GetType().IsAuditable(_auditConfiguration.AuditEntities)
//       select new AuditEntry(entry, _auditConfiguration, StorageDefinition)).ToList();
//
//     await BeginTrackingAuditEntriesAsync(auditEntries.Where(e => !e.HasTemporaryProperties));
//
//     // keep a list of entries where the value of some properties are unknown at this step
//     return auditEntries.Where(e => e.HasTemporaryProperties);
//   }
//
//   private async Task OnAfterSaveChangesAsync(IEnumerable<AuditEntry> entityAudits)
//   {
//     var auditEntries = entityAudits as AuditEntry[] ?? entityAudits.ToArray();
//
//     if (!auditEntries.Any())
//       return;
//
//     await BeginTrackingAuditEntriesAsync(auditEntries);
//     // foreach (var auditEntry in auditEntries)
//     // {
//     //     await _auditModuleEfContext.SaveAuditAsync(auditEntry);
//     // }
//
//   }
//   
//   private async Task BeginTrackingAuditEntriesAsync(IEnumerable<AuditEntry> auditEntries)
//   {
//     foreach (var auditEntry in auditEntries)
//     {
//       auditEntry.Update();
//       await SaveAuditAsync(auditEntry);
//     }
//   }
//
//   private async Task SaveAuditAsync(AuditEntry auditEntry)
//   {
//    // await Mediator.Send(new AuditSaveCommand(auditEntry));
//   }
//
//   
//
//
//   #endregion
//
//
// }