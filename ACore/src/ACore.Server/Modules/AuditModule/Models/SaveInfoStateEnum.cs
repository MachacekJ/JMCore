namespace ACore.Server.Modules.AuditModule.Models;

public enum AuditInfoStateEnum
{
  // Detached,
  // /// <summary>
  // ///     The entity is being tracked by the context and exists in the database. Its property
  // ///     values have not changed from the values in the database.
  // /// </summary>
  // Unchanged,
  /// <summary>
  ///     The entity is being tracked by the context and exists in the database. It has been marked
  ///     for deletion from the database.
  /// </summary>
  Deleted,
  /// <summary>
  ///     The entity is being tracked by the context and exists in the database. Some or all of its
  ///     property values have been modified.
  /// </summary>
  Modified,
  /// <summary>
  ///     The entity is being tracked by the context but does not yet exist in the database.
  /// </summary>
  Added,
}