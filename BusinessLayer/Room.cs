using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhumlaKamnandiProject.BusinessLayer.Employee;
using static PhumlaKamnandiProject.BusinessLayer.Hotel;

namespace PhumlaKamnandilProject.BusinessLayer
{
    public class Room
    {
        #region attributes
        private string roomID; //room number or roomID ?
        private string bookingID;
        private string hotelID;
        private string hotelName;
        private int numOfRooms; //number of avail rooms
        private RoomType roomTypeVal;
        private double price;
        private roomStatus roomValue;
        #endregion

        #region properties

        public enum roomStatus
        {
            unAvailable = 0,
            available = 1
        }

        public enum RoomType
        {
            classic = 0,
            deluxe = 1


        }

        public RoomType RoomTypeVal
        {
            get
            {
                return roomTypeVal;
            }

            set
            {
                roomTypeVal = value;
            }
        }

        public roomStatus RoomValue
        {
            get
            {
                return roomValue;
            }

            set
            {
                roomValue = value;
            }
        }
        public string RoomID
        {
            get
            {
                return roomID;
            }

            set
            {
                roomID = value;
            }
        }

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

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

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

        public int NumOfRooms
        {
            get
            {
                return numOfRooms;
            }

            set
            {
                numOfRooms = value;
            }
        }

        #endregion

        #region constructors
        public Room()
        {
            roomID = "";
            bookingID = "";
            hotelID = "";
            hotelName = "";
            numOfRooms = 0;
            roomTypeVal = RoomType.classic ;
            price = 0;
            roomValue = 0;
        }

        public Room(string roomNumber, string BookingID, string hotelID, int numOfRooms, string hotelName, RoomType roomTypeVal)
        {
            this.roomID = roomNumber;
            this.bookingID = BookingID;
            this.hotelID = hotelID;
            this.hotelName = hotelName;
            this.numOfRooms = numOfRooms;
            this.roomTypeVal = roomTypeVal;

        }
        #endregion

    }
}
