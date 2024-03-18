using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.AuditStructure.Scripts.Postgres
{
    // ReSharper disable once InconsistentNaming
    public class V1_0_0_1AuditTables : DbVersionScriptsBase
    {
        public override Version Version => new("1.0.0.1");

        public override List<string> AllScripts =>
            new()
            {
                @"
create table public.""AuditTable""
(
    ""Id"" integer generated always as identity
        primary key,
    ""TableName"" varchar(255) not null,
    ""SchemaName"" varchar(255) null
);

alter table public.""AuditTable""
    owner to postgres;

create table if not exists public.""Audit""
(
    ""Id"" bigint generated always as identity
        primary key,
    ""AuditTableId"" integer not null
        constraint ""FK_Audit_AuditTable""
            references ""AuditTable""
);

alter table public.""Audit""
    owner to postgres;


create unique index ""IX_AuditColumn_AuditTableIdName""
    on ""AuditTable"" (""TableName"", ""SchemaName"");
"
            };
    }
}

