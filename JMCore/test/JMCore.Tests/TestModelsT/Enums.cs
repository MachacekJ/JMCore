namespace JMCore.Tests.TestModelsT;

/// <summary>
/// Test environment type Core/Dev.
/// </summary>
[Flags]
public enum TestEnvironmentTypeEnum
{
    /// <summary>
    /// This test will be ran every time.
    /// </summary>
    Core = 1,

    /// <summary>
    /// Test for quicker development. Auxiliary test.
    /// </summary>
    Dev = 2
}