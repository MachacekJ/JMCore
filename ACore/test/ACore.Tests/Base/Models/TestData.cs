using System.Reflection;

namespace ACore.Tests.BaseInfrastructure.Models;

public class TestData
{
    private readonly string _methodName = string.Empty;
    private readonly string _testId = string.Empty;
    private string _testName = null!;
    private readonly string[] _replaceLetter = { ".", "<", ">", "+" };

    private const int MaximumLengthOfDb = 63;
  
    private readonly List<string> _shrinkStrings =
    [
        nameof(ACore),
        "TestsIntegrations",
        "ServerT",
        "StoragesT",
        "ModulesT"
    ];
    
    public DatabaseManipulationEnum DatabaseManipulation { get; set; } = DatabaseManipulationEnum.Default;
    
    public string TestId
    {
        get => _testId;
        private init
        {
            _testId = value + (string.IsNullOrEmpty(_methodName) ? string.Empty : "." + _methodName);

            if (string.IsNullOrEmpty(_testId))
                _testId = "UNKNOWN_TEST_ID" + Guid.NewGuid();

            _testName = _testId;
        }
    } 
    
    /// <summary>
    /// Name of test important for DB name and log file name.
    /// </summary>
    public string TestName
    {
        get
        {
            if (_testName == null)
                throw new Exception("TestName is null. Set TestId.");
            return _replaceLetter.Aggregate(_testName, (current, letter) => current.Replace(letter, "_"));
        }
        set => _testName = value;
    }
    
    public TestData(MemberInfo? method)
    {
        if (method == null)
            throw new ArgumentException($"{nameof(TestData)}.{nameof(method)} is null");
        
        TestId = method.DeclaringType?.FullName!;
    }

    public string GetDbName()
    {
        var testName = _shrinkStrings.Aggregate(TestName, (current, name)
            => current.Replace($"{name}_", string.Empty)).ToLower();
        if (testName.Length > MaximumLengthOfDb)
            testName = testName.Substring(testName.Length - MaximumLengthOfDb);
        return testName;
    }
}