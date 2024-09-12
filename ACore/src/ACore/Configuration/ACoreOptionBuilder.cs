using ACore.Modules.MemoryCacheModule.Configuration;

namespace ACore.Configuration;

public class ACoreOptionBuilder
{
  private MemoryCacheModuleOptionsBuilder? _memoryCacheOptionsBuilder;
  private ACoreOptionBuilder()
  {
  }

  public static ACoreOptionBuilder Empty() => new();


  public ACoreOptionBuilder AddMemoryCacheModule(Action<MemoryCacheModuleOptionsBuilder> action)
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