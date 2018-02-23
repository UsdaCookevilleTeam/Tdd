using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TddDemo.Dal;
using TddDemo.Models;

namespace TddDemo.Tests
{
    [TestClass]
    public class CustomerRepositoryTest_UnitTests
    {
        public TestContext DemoRepositoryTestContext { get; set; }
        public readonly ICustomerRepository MockDemoCustomerRepo;
        List<Customer> myMockCustomerList;
        Random random = new Random();

        public CustomerRepositoryTest_UnitTests ()
        {
            myMockCustomerList = new List<Customer>()
            {
                new Customer { Id = 1, FirstName = "Test", MiddleName = "User", LastName = "Person1", Email = "tp1@email.com", Phone = "1111111111", Address = "1 Test Address" },
                new Customer { Id = 2, FirstName = "Test", MiddleName = "User", LastName = "Person2", Email = "tp2@email.com", Phone = "2222222222", Address = "2 Test Address" },
                new Customer { Id = 3, FirstName = "Test", MiddleName = "User", LastName = "Person3", Email = "tp3@email.com", Phone = "3333333333", Address = "3 Test Address" },
                new Customer { Id = 4, FirstName = "Test", MiddleName = "User", LastName = "Person4", Email = "tp4@email.com", Phone = "4444444444", Address = "4 Test Address" },
                new Customer { Id = 5, FirstName = "Test", MiddleName = "User", LastName = "Person5", Email = "tp5@email.com", Phone = "5555555555", Address = "5 Test Address" },
                new Customer { Id = 6, FirstName = "Test", MiddleName = "User", LastName = "Person6", Email = "tp6@email.com", Phone = "6666666666", Address = "6 Test Address" },
                new Customer { Id = 7, FirstName = "Test", MiddleName = "User", LastName = "Person7", Email = "tp7@email.com", Phone = "7777777777", Address = "7 Test Address" },
                new Customer { Id = 8, FirstName = "Test", MiddleName = "User", LastName = "Person8", Email = "tp8@email.com", Phone = "8888888888", Address = "8 Test Address" },
                new Customer { Id = 9, FirstName = "Test", MiddleName = "User", LastName = "Person9", Email = "tp9@email.com", Phone = "9999999999", Address = "9 Test Address" },
                new Customer { Id = 10, FirstName = "Test", MiddleName = "User", LastName = "Person10", Email = "tp10@email.com", Phone = "1010101010", Address = "10 Test Address" },
            };

            var myMockCustomerRepo = new Mock<ICustomerRepository>();
            //Get all
            myMockCustomerRepo.Setup(stub => stub.GetAllCustomers()).Returns(myMockCustomerList);
            //Get by id
            myMockCustomerRepo.Setup(stub => stub.GetCustomer(It.IsAny<int>())).Returns((int i) => myMockCustomerList.Where(x => x.Id == i).FirstOrDefault());
            //Add
            myMockCustomerRepo.Setup(stub => stub.AddCustomer(It.IsAny<Customer>())).Returns((Customer target) =>
               {
                   if (target.Id.Equals(default(int)))
                   {
                       target.Id = myMockCustomerList.Count() + 1;
                       myMockCustomerList.Add(target);
                   }
                   return true;
               });
            //Delete
            myMockCustomerRepo.Setup(stub => stub.DeleteCustomer(It.IsAny<int>())).Returns((int id) =>
            {
                myMockCustomerList.RemoveAll(c => c.Id == id);
                return true;
            });
            //Update
            myMockCustomerRepo.Setup(stub => stub.UpdateCustomer(It.IsAny<Customer>())).Returns((Customer target) =>
            {
                 var original = myMockCustomerList.Where(q => q.Id == target.Id).FirstOrDefault();
                 if (original == null)
                 {
                     return target;
                 }

                 original.FirstName = target.FirstName;
                 original.MiddleName = target.MiddleName;
                 original.LastName = target.LastName;
                 original.Email = target.Email; ;
                 original.Phone = target.Phone; ;
                 original.Address = target.Address; ;

                return original;
             });



            MockDemoCustomerRepo = myMockCustomerRepo.Object;
        }

        [TestMethod]
        public void CanReturnAllCustomers()
        {
            var myCustomersTest = new List<Customer>();
            myCustomersTest = MockDemoCustomerRepo.GetAllCustomers();

            Assert.AreNotEqual(0, myCustomersTest.Count);
            Assert.IsNotNull(myCustomersTest);
            foreach (Customer cust in myMockCustomerList)
            {
                Assert.IsInstanceOfType(cust, typeof(Customer));
            }
        }

        [TestMethod]
        public void CanReturnCustomerById()
        {
            var randomInt = random.Next(1, myMockCustomerList.Count());

            Customer myCustomerTest = new Customer();
            myCustomerTest = MockDemoCustomerRepo.GetCustomer(randomInt);

            Assert.AreEqual(randomInt, myCustomerTest.Id);
            Assert.IsNotNull(myCustomerTest);
            Assert.IsInstanceOfType(myCustomerTest, typeof(Customer));
        }

        [TestMethod]
        public void CanInsertCustomer()
        {
            Customer myNewCustomer = new Customer { Id = 0, FirstName = "Test", MiddleName = "Demo", LastName = "User", Email = "testdemouser11@emailcom", Phone = "1101101101", Address = "1111 Some Address" };
            Assert.AreEqual(10, myMockCustomerList.Count());

            bool result = MockDemoCustomerRepo.AddCustomer(myNewCustomer);
            Assert.AreEqual(result, true);
            Assert.AreEqual(11, myMockCustomerList.Count());

            Customer testCustomer = MockDemoCustomerRepo.GetCustomer(11);
            Assert.IsNotNull(testCustomer);
            Assert.IsInstanceOfType(testCustomer, typeof(Customer));
            Assert.AreEqual(11, testCustomer.Id);
        }

        [TestMethod]
        public void CanUpdateCustomer()
        {
            var randomInt = random.Next(1, myMockCustomerList.Count());
            Customer customerToUpdate = MockDemoCustomerRepo.GetCustomer(randomInt);
            customerToUpdate.FirstName = "Updated";

            Customer myUpdatedCustomer = MockDemoCustomerRepo.UpdateCustomer(customerToUpdate);

            Assert.IsNotNull(myUpdatedCustomer);
            Assert.AreEqual(myUpdatedCustomer.FirstName, "Updated");
        }

        [TestMethod]
        public void CanDeleteCustomer()
        {
            var randomInt = random.Next(1, myMockCustomerList.Count());
            int customerCount = MockDemoCustomerRepo.GetAllCustomers().Count();

            Customer myCustomerTest = new Customer();
            bool returnVal = MockDemoCustomerRepo.DeleteCustomer(randomInt);

            Assert.AreEqual(returnVal, true);
            Assert.AreNotEqual(customerCount, myMockCustomerList.Count);
        }
    }


    [TestClass]
    public class CustomerRepositoryTest_IntegrationTests
    {
        [TestMethod]

        public void GetAllCustomers_IntegrationTest()
        {
            CustomerRepository myCustomerRepo = new CustomerRepository();
            List<Customer> myCustomers = new List<Customer>();

            myCustomers = myCustomerRepo.GetAllCustomers();

            Assert.AreNotEqual(0, myCustomers.Count);
            Assert.IsNotNull(myCustomers);
            foreach (Customer cust in myCustomers)
            {
                Assert.IsInstanceOfType(cust, typeof(Customer));
            }
        }

        [TestMethod]
        public void GetCustomerById_IntegrationTest()
        {
            CustomerRepository myCustomerRepo = new CustomerRepository();
            Random myRandom = new Random();
            var myRandomInt = myRandom.Next(1, 5);

            Customer myCustomer = myCustomerRepo.GetCustomer(myRandomInt);

            Assert.IsNotNull(myCustomer);
            Assert.AreEqual(myCustomer.Id, myRandomInt);
            Assert.IsInstanceOfType(myCustomer, typeof(Customer));
        }

        [TestMethod]
        public void AddCustomer_IntegrationTest()
        {
            CustomerRepository myCustomerRepo = new CustomerRepository();
            List<Customer> myCustomers = new List<Customer>();
            myCustomers = myCustomerRepo.GetAllCustomers();
            Random myRandom = new Random();
            var myRandomInt = myRandom.Next(1, myCustomers.Count);

            Customer myNewCustomer = new Customer();
            myNewCustomer.Id = myCustomers.Count() + 1;
            myNewCustomer.FirstName = "Test";
            myNewCustomer.MiddleName = "Demo";
            myNewCustomer.LastName = "User" + myNewCustomer.Id;
            myNewCustomer.Email = "test" + myNewCustomer.Id + "@email.com";
            myNewCustomer.Phone = "1234567890";
            myNewCustomer.Address = myNewCustomer.Id + " Some Test Adress";

            bool result = myCustomerRepo.AddCustomer(myNewCustomer);
            Customer myReturnCustomer = myCustomerRepo.GetCustomer(myNewCustomer.Id);

            Assert.AreEqual(result, true);
            Assert.IsNotNull(myReturnCustomer);
            Assert.AreEqual(myNewCustomer.Id, myReturnCustomer.Id);
        }

        [TestMethod]
        public void UpdateCustomer_IntegrationTest()
        {
            CustomerRepository myCustomerRepo = new CustomerRepository();
            List<Customer> myCustomers = new List<Customer>();
            myCustomers = myCustomerRepo.GetAllCustomers();
            Random myRandom = new Random();
            var myRandomInt = myRandom.Next(1, myCustomers.Count);
            string updatedMiddleName = "Updated";

            Customer myNewCustomer = myCustomerRepo.GetCustomer(myRandomInt);
            myNewCustomer.MiddleName = updatedMiddleName;

            Customer myReturnCustomer = myCustomerRepo.UpdateCustomer(myNewCustomer);

            Assert.IsNotNull(myReturnCustomer);
            Assert.AreEqual(myNewCustomer.Id, myReturnCustomer.Id);
            Assert.AreEqual(myReturnCustomer.MiddleName, updatedMiddleName);
        }
    }
}
