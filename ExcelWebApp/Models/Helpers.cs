using ExcelDataReader;
using ExcelWebApp.Db.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExcelWebApp.Models
{
    public class Helpers
    {
        public static List<PersonViewModel> MappingPersonDbToPersonVM(List<Person> personsDB)
        {
            var persons = new List<PersonViewModel>();
            foreach (var personDB in personsDB)
            {
                persons.Add(new PersonViewModel()
                {
                    Id = personDB.Id,
                    Name = personDB.Name,
                    City = personDB.City,
                    Age = personDB.Age,
                    Gender = personDB.Gender
                });
            }
            return persons;
        }

        public static List<Person> MappingPersonVMToPersonDb(List<PersonViewModel> personsVM)
        {
            var persons = new List<Person>();
            foreach (var personDB in personsVM)
            {
                persons.Add(new Person()
                {
                    Id = personDB.Id,
                    Name = personDB.Name,
                    City = personDB.City,
                    Age = personDB.Age,
                    Gender = personDB.Gender
                });
            }
            return persons;
        }
        public static List<Male> GetMaleListFromPersons(List<PersonViewModel> personsVM)
        {
            return  (from plane in personsVM
                                  group plane by plane.Gender into grouppenPlanes
                                  where grouppenPlanes.Count() > 1
                                  select new Male
                                  {
                                      Gender = grouppenPlanes.Key,
                                      Count = grouppenPlanes.Count()
                                  })
         .ToList();
        }


        public static void TrySafeFromExcel(IFormFile uploadedFile, ModelStateDictionary ModelState, List<PersonViewModel> newPersons)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    uploadedFile.CopyTo(stream);
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var count = 0;
                        while (reader.Read())
                        {
                            if (count > 0)
                            {
                                var newPerson = new PersonViewModel();
                                                              

                                if (!string.IsNullOrEmpty(reader.GetValue(0).ToString()))
                                {
                                    newPerson.Name = reader.GetValue(0)?.ToString();
                                }
                                else
                                {
                                    ModelState.AddModelError("", $"Строка {count}: не заполнено имя");
                                }
                                if (!string.IsNullOrEmpty(reader.GetValue(1)?.ToString()))
                                {
                                    newPerson.City = reader.GetValue(1)?.ToString();
                                }
                                else 
                                {
                                    ModelState.AddModelError("", $"Строка {count}: не заполнен город");
                                }
                                if (!string.IsNullOrEmpty(reader.GetValue(2)?.ToString()))
                                {
                                    newPerson.Gender = reader.GetValue(2)?.ToString();
                                }
                                else
                                {
                                    ModelState.AddModelError("", $"Строка {count}: не заполнен пол");
                                }
                                if (!string.IsNullOrEmpty(reader.GetValue(3)?.ToString()))
                                {
                                    var age = 0;
                                    Int32.TryParse(reader.GetValue(3).ToString(), out age);
                                    newPerson.Age = age;
                                }
                                else
                                {
                                    ModelState.AddModelError("", $"Строка {count}: не заполнен возраст");
                                }
                                newPersons.Add(newPerson);
                            }
                            count++;
                        }
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Неверный формат файла");
            }
        }
    }
}
