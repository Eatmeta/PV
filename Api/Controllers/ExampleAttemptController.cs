using Api.Models;
using Application.Examples.Commands.CreateExampleAttempt;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]

public class ExampleAttemptController : BaseController
{
    private readonly IMapper _mapper;
    public ExampleAttemptController(IMapper mapper) => _mapper = mapper;
    
    [HttpPost("CreateExampleAttempt")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<long>> Create([FromBody] CreateExampleAttemptDto createExampleAttemptDto)
    {
        var command = _mapper.Map<CreateExampleAttemptCommand>(createExampleAttemptDto);
        var exampleAttemptId = await Mediator.Send(command);

        return Ok(exampleAttemptId);
    }
}