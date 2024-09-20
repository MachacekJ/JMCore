using ACore.Modules.MemoryCacheModule.Configuration;

namespace ACore.Configuration;

public class ACoreOptionsBuilder
{
  private MemoryCacheModuleOptionsBuilder? _memoryCacheOptionsBuilder;
  private string _saltForHash = string.Empty;
  private ACoreOptionsBuilder()
  {
  }

  public static ACoreOptionsBuilder Empty() => new();

  public ACoreOptionsBuilder AddSaltForHash(string salt)
  {
    _saltForHash = salt;
    return this;
  }

  public ACoreOptionsBuilder AddMemoryCacheModule(Action<MemoryCacheModuleOptionsBuilder> action)
  {
    _memoryCacheOptionsBuilder ??= MemoryCacheModuleOptionsBuilder.Empty();
    action(_memoryCacheOptionsBuilder);
    _memoryCacheOptionsBuilder.Activate();
    return this;
  }

  public ACoreOptions Build()
  {
    return new ACoreOptions
    {
      SaltForHash = _saltForHash,
      MemoryCacheModuleOptions = _memoryCacheOptionsBuilder?.Build() ?? throw new Exception($"{nameof(_memoryCacheOptionsBuilder)} is null.")
    };
  }
}