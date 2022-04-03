using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace HotelManagementWeb.Models
{
    public class HotelDatabaseLayer
    {

        protected static HMSContext HotelDatabase = new HMSContext();

        public static List<Room> GetRooms()
        {
            return HotelDatabase.Rooms.ToList();

        }

        public static List<Booking> GetBookings()
        {
            return HotelDatabase.Bookings.ToList();
        }

        public static List<RoomTypes> GetRoomTypes()
        {
            return HotelDatabase.RoomTypes.ToList();
        }

        public static List<BookingStatus> GetBookingStatus()
        {
            return HotelDatabase.BookingStatus.ToList();
        }



    }

    //User database 
    public class UserDatabaseLayer
    {
        private static ApplicationDbContext UserDatabase = new ApplicationDbContext();

        public static List<ApplicationUser> GetAllUsers()
        {
            return UserDatabase.Users.ToList();
        }


        public static void DeleteUser(string Id)
        {
            var ExistingUser = GetAllUsers().Find(user => user.Id == Id);
            if (ExistingUser != null)
            {
                UserDatabase.Users.Remove(ExistingUser);
                UserDatabase.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }


    }


    public class RoomCrudOperations : HotelDatabaseLayer
    {

        public static void AddRoomToDatabase(Room NewRoom)
        {
            if (NewRoom != null) try
                {
                    HotelDatabase.Rooms.Add(NewRoom);
                    HotelDatabase.SaveChanges();
                }
                catch (Exception exception)
                {
                }
        }

        public static void UpdateExistingRoomInDatabase(Room ExistingRoom)
        {
            if (ExistingRoom != null)
                try
                {
                    HotelDatabase.Rooms.AddOrUpdate(ExistingRoom);
                    HotelDatabase.SaveChanges();
                }
                catch (Exception exception)
                {
                }
        }

        public static void DeleteRoomFromDatabase(Room ExistingRoom)
        {
            try
            {

                HotelDatabase.Rooms.Remove(ExistingRoom);
                HotelDatabase.SaveChanges();
            }
            catch (Exception exception)
            {
            }
        }
    }
    public class BookingOperations : HotelDatabaseLayer
    {
        public static void AddNewBookings(Booking model)

        {
            if (model != null)
            {
                HotelDatabase.Bookings.Add(model);
                HotelDatabase.SaveChanges();
            }

        }
        public static void DeleteExistingBookingInDatabase(Booking model)
        {
            if (model != null)
            {
                var ExistingBooking = HotelDatabase.Bookings.Where(item => item.BookingId == model.BookingId).First();
                HotelDatabase.Bookings.Remove(ExistingBooking);
                HotelDatabase.SaveChanges();
            }
        }
    }


}