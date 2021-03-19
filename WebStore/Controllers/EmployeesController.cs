using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Staff")]
    public class EmployeesController : Controller
    {
        private readonly List<Employee> _Employees;

        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }

        //[Route("all")]
        public IActionResult Index() => View(_Employees);

        //[Route("info-(id-{id})")]
        public IActionResult Details(int Id)
        {
            var employee = _Employees.FirstOrDefault(e => e.Id == Id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }
    }
}
