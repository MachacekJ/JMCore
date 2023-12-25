namespace JMCore.Extensions;

public static class ExceptionExtensions
{
    public static string MessageRecur(this Exception ex)
    {
        var allmess = string.Empty;
        RekurInnerMess(ex, ref allmess);
        return allmess;
    }

    private static void RekurInnerMess(Exception ex, ref string mess)
    {
        if (!string.IsNullOrEmpty(ex.Message))
            mess += "->" + ex.Message;


        var aex = ex as AggregateException;
        if (aex != null)
        {
            foreach (var exitem in aex.InnerExceptions)
            {
                RekurInnerMess(exitem, ref mess);
            }
        }
        else if (ex.InnerException != null)
        {
            RekurInnerMess(ex.InnerException, ref mess);
        }
    }
}