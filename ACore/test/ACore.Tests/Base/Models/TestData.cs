using System.Reflection;

namespace ACore.Tests.Base.Models;

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
           return;
        
        TestId = method.DeclaringType?.FullName ?? throw new ArgumentNullException(nameof(method));
    }

    public string GetDbName()
    {
        var testName = _shrinkStrings.Aggregate(TestName, (current, name)
            => current.Replace($"{name}_", string.Empty)).ToLower();
        testName += Guid.NewGuid();
        testName = testName.Replace("-", string.Empty);
        
        if (testName.Length > MaximumLengthOfDb)
            testName = testName.Substring(testName.Length - MaximumLengthOfDb);
        return testName;
    }
}