using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public class Booking
    {
        #region attributes
        private string bookingID;
        private string customerID;
        private System.DateTime bookingDate;
        private int totalCost;
        private string specialNote;

        protected BookingStatus bookingVal;

        public enum BookingStatus
        {
            NotBooked = 0,
            Booked = 1
        }
        #endregion


        #region properties
        public string BookingID
        {
            get
            {
                return bookingID;
            }

            set
            {
                bookingID = value;
            }
        }

        public DateTime BookingDate
        {
            get
            {
                return BookingDate;
            }

            set
            {
                BookingDate = value;
            }
        }



        public string SpecialNote
        {
            get
            {
                return specialNote;
            }

            set
            {
                specialNote = value;
            }
        }



        public int TotalCost
        {
            get
            {
                return totalCost;
            }

            set
            {
                totalCost = value;
            }
        }

        public string CustomerID
        {
            get
            {
                return customerID;
            }

            set
            {
                customerID = value;
            }
        }

        public BookingStatus BookingValue
        {
            get
            {
                return bookingVal;
            }
            set
            {
                bookingVal = value;
            }
        }

        #endregion

        #region constructors
        public Booking()
        {
            bookingID = "";
            customerID = "";
            bookingDate = default(System.DateTime);
            totalCost = 0;
            specialNote = "";
            bookingVal = 0;
        }

        public  Booking(string bookingID, string customerID, DateTime bookingDate, int totalCost, string specialNote)
        {
            this.bookingID = bookingID;
            this.customerID = customerID;
            this.bookingDate = bookingDate;
            this.totalCost = totalCost;
            this.specialNote = specialNote;
            bookingVal = 0;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            return this.bookingID;
        }
        #endregion
    }
}