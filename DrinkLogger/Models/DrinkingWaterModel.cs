using System.ComponentModel.DataAnnotations;

namespace DrinkLogger.Models
{
    public class DrinkingWaterModel
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value for {0} must be positive")]
        public int Quantity { get; set; }
    }
}
