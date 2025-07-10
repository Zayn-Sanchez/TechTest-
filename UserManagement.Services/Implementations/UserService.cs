using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _db;
    public UserService(IDataContext db) => _db = db;

    public IEnumerable<User> GetAll() => _db.GetAll<User>().ToList();

    public IEnumerable<User> FilterByActive(bool isActive) =>
        _db.GetAll<User>()
           .Where(u => u.IsActive == isActive)
           .ToList();

    public User? Get(long id) =>
        _db.GetAll<User>().SingleOrDefault(u => u.Id == id);

    public void Add(User user) => _db.Create(user);

    public void Update(User user)
    {
        var tracked = _db.GetAll<User>().Single(u => u.Id == user.Id);

        tracked.Forename = user.Forename;
        tracked.Surname = user.Surname;
        tracked.Email = user.Email;
        tracked.DateOfBirth = user.DateOfBirth;
        tracked.IsActive = user.IsActive;

        _db.Update(tracked);
    }

    public void Delete(long id)
    {
        var user = Get(id);
        if (user is not null)
            _db.Delete(user);
    }
}
