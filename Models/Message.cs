using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class Message
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Текст комментария обязателен к заполнению")]
    public string Text { get; set; }
    public DateTime DateOfSend { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ThemeId { get; set; }
    public Theme? Theme { get; set; }
}