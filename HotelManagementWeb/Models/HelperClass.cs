
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Hosting;
using System.Drawing.Imaging;
using System.IO;




namespace HotelManagementWeb.Models
{
    public class HelperClass
    {



        public static List<Room> AvailableRooms(CheckInOut model)
        {
            List<Room> ListOfRooms = HotelDatabaseLayer.GetRooms();
            BookedRooms(model).ForEach(room => ListOfRooms.RemoveAll(item => item.RoomId == room.AssignRoomId));
            foreach (var item in ListOfRooms)
            {
                item.Type = HotelDatabaseLayer.GetRoomTypes().Find(eachItem => eachItem.RoomTypeId == item.RoomTypeId).RoomType;
                item.CheckIn = model.CheckIn;
                item.CheckOut = model.CheckOut;
            }
            return ListOfRooms;
        }


        static List<Booking> BookedRooms(CheckInOut model)
        {

            return HotelDatabaseLayer.GetBookings().FindAll(item => (model.CheckIn.Date >= item.BookingFrom.Date && model.CheckIn.Date <= item.BookingTo.Date) ||
                (model.CheckOut.Date >= item.BookingFrom.Date && model.CheckOut.Date <= item.BookingTo.Date) ||
                model.CheckIn.Date <= item.BookingFrom.Date && model.CheckOut.Date >= item.BookingTo.Date);


        }


        public static List<Booking> IndividualUserBookings(string UserEmail)

        { 
                var UserBookingsList = HotelDatabaseLayer.GetBookings().FindAll(item => item.CustomerEmail == UserEmail);
                var ListOfRooms = HotelDatabaseLayer.GetRooms();
                foreach (var item in UserBookingsList)
                {
                    item.RoomNumber = ListOfRooms.Find(room => room.RoomId == item.AssignRoomId).RoomNumber;
                    item.QRCode = GenerateQRCode($"Name : {item.CustomerName},\nCheckIn : {item.BookingFrom.ToShortDateString()}," +
                        $"\nCheckOut : {item.BookingTo.ToShortDateString()},\nRoomNo : {item.RoomNumber}," +
                        $"\nAmount Paid:{item.TotalAmount}");
                }
                return UserBookingsList;

            
        }

        public static string AddImageToProjectFolder(Room model)
        {
            Random randomnumber = new Random();
            string ImageName = model.RoomNumber + randomnumber.Next() + Path.GetExtension(model.UploadedImage.FileName);
            model.UploadedImage.SaveAs(HostingEnvironment.MapPath("~/Content/RoomImages/" + ImageName));
            return ImageName;
        }

        public static void DeleteExistingImageInProjectFolder(string RoomImage)
        {
            FileInfo file = new FileInfo(HostingEnvironment.MapPath("~/Content/RoomImages/" + RoomImage));
            file.Delete();
        }

            public static Room FillDropDownListItems(Room model)
            {
                if (model != null)
                {
                    model.RoomTypeList = HotelDatabaseLayer.GetRoomTypes();
                    model.BookStatusList = HotelDatabaseLayer.GetBookingStatus();
                    return model;
                }
                else
                {
                    return model;
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


        public static List<Room> GetRoomsForDashboard()
        {
            List<Room> ListOfRooms = HotelDatabaseLayer.GetRooms();
            var RoomTypes = HotelDatabaseLayer.GetRoomTypes();
            var BookingStatus = HotelDatabaseLayer.GetBookingStatus();
            foreach (var IndividualRoom in ListOfRooms)
            {
                IndividualRoom.Type = RoomTypes.Find(option => option.RoomTypeId == IndividualRoom.RoomTypeId).RoomType;
                IndividualRoom.Status = BookingStatus.Find(option => option.BookingStatusId == IndividualRoom.BookingStatusId).Status;
                var model = ListOfRooms.Find(item => item.RoomId == IndividualRoom.RoomId);
                model = IndividualRoom;
            }
            return ListOfRooms;
        }
    }

}

