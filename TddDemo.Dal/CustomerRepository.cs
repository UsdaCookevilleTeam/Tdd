using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TddDemo.Models;

namespace TddDemo.Dal
{
    public class CustomerRepository : ICustomerRepository
    {
        Demo_TddEntities context = new Demo_TddEntities();

        public List<Customer> GetAllCustomers()
        {
            List<Customer> myCustomers = new List<Customer>();

            myCustomers = (from c in context.Customers
                           orderby c.Id
                           select c).ToList();

            return myCustomers;
        }

        public Customer GetCustomer(int id)
        {
            Customer myCustomer = new Customer();

            myCustomer = context.Customers
                        .Where(c => c.Id == id)
                        .FirstOrDefault<Customer>();

            return myCustomer;
        }

        public bool AddCustomer(Customer myCustomer)
        {
            Customer myNewCustomer = new Customer();

            myNewCustomer.Id = myCustomer.Id;
            myNewCustomer.FirstName = myCustomer.FirstName;
            myNewCustomer.MiddleName = myCustomer.MiddleName;
            myNewCustomer.LastName = myCustomer.LastName;
            myNewCustomer.Email = myCustomer.Email;
            myNewCustomer.Phone = myCustomer.Phone;
            myNewCustomer.Address = myCustomer.Address;

            context.Customers.Add(myNewCustomer);
            context.SaveChanges();

            return true;
        }

        public Customer UpdateCustomer(Customer updatedCustomer)
        {
            Customer myCustomer = (from c in context.Customers
                                    where c.Id == updatedCustomer.Id
                                    select c).FirstOrDefault();

            if (myCustomer != null)
            {
                myCustomer.FirstName = !String.IsNullOrEmpty(updatedCustomer.FirstName) ? updatedCustomer.FirstName : myCustomer.FirstName;
                myCustomer.MiddleName = !String.IsNullOrEmpty(updatedCustomer.MiddleName) ? updatedCustomer.MiddleName : myCustomer.MiddleName;
                myCustomer.LastName = !String.IsNullOrEmpty(updatedCustomer.LastName) ? updatedCustomer.LastName : myCustomer.LastName;
                myCustomer.Email = !String.IsNullOrEmpty(updatedCustomer.Email) ? updatedCustomer.Email : myCustomer.Email;
                myCustomer.Phone = !String.IsNullOrEmpty(updatedCustomer.Phone) ? updatedCustomer.Phone : myCustomer.Phone;
                myCustomer.Address = !String.IsNullOrEmpty(updatedCustomer.Address) ? updatedCustomer.Address : myCustomer.Address;

                context.SaveChanges();
            }

            return myCustomer;
        }

        public bool DeleteCustomer(int id)
        {
            using (context)
            {
                Customer myCustomer = (from c in context.Customers
                                       where c.Id == id
                                       select c).FirstOrDefault();

                if (myCustomer != null)
                {
                    context.Customers.Remove(myCustomer);
                    context.SaveChanges();

                    return true;
                }
            }

            return false;
        }
    }
}
