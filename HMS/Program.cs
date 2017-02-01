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
        var ctx = new HmsEntities1();
        Program pointerTo = new Program();
        List<DailyReport> dailyReport = pointerTo.DailyRoomsReport();
        foreach (var element in dailyReport)
        {
            Console.WriteLine("Room Id:"+element.RoomId+"\t\tStatus:"+element.Status + "\tDate:" + element.Day);
        }
        Console.ReadKey();
    }

    public void AddGuest(string name, DateTime dob, string phone)
    {
        var context = new HmsEntities1();
        var guest = new Guest() { Name = name, DOB = dob, Phone = phone };
        context.Guests.Add(guest);
        context.SaveChanges();
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
            for (var day = DateTime.Today; day <= DateTime.Today.AddMonths(1); day = day.AddDays(1))
            {
                var dailyReport = new DailyReport() { RoomId = room.RoomId, Day = day, Status = getRoomStatus(ctx, room.RoomId, day) };
                result.Add(dailyReport);
            }
        }

        return result;
    }

    public string getRoomStatus(HmsEntities1 ctx, long roomId, DateTime day)
    {
        ctx = new HmsEntities1();
        var currentBooking = ctx.Booking.Where(p => p.CheckinDate <= day && p.CheckOutDate >= day)
                                .Where(p => p.RoomId == roomId).ToList();
        if (currentBooking.Count() > 1)
            return "Conflict";
        else if (currentBooking.Count() == 1)
            return "occupied";
        else
            return "Available";
    }
}

public class DailyReport
{
    public long RoomId { get; set; }
    public string Status { get; set; }
    public DateTime Day { get; set; }
}
}



