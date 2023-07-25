// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using BadgeBoard.Api.Extensions.Response;
using BadgeBoard.Api.Modules.Sample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBoard.Api.Modules.Sample.Controllers;

[ApiController]
[Route("api/sample/")]
public class WeatherController : ControllerBase
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherController> _logger;


    public WeatherController(ILogger<WeatherController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    [Route("weather")]
    public IEnumerable<Weather> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Weather {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }


    [Authorize]
    [HttpGet]
    [Route("secure")]
    public ApiResponse Secure()
    {
        return new GoodResponse(new GoodDto());
    }
}