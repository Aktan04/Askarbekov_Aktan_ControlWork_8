using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class Theme
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Название темы не должно быть пустым")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Описание темы не должно быть пустым")]
    public string Description { get; set; }
    public DateTime DateOfCreation { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Message>? Messages { get; set; }
}