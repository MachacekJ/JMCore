using ACore.Server.Storages.EF;

namespace ACore.Server.Modules.AuditModule.Storage.SQL.PG.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_1AuditTables : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.1");

    public override List<string> AllScripts =>
        new()
        {
            @"
CREATE TABLE audit_table
(
    audit_table_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    table_name VARCHAR(255) NOT NULL,
    schema_name VARCHAR(255)
);

CREATE TABLE audit_user
(
    audit_user_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    user_id VARCHAR(450) NOT NULL,
    user_name VARCHAR(255)
);

CREATE TABLE audit_column
(
    audit_column_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    audit_table_id INT NOT NULL
        CONSTRAINT fk_audit_column__audit_table
            REFERENCES audit_table(audit_table_id),
    column_name VARCHAR(255) NOT NULL,
    data_type VARCHAR(1024) NOT NULL
);

CREATE TABLE audit
(
    audit_id BIGINT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    audit_table_id INT NOT NULL
        CONSTRAINT fk_audit__audit_table
            REFERENCES audit_table(audit_table_id),
    pk_value BIGINT,
    pk_value_string VARCHAR(450),
    audit_user_id INT
        CONSTRAINT fk_audit__audit_user
            REFERENCES audit_user(audit_user_id),
    date_time timestamp NOT NULL,
    entity_state smallint NOT NULL
);

CREATE TABLE audit_value
(
    audit_value_id BIGINT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    audit_id INT NOT NULL
        CONSTRAINT fk_audit_value__audit
            REFERENCES audit(audit_id),
    audit_column_id INT
        CONSTRAINT fk_audit_value__audit_column
            REFERENCES audit_column(audit_column_id),
    old_value_string TEXT,
    new_value_string TEXT,
    old_value_int INT,
    new_value_int INT,
    old_value_long BIGINT,
    new_value_long BIGINT,
    old_value_bool BOOL,
    new_value_bool BOOL,
    old_value_guid UUID,
    new_value_guid UUID
);

CREATE UNIQUE INDEX idx_audit_table_c_table_name_schema_name
    ON audit_table (table_name, schema_name);

CREATE UNIQUE INDEX idx_audit_user_c_user_id 
    ON audit_user (user_id);

CREATE UNIQUE INDEX idx_audit_column_c_audit_table_id_column_name
    ON audit_column (audit_table_id, column_name);
"
        };
}