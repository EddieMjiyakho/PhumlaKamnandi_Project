using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhumlaKamnandilProject.BusinessLayer;
using PhumlaKamnandiProject.DatabaseLayer;


namespace PhumlaKamnandiProject.BusinessLayer
{
    public class EmployeeController
    {

        EmployeeDB employeeDB;
        Collection<Employee> employees;

        #region Properties
        public Collection<Employee> AllEmployees
        {
            get
            {
                return employees;
            }
        }
        #endregion
        public EmployeeController()
        {

            employeeDB = new EmployeeDB();
            employees = employeeDB.AllEmployees;
        }

        #region Database Communication
        public void DataMaintenance(Employee anEmp, DB.DBOperation operation)
        {
            int index = 0;
            //perform a given database operation to the dataset in meory; 
            employeeDB.DataSetChange(anEmp, operation);

            //perform operations on the collection
            switch (operation)
            {
                case DB.DBOperation.Add:
                    //*** Add the employee to the Collection
                    employees.Add(anEmp);
                    break;
                case DB.DBOperation.Edit:
                    index = FindIndex(anEmp);
                    employees[index] = anEmp;  // replace employee at this index with the updated employee
                    break;

            }

        }

        //***Commit the changes to the database
        public bool FinalizeChanges(Room item)
        {
            //***call the EmployeeDB method that will commit the changes to the database
            return employeeDB.UpdateDataSource(item);
        }
        #endregion

        #region Search Methods
        
        public Collection<Room> FindEmployeeID(Collection<Room> rooms, string bookingID)
        {
            Collection<Room> matches = new Collection<Room>();

            foreach (Room item in rooms)
            {
                if (item.BookingID == bookingID)
                {
                    matches.Add(item);
                }
            }
            return matches;
        }

        public Collection<Employee> FindEmpID(string empID)
        {
            Collection<Employee> matches = new Collection<Employee>();

            foreach (Employee eachEmployee in employees)
            {
                if (eachEmployee.EmployeeID == empID)
                {
                    matches.Add(eachEmployee);
                }
            }
            return matches;
        }
        //This method receives a employee ID as a parameter; finds the employee object in the collection of employees and then returns this object
        public Employee Find(string empID)
        {
            int index = 0;
            bool found = (employees[index].EmployeeID == empID);  //check if it is the first student
            int count = employees.Count;
            while (!(found) && (index < employees.Count - 1))  //if not "this" student and you are not at the end of the list 
            {
                index = index + 1;
                found = (employees[index].EmployeeID == empID);   // this will be TRUE if found
            }
            return employees[index];  // this is the one!  
        }

        public int FindIndex(Employee anEmployee)
        {
            int counter = 0;
            bool found = false;
            found = (anEmployee.EmployeeID == employees[counter].EmployeeID);   //using a Boolean Expression to initialise found
            while (!(found) & counter < employees.Count - 1)
            {
                counter += 1;
                found = (anEmployee.EmployeeID == employees[counter].EmployeeID);
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
