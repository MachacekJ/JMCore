using JMCore.Server.Storages.EF;

// ReSharper disable InconsistentNaming

namespace JMCore.Tests.Implementations.Storages.TestModule.Storages.PG.TestStorageModule.Scripts;

public class V1_0_1_4TestPK : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.4");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l = new()
            {
                @"
CREATE TABLE test_pk_guid
(
    test_pk_guid_id UUID
        PRIMARY KEY,
    name VARCHAR(20)
);

CREATE TABLE test_pk_string
(
    test_pk_string_id VARCHAR(50)
        PRIMARY KEY,
    name VARCHAR(20)
);

"
            };


            return l;
        }
    }
}