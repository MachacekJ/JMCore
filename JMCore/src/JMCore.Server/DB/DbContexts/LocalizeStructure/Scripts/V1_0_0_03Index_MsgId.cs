﻿using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.LocalizeStructure.Scripts
{
    // ReSharper disable once InconsistentNaming
    public class V1_0_0_03Index_MsgId : DbVersionScriptsBase
    {
        public override Version Version => new("1.0.0.3");

        public override List<string> AllScripts
        {
            get
            {

                List<string> l = new()
                {
	                @"
CREATE NONCLUSTERED INDEX [IX_Localizations_MsgId] ON [dbo].[Localization]
(
	[MsgId] ASC
)
GO
"
                };
                return l;
            }
        }
    }
}


/*

CREATE PROCEDURE dbo.sp_Localization_Save 
	@MsgId varchar(255),
	@Translation nvarchar(max)      ,
    @LCID                 int     ,
    @ContextId            varchar(1024)   
	AS
BEGIN
    DECLARE @transl  nvarchar(max) = NULL;
	DECLARE @id int;
    SELECT @transl=Translation, @id = Id FROM dbo.Localizations WHERE [LCID]=@LCID AND [MsgId]=@MsgId AND [ContextId]=@ContextId --AND [Translation]=@Translation)
	
	IF (@transl IS NOT NULL)
	BEGIN
	  IF NOT(@transl = @Translation)
	  BEGIN
	    UPDATE dbo.Localizations SET Translation = @Translation, Changed = SYSUTCDATETIME() WHERE Id = @id
	  END
	  SELECT 0;
	END
	ELSE
	BEGIN
	   INSERT INTO dbo.Localizations ([LCID],[MsgId],[ContextId],[Translation],[Changed]) VALUES (@LCID,@MsgId,@ContextId,SYSUTCDATETIME())
	   SELECT CAST(SCOPE_IDENTITY() as int)
	END
END
GO

 
 */