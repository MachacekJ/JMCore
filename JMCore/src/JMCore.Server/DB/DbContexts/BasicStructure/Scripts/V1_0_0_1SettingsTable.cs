using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.BasicStructure.Scripts
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
CREATE TABLE [dbo].[Setting](
    [Id]                   int NOT NULL IDENTITY(1,1),
	[Key]			       nvarchar(100) NOT NULL    ,
	[Value]                nvarchar(max) NOT NULL     ,
	[IsSystem]             bit NULL    ,

 CONSTRAINT[Pk_Setting_Id] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO");


                return l;
            }
        }
    }
}
