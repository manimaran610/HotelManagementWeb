using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace HotelManagementWeb.Models
{
    public class HelperClass
    {



        public static List<Room> AvailableRoomList(CheckInOut model)
        {
            using (var database = new HMSContext())
            {
                List<Room> ListOfRooms = database.Rooms.ToList();
                BookedRoomsList(database, model).ForEach(room => ListOfRooms.RemoveAll(item => item.RoomId == room.AssignRoomId));
                foreach (var item in ListOfRooms)
                {
                    item.Type = database.RoomTypes.ToList().Find(eachItem => eachItem.RoomTypeId == item.RoomTypeId).RoomType;
                    item.CheckIn = model.CheckIn;
                    item.CheckOut = model.CheckOut;
                }

                return ListOfRooms;

            }

        }

        static List<Booking> BookedRoomsList(HMSContext database, CheckInOut model)
        {

            return database.Bookings.ToList().FindAll(item => (model.CheckIn.Date >= item.BookingFrom.Date && model.CheckIn.Date <= item.BookingTo.Date) ||
                (model.CheckOut.Date >= item.BookingFrom.Date && model.CheckOut.Date <= item.BookingTo.Date) ||
                model.CheckIn.Date <= item.BookingFrom.Date && model.CheckOut.Date >= item.BookingTo.Date);


        }


        public static List<Booking> IndividualUserBookings(string UserEmail)

        {
            using (var database = new HMSContext())
            {
                var UserBookingsList = database.Bookings.ToList().FindAll(item => item.CustomerEmail == UserEmail);
                var ListOfRooms = database.Rooms.ToList();
                foreach (var item in UserBookingsList)
                {
                    item.RoomNumber = ListOfRooms.Find(room => room.RoomId == item.AssignRoomId).RoomNumber;
                    item.QRCode = GenerateQRCode($"Name : {item.CustomerName},\nCheckIn : {item.BookingFrom.ToShortDateString()}," +
                        $"\nCheckOut : {item.BookingTo.ToShortDateString()},\nRoomNo : {item.RoomNumber}," +
                        $"\nAmount Paid:{item.TotalAmount}");
                }
                return UserBookingsList;

            }
        }
        public static string GenerateQRCode(string QRString)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRString, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap bitMap = qrCode.GetGraphic(20);
                bitMap.Save(ms, ImageFormat.Png);
                return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
            }
        }
    }

}

