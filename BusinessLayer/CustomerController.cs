using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhumlaKamnandiProject.BusinessLayer;
using PhumlaKamnandiProject.DatabaseLayer;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public class CustomerController
    {

        CustomerDB customerDB;
        Collection<Customer> customers;

        #region Properties
        public Collection<Customer> AllCustomers
        {
            get
            {
                return customers;
            }
        }
        #endregion
        public CustomerController()
        {

            customerDB = new CustomerDB();
            customers = customerDB.AllCustomers;
        }

        #region Database Communication
        public void DataMaintenance(Customer aCust, DB.DBOperation operation)
        {
            int index = 0;
            //perform a given database operation to the dataset in meory; 
            customerDB.DataSetChange(aCust, operation);

            //perform operations on the collection
            switch (operation)
            {
                case DB.DBOperation.Add:
                    //*** Add the employee to the Collection
                    customers.Add(aCust);
                    break;
                case DB.DBOperation.Edit:
                    index = FindIndex(aCust);
                    customers[index] = aCust;  // replace employee at this index with the updated employee
                    break;
                case DB.DBOperation.Delete:
                    index = FindIndex(aCust);  // find the index of the specific employee in collection
                    customers.RemoveAt(index);  // remove that employee form the collection
                    break;
            }

        }

        //***Commit the changes to the database
        public bool FinalizeChanges(Customer customer)
        {
            //***call the EmployeeDB method that will commit the changes to the database
            return customerDB.UpdateDataSource(customer);
        }
        #endregion

        #region Search Methods

        public Customer Find(string customerID)
        {
            int index = 0;
            bool found = (customers[index].CustomerID == customerID);  //check if it is the first student
            int count = customers.Count;
            while (!(found) && (index < customers.Count - 1))  //if not "this" student and you are not at the end of the list 
            {
                index = index + 1;
                found = (customers[index].CustomerID == customerID);   // this will be TRUE if found
            }
            return customers[index];  // this is the one!  
        }

        public int FindIndex(Customer aCust)
        {
            int counter = 0;
            bool found = false;
            found = (aCust.CustomerID == customers[counter].CustomerID);   //using a Boolean Expression to initialise found
            while (!(found) & counter < customers.Count - 1)
            {
                counter += 1;
                found = (aCust.CustomerID == customers[counter].CustomerID);
            }
            if (found)
            {
                return counter;
            }
            else
            {
                return -1;
            }
        }
        #endregion
    }
}
