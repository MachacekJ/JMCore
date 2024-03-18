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
create table if not exists public.""Setting""
(
    ""Id""       integer generated always as identity
        primary key,
    ""Key""      varchar(100) not null,
    ""Value""    varchar      not null,
    ""IsSystem"" boolean
);

alter table public.""Setting""
    owner to postgres;
");


                return l;
            }
        }
    }
}
