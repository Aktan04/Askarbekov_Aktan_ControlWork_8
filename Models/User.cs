using Microsoft.AspNetCore.Identity;

namespace Forum.Models;

public class User : IdentityUser<int>
{
    public string Avatar { get; set; }
    public string NickName { get; set; }
}