using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesApiController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

        [HttpGet] // http://localhost:5001/api/employees
        public IEnumerable<Employee> Get() => _EmployeesData.Get();
        
        [HttpGet("{id}")] // http://localhost:5001/api/employees/5
        public Employee Get(int id) => _EmployeesData.Get(id);
        
        [HttpGet("employee")] // http://localhost:5001/api/employees/employee?LastName=Иванов&FirstName=Иван&Patronymic=Иванович
        public Employee GetByName(string LastName, string FirstName, string Patronymic) => _EmployeesData.GetByName(LastName, FirstName, Patronymic);
        
        [HttpPost]
        public int Add(Employee employee) => _EmployeesData.Add(employee);

        [HttpPost("employee")] // post -> http://localhost:5001/api/employees/employee?LastName=Иванов&FirstName=Иван&Patronymic=Иванович&Age=37
        public Employee Add(string LastName, string FirstName, string Patronymic, int Age) => _EmployeesData.Add(LastName, FirstName, Patronymic, Age);
        
        //[HttpPut("{id}")]// put -> http://localhost:5001/api/employees/5
        [HttpPut]// put -> http://localhost:5001/api/employees/5
        public void Update(/*int id, */Employee employee) => _EmployeesData.Update(employee);
        
        [HttpDelete("{id}")]
        public bool Delete(int id) => _EmployeesData.Delete(id);
    }
}
