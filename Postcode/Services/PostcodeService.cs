using Postcode.Data;
using Postcode.Dto;
using Postcode.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Postcode.Constants;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Postcode.Services
{
    public class PostcodeService
    {
        static readonly HttpClient client = new HttpClient();
        private readonly postcodeContext _context;

        public PostcodeService(postcodeContext context)
        {
            _context = context;
        }

        public async Task<List<PostcodeCoordinateDto>> GetPostcodes(List<string> postcodes)
        {

            var postcodeCoordinatesDto = new List<PostcodeCoordinateDto>();
            var postcodesDto = new PostcodesDto();

            foreach(var postcode in postcodes)
            {
                var postcodeCoordinateDto = await ExistPostcode(postcode);
                if (postcodeCoordinateDto != null)
                    postcodeCoordinatesDto.Add(postcodeCoordinateDto);
                else
                {
                    //add postcode to the list
                    postcodesDto.Postcodes.Add(postcode);

                }
            }

            if (postcodesDto.Postcodes.Count > 0)
            {
                var postcodeBulkIoDto = await GetPostcodeBulkIo(postcodesDto);
                
                foreach (var bulkResultDto in postcodeBulkIoDto.Result)
                {
                    if (bulkResultDto.Result != null)
                    {
                        var postcodeCoordinate = new PostcodeCoordinate
                        {
                            Postcode = bulkResultDto.Result.Postcode,
                            Latitude = bulkResultDto.Result.Latitude,
                            Longitude = bulkResultDto.Result.Longitude

                        };
                        //cache result
                        var postcodeCoordinateDto = await CachePostcode(postcodeCoordinate);
                        //add result to listDto
                        postcodeCoordinatesDto.Add(postcodeCoordinateDto);
                    }
                    else
                    {
                        var postcodeCoordinateDto = new PostcodeCoordinateDto
                        {
                            Postcode = bulkResultDto.Query + " invalid",
                            Coordinates = null
                        };
                        postcodeCoordinatesDto.Add(postcodeCoordinateDto);
                    }

                }
            }

            return postcodeCoordinatesDto;

        }

        public async Task<PostcodeCoordinateDto> GetPostcode(string postcode)
        {

            var postcodeCoordinateDto = await ExistPostcode(postcode);
            if (postcodeCoordinateDto == null)
            {
                //get postcode
                var postcodeCoordinate = await GetPostcodeIo(postcode);

                //cache postcode
                if (postcodeCoordinate != null)
                    postcodeCoordinateDto = await CachePostcode(postcodeCoordinate);

            }

            return postcodeCoordinateDto;
        }

        public async Task<PostcodeBulkIoDto> GetPostcodeBulkIo(PostcodesDto postcodesDto)
        {
            var postcodeBulkIoDto = new PostcodeBulkIoDto();

            try
            {
                var response = await client.PostAsJsonAsync(ApiUrl.Postcode, postcodesDto);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    postcodeBulkIoDto = JsonConvert.DeserializeObject<PostcodeBulkIoDto>(responseBody);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetPostcodeBulkIo, error: {ex.ToString()}");
            }

            return postcodeBulkIoDto;

        }

        public async Task<PostcodeCoordinate> GetPostcodeIo(string postcode)
        {
            try
            {
                var response = await client.GetAsync(ApiUrl.Postcode + postcode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var _postcodeCoordinate = JsonConvert.DeserializeObject<PostcodeIoDto>(responseBody);

                    var postcodeCoordinate = new PostcodeCoordinate
                    {
                        Postcode = postcode,
                        Latitude = _postcodeCoordinate.Result.Latitude,
                        Longitude = _postcodeCoordinate.Result.Longitude

                    };
                    return postcodeCoordinate;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetPostcodeIo, error: {ex.ToString()}");
                return null;
            }
        }

        public async Task<PostcodeCoordinateDto> CachePostcode(PostcodeCoordinate postcodeCoordinate)
        {

            await _context.PostcodeCoordinates.AddAsync(postcodeCoordinate);

            await _context.SaveChangesAsync();

            var postcodeCoordinateDto = new PostcodeCoordinateDto
            {
                Postcode = postcodeCoordinate.Postcode,
                Coordinates = new CoordinateDto
                {
                    Latitude = postcodeCoordinate.Latitude,
                    Longitude = postcodeCoordinate.Longitude
                }
            };

            return postcodeCoordinateDto;
        }

        public async Task<PostcodeCoordinateDto> ExistPostcode(string postcode)
        {

            var postcodeCoordinate = await _context.PostcodeCoordinates
                                                .SingleOrDefaultAsync(p => p.Postcode == postcode);

            if (postcodeCoordinate == null)
                return null;
            else
            {
                var postcodeCoordinateDto = new PostcodeCoordinateDto
                {
                    Postcode = postcode,
                    Coordinates = new CoordinateDto
                    {
                        Latitude = postcodeCoordinate.Latitude,
                        Longitude = postcodeCoordinate.Longitude
                    }
                };

                return postcodeCoordinateDto;
            }


        }
    }
}
