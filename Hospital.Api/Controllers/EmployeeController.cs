using Hospital.Core.DTOs.Common;
using Hospital.Core.DTOs.Employee;
using Hospital.Core.Entities;
using Hospital.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // -------------------------------------------------------------
        // PAGINATION + SEARCH + FILTERS
        // GET: api/employee?page=1&pageSize=10&search=ahmed&role=doctor&hiredAfter=2022-01-01
        // -------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? role = null,
            [FromQuery] DateTime? hiredAfter = null,
            [FromQuery] DateTime? hiredBefore = null)
        {
            var result = await _employeeService.GetPagedAsync(
                page,
                pageSize,
                search,
                role,
                hiredAfter,
                hiredBefore
            );

            return Ok(result);
        }

        // GET: api/employee/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            return Ok(employee);
        }

        // ----------------------------------------------------
        // SEARCH ONLY endpoint (optional)
        // api/employee/search?term=whatever
        // ----------------------------------------------------
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term cannot be empty.");

            var results = await _employeeService.SearchAsync(term);
            return Ok(results);
        }

        // POST: api/employee
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateDto dto)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,
                HireDate = DateTime.UtcNow
            };

            var created = await _employeeService.CreateAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/employee/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            if (dto.FirstName != null) employee.FirstName = dto.FirstName;
            if (dto.LastName != null) employee.LastName = dto.LastName;
            if (dto.Email != null) employee.Email = dto.Email;
            if (dto.Phone != null) employee.Phone = dto.Phone;
            if (dto.Role != null) employee.Role = dto.Role;

            var result = await _employeeService.UpdateAsync(id, employee);
            return result ? NoContent() : BadRequest();
        }

        // DELETE: api/employee/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
