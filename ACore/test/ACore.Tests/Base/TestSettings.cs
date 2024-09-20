using Microsoft.Extensions.Configuration;

namespace ACore.Tests.Base;

public class TestSettings
{
    
    public static TestSettings CreateFromConfig(IConfiguration configuration)
    {
        var res = new TestSettings();
        return res;
    }
}