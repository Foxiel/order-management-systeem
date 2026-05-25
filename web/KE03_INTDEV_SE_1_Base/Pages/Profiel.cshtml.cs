using DataAccessLayer.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class ProfielModel : PageModel
    {
        public void OnGet()
        {
        }

        public JsonResult OnPostLogin([FromBody] LoginRequest request)
        {
            AccountDAL accountDAL = new AccountDAL();

            bool success = accountDAL.LoginByUsernameAndPassword(
                request.Username,
                request.Password
            );

            return new JsonResult(new
            {
                success = success
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}