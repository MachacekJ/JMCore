namespace JMCore.Server.Storages.Modules;

[Flags]
public enum StorageNativeModuleTypeEnum
{
  BasicModule = 1 << 1,
  AuditModule = 1 << 2 ,
  LocalizeModule = 1 << 3 ,
  UnitTestModule= 1 << 4
}