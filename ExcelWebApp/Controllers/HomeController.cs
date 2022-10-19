using ExcelWebApp.Db.Interfaces;
using ExcelWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExcelWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonsRepository personsRepository;

        public HomeController(IPersonsRepository personsRepository)
        {
            this.personsRepository = personsRepository;
        }

        public IActionResult Index()
        {
            var personsDB = personsRepository.GetAllPersons();
            var personsVM = Helpers.MappingPersonDbToPersonVM(personsDB);
            var maleFemaleList = Helpers.GetMaleListFromPersons(personsVM);
            var male = maleFemaleList.FirstOrDefault(x => x.Gender == "М")?.Count;
            var female = maleFemaleList.FirstOrDefault(x => x.Gender == "Ж")?.Count;
            ViewBag.Male = male;
            ViewBag.Female = female;
            return View(personsVM);
        }
     
        [HttpPost]
        public IActionResult AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                List<PersonViewModel> newPersons = new List<PersonViewModel>();
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = new MemoryStream())
                {
                    uploadedFile.CopyTo(stream);
                    Helpers.TrySafeFromExcel(uploadedFile, ModelState, newPersons);
                }
                var newPersonsDB = Helpers.MappingPersonVMToPersonDb(newPersons);
                personsRepository.AddPersons(newPersonsDB);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}