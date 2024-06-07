using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using PhumlaKamnandiProject.BusinessLayer;
using PhumlaKamnandiProject.DatabaseLayer;

namespace PhumlaKamnandiProject.DatabaseLayer 
{
    public class HotelDB : DB
    {

        private string table1 = "Hotels";
        private string sqlLocal1 = "SELECT * FROM Hotels";


        private Collection<Hotel> hotels;


        public struct ColumnAttribs
        {
            public string myName;
            public SqlDbType myType;
            public int mySize;
        }


        public HotelDB() : base()
        {
            hotels = new Collection<Hotel>();
            FillDataSet(sqlLocal1, table1);
            Add2Collection(table1);

        }
        public Collection<Hotel> AllHotels
        {
            get
            {
                return hotels;
            }
        }
        public DataSet GetDataSet()
        {
            return dsMain;
        }

        #region Database Operations CRUD --- Add the object's values to the database
        public void DataSetChange(Hotel aHotel, DB.DBOperation operation)
        {
            DataRow aRow = null;
            string dataTable = table1;
             

            switch (operation)
            {
                case DB.DBOperation.Add:
                    aRow = dsMain.Tables[dataTable].NewRow();
                    FillRow(aRow, aHotel, operation);
                    //Add to the dataset
                    dsMain.Tables[dataTable].Rows.Add(aRow);
                    break;
                case DB.DBOperation.Edit:
                    // to Edit
                    aRow = dsMain.Tables[dataTable].Rows[FindRow(aHotel, dataTable)];
                    FillRow(aRow, aHotel, operation);
                    break;
            }
        }
        #endregion

        #region Utility Methods
        private void Add2Collection(string table)
        {
            //Declare references to a myRow object and an Employee object
            DataRow myRow = null;
            Hotel aHotel;

            //READ from the table  
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    //Instantiate a new Employee object
                    aHotel = new Hotel();

                    aHotel.HotelID = Convert.ToString(myRow["HotelID"]).TrimEnd();
                    aHotel.HotelName = Convert.ToString(myRow["HotelName"]).TrimEnd();
                    aHotel.SignInDate = (System.DateTime)(myRow["SignInDate"]);
                    aHotel.TotalNumOfRooms = Convert.ToInt32(myRow["TotalNumOfRooms"]);
                    aHotel.SignOutDate = (System.DateTime)(myRow["SignOutDate"]);
                    hotels.Add(aHotel);
                }
            }
        }

        private void FillRow(DataRow aRow, Hotel aHotel, DB.DBOperation operation)
        {
            if (operation == DB.DBOperation.Add)
            {
                aRow["HotelID"] = aHotel.HotelID;  //square brackets to indicate index of collections of fields in row.
            }

            aRow["HotelName"] = aHotel.HotelName;
            aRow["SignInDate"] = aHotel.SignInDate;
            aRow["TotalNumOfRooms"] = aHotel.TotalNumOfRooms;
            aRow["SignOutDate"] = aHotel.SignOutDate;
        }

        //The FindRow method finds the row for a specific employee(by ID)  in a specific table
        private int FindRow(Hotel aHotel, string table)
        {
            int rowIndex = 0;
            DataRow myRow;
            int returnValue = -1;
            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    if (aHotel.HotelID == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["HotelID"]))
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
        private void Build_INSERT_Parameters(Hotel aHotel)
        {
            //Create Parameters to communicate with SQL INSERT
            SqlParameter param = default(SqlParameter);
            param = new SqlParameter("@HotelID", SqlDbType.NVarChar, 20, "HotelID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@HotelName", SqlDbType.NVarChar, 50, "HotelName");
            daMain.InsertCommand.Parameters.Add(param);

            //Do the same for Description & answer -ensure that you choose the right size
            param = new SqlParameter("@SignInDate", SqlDbType.DateTime, 20, "SignInDate");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@TotalNumOfRooms", SqlDbType.Money, 6, "TotalNumOfRooms");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@SignOutDate", SqlDbType.DateTime, 20, "SignOutDate");
            daMain.InsertCommand.Parameters.Add(param);

        }

        private void Build_UPDATE_Parameters(Hotel aHotel)
        {
            //---Create Parameters to communicate with SQL UPDATE
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@HotelName", SqlDbType.NVarChar, 50, "HotelName");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            //Do for all fields other than ID and EMPID as for Insert 
            param = new SqlParameter("@SignInDate", SqlDbType.DateTime, 20, "SignInDate");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@TotalNumOfRooms", SqlDbType.Money, 6, "TotalNumOfRooms");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@SignOutDate", SqlDbType.DateTime, 20, "SignOutDate");
            param.SourceVersion = DataRowVersion.Current;
            daMain.UpdateCommand.Parameters.Add(param);

            param = new SqlParameter("@Original_HotelID", SqlDbType.NVarChar, 20, "HotelID");
            param.SourceVersion = DataRowVersion.Original;
            daMain.UpdateCommand.Parameters.Add(param);
        }

        private void Create_INSERT_Command(Hotel aHotel)
        {
            //Create the command that must be used to insert values into the Books table..
            daMain.InsertCommand = new SqlCommand("INSERT into Hotel (HotelID, HotelName, SignIntDate, TotalNumOfRooms, SignOutDate) VALUES (@HotelID, @HotelName, @SignInDate, @TotalNumOfRooms, @SignOutDate)", cnMain);
            Build_INSERT_Parameters(aHotel);

        }

        private void Create_UPDATE_Command(Hotel aHotel)
        {
            //Create the command that must be used to insert values into one of the three tables
            //Assumption is that the ID and EMPID cannot be changed

            daMain.UpdateCommand = new SqlCommand("UPDATE Hotel SET HotelName = @HotelName, SignIntDate = @SignIntDate, TotalNumOfRooms = @TotalNumOfRooms, SignOutDate = @SignOutDate  " + "WHERE HotelID = @Original_HotelID", cnMain);

            Build_UPDATE_Parameters(aHotel);
        }

        public bool UpdateDataSource(Hotel aHotel)
        {
            bool success = true;
            Create_INSERT_Command(aHotel);
            Create_UPDATE_Command(aHotel);
            success = UpdateDataSource(sqlLocal1, table1);
            return success;
        }
        #endregion

        #region Data Reader for reporting
        public DataTable ReadDataTotalNumOfRooms()
        {
            //Declare references (for table, reader and command)
            DataTable quantyReportTable = new DataTable();
            SqlDataReader reader;
            SqlCommand command;
            string selectString = "select Hotel.TotalNumOfRooms, count(TotalNumOfRooms) as quantyReportTable from Hotel group by TotalNumOfRooms ";
            try
            {
                command = new SqlCommand(selectString, cnMain);
                cnMain.Open();  //open the connection
                command.CommandType = CommandType.Text;//Command Type
                reader = command.ExecuteReader(); //Read from table

                //  read data from readerObject and load in table 
                quantyReportTable.Load(reader);
                reader.Close();
                cnMain.Close();
                return quantyReportTable;
            }

            catch
            {
                return (null);
            }
        }


        #endregion

    }
}

