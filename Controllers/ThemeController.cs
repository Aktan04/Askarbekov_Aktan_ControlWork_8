using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Controllers;

public class ThemeController : Controller
{
    private readonly ForumContext _context;
    private readonly UserManager<User> _userManager;

    public ThemeController(ForumContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 3;                
        var themes = await _context.Themes.Include(t => t.User)
            .Include(t => t.Messages)
            .OrderByDescending(t => t.DateOfCreation)
            .ToListAsync();
        var count = themes.Count();
        var items = themes.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
        ThemeIndexViewModel viewModel = new ThemeIndexViewModel()
        {
            PageViewModel = pageViewModel,
            Themes = items
        };
        return View(viewModel);
    }
    
    public async Task<IActionResult> Create()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Theme theme)
    {
        int? userId = Convert.ToInt32(_userManager.GetUserId(User));
        theme.UserId = userId.Value;
        theme.DateOfCreation = DateTime.UtcNow;
        if (ModelState.IsValid)
        {
            _context.Add(theme);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(theme);
    }
    public async Task<IActionResult> Details(int? themeId)
    {
        var theme = await _context.Themes.Include(t => t.User).Include(t => t.Messages)
            .FirstOrDefaultAsync(t => t.Id == themeId);
        if (theme == null)
        {
            return NotFound();
        }
        return View(theme);
    }

    public async Task<IActionResult> AddMessage(int themeId, string text)
    {

        int? userId = Convert.ToInt32(_userManager.GetUserId(User));

        if (string.IsNullOrWhiteSpace(text))
        {
            return BadRequest("Текст комментария обязателен к заполнению");
        }

        var message = new Message
        {
            ThemeId = themeId,
            Text = text,
            DateOfSend = DateTime.UtcNow,
            UserId = userId.Value
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var messageWithUser = await _context.Messages
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == message.Id);

        if (messageWithUser == null)
        {
            return StatusCode(500, "Ошибка при получении сохраненного сообщения.");
        }


        return Json(new
        {
            success = true,
            message = new
            {
                UserNickName = messageWithUser.User?.NickName,
                Text = messageWithUser.Text,
                DateOfSend = messageWithUser.DateOfSend.ToString("g"),
                Avatar = messageWithUser.User?.Avatar
            }
        });
    }

}