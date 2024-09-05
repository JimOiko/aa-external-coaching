using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API
{
    [ApiController]
    [Route("/api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAsync()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();

                if (customers == null || !customers.Any())
                {
                    _logger.LogInformation("No customers found.");
                    return NotFound("No customers found.");
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving customers.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetByIdAsync(Guid id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    _logger.LogInformation($"Customer with ID {id} not found.");
                    return NotFound($"Customer with ID {id} not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("getByEmail/{email}")]
        public async Task<ActionResult<Customer>> GetByEmailAsync(string email)
        {
            try
            {
                var customer = await _customerService.GetCustomerByEmailAsync(email);

                if (customer == null)
                {
                    _logger.LogInformation($"Customer with Email {email} not found.");
                    return NotFound($"Customer with Email {email} not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Customer customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Customer data is null.");
            }

            try
            {
                await _customerService.CreateCustomerAsync(customerDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Customer customerDto)
        {
            if (customerDto == null || id != customerDto.Id)
            {
                return BadRequest("Customer data is invalid.");
            }

            try
            {
                var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound($"Customer with ID {id} not found.");
                }
                // Update the existing customer entity with the new data
                existingCustomer.Name = customerDto.Name;
                existingCustomer.Email = customerDto.Email;
                existingCustomer.PhoneNumber = customerDto.PhoneNumber;
                await _customerService.UpdateCustomerAsync(existingCustomer);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteAsync(string email)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(email);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
