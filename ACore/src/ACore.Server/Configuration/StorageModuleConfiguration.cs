using ACore.Server.Modules.AuditModule.Configuration;

namespace ACore.Server.Configuration;

public class StorageModuleConfiguration
{
  public bool UseMemoryStorage { get; set; }
  public TestModuleMongoOptions? MongoDb { get; set; }
  public TestModulePGOptions? PGDb { get; set; }
}

public class TestModuleMongoOptions(TestModuleMongoConnectionOptions readWrite, string dbName, TestModuleMongoConnectionOptions? readOnly = null)
{
  public TestModuleMongoConnectionOptions ReadOnly => readOnly ?? readWrite; 
  public TestModuleMongoConnectionOptions ReadWrite => readWrite;
  public string DbName => dbName;
}

public class TestModulePGOptions(string readWriteConnectionString, string? readOnlyConnectionString = null)
{
  public string ReadOnlyConnectionString => readOnlyConnectionString ?? readWriteConnectionString; 
  public string ReadWriteConnectionString => readWriteConnectionString;
}

public class TestModuleMongoConnectionOptions(string connectionString)
{
  public string ConnectionString => connectionString;
}