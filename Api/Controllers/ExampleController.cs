using Api.Models;
using Application.Examples.Queries.GetExampleDetails;
using Application.Examples.Queries.GetExampleList;
using Application.Examples.Queries.GetFourRandomExampleDetails;
using Application.Examples.Queries.GetRandomExampleDetails;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
public class ExampleController : BaseController
{
    private readonly IMapper _mapper;
    private readonly ResponseDto _response;
    public ExampleController(IMapper mapper)
    {
        _mapper = mapper;
        _response = new ResponseDto();
    }

    [HttpGet("GetListOfExamples")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExampleListDto>> GetAll()
    {
        var query = new GetExampleListQuery();
        var dto = await Mediator.Send(query);
        return Ok(dto);
    }

    [HttpGet("GetExampleDetails/{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExampleDetailsDto>> Get(int id)
    {
        var query = new GetExampleDetailsQuery
        {
            Id = id
        };

        var dto = await Mediator.Send(query);
        return Ok(dto);
    }

    [HttpGet("GetRandomExampleDetails")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<object> GetRandom()
    {
        var query = new GetRandomExampleDetailsQuery();
        try
        {
            var dto = await Mediator.Send(query);
            _response.Result = dto;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> {ex.ToString()};
        }
        return _response;
        //return Ok(dto);
    }
    
    [HttpGet("GetFourRandomExampleDetails")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExampleListDto>> GetRandomFour()
    {
        var query = new GetFourRandomExampleDetailsQuery();
        var dto = await Mediator.Send(query);
        return Ok(dto);
    }
}