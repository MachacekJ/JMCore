using ACore.Server.Storages.EF;

// ReSharper disable InconsistentNaming

namespace ACore.Tests.Implementations.Modules.TestModule.Storages.PG.Scripts;

public class V1_0_1_2TestAuditTables : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.2");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l =
            [
                @"
CREATE TABLE test_attribute_audit
(
    test_attribute_audit_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50),
    not_auditable_column VARCHAR(50),
    created timestamp
);

CREATE TABLE test_manual_audit
(
    test_manual_audit_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50),
    not_auditable_column VARCHAR(50),
    created timestamp
);

"
            ];


            return l;
        }
    }
}