namespace Share;

/// <summary>
/// callController에서 사용
/// </summary>
public class callModel
{
    /// <summary>
    /// 경로
    /// </summary>
    private string path = String.Empty;
    public string Path
    {
        get => path;
        set => path = value;
    }

    /// <summary>
    /// 카운트
    /// </summary>
    private int count;
    public int Count
    {
        get => count;
        set => count = value;
    }
}

