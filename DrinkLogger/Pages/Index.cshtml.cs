using DrinkLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace DrinkLogger.Pages
{
    public class IndexModel : PageModel
    {
        public List<DrinkingWaterModel> Records { get; set; }
        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Records = GetAllRecords();
        }

        private List<DrinkingWaterModel> GetAllRecords()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = $"SELECT * FROM drink_water";
                var tableData = new List<DrinkingWaterModel>();
                SqliteDataReader reader = tablecmd.ExecuteReader();
                while(reader.Read())
                {
                    tableData.Add(new DrinkingWaterModel()
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentUICulture.DateTimeFormat),
                        Quantity = reader.GetInt32(2)
                    });
                }
                return tableData;
            }
        }
    }
}