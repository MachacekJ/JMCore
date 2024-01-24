using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.BasicStructure.Scripts.Postgres
{
    // ReSharper disable once InconsistentNaming
    public class V1_0_0_1SettingsTable : DbVersionScriptsBase
    {
        public override Version Version
        {
            get
            {
                return new Version("1.0.0.1");
            }
        }

        public override List<string> AllScripts
        {
            get
            {

                List<string> l = new ();
                l.Add(@"
CREATE TABLE IF NOT EXISTS public.""Setting""
(
    ""Id"" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    ""Key"" character varying(100) COLLATE pg_catalog.""default"" NOT NULL,
    ""Value"" character varying COLLATE pg_catalog.""default"" NOT NULL,
    ""IsSystem"" bit(1),
    CONSTRAINT ""Setting_pkey"" PRIMARY KEY (""Id"")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.""Setting""
    OWNER to postgres;
");


                return l;
            }
        }
    }
}
