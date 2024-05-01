﻿using JMCore.Server.Storages.Abstract;

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Scripts.Postgres;

// ReSharper disable once InconsistentNaming
public class V1_0_1_1TestTable : DbVersionScriptsBase
{
    public override Version Version => new ("1.0.0.1");

    public override List<string> AllScripts
    {
        get
        {

            List<string> l = new()
            {
                @"
CREATE TABLE test
(
    test_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50),
    created timestamp
);
"
            };


            return l;
        }
    }
}