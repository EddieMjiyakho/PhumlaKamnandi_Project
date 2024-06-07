using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using PhumlaKamnandiProject.BusinessLayer;


namespace PhumlaKamnandiProject.DatabaseLayer
{
    public class CustomerDB : DB
    {

        #region Data members        
        private string table1 = "Customers";
        private string sqlLocal1 = "SELECT * FROM Customers";
        public Collection<Customer> customers;
        #endregion

        #region properties
        public Collection<Customer> AllCustomers
        {
            get
            {
                return customers;
            }

        }
        #endregion

        public struct ColumnAttribs
        {
            public string myName;
            public SqlDbType myType;
            public int mySize;
        }


        public CustomerDB() : base()
        {
            customers = new Collection<Customer>();
            FillDataSet(sqlLocal1, table1);
            Add2Collection(table1);

        }

        public DataSet GetDataSet()
        {
            return dsMain;
        }

        #region Database Operations CRUD --- Add the object's values to the database
        public void DataSetChange(Customer aCust, DB.DBOperation operation)
        {
            DataRow aRow = null;
            string dataTable = table1;
            //***In this case the dataset change refers to adding to a database table
            //***We now have  3 tables.. once they are placed in an array .. this becomes easier 

            switch (operation)
            {
                case DB.DBOperation.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();
                    FillRow(aRow, aCust, operation);
                    //Add to the dataset
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DB.DBOperation.Edit:
                    // to Edit
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aCust, dataTable)];
                    FillRow(aRow, aCust, operation);
                    break;
                case DB.DBOperation.Delete:
                    //to delete
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aCust, dataTable)];
                    aRow.Delete();
                    break;
            }
        }
        #endregion

        #region Utility Methods
        private void Add2Collection(string table)
        {
            //Declare references to a myRow object and an Employee object
            DataRow myRow = null;
            Customer aCust;

            //READ from the table  
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    //Instantiate a new Customer object
                    aCust = new Customer();
                    //Obtain each customer attribute from the specific field in the row in the table
                    aCust.CustomerID = Convert.ToString(myRow["CustomerID"]).TrimEnd();
                    aCust.Name = Convert.ToString(myRow["CustomerName"]).TrimEnd(); ;
                    aCust.Surname = Convert.ToString(myRow["CustomerSurname"]).TrimEnd();
                    aCust.Phone = Convert.ToString(myRow["CustomerPhone"]).TrimEnd();
                    aCust.IDNumber1 = long.Parse((myRow["CustomerIDNumber"]).ToString());
                    aCust.AccountStatus = Convert.ToString(myRow["CustomerAccountStatus"]).TrimEnd();
                    aCust.CustomerAddress = Convert.ToString(myRow["CustomerAddress"]).TrimEnd();

                    // add to collection
                    customers.Add(aCust);
                }
            }
        }
        private void FillRow(DataRow aRow, Customer aCust, DB.DBOperation operation)
        {
            //Customer aCust;

            if (operation == DB.DBOperation.Add)
            {
                aRow["CustomerID"] = aCust.CustomerID;  //NOTE square brackets to indicate index of collections of fields in row.
            }

            aRow["CustomerName"] = aCust.Name;
            aRow["CustomerSurname"] = aCust.Surname;
            aRow["CustomerPhone"] = aCust.Phone;
            aRow["CustomerIDNumber"] = aCust.IDNumber1;
            aRow["CustomerAccountStatus"] = aCust.AccountStatus;
            aRow["CustomerAddress"] = aCust.CustomerAddress;
            //*** For each role add the specific data variables

        }

        //The FindRow method finds the row for a specific employee(by ID)  in a specific table
        private int FindRow(Customer aCust, string table)
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
                    if (aCust.CustomerID == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["CustomerID"]))
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
        private void Build_INSERT_Parameters(Customer aCust)
        {
            //Create Parameters to communicate with SQL INSERT
            //https://www.google.co.za/webhp?sourceid=chrome-instant&ion=1&espv=2&ie=UTF-8#q=size+in+bytes+of+Int+in+SQL
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 20, "CustomerID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerName", SqlDbType.NVarChar, 50, "CustomerName");
            daMain.InsertCommand.Parameters.Add(param);

            //Do the same for Description & answer -ensure that you choose the right size
            param = new SqlParameter("@CustomerSurname", SqlDbType.NVarChar, 50, "CustomerSurname");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerPhone", SqlDbType.NVarChar, 10, "CustomerPhone");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerIDNumber", SqlDbType.NVarChar, 10, "CustomerIDNumber");
            daMain.InsertCommand.Parameters.Add(param);


            param = new SqlParameter("@CustomerAccountStatus", SqlDbType.TinyInt, 1, "CustomerAccountStatus");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerAddress", SqlDbType.NVarChar, 50, "CustomerAddress");
            daMain.InsertCommand.Parameters.Add(param);


            //***https://msdn.microsoft.com/en-za/library/ms179882.aspx
        }

        private void Build_UPDATE_Parameters(Customer aCust)
        {
            //---Create Parameters to communicate with SQL UPDATE
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@CustomerName", SqlDbType.NVarChar, 50, "CustomerName");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerSurname", SqlDbType.NVarChar, 50, "CustomerSurname");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerPhone", SqlDbType.NVarChar, 10, "CustomerPhone");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerIDNumber", SqlDbType.NVarChar, 10, "CustomerIDNumber");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerAccountStatus", SqlDbType.TinyInt, 1, "CustomerAccountStatus");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerAddress", SqlDbType.NVarChar, 50, "CustomerAddress");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            //testing the ID of record that needs to change with the original ID of the record
            param = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 20, "CustomerID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);
        }

        private void Build_DELETE_Parameters()
        {
            //--Create Parameters to communicate with SQL DELETE
            SqlParameter param;
            param = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 20, "CustomerID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.DeleteCommand.Parameters.Add(param);
        }
        private void Create_INSERT_Command(Customer aCust)
        {
            //Create the command that must be used to insert values into the Books table..
            daMain.InsertCommand = new SqlCommand("INSERT into Customers (CustomerID, CustomerName,CustomerSurname, CustomerPhone, CustomerIDNumber, CustomerCurrentCredit, CustomerCreditStatus , CustomerAddress) VALUES (@CustomerID, @CustomerName, @CustomerSurname, @CustomerPhone, @CustomerIDNumber, @CustomerCurrentCredit, @CustomerCreditStatus, @CustomerAddress)", cnMain);
            Build_INSERT_Parameters(aCust);
        }

        private void Create_UPDATE_Command(Customer aCust)
        {
            //Create the command that must be used to insert values into one of the three tables
            //Assumption is that the ID and EMPID cannot be changed

            daMain.UpdateCommand = new SqlCommand("UPDATE Customers SET CustomerName =@CustomerName, CustomerSurname = @CustomerSurname, CustomerPhone =@CustomerPhone, CustomerIDNumber = @CustomerIDNumber, CustomerCurrentCredit = @CustomerCurrentCredit, CustomerCreditStatus = @CustomerCreditStatus, CustomerAddress =  @CustomerAddress " + "WHERE CustomerID = @CustomerID", cnMain);
            Build_UPDATE_Parameters(aCust);

        }

        private string Create_DELETE_Command(Customer aCust)
        {
            string errorString = null;

            //Create the command that must be used to delete values from the the Customer table
            daMain.DeleteCommand = new SqlCommand("DELETE FROM Customers WHERE CustomerID = @CustomerID", cnMain);


            try
            {
                Build_DELETE_Parameters();
            }
            catch (Exception errObj)
            {
                errorString = errObj.Message + "  " + errObj.StackTrace;
            }
            return errorString;
        }

        public bool UpdateDataSource(Customer aCust)
        {
            bool success = true;
            Create_INSERT_Command(aCust);
            Create_UPDATE_Command(aCust);
            Create_DELETE_Command(aCust);
            success = UpdateDataSource(sqlLocal1, table1);

            return success;
        }
        #endregion

    }
}