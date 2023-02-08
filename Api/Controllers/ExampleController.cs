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
    public ExampleController(IMapper mapper) => _mapper = mapper;

    [HttpGet("GetListOfExamples")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExampleListVm>> GetAll()
    {
        var query = new GetExampleListQuery();
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("GetExampleDetails/{id:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExampleDetailsVm>> Get(long id)
    {
        var query = new GetExampleDetailsQuery
        {
            Id = id
        };

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("GetRandomExampleDetails")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ExampleDetailsVm>> GetRandom()
    {
        var query = new GetRandomExampleDetailsQuery();
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
    
    [HttpGet("GetFourRandomExampleDetails")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FourExamplesVm>> GetRandomFour()
    {
        var query = new GetFourRandomExampleDetailsQuery();
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
}