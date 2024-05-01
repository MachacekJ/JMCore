using JMCore.Server.Storages.Abstract;

namespace JMCore.Server.Storages.DbContexts.LocalizeStructure.Scripts.MSSQL
{
    // ReSharper disable once InconsistentNaming
    public class V1_0_0_01LocalizationTable : DbVersionScriptsBase
    {
        public override Version Version => new("1.0.0.1");

        public override List<string> AllScripts
        {
            get
            {

                List<string> l = new()
                {
	                @"
CREATE TABLE [dbo].[Localization](
    [Id]                   int NOT NULL IDENTITY(1,1),
	[MsgId]			       varchar(255) NOT NULL    ,
	[Translation]          nvarchar(max) NOT NULL     ,
    [LCID]                 int NOT NULL    ,
    [ContextId]            varchar(1024) NOT NULL    ,
    [Scope]                int NOT NULL, 
    [Changed]              datetime2 NULL

 CONSTRAINT[Pk_Localization_Id] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO



"
                };
                return l;
            }
        }
    }
}


/*
CREATE PROCEDURE [dbo].[sp_Localization_Save] 
	@MsgId varchar(255),
	@Translation nvarchar(max)      ,
    @LCID int     ,
    @ContextId varchar(1024)  ,
    @Scope int,        
	@IsChange bit OUTPUT,
	@Id int = 0 OUTPUT
	AS
BEGIN
    SET @IsChange = 0

    DECLARE @transl  nvarchar(max) = NULL;

    SELECT @transl=Translation, @Id = Id FROM dbo.Localization WHERE [LCID]=@LCID AND [MsgId]=@MsgId AND [ContextId]=@ContextId AND [Scope]=@Scope --AND [Translation]=@Translation)
	
	IF (@transl IS NOT NULL)
		BEGIN
		  IF NOT(@transl = @Translation)
		  BEGIN
			UPDATE dbo.Localization SET Translation = @Translation, Changed = SYSUTCDATETIME() WHERE Id = @Id
			SET @IsChange = 1
		  END
		END
	ELSE
		BEGIN
		   INSERT INTO dbo.Localization ([LCID],[MsgId],[ContextId],[Scope],[Translation],[Changed]) VALUES 
                                         (@LCID,@MsgId,@ContextId,@Scope,@Translation,SYSUTCDATETIME())
		   SET @Id = CAST(SCOPE_IDENTITY() as int)
		   SET @IsChange = 1
		END

    SELECT Id, MsgId, Translation, LCID, ContextId, Scope, Changed FROM dbo.Localization WHERE Id=@Id
END

 
 */