using JMCore.Tests.TestModelsT;
using Microsoft.Extensions.Configuration;

namespace JMCore.Tests;

public class TestSettings
{
    public TestEnvironmentTypeEnum TestType { get; protected set; }
    
    public static TestSettings CreateFromConfig(IConfiguration configuration)
    {
        var res = new TestSettings();
        var rr = configuration["TestSettings:TestType"] ?? throw new InvalidOperationException();
        if (rr.Contains("Dev"))
            res.TestType |= TestEnvironmentTypeEnum.Dev;
        if (rr.Contains("Core"))
            res.TestType |= TestEnvironmentTypeEnum.Core;

        return res;
    }
}