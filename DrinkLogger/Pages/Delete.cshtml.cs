using DrinkLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace DrinkLogger.Pages
{
    public class DeleteModel : PageModel
    {
        private IConfiguration _configuration;
        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }
        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet(int id)
        {
            DrinkingWater = GetById(id);
            return Page();
        }

        private DrinkingWaterModel GetById(int id)
        {
            var drinkingWaterRecord = new DrinkingWaterModel();
            using(var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();
                tablecmd.CommandText = $"SELECT * FROM drink_water WHERE Id = {id}";
                SqliteDataReader reader = tablecmd.ExecuteReader();

                while (reader.Read())
                {
                    drinkingWaterRecord.Id = reader.GetInt32(0);
                    drinkingWaterRecord.Date = DateTime.Parse(reader.GetString(1),
                        CultureInfo.CurrentUICulture.DateTimeFormat);
                    drinkingWaterRecord.Quantity = reader.GetInt32(2);
                }
                return drinkingWaterRecord;
            }
        }
        public IActionResult OnPost(int id)
        {
            using(var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();
                tablecmd.CommandText = $"DELETE FROM drink_water WHERE Id = {id}";
                tablecmd.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}
