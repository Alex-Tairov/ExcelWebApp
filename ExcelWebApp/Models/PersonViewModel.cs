using System.ComponentModel.DataAnnotations;

namespace ExcelWebApp.Models
{
    public class PersonViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
