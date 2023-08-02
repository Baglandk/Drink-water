using DrinkLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace DrinkLogger.Pages
{
    public class UpdateModel : PageModel
    {
        private IConfiguration _configuration;
        public UpdateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public DrinkingWaterModel drinkingWaterModel { get; set; }
        public IActionResult OnGet(int id)
        {
            drinkingWaterModel = GetById(id);
            return Page();
        }
        private DrinkingWaterModel GetById(int id)
        {
            var drinkingWaterRecord = new DrinkingWaterModel();
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection")))
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
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText =
                    $"UPDATE drink_water SET date = '{drinkingWaterModel.Date}', quantity = {drinkingWaterModel.Quantity} WHERE Id = {drinkingWaterModel.Id}";
                tablecmd.ExecuteNonQuery();
                connection.Close();
            }

            return RedirectToPage("./Index");
        }
    }
}
