using ACore.Modules.MemoryCacheModule.Configuration;

namespace ACore.Configuration;

public class ACoreOptionsBuilder
{
  private MemoryCacheModuleOptionsBuilder? _memoryCacheOptionsBuilder;
  private ACoreOptionsBuilder()
  {
  }

  public static ACoreOptionsBuilder Empty() => new();


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
      MemoryCacheModuleOptions = _memoryCacheOptionsBuilder?.Build() ?? throw new Exception($"{nameof(_memoryCacheOptionsBuilder)} is null.")
    };
  }
}