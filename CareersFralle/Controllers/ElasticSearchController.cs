using CareersFralle.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace CareersFralle.Controllers;

public class ElasticSearchController : Controller
{
    private readonly IPostsService _postService;
    private readonly IElasticClient _elasticClient;

    public ElasticSearchController(IPostsService postService, IElasticClient elasticClient)
    {
        _postService = postService;
        _elasticClient = elasticClient;
    }

    public async Task<IActionResult> Reindex(CancellationToken cancellationToken)
    {
        var posts = await _postService.GetPosts(cancellationToken);
        var result = await _elasticClient.IndexManyAsync(posts);

        return Ok();
    }
}
