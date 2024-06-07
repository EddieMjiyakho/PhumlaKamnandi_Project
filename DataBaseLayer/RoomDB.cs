using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using PhumlaKamnandilProject.BusinessLayer;

namespace PhumlaKamnandiProject.DatabaseLayer
{
    public class RoomDB : DB
    {

        #region Data members        
        private string table1 = "Room";
        private string sqlLocal1 = "SELECT * FROM Rooms";
        private Collection<Room> rooms;
        #endregion


        #region properties
        public Collection<Room> AllBookingRooms
        {
            get
            {
                return rooms;
            }

        }
        #endregion


        public struct ColumnAttribs
        {
            public string myName;
            public SqlDbType myType;
            public int mySize;
        }


        public RoomDB() : base()
        {
            rooms = new Collection<Room>();
            FillDataSet(sqlLocal1, table1);
            Add2Collection(table1);

        }

        public DataSet GetDataSet()
        {
            return dsMain;
        }

        public void DataSetChange(Room aRoom, DB.DBOperation operation)
        {
            DataRow aRow = null;
            string dataTable = table1;
            switch (operation)
            {
                case DB.DBOperation.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();
                    FillRow(aRow, aRoom, operation);
                    //Add to the dataset
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DB.DBOperation.Edit:
                    // to Edit
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aRoom, dataTable)];
                    FillRow(aRow, aRoom, operation);
                    break;

                case DB.DBOperation.Delete:
                    //to delete
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aRoom, dataTable)];
                    aRow.Delete();
                    break;
            }
        }


        #region Utility Methods
        private void Add2Collection(string table)
        {
            //Declare references to a myRow object and an Employee object
            DataRow myRow = null;
            Room item;

            //READ from the table  
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    //Instantiate a new Customer object
                    item = new Room();
                    //Obtain each customer attribute from the specific field in the row in the table
                    item.RoomID = Convert.ToString(myRow["RoomID"]).TrimEnd();
                    item.BookingID = Convert.ToString(myRow["BookingID"]).TrimEnd(); ;
                    item.HotelID = Convert.ToString(myRow["HotelID"]).TrimEnd();
                    item.NumOfRooms = Convert.ToInt32(myRow["NumOfRooms"]);
                    // add to collection
                    rooms.Add(item);
                }
            }
        }

        private void FillRow(DataRow aRow, Room aRoom, DB.DBOperation operation)
        {
            if (operation == DB.DBOperation.Add)
            {
                aRow["RoomID"] = aRoom.RoomID;  //NOTE square brackets to indicate index of collections of fields in row.
            }

            aRow["BookingID"] = aRoom.BookingID;
            aRow["HotelID"] = aRoom.HotelID;
            aRow["NumOfRooms"] = aRoom.NumOfRooms;
        }


        //The FindRow method finds the row for a specific employee(by ID)  in a specific table
        private int FindRow(Room item, string table)
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
                    if (item.RoomID == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["RoomID"]))
                    {
                        returnValue = rowIndex;
                    }
                }
                rowIndex += 1;
            }
            return returnValue;
        }

        //ADDED BY TONNY 
        #region Data reader for reporting
        public DataTable ReadDataRoomSpilt()
        {
            //Declare references (for table, reader and command)
            DataTable salesReportTable = new DataTable();
            SqlDataReader reader;
            SqlCommand command;
            string selectString = "select Room.HotelID, count(HotelID) as salesGroupTotal from Room group by HotelID ";
            try
            {
                command = new SqlCommand(selectString, cnMain);
                cnMain.Open();  //open the connection
                command.CommandType = CommandType.Text;//Command Type
                reader = command.ExecuteReader(); //Read from table

                //  read data from readerObject and load in table 
                salesReportTable.Load(reader);
                reader.Close();
                cnMain.Close();
                return salesReportTable;
            }


            catch (Exception errObj)
            {
                String errorString = errObj.Message + "  " + errObj.StackTrace;
                return null;
            }

        }

        #endregion


        // Build Parameters, Create Commands & Update database
        private void Build_INSERT_Parameters(Room aRoom)
        {
            //Create Parameters to communicate with SQL INSERT
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@RoomID", SqlDbType.NVarChar, 20, "RoomID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@BookingID", SqlDbType.NVarChar, 20, "BookingID");
            daMain.InsertCommand.Parameters.Add(param);

            //Do the same for Description & answer -ensure that you choose the right size
            param = new SqlParameter("@HotelID", SqlDbType.NVarChar, 20, "HotelID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@NumOfRooms", SqlDbType.Int, 6, "NumOfRooms");
            daMain.InsertCommand.Parameters.Add(param);

        }

        private void Build_UPDATE_Parameters(Room aRoom)
        {
            //---Create Parameters to communicate with SQL UPDATE
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@BookingID", SqlDbType.NVarChar, 20, "BookingID");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@HotelID", SqlDbType.NVarChar, 20, "HotelID");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@NumOfRooms", SqlDbType.Int, 6, "NumOfRooms");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            //testing the ID of record that needs to change with the original ID of the record
            param = new SqlParameter("@Original_RoomID", SqlDbType.NVarChar, 20, "RoomID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);
        }


        private void Build_DELETE_Parameters()
        {
            //--Create Parameters to communicate with SQL DELETE
            SqlParameter param;
            param = new SqlParameter("@RoomID", SqlDbType.NVarChar, 20, "RoomID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.DeleteCommand.Parameters.Add(param);
        }

        private void Create_INSERT_Command(Room aRoom)
        {
            //Create the command that must be used to insert values into the Books table..
            daMain.InsertCommand = new SqlCommand("INSERT into Room (RoomID, BookingID, HotelID, NumOfRooms) VALUES (@RoomID, @BookingID, @HotelID, @NumOfRooms)", cnMain);
            Build_INSERT_Parameters(aRoom);

        }

        private void Create_UPDATE_Command(Room aRoom)
        {
            //Create the command that must be used to insert values into one of the three tables
            //Assumption is that the ID and EMPID cannot be changed

            daMain.UpdateCommand = new SqlCommand("UPDATE Room SET BookingID = @BookingID, HotelID = @HotelID, NumOfRooms = @NumOfRooms " + "WHERE RoomID = @Original_RoomID", cnMain);

            Build_UPDATE_Parameters(aRoom);
        }

        private string Create_DELETE_Command(Room aRoom)
        {
            string errorString = null;

            //Create the command that must be used to delete values from the the Customer table
            daMain.DeleteCommand = new SqlCommand("DELETE FROM Room WHERE RoomID = @RoomID", cnMain);


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

        public bool UpdateDataSource(Room aRoom)
        {
            bool success = true;
            Create_INSERT_Command(aRoom);
            Create_UPDATE_Command(aRoom);
            Create_DELETE_Command(aRoom);
            success = UpdateDataSource(sqlLocal1, table1);

            return success;
        } 
        #endregion
    }
}
