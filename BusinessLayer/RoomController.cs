using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhumlaKamnandilProject.BusinessLayer;
using PhumlaKamnandiProject.DatabaseLayer;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public class RoomController
    {

        RoomDB roomsDB;
        HotelDB hotelDB;
        Collection<Room> rooms;

        #region Properties
        public Collection<Room> AllBookingRooms
        {
            get
            {
                return rooms;
            }
        }
        #endregion
        public RoomController()
        {

            roomsDB = new RoomDB();
            rooms = roomsDB.AllBookingRooms;
        }

        #region Database Communication
        public void DataMaintenance(Room items, DB.DBOperation operation)
        {
            int index = 0;
            //perform a given database operation to the dataset in meory; 
            roomsDB.DataSetChange(items, operation);

            //perform operations on the collection
            switch (operation)
            {
                case DB.DBOperation.Add:
                    
                    rooms.Add(items);
                    break;
                case DB.DBOperation.Edit:
                    index = FindIndex(items);
                    rooms[index] = items;  
                    break;
                case DB.DBOperation.Delete:
                    index = FindIndex(items);  // find the index of the specific employee in collection
                    rooms.RemoveAt(index);  
                    break;
            }

        }

        
        public bool FinalizeChanges(Room item)
        {
           
            return roomsDB.UpdateDataSource(item);
        }
        #endregion

        #region Search Methods
        
        public Collection<Room> FindByBookingID(Collection<Room> rooms, string bookingID)
        {
            Collection<Room> matches = new Collection<Room>();

            foreach (Room item in rooms)
            {
                if (item.BookingID == bookingID)
                {
                    matches.Add(item);
                }
            }
            return matches;
        }

        public Collection<Room> FindByBookingID(string bookingID)
        {
            Collection<Room> matches = new Collection<Room>();

            foreach (Room item in rooms)
            {
                if (item.BookingID == bookingID)
                {
                    matches.Add(item);
                }
            }
            return matches;
        }
        
        public Room Find(string roomID)
        {
            int index = 0;
            bool found = (rooms[index].RoomID == roomID);  
            int count = rooms.Count;
            while (!(found) && (index < rooms.Count - 1))   
            {
                index = index + 1;
                found = (rooms[index].RoomID == roomID);   
            }
            return rooms[index];    
        }

        public int FindIndex(Room item)
        {
            int counter = 0;
            bool found = false;
            found = (item.RoomID == rooms[counter].RoomID);   
            while (!(found) & counter < rooms.Count - 1)
            {
                counter += 1;
                found = (item.RoomID == rooms[counter].RoomID);
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
