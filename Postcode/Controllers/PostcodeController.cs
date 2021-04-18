using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Postcode.Dto;
using Postcode.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postcode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostcodeController : ControllerBase
    {
        private readonly PostcodeService _postcodeService;

        public PostcodeController(PostcodeService postcodeService)
        {
            _postcodeService = postcodeService;

        }

        [HttpPost]
        public async Task<IActionResult> GetPostcodes([FromBody] List<string> postcodes)
        {
            try
            {
                if (postcodes.Count == 0)
                {
                    var errorDto = new ErrorDto
                    {
                        Status = 422,
                        Error = "UnprocessableEntity",
                        Message = "A list with at least one postcode is mandatory"
                    };

                    return StatusCode(422, errorDto);
                }


                var postcodeCoordinateDto = await _postcodeService.GetPostcodes(postcodes);

                return Ok(postcodeCoordinateDto);

            }

            catch (Exception ex)
            {
                var errorDto = new ErrorDto
                {
                    Status = 500,
                    Error = ex.ToString(),
                    Message = ex.Message
                };

                return BadRequest(errorDto);
            }
        }

        [HttpGet("{postcode?}")]
        public async Task<IActionResult> GetPostcode(string postcode)
        {

            try
            {

                //postcode is mandatory
                if (string.IsNullOrEmpty(postcode))
                {
                    var errorDto = new ErrorDto
                    {
                        Status = 422,
                        Error = "UnprocessableEntity",
                        Message = "The postcode is mandatory"
                    };

                    return StatusCode(422, errorDto);
                }

                var postcodeCoordinateDto = await _postcodeService.GetPostcode(postcode);

                if (postcodeCoordinateDto == null)
                {
                    var errorDto = new ErrorDto
                    {
                        Status = 404,
                        Error = "NotFound",
                        Message = "You've entered an invalid postcode"
                    };

                    return StatusCode(404, errorDto);
                }


                return Ok(postcodeCoordinateDto);

            }
            catch (Exception ex)
            {
                var errorDto = new ErrorDto
                {
                    Status = 500,
                    Error = ex.ToString(),
                    Message = ex.Message
                };

                return BadRequest(errorDto);
            }


        }
    }
}
