using DrinkLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace DrinkLogger.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
                return Page();

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText =
                    $"INSERT INTO drink_water(date, quantity) VALUES ('{DrinkingWater.Date}', {DrinkingWater.Quantity})";
                tablecmd.ExecuteNonQuery();
                connection.Close();
            }

            return RedirectToPage("./Index");
        }

    }
}
