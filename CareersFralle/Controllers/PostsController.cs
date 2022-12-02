using CareersFralle.Data;
using CareersFralle.Models;
using CareersFralle.Repository;
using CareersFralle.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareersFralle.Controllers
{
    public class PostsController : Controller
    {
        private readonly DataContext _context;
        private readonly IPostsService _postService;
        private readonly IPostsRepository _postRepository;

        public PostsController(DataContext context, IPostsRepository postRepository, IPostsService postService)
        {
            _context = context;
            _postRepository = postRepository;
            _postService = postService;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _postService.GetPosts());
        }

        // GET: Posts/Generate/20
        public async Task<IActionResult> Generate(int count = 20)
        {
            await _postService.Generate(count);

            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var post = await _postService.GetPost(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Created,Title,Description,Host,Url")] Post post)
        {
            if (ModelState.IsValid)
            {
                await _postService.CreatePost(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }
    }
}
