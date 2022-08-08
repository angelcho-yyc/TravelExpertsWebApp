using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelExpertsData
{
    /// <summary>
    /// methods to work with Customer table
    /// </summary>
    public static class CustomerManager
    {
        /// <summary>
        /// authenticate customer by checking userId and password
        /// </summary>
        /// <param name="userId">username</param>
        /// <param name="password">password</param>
        /// <returns>authenticated customer or null if not found</returns>
        public static Customer Authenticate(string userId, string password)
        {
            TravelExpertsContext db = new TravelExpertsContext();
            Customer customer = db.Customers.SingleOrDefault(c => c.UserId == userId && c.Password == password);
            return customer; // return customer if match with database or null if not found
        }

        /// <summary>
        /// get customer by Id
        /// </summary>
        /// <param name="custId">customer Id</param>
        /// <returns>customer or null if not found</returns>
        public static Customer GetCustomer(int custId)
        {
            TravelExpertsContext db = new TravelExpertsContext();
            Customer customer = db.Customers.Find(custId);
            return customer;
        }

        /// <summary>
        /// update customer information
        /// </summary>
        /// <param name="newData">new customer data</param>
        /// <returns>customer with updated information</returns>
        public static Customer UpdateInfo(Customer newData)
        {
            TravelExpertsContext db = new TravelExpertsContext();
            Customer customer = db.Customers.Find(newData.CustomerId);
            customer.CustFirstName = newData.CustFirstName;
            customer.CustLastName = newData.CustLastName;
            customer.CustAddress = newData.CustAddress;
            customer.CustCity = newData.CustCity;
            customer.CustProv = newData.CustProv;
            customer.CustCountry = newData.CustCountry;
            customer.CustPostal = newData.CustPostal;
            customer.CustEmail = newData.CustEmail;
            customer.CustHomePhone = newData.CustHomePhone;
            customer.CustBusPhone = newData.CustBusPhone;
            customer.Password = newData.Password;
            db.SaveChanges();

            return customer;

        }
    }
}
