using System.Reflection;

namespace JMCore.Tests.TestModelsT;

public class TestData
{
    private readonly string _methodName = string.Empty;
    private readonly string _testId = string.Empty;
    private string _testName = null!;
    private readonly string[] _replaceLetter = { ".", "<", ">", "+" };

    public TestData(MemberInfo? method)
    {
        if (method == null)
            throw new ArgumentException($"{nameof(TestData)}.{nameof(method)} is null");
        
        TestId = method.DeclaringType?.FullName!;
    }

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
    /// Type of test, default is Core.
    /// </summary>
    public TestEnvironmentTypeEnum TestEnvironmentType { get; init; } = TestEnvironmentTypeEnum.Core;

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
    }
}