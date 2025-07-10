using System.Linq;
using UserManagement.Services;                      
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

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
}
