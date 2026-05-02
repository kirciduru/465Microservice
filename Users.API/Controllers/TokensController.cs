using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.APP.Features.Tokens;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public TokensController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("~/api/Token")]
        public async Task<IActionResult> Token(TokenRequest request)
        {
            request.SecurityKey = _configuration["SecurityKey"];
            request.Issuer = _configuration["Issuer"];
            request.Audience = _configuration["Audience"];
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response is null)
                    return BadRequest("User not found!");
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
        
        [HttpPost]
        [Route("~/api/RefreshToken")] // api/RefreshToken
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            request.SecurityKey = _configuration["SecurityKey"];
            request.Issuer = _configuration["Issuer"];
            request.Audience = _configuration["Audience"];
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response is null)
                    return BadRequest("User not found!");
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    } 
}

