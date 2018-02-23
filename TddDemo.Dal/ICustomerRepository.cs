using System.Collections.Generic;
using TddDemo.Models;

namespace TddDemo.Dal
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomer(int id);
        bool AddCustomer(Customer myCustomer);
        Customer UpdateCustomer(Customer updatedCustomer);
        bool DeleteCustomer(int id);
    }
}