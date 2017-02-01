using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS
{
    class Program
    {
        static void Main(string[] args)
        {
            Program pointerTo = new Program();
            List<DailyReport> report = new List<DailyReport>();
            report = pointerTo.DailyRoomsReport();
            foreach (var element in report)
            {
                Console.WriteLine("Date:");
                Console.WriteLine(Convert.ToString(element.Day));
                Console.WriteLine("RoomID:");
                Console.WriteLine(Convert.ToString(element.RoomId));
                Console.WriteLine(element.Status);
            }
            Console.ReadKey();

        }

        public void AddGuest(string name, DateTime dob, string phone)
        {
            var ctx = new HmsEntities1();
            var guest = new Guest() { Name = name, DOB = dob, Phone = phone };
            ctx.Guests.Add(guest);
            ctx.SaveChanges();
        }

        public void RemoveGuest(int guestId)
        {
            var ctx = new HmsEntities1();
            var bookingsToBeRemove = ctx.Booking.Where(o => o.GuestId == guestId).ToArray();
            ctx.Booking.RemoveRange(bookingsToBeRemove);
            var guestToRemove = ctx.Guests.Where(o => o.GuestID == guestId).FirstOrDefault();
            if (guestToRemove != null)
            {
                ctx.Guests.Remove(guestToRemove);
            }
            ctx.SaveChanges();
        }

        public List<DailyReport> DailyRoomsReport()
        {
            var ctx = new HmsEntities1();
            var result = new List<DailyReport>();

            foreach (var room in ctx.Rooms.OrderBy(o => o.RoomId))
            {
                for (var day = DateTime.Today; day <= DateTime.Today.AddMonths(1); day=day.AddDays(1))
                {
                    var dailyReport = new DailyReport() { RoomId = room.RoomId, Day = day };
                    dailyReport.Status = getRoomStatus(ctx, dailyReport.RoomId, dailyReport.Day);
                    result.Add(dailyReport);
                }
            }

            return result;
        }

        public string getRoomStatus(HmsEntities1 ctx, long roomId, DateTime day)
        {

            var currentBooking = ctx.Booking.Where(p => !(p.CheckinDate >= day && p.CheckOutDate <= day))
                                   .Where(p => p.RoomId == roomId).ToList();
            if (currentBooking.Count() > 1)
            {
                return "Conflict";
            }
            if (currentBooking.Count() == 1)
            {
                return "Available";
            }
            else
                return "NotAvailable";

        }
    }

    public class DailyReport
    {
        public long RoomId { get; set; }
        public string Status { get; set; }
        public DateTime Day { get; set; }
    }
}



