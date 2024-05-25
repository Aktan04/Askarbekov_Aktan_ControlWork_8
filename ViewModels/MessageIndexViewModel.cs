using Forum.Models;

namespace Forum.ViewModels;

public class MessageIndexViewModel
{
    public Theme Theme { get; set; }
    public List<Message> Messages { get; set; }
    public PageViewModel PageViewModel { get; set; }
}