using ExcelWebApp.Db.Interfaces;
using ExcelWebApp.Db.Models;
using PersonDb.Repositories;

namespace ExcelWebApp.Db.Repositories
{
    public class PersonsDbRepository : IPersonsRepository
    {
        private readonly DatabaseContext databaseContext;
        public PersonsDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public PersonsDbRepository()
        {
        }

        public List<Person> GetAllPersons()
        {
            return databaseContext.Persons.ToList();
        }

        public void AddPersons(List<Person> persons)
        {
            databaseContext.Persons.AddRange(persons);
            databaseContext.SaveChanges();
        }

        public Person GetPersonById(Guid id)
        {
            return databaseContext.Persons.FirstOrDefault(p => p.Id == id);
        }

    }
}
