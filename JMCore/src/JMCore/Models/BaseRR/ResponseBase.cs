using System.Text.Json.Serialization;

namespace JMCore.Models.BaseRR;

public class ResponseBase
{
    public static readonly int Code_None = 0;
    
    private int _code;

    public int Code
    {
        get => _code;
        set
        {
            if (!AllStatus.ContainsKey(value))
                throw new Exception($"Status code {value} is not registered.");
            _code = value;
        }
    }

    public DateTime? Time { get; set; }

    public string ShortMessage { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    [JsonIgnore]
    public Exception? Exception { get; set; }

    [JsonIgnore]
    public bool IsError => Code < 0;

    [JsonIgnore]
    public bool IsWarning => Code > 0;
    
    [JsonIgnore] 
    protected Dictionary<int, string> AllStatus { get; } = new();

    protected ResponseBase()
    {
        AllStatus.Add(Code_None, nameof(Code_None));
    }
}