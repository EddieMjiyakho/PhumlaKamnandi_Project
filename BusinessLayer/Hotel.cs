using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public class Hotel
    {
        #region attributes
        private string hotelID;
        private string hotelName;
        private DateTime signInDate;
        private DateTime signOutDate;
        private int totalNumOfRooms;
       
       
        #endregion

        #region Properties
        public string HotelID
        {
            get
            {
                return hotelID;
            }

            set
            {
                hotelID = value;
            }
        }

        //public enum hotelStatus //if fully booked or not
        //{
        //    availableForBooking = 0,
        //    notAvailableForBooking = 1
       // }

        public string HotelName
        {
            get
            {
                return hotelName;
            }

            set
            {
                hotelName = value;
            }
        }

        public DateTime SignInDate
        {
            get
            {
                return signInDate;
            }

            set
            {
                signInDate = value;
            }
        }

        public DateTime SignOutDate
        {
            get
            {
                return signOutDate;
            }

            set
            {
                signOutDate = value;
            }
        }

        public int TotalNumOfRooms
        {
            get
            {
                return totalNumOfRooms;
            }

            set
            {
                totalNumOfRooms = value;
            }
        }

        

       

        #endregion

        #region constructors
        public Hotel()
        {
            hotelID = "";
            hotelName = "";
            signInDate = default(System.DateTime);
            signOutDate = default(System.DateTime);
            totalNumOfRooms = 0;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            return this.hotelID + ": " + hotelName;
        }
        #endregion

    }
}