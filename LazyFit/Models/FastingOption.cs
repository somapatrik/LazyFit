namespace LazyFit.Models;

public class FastingOption
{
    public int Hours { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ShouldEnd { get; set; }

    public FastingOption(int hours, string name, string description)
    {
        Hours = hours;
        Name = name;
        Description = description;
        DateTime shouldEnd = DateTime.Now.AddHours(hours);
        ShouldEnd = "Until " + shouldEnd.ToString("g");
    }
}
