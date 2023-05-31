using PostSearchPlatform.Models;
using PostSearchPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace PostSearchPlatform.Controllers;

public class PostsController : Controller
{
    private readonly IPostsService _postService;
    private readonly IElasticClient _elasticClient;
    private const int SearchResultsSize = 10;
    private const string WildcardCharacter = "*";

    public PostsController(IPostsService postService, IElasticClient elasticClient)
    {
        _postService = postService;
        _elasticClient = elasticClient;
    }

    public async Task<JsonResult> Search(string keyword, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return Json(new { error = "Keyword cannot be empty." });
        }

        try
        {
            var result = await _elasticClient.SearchAsync<Post>(
                s => s.Query(
                    q => q.QueryString(
                        d => d.Query(WildcardCharacter + keyword + WildcardCharacter)
                    )).Size(SearchResultsSize), cancellationToken);

            return Json(result.Documents);
        }
        catch (Exception ex)
        {
            return Json(new { error = ex.Message });
        }
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            return View(await _postService.GetPosts(cancellationToken));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> Generate(int count, CancellationToken cancellationToken)
    {
        if (count < 1)
        {
            return BadRequest("Count must be greater than 0.");
        }

        try
        {
            await _postService.Generate(count, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
