using ExcelWebApp.Db.Models;

namespace ExcelWebApp.Db.Interfaces
{
    public interface IPersonsRepository
    {
        void AddPersons(List<Person> persons);
        List<Person> GetAllPersons();
        Person GetPersonById(Guid id);
    }
}