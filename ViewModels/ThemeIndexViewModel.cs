using Forum.Models;

namespace Forum.ViewModels;

public class ThemeIndexViewModel
{
    public IEnumerable<Theme> Themes { get; set; }
    public PageViewModel PageViewModel { get; set; }
}