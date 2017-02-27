using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyStarwarsApi.Context;
using MyStarwarsApi.Models;
using MyStarwarsApi.Models.ViewModel;

namespace MyStarwarsApi.Controllers{
    
    [Authorize]
    public class AccountController : Controller{
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SqliteDbContext _dbContext;

        private static bool _databaseChecked;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ILoggerFactory loggerFactory,
            SqliteDbContext dbContext
        ){
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model){
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
                return Ok();

            return BadRequest(ModelState);
        }
 

        public static void EnsureDatabaseCreated(SqliteDbContext context){
            if(!_databaseChecked){
                _databaseChecked = true;
                context.Database.EnsureCreated();
            }
        }
    }
}