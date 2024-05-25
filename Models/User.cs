using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models;

public class User : IdentityUser<int>
{
    public string? Avatar { get; set; }
    [Required(ErrorMessage = "Имя пользователя обязателен к заполнению")]
    public string NickName { get; set; }
    public ICollection<Message>? Messages { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    [NotMapped]
    public int MessageCount
    {
        get
        {
            return Messages?.Count() ?? 0;
        }
    }
}