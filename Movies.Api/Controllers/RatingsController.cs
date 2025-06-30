namespace Movies.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [Authorize]
    [HttpPut(ApiEndpoints.Movies.Rate)]
    public async Task<IActionResult> RateMovie([FromRoute] Guid id,
                                               [FromBody] RateMovieRequest request, 
                                               CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null) return BadRequest("User not found");

        var result = await _ratingService.RateMovieAsync(id, request.Rating, userId.Value, cancellationToken);
        return result ? Ok() : NotFound();
    }

    [Authorize]
    [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
    public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null) return BadRequest();

        var result = await _ratingService.DeleteRatingAsync(id, userId.Value, cancellationToken);

        return result ? Ok() : NotFound();
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    public async Task<IActionResult> GetUserRatings(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        if (userId == null) return BadRequest();

        var ratings = await _ratingService.GetRatingsForUserAsync(userId.Value, cancellationToken);

        var ratingsResponse = ratings.MapToResponse();

        return !ratingsResponse.Any() ? NotFound() : Ok(ratingsResponse);
    }
}
