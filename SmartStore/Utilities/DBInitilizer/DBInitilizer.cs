using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace SmartStore.Utilities.DBInitilizer
{
    public class DBInitilizer : IDBInitilizer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DBInitilizer> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DBInitilizer(ApplicationDbContext dbContext, ILogger<DBInitilizer> logger,
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task Initilize()
        {
            Console.WriteLine($"_context: {_dbContext != null}");
            Console.WriteLine($"_roleManager: {_roleManager != null}");
            Console.WriteLine($"_userManager: {_userManager != null}");
            Console.WriteLine($"_logger: {_logger != null}");
           
            try
            {
                if (_dbContext!.Database.GetPendingMigrations().Any())
                    _dbContext.Database.Migrate();

                if(_roleManager!.Roles.IsNullOrEmpty())
                {
                    _roleManager.CreateAsync(new(SD.ROLE_SUPERADMIN)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.ROLE_CUSTOMER)).GetAwaiter().GetResult();
                }

                _userManager!.CreateAsync(new()
                {
                    Email = "moazaboefadle@gmail.com",
                    UserName = "MoazAdmin23",
                    EmailConfirmed = true,
                    FirstName = "Moaz",
                    LastName = "Adel"
                }, password: "Moaz12345@").GetAwaiter().GetResult();

                var user = _userManager.FindByNameAsync("SuperAdmin").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user!, SD.ROLE_SUPERADMIN).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                _logger!.LogError($"error: {ex.Message}");
            }
        }
    }
}
