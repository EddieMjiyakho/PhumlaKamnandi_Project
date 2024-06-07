using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using PhumlaKamnandiProject.BusinessLayer;
using PhumlaKamnandilProject.BusinessLayer;

namespace PhumlaKamnandiProject.DatabaseLayer
{
    public class EmployeeDB : DB
    {

        #region Data members        
        private string table1 = "Employees";
        private string sqlLocal1 = "SELECT * FROM Employees";
        private Collection<Employee> employees;
        #endregion


        #region properties
        public Collection<Employee> AllEmployees
        {
            get
            {
                return employees;
            }

        }
        #endregion


        public struct ColumnAttribs
        {
            public string myName;
            public SqlDbType myType;
            public int mySize;
        }

        public EmployeeDB() : base()
        {
            employees = new Collection<Employee>();
            FillDataSet(sqlLocal1, table1);
            Add2Collection(table1);

        }

        public DataSet GetDataSet()
        {
            return dsMain;
        }

        public void DataSetChange(Employee anEmp, DB.DBOperation operation)
        {
            DataRow aRow = null;
            string dataTable = table1;

            switch (operation)
            {
                case DB.DBOperation.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();
                    FillRow(aRow, anEmp, operation);
                    //Add to the dataset
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DB.DBOperation.Edit:
                    // to Edit
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(anEmp, dataTable)];
                    FillRow(aRow, anEmp, operation);
                    break;

                case DB.DBOperation.Delete:
                    //to delete
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(anEmp, dataTable)];
                    aRow.Delete();
                    break;
            }
        }


        #region Utility Methods
        private void Add2Collection(string table)
        {
            //Declare references to a myRow object and an Employee object
            //Declare references to a myRow object and an Employee object
            DataRow myRow = null;
            Employee anEmployee;

            //READ from the table  
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    //Instantiate a new Employee object
                    anEmployee = new Employee();

                    anEmployee.EmployeeID = Convert.ToString(myRow["EmpID"]).TrimEnd();
                    anEmployee.Name = Convert.ToString(myRow["Name"]).TrimEnd(); ;
                    anEmployee.Phone = Convert.ToString(myRow["Phone"]).TrimEnd();
                    anEmployee.RoleValue = (Employee.Role)Convert.ToByte((myRow["Role"])); 
                    employees.Add(anEmployee);
                }
            }
        }

        private void FillRow(DataRow aRow, Employee anEmployee, DB.DBOperation operation)
        {
            if (operation == DB.DBOperation.Add)
            {
                aRow["EmpID"] = anEmployee.EmployeeID;  //NOTE square brackets to indicate index of collections of fields in row.
            }
            aRow["Name"] = anEmployee.Name;
            aRow["Phone"] = anEmployee.Phone;
            aRow["Role"] = (int)anEmployee.RoleValue;
        }


        //The FindRow method finds the row for a specific employee(by ID)  in a specific table
        private int FindRow(Employee anEmployee, string table)
        {
            int rowIndex = 0;
            DataRow myRow;
            int returnValue = -1;
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                //Ignore rows marked as deleted in dataset
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    //In c# there is no item property (but we use the 2-dim array) it is automatically known to the compiler when used as below
                    if (anEmployee.EmployeeID == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["EmpID"]))
                    {
                        returnValue = rowIndex;
                    }
                }
                rowIndex += 1;
            }
            return returnValue;
        }
        #endregion


        #region Build Parameters, Create Commands & Update database

        private void Build_UPDATE_Parameters(Room aItem)
        {
            //---Create Parameters to communicate with SQL UPDATE
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@Name", SqlDbType.NVarChar, 20, "Name");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Phone", SqlDbType.NVarChar, 10, "Phone");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("Role", SqlDbType.Int, 1, "Role");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            //testing the ID of record that needs to change with the original ID of the record
            param = new SqlParameter("@Original_EmpID", SqlDbType.NVarChar, 15, "EmpID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);
        }


        private void Create_UPDATE_Command(Room aItem)
        {
            //Create the command that must be used to insert values into one of the three tables
            //Assumption is that the ID and EMPID cannot be changed

            daMain.UpdateCommand = new SqlCommand("UPDATE Employees SET Name = @Name, Phone = @Phone, Role = @Role " + "WHERE EmpID = @Original_EmpID", cnMain);

            Build_UPDATE_Parameters(aItem);
        }


        public bool UpdateDataSource(Room aItem)
        {
            bool success = true;
            Create_UPDATE_Command(aItem);
            success = UpdateDataSource(sqlLocal1, table1);

            return success;
        }
        #endregion
    }
}
