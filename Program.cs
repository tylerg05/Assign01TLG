using System;
using NLog.Web;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Assign01TLG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program started");

            try
            {
                // DB
                var db = new EventContext();

                // Generate Locations
                if (!db.Locations.Any())
                {
                    db.Locations.Add(new Location { Name = "Living Room" });
                    db.Locations.Add(new Location { Name = "Bedroom" });
                    db.Locations.Add(new Location { Name = "Bathroom" });
                    db.SaveChanges();
                    Console.WriteLine("Locations added");
                }

                // Generate Events

                if (!db.Events.Any()) {
                    Random gen = new Random();
                    // for each month June (6) through November (11)
                    for (int m = 6; m <= 11; m++) {
                        // for each day in month
                        for (int j = 1; j <= DateTime.DaysInMonth(2020, m); j++)
                        {
                            // number of events per day
                            int i = gen.Next(0,6);
                            // day loop
                            for (var k = 0; k < i; k++ ) {
                                // generate time and location
                                Event newEvent = new Event();
                                DateTime dt = new DateTime(2020, m, j, gen.Next(0, 24), gen.Next(0, 60), gen.Next(0, 60));
                                Console.WriteLine(dt);

                                // create event
                                int randLoc = gen.Next(1,4);
                                newEvent.Location = db.Locations.Where(b => b.LocationId == randLoc).FirstOrDefault();
                                Console.WriteLine(newEvent.Location.Name);
                                newEvent.TimeStamp = dt;
                                newEvent.Flagged = false;

                                // Add to DB
                                db.AddEvent(newEvent);
                            }
                        }
                    }
                    Console.WriteLine("Events added");
                }

                // Display all Events from the database
                var query = db.Events.OrderBy(b => b.EventId);

                Console.WriteLine("");
                Console.WriteLine("All events in the database:");
                foreach (var item in query)
                {
                    var itemLocation = db.Locations.Where(b => b.LocationId == item.LocationId).FirstOrDefault();
                    Console.WriteLine(item.TimeStamp + " - " + itemLocation.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
