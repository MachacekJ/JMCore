using ACore.Server.Storages.Definitions.EF.Base.Scripts;

// ReSharper disable InconsistentNaming

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.PG.Scripts;

public class V1_0_1_5TestParentChild : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.5");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l =
            [
                @"
CREATE TABLE test_menu
(
   test_menu_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50),
    last_modify timestamp
);

CREATE TABLE test_category
(
    test_category_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    test_menu_id INT,
    test_category_parent_id INT,
    name VARCHAR(50),
    CONSTRAINT fk_test_category__test_menu
      FOREIGN KEY(test_menu_id) 
        REFERENCES test_menu(test_menu_id),
    CONSTRAINT fk_test_category__test_category
      FOREIGN KEY(test_category_parent_id) 
        REFERENCES test_category(test_category_id)
);

CREATE INDEX idx_test_category__test_category_parent_id 
ON test_category(test_category_id);
"
            ];


            return l;
        }
    }
}