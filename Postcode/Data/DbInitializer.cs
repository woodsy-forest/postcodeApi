using Microsoft.EntityFrameworkCore;
using Postcode.Models;
using System.Linq;

namespace Postcode.Data
{
    public class DbInitializer
    {
        public static void Initialize(postcodeContext context)
        {

            context.Database.Migrate();

            /*
            if (!context.PostcodeCoordinates.Any())
            {
                var tmp = new PostcodeCoordinate
                {
                    Postcode = "EX1 1NT",
                    Longitude = -3.525432,
                    Latitude = 50.721837
                };

                context.PostcodeCoordinates.Add(tmp);
                context.SaveChanges();
            }
            */

        }
    }
}
