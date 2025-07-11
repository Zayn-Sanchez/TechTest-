using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Models;
using UserManagement.Web.Models.Users;
using UserManagement.Data;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IDataContext _db;

    public UsersController(IUserService userService, IDataContext db)
    {
        _userService = userService;
        _db = db;
    }

    [HttpGet]
    public ViewResult List([FromQuery] UserFilter filter = UserFilter.All)
    {
        var source = filter switch
        {
            UserFilter.ActiveOnly => _userService.FilterByActive(true),
            UserFilter.NonActive => _userService.FilterByActive(false),
            _ => _userService.GetAll()
        };

        var items = source.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel { Items = items.ToList() };
        return View(model);
    }

    [HttpGet("{id:long}")]
    public IActionResult Details(long id)
    {
        var user = _userService.Get(id);
        if (user is null) return NotFound();

        ViewBag.Logs = _db.Logs!
                          .Where(l => l.UserId == id)
                          .OrderByDescending(l => l.WhenUtc)
                          .Take(20)
                          .ToList();

        return View(user);
    }

    [HttpGet("add")]
    public IActionResult Add() => View();

    [HttpPost("add")]
    public IActionResult Add(User user)
    {
        if (!ModelState.IsValid) return View(user);
        _userService.Add(user);
        return RedirectToAction(nameof(List));
    }

    [HttpGet("edit/{id:long}")]
    public IActionResult Edit(long id)
    {
        var user = _userService.Get(id);
        return user is null ? NotFound() : View(user);
    }

    [HttpPost("edit/{id:long}")]
    public IActionResult Edit(long id, User user)
    {
        if (!ModelState.IsValid) return View(user);
        _userService.Update(user);
        return RedirectToAction(nameof(List));
    }
    [HttpGet("delete/{id:long}")]
    public IActionResult Delete(long id)
    {
        var user = _userService.Get(id);
        return user is null ? NotFound() : View(user);
    }

    [HttpPost("delete/{id:long}")]
    public IActionResult DeleteConfirmed(long id)
    {
        _userService.Delete(id);
        return RedirectToAction(nameof(List));
    }
}
