using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhumlaKamnandiProject.DatabaseLayer;


namespace PhumlaKamnandiProject.BusinessLayer
{
    public class BookingController
    {

        BookingDB bookingDB;
        Collection<Booking> bookings; 

        #region Properties
        public Collection<Booking> AllBookings
        {
            get
            {
                return bookings;
            }
        }
        #endregion
        public BookingController()
        {

            bookingDB = new BookingDB();
            bookings = bookingDB.AllBookings;
        }

        #region Database Communication
        public void DataMaintenance(Booking aBooking, DB.DBOperation operation)
        {
            int index = 0;
            //perform a given database operation to the dataset in meory; 
            bookingDB.DataSetChange(aBooking, operation);
            //perform operations on the collection
            switch (operation)
            {
                case DB.DBOperation.Add:
                    //*** Add the employee to the Collection
                    bookings.Add(aBooking);
                    break;
                case DB.DBOperation.Edit:
                    index = FindIndex(aBooking);
                    bookings[index] = aBooking;  // replace employee at this index with the updated employee
                    break;
            }
        }

        //***Commit the changes to the database
        public bool FinalizeChanges(Booking aBooking)
        {
            //***call the EmployeeDB method that will commit the changes to the database
            return bookingDB.UpdateDataSource(aBooking);
        }
        #endregion

        #region Search Methods
        //This method  (function) searched through all the employess to finds onlly those with the required role
        public Collection<Booking> FindByStatus(Collection<Booking> bookings, Booking.BookingStatus bookingVal)
        {
            Collection<Booking> matches = new Collection<Booking>();

            foreach (Booking booking in bookings)
            {
                if (booking.BookingValue == bookingVal)
                {
                    matches.Add(booking);
                }
            }
            return matches;
        }

        public Collection<Booking> FindByStatus(Booking.BookingStatus bookingVal)
        {
            Collection<Booking> matches = new Collection<Booking>();

            foreach (Booking booking in bookings)
            {
                if (booking.BookingValue == bookingVal)
                {
                    matches.Add(booking);
                }
            }
            return matches;
        }

        //This method receives a employee ID as a parameter; finds the employee object in the collection of employees and then returns this object
        public Booking Find(string bookingID)
        {
            int index = 0;
            bool found = (bookings[index].BookingID == bookingID);  //check if it is the first student
            int count = bookings.Count;
            while (!(found) && (index < bookings.Count - 1))  //if not "this" student and you are not at the end of the list 
            {
                index = index + 1;
                found = (bookings[index].BookingID == bookingID);   // this will be TRUE if found
            }
            return bookings[index];  // this is the one!  
        }

        public int FindIndex(Booking aBooking)
        {
            int counter = 0;
            bool found = false;
            found = (aBooking.BookingID == bookings[counter].BookingID);   //using a Boolean Expression to initialise found
            while (!(found) & counter < bookings.Count - 1)
            {
                counter += 1;
                found = (aBooking.BookingID == bookings[counter].BookingID);
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