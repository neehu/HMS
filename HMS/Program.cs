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
            //Guest
            var ctx = new HmsEntities1();
            Console.WriteLine("Enter the guest id");
            var id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the guest name");
            var name = Console.ReadLine();
            Console.WriteLine("Enter the DOB");
            var dob = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Enter the PHONE");
            var phone = Console.ReadLine();
            var guest = new Guest() { GuestID = id, Name = name, DOB = dob, Phone = phone };
            ctx.Guests.Add(guest);

            Console.WriteLine("Enter the id of guest to be removed ");
            var ID = Convert.ToInt16(Console.ReadLine());
            var result = from id1 in ctx.Guests
                         where id1.GuestID == ID
                         select id;
            //foreach (var result2 in result)
            //    ctx.Guests.Remove(Convert.ToString(result2));

            //BOOKING

            Console.WriteLine("Enter the Check In Date:");
            DateTime ArrivalDate = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Enter the Check Out Date:");
            DateTime DepartureDate = Convert.ToDateTime(Console.ReadLine());
            
            var insert = new Booking() { CheckinDate = ArrivalDate, CheckOutDate = DepartureDate };
            ctx.Bookings.Add(insert);

            var month = from bookings in ctx.Bookings
                        where bookings.StatusId !=1
                        select bookings;

            var check = from r in ctx.Rooms
                        select r;

            DateTime test = DateTime.Now.AddMonths(1);

            int index = 0;
            int index2 = 0;
            foreach (var value in check)
            {

                DateTime[] days = new DateTime[31];
                index2++;
                index = 0;
                for (DateTime compareDate = DateTime.Now; compareDate <= test; compareDate = compareDate.AddDays(1))
                {
                    foreach (var val in month)
                    {
                        if (val.Room.RoomId == index2)
                        {
                            if (compareDate.CompareTo(val.CheckinDate) > 0 && compareDate.CompareTo(val.CheckOutDate) < 0)
                                compareDate = compareDate.AddDays(1);
                            else
                                break;
                        }
                        else
                            days[index] = compareDate;
                    }
                    index++;
                }
                Console.WriteLine("The availability for Room {0} for a month is : ", index2);
                for (int i = 0; i < 31; i++)
                {
                    Console.Write(days[i]);
                }
                Console.WriteLine("");
            }
                Console.ReadKey();
                Console.WriteLine();
                ctx.SaveChanges();
                }
            }
        }
    


