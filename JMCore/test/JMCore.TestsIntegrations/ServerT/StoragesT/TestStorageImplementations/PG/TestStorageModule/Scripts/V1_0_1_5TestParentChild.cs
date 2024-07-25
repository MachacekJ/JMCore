using JMCore.Server.Storages.Base.EF;

// ReSharper disable InconsistentNaming

namespace JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.PG.TestStorageModule.Scripts;

public class V1_0_1_5TestParentChild : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.5");

    public override List<string> AllScripts
    {
        get
        {
            List<string> l = new()
            {
                @"
CREATE TABLE test_rootcategory
(
   test_rootcategory_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    name VARCHAR(50),
    last_modify timestamp
);

CREATE TABLE test_category
(
    test_category_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    test_rootcategory_id INT,
    test_category_parent_id INT,
    name VARCHAR(50),
    CONSTRAINT fk_test_category__test_rootcategory
      FOREIGN KEY(test_rootcategory_id) 
        REFERENCES test_rootcategory(test_rootcategory_id),
    CONSTRAINT fk_test_category__test_category
      FOREIGN KEY(test_category_parent_id) 
        REFERENCES test_category(test_category_id)
);

CREATE INDEX idx_test_category__test_parent_id 
ON test_category(test_category_id);
"
            };


            return l;
        }
    }
}