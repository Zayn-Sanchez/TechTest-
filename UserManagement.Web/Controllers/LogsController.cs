using System.Linq; 
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;



[Route("logs")]
public class LogsController : Controller
{
    private readonly IDataContext _db;
    public LogsController(IDataContext db) => _db = db;

    [HttpGet] 
    public IActionResult Index() =>
        View(_db.Logs!.OrderByDescending(l => l.WhenUtc).ToList());

    [HttpGet("{id:long}")]   
    public IActionResult Details(long id)
    {
        var log = _db.Logs!.SingleOrDefault(l => l.Id == id);
        return log is null ? NotFound() : View(log);
    }
}
