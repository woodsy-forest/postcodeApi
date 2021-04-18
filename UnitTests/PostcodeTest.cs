using Microsoft.EntityFrameworkCore;
using Postcode.Data;
using Postcode.Dto;
using Postcode.Models;
using Postcode.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class PostcodeTest
    {

        private readonly DbContextOptions<postcodeContext> dbContextOptions = new DbContextOptionsBuilder<postcodeContext>()
            .UseInMemoryDatabase(databaseName: "PostcodeDb")
            .Options;

        private readonly PostcodeService _postcodeService;


        public PostcodeTest()
        {
            SeedDb();

            _postcodeService = new PostcodeService(new postcodeContext(dbContextOptions));

            //controller = new ReservationsController(new PrimeDbContext(dbContextOptions));
        }

        private void SeedDb()
        {
            using var context = new postcodeContext(dbContextOptions);

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

        }

        [Fact]
        public void ExistPostcode()
        {
            var postcodeCoordinateDto = _postcodeService.ExistPostcode("EX1 1NT").Result;
            Assert.Equal(-3.525432, postcodeCoordinateDto.Coordinates.Longitude);
            Assert.Equal(50.721837, postcodeCoordinateDto.Coordinates.Latitude);
        }

        [Fact]
        public void GetPostcodeIoValid()
        {
            var postcodeCoordinate = _postcodeService.GetPostcodeIo("EX1 1NT").Result;
            Assert.Equal(-3.525432, postcodeCoordinate.Longitude);
            Assert.Equal(50.721837, postcodeCoordinate.Latitude);
        }

        [Fact]
        public void GetPostcodeIoInValid()
        {
            var postcodeCoordinateDto = _postcodeService.GetPostcodeIo("EX1").Result;
            Assert.Null(postcodeCoordinateDto);
        }

        [Fact]
        public void GetPostcodeBulkIo()
        {
            var postcodes =  new PostcodesDto();
            postcodes.Postcodes.Add("OX49 5NU");
            postcodes.Postcodes.Add("M32 0JG");
            postcodes.Postcodes.Add("NE30 1DP1");
            var postcodeBulkIoDto = _postcodeService.GetPostcodeBulkIo(postcodes).Result;
            Assert.Equal(3, postcodeBulkIoDto.Result.Count);
            foreach(var tmp in postcodeBulkIoDto.Result)
            {
                switch (tmp.Query)
                {
                    case "OX49 5NU":
                        Assert.Equal(51.655929, tmp.Result.Latitude);
                        Assert.Equal(-1.069752, tmp.Result.Longitude);
                        break;

                    case "NE30 1DP1":
                        Assert.Null(tmp.Result);
                        break;
                }
                
            }
           

        }


    }
}
