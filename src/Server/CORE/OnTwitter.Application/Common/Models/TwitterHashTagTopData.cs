namespace OnTwitter.Application.Models;

public class TwitterHashTagTopData
{
    public double TotalTwitterHashTagsTotal { get; set; }
    public List<TwitterHashTagTop> TwitterHashTagTops { get; set; } = new List<TwitterHashTagTop>();
}
