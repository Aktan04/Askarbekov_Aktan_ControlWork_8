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
    
    [Authorize]
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
        theme.DateOfCreation = DateTime.UtcNow.AddHours(6);
        if (ModelState.IsValid)
        {
            _context.Add(theme);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(theme);
    }
    
    public async Task<IActionResult> Details(int? themeId, int page = 1)
    {
        int pageSize = 5;
        var theme = await _context.Themes.Include(t => t.User).Include(t => t.Messages)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.Id == themeId);
        if (theme == null)
        {
            return NotFound();
        }
        var messages = theme.Messages.OrderBy(m => m.DateOfSend);
        var count = messages.Count();
        var items = messages.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var viewModel = new MessageIndexViewModel
        {
            Theme = theme,
            Messages = items,
            PageViewModel = new PageViewModel(count, page, pageSize)
        };
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new
            {
                success = true,
                messages = items.Select(m => new
                {
                    UserAvatar = m.User?.Avatar,
                    UserNickName = m.User?.NickName,
                    Text = m.Text,
                    DateOfSend = m.DateOfSend.ToString("g")
                }),
                pagination = new
                {
                    hasPreviousPage = viewModel.PageViewModel.HasPreviousPage,
                    hasNextPage = viewModel.PageViewModel.HasNextPage,
                    currentPage = viewModel.PageViewModel.PageNumber,
                    totalPages = viewModel.PageViewModel.TotalPages
                }
            });
        }
        return View(viewModel);
    }
    
    [Authorize]
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
            DateOfSend = DateTime.UtcNow.AddHours(6),
            UserId = userId.Value
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        var totalMessages = await _context.Messages.CountAsync(m => m.ThemeId == themeId);
        var messagesPerPage = 5; 
        var totalPages = (int)Math.Ceiling(totalMessages / (double)messagesPerPage);
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
            },
            totalPages
        });
    }

}