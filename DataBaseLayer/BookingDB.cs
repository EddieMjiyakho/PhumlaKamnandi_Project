using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PhumlaKamnandiProject.BusinessLayer;

namespace PhumlaKamnandiProject.DatabaseLayer
{
    public class BookingDB : DB
    {

        private string table1 = "Bookings";
        private string sqlLocal1 = "SELECT * FROM Bookings";

        private Collection<Booking> bookings;


        public struct ColumnAttribs
        {
            public string myName;
            public SqlDbType myType;
            public int mySize;
        }


        public BookingDB() : base()
        {
            bookings = new Collection<Booking>();
            FillDataSet(sqlLocal1, table1);
            Add2Collection(table1);

        }
        public Collection<Booking> AllBookings
        {
            get
            {
                return bookings;
            }
        }
        public DataSet GetDataSet()
        {
            return dsMain;
        }

        #region Database Operations CRUD --- Add the object's values to the database
        public void DataSetChange(Booking aBooking, DB.DBOperation operation)
        {
            DataRow aRow = null;
            string dataTable = table1;
            //***In this case the dataset change refers to adding to a database table
            //***We now have  3 tables.. once they are placed in an array .. this becomes easier 

            switch (operation)
            {
                case DB.DBOperation.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();
                    FillRow(aRow, aBooking, operation);
                    //Add to the dataset
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DB.DBOperation.Edit:
                    // to Edit
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aBooking, dataTable)];
                    FillRow(aRow, aBooking, operation);
                    break;

                case DB.DBOperation.Delete:
                    //to delete
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aBooking, dataTable)];
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
            Booking aBooking;

            //READ from the table  
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    //Instantiate a new Employee object
                    aBooking = new Booking();

                    aBooking.BookingID = Convert.ToString(myRow["BookingID"]).TrimEnd();
                    aBooking.CustomerID = Convert.ToString(myRow["CustomerID"]).TrimEnd();
                    aBooking.BookingDate = (System.DateTime)(myRow["BookingDate"]);
                    aBooking.TotalCost = Convert.ToInt32(myRow["TotalCost"]);
                    aBooking.SpecialNote = Convert.ToString(myRow["SpecialNote"]).TrimEnd();
                    aBooking.BookingValue = (Booking.BookingStatus)(myRow["BookingStatus"]);
                    bookings.Add(aBooking);
                }
            }
        }

        private void FillRow(DataRow aRow, Booking aBooking, DB.DBOperation operation)
        {
            if (operation == DB.DBOperation.Add)
            {
                aRow["BookingID"] = aBooking.BookingID;  //NOTE square brackets to indicate index of collections of fields in row.
            }

            aRow["CustomerID"] = aBooking.CustomerID;
            aRow["OrderDate"] = aBooking.BookingDate;
            aRow["TotalCost"] = aBooking.TotalCost;
            aRow["SpecialNote"] = aBooking.SpecialNote;
            aRow["OrderStatus"] = (byte)aBooking.BookingValue;
        }

        //The FindRow method finds the row for a specific employee(by ID)  in a specific table
        private int FindRow(Booking aBooking, string table)
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
                    if (aBooking.BookingID == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["BookingID"]))
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
        private void Build_INSERT_Parameters(Booking aBooking)
        {
            //Create Parameters to communicate with SQL INSERT
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@BookingID", SqlDbType.NVarChar, 20, "BookingID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 20, "CustomerID");
            daMain.InsertCommand.Parameters.Add(param);

            //Do the same for Description & answer -ensure that you choose the right size
            param = new SqlParameter("@BookingDate", SqlDbType.DateTime, 100, "BookingDate");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@TotalCost", SqlDbType.Money, 6, "TotalCost");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@SpecialNote", SqlDbType.NVarChar, 200, "SpecialNote");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@BookingStatus", SqlDbType.TinyInt, 1, "BookingStatus");
            daMain.InsertCommand.Parameters.Add(param);

        }

        private void Build_UPDATE_Parameters(Booking aBooking)
        {
            //---Create Parameters to communicate with SQL UPDATE
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 20, "CustomerID");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            //Do for all fields other than ID and EMPID as for Insert 
            param = new SqlParameter("@BookingDate", SqlDbType.DateTime, 100, "BookingDate");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@TotalCost", SqlDbType.Money, 6, "TotalCost");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@SpecialNote", SqlDbType.NVarChar, 200, "SpecialNote");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@BookingStatus", SqlDbType.TinyInt, 1, "BookingStatus");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            //testing the ID of record that needs to change with the original ID of the record
            param = new SqlParameter("@Original_OrderID", SqlDbType.NVarChar, 20, "BookingID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);
        }


        private void Build_DELETE_Parameters()
        {
            //--Create Parameters to communicate with SQL DELETE
            SqlParameter param;
            param = new SqlParameter("@BookingID", SqlDbType.NVarChar, 20, "BookingID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.DeleteCommand.Parameters.Add(param);
        }

        private void Create_INSERT_Command(Booking aBooking)
        {
            //Create the command that must be used to insert values into the Books table..
            daMain.InsertCommand = new SqlCommand("INSERT into Bookings (BookingID, CustomerID, BookingDate, TotalCost, SpecialNote, BookingStatus) VALUES (@BookingID, @CustomerID, @BookingDate, @TotalCost, @SpecialNote, @BookingStatus)", cnMain);
            Build_INSERT_Parameters(aBooking);

        }

        private void Create_UPDATE_Command(Booking aBooking)
        {
            //Create the command that must be used to insert values into one of the three tables
            //Assumption is that the ID and EMPID cannot be changed

            daMain.UpdateCommand = new SqlCommand("UPDATE Bookings SET CustomerID = @CustomerID, BookingDate = @BookingDate, TotalCost = @TotalCost, SpecialNote = @SpecialNote , BookingStatus = @BookingStatus  " + "WHERE BookingID = @Original_BookingID", cnMain);

            Build_UPDATE_Parameters(aBooking);
        }

        private string Create_DELETE_Command(Booking aBooking)
        {
            string errorString = null;

            //Create the command that must be used to delete values from the the Customer table
            daMain.DeleteCommand = new SqlCommand("DELETE FROM Bookings WHERE BookingID = @BookingID", cnMain);


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


        public bool UpdateDataSource(Booking aBooking)
        {
            bool success = true;
            Create_INSERT_Command(aBooking);
            Create_UPDATE_Command(aBooking);
            Create_DELETE_Command(aBooking);
            success = UpdateDataSource(sqlLocal1, table1);
            return success;
        }
        #endregion

    }
}

