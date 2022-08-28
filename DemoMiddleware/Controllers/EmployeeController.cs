using System;
using DemoMiddleware.DTOs;
using DemoMiddleware.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DemoMiddleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController()
        {
        }
        [HttpGet("{id}")]
        public ActionResult<Employee> GetById(int id)
        {
            var employee = new Employee()
            {
                ID = id,
                FirstName = "first name",
                LastName = "last name",
                DateOfBirth = DateTime.Now.AddYears(-31)
            };
            return Ok(employee);
        }
        [HttpPost]
        public IActionResult CreateEmployee([FromBody]EmployeeDTO employeeRequest)
        {
            var employee = new Employee()
            {
                ID = employeeRequest.ID,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                DateOfBirth = DateTime.Now.AddYears(-31)
            };
            return Ok(employee);
        }
    }
}

