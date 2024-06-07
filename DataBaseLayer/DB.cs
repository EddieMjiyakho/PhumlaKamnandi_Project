using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Windows.Forms;
using PhumlaKamnandiProject.Properties;


namespace PhumlaKamnandiProject.DatabaseLayer
{

    public class DB
    {
        private string strConn = Settings.Default.PhumlaKamnandiConnectionString;
        protected SqlConnection cnMain;
        protected DataSet dsMain;
        protected SqlDataAdapter daMain;

        protected string aSQLstring;
        public enum DBOperation
        {
            Add = 0,
            Edit = 1,
            Delete = 2
        }
        public DB()
        {
            try
            {

                cnMain = new SqlConnection(strConn);
                dsMain = new DataSet();
            }
            catch (SystemException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Error");
                return;
            }
        }

        public void FillDataSet(string aSQLstring, string aTable)
        {

            try
            {
                daMain = new SqlDataAdapter(aSQLstring, cnMain);
                cnMain.Open();

                daMain.Fill(dsMain, aTable);
                cnMain.Close();
            }
            catch (Exception errObj)
            {
                MessageBox.Show(errObj.Message + "  " + errObj.StackTrace);
            }
        }

        protected bool UpdateDataSource(string sqlLocal, string table)
        {
            bool success;
            try
            {

                cnMain.Open();
                //***update the database table via the data adapter
                daMain.Update(dsMain, table);
                //---close the connection
                cnMain.Close();
                //refresh the dataset
                FillDataSet(sqlLocal, table);
                success = true;
            }
            catch (Exception errObj)
            {
                MessageBox.Show(errObj.Message + "  " + errObj.StackTrace);
                success = false;
            }
            finally
            {
            }
            return success;
        }
    }
}
