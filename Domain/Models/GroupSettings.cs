namespace Domain.Models;

public class GroupSettings
{
    public int Hour { get; set; }
    public int Minute { get; set; }

    public enum Periodic
    {
        Daily,
        Weekly
    }
}