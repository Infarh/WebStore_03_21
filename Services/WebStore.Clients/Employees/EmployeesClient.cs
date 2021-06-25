﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WebStore.Clients.Base;
using WebStore.Domain.Models;
using WebStore.Interfaces.Servcies;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        private readonly ILogger<EmployeesClient> _Logger;

        public EmployeesClient(HttpClient Client, ILogger<EmployeesClient> Logger)
            : base(Client, "api/employees") =>
            _Logger = Logger;

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(Address);

        public Employee Get(int id) => Get<Employee>($"{Address}/{id}");

        public Employee GetByName(string LastName, string FirstName, string Patronymic) =>
            Get<Employee>($"{Address}/employee?LastName={LastName}&FirstName={FirstName}&Patronymic={Patronymic}");

        public int Add(Employee employee) => Post(Address, employee).Content.ReadFromJsonAsync<int>().Result;

        public Employee Add(string LastName, string FirstName, string Patronymic, int Age) =>
            Post($"{Address}/employee?LastName={LastName}&FirstName={FirstName}&Patronymic={Patronymic}", "")
               .Content.ReadFromJsonAsync<Employee>().Result;

        public void Update(Employee employee) => Put(Address, employee);

        public bool Delete(int id)
        {
            _Logger.LogInformation("Удаление сотрудника id:{0}...", id);
            var result = Delete($"{Address}/{id}").IsSuccessStatusCode;
            _Logger.LogInformation("Удаление сотрудника id:{0} - {1}",
                id, result ? "выполнено" : "не найден");
            return result;
        }
    }
}