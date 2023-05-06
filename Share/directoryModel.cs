namespace Share;

/// <summary>
/// directory 구조 model
/// </summary>
public class directoryModel
{
    /// <summary>
    /// 
    /// </summary>
    private string name = String.Empty;
    public string Name
    {
        get => name;
        set => name = value;
    }

    /// <summary>
    /// 
    /// </summary>
    private string path = String.Empty;
    public string Path
    {
        get => path;
        set => path = value;
    }

    /// <summary>
    /// 
    /// </summary>
    private List<directoryModel> children = new List<directoryModel>();
    public List<directoryModel> Children
    {
        get => children;
        set => children = value;
    }

}