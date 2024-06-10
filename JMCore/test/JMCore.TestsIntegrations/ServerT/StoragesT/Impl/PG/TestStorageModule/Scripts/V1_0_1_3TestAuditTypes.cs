using JMCore.Server.Storages.Base.EF;

// ReSharper disable InconsistentNaming

namespace JMCore.TestsIntegrations.ServerT.StoragesT.Impl.PG.TestStorageModule.Scripts;

public class V1_0_1_3TestAuditTypes : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.3");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l =
            [
                @"
CREATE TABLE test_value_type
(
    test_value_type_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    int_not_null INT NOT NULL,
    int_null INT,
    big_int_not_null BIGINT NOT NULL,
    big_int_null BIGINT,
    bit2 BOOL NOT NULL,
    char2 CHAR(10) NOT NULL,
    date2 DATE NOT NULL,
    datetime2 TIMESTAMP NOT NULL,
    decimal2 NUMERIC(19,4) NOT NULL,
    nchar2 CHAR(10) NOT NULL,
    nvarchar2 CHAR(10) NOT NULL,
    smalldatetime2 timestamp NOT NULL,
    smallint2 SMALLINT NOT NULL,
    tinyint2 SMALLINT NOT NULL,
    guid2 UUID NOT NULL,
    varbinary2 BYTEA NOT NULL,
    varchar2 VARCHAR(100) NOT NULL
);
"
            ];


            return l;
        }
    }
}