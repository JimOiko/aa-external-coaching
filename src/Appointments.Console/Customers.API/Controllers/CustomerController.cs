using AppointmentManagementSystem.DomainObjects;
using Customers.API.Interfaces;
using Customers.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers
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

        [HttpGet("get", Name = "GetCustomers")]
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


        [HttpGet("getById/{customerId}", Name = "GetCustomerById")]
        public async Task<ActionResult<Customer>> GetByIdAsync(Guid customerId)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(customerId);

                if (customer == null)
                {
                    _logger.LogInformation($"Customer with ID {customerId} not found.");
                    return NotFound($"Customer with ID {customerId} not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("getByEmail/{email}", Name = "GetCustomerByEmail")]
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
        [Route("create")]
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

        [HttpPut("update/{customerId}")]
        public async Task<IActionResult> UpdateAsync(Guid customerId, Customer customerDto)
        {
            if (customerDto == null || customerId != customerDto.Id)
            {
                return BadRequest("Customer data is invalid.");
            }

            try
            {
                var existingCustomer = await _customerService.GetCustomerByIdAsync(customerId);
                if (existingCustomer == null)
                {
                    return NotFound($"Customer with ID {customerId} not found.");
                }
                // Update the existing customer entity with the new data
                existingCustomer.Name = customerDto.Name;
                existingCustomer.Email = customerDto.Email;
                existingCustomer.PhoneNumber = customerDto.PhoneNumber;
                await _customerService.UpdateCustomerAsync(existingCustomer);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("delete/{email}")]
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
