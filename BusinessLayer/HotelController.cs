using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhumlaKamnandiProject.DatabaseLayer;

namespace PhumlaKamnandiProject.BusinessLayer
{
    public class HotelController
    {

        HotelDB hotelDB;
        Collection<Hotel> hotels;

        #region Properties
        public Collection<Hotel> AllHotels
        {
            get
            {
                return hotels;
            }
        }
        #endregion
        public HotelController()
        {

            hotelDB = new HotelDB();
            hotels = hotelDB.AllHotels;
        }

        #region Database Communication
        public void DataMaintenance(Hotel aHotel, DB.DBOperation operation)
        {
            int index = 0;
            
            hotelDB.DataSetChange(aHotel, operation);
           
            switch (operation)
            {
                case DB.DBOperation.Add:
                   
                    hotels.Add(aHotel);
                    break;
                case DB.DBOperation.Edit:
                    index = FindIndex(aHotel);
                    hotels[index] = aHotel;  
                    break;
            }
        }

       
        public bool FinalizeChanges(Hotel aHotel)
        {
           
            return hotelDB.UpdateDataSource(aHotel);
        }
        #endregion

        #region Search Methods

        /*public Collection<Hotel> FindByStatus(Collection<Hotel> hotels, Hotel.hotelStatus hotelVal)
        {
            Collection<Hotel> matches = new Collection<Hotel>();

            foreach (Hotel hotel in hotels)
            {
                if (Hotel.hotelStatus.notAvailableForBooking == hotelVal)   
                {
                    if (hotel.ExpiryDate < System.DateTime.Now) { matches.Add(hotel); }

                }
                else
                {
                    if (hotel.SignInDate > System.DateTime.Now) { matches.Add(hotel); }
                }
            }
            return matches;
        }

        public Collection<Hotel> FindByStatus(Hotel.hotelStatus hotelVal)
        {
            Collection<Hotel> matches = new Collection<Hotel>();

            foreach (Hotel hotel in hotels)
            {
                if (Hotel.hotelStatus.notAvailableForBooking == hotelVal)   
                {
                    if (hotel.SignInDate < System.DateTime.Now) { matches.Add(hotel); }

                }
                else
                {
                    if (hotel.SignInDate > System.DateTime.Now) { matches.Add(hotel); }
                }
            }
            return matches;
        } */


        public Hotel Find(string hotelID)
        {
            int index = 0;
            bool found = (hotels[index].HotelID == hotelID);  
            int count = hotels.Count;
            while (!(found) && (index < hotels.Count - 1))  
            {
                index = index + 1;
                found = (hotels[index].HotelID == hotelID);   
            }
            return hotels[index];    
        }

        public int FindIndex(Hotel aHotel)
        {
            int counter = 0;
            bool found = false;
            found = (aHotel.HotelID == hotels[counter].HotelID);   //using a Boolean Expression to initialise found
            while (!(found) & counter < hotels.Count - 1)
            {
                counter += 1;
                found = (aHotel.HotelID == hotels[counter].HotelID);
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