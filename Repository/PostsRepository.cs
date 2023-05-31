using PostSearchPlatform.Data;
using PostSearchPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace PostSearchPlatform.Repository;

public interface IPostsRepository
{
    Task<IEnumerable<Post>> GetPosts(CancellationToken cancellationToken);
    Task<Post?> GetPost(int id, CancellationToken cancellationToken);
    Task<Post> CreatePost(Post post, CancellationToken cancellationToken);
    Task<IEnumerable<Post>> AddPosts(IEnumerable<Post> posts, CancellationToken cancellationToken);
}

public class PostsRepository : IPostsRepository
{
    private readonly DataContext _context;

    public PostsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetPosts(CancellationToken cancellationToken) => await _context.Post.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Post?> GetPost(int id, CancellationToken cancellationToken) => await _context.Post.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<Post> CreatePost(Post post, CancellationToken cancellationToken)
    {
        await _context.Post.AddAsync(post, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return post;
    }

    public async Task<IEnumerable<Post>> AddPosts(IEnumerable<Post> posts, CancellationToken cancellationToken)
    {
        await _context.Post.AddRangeAsync(posts, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return posts;
    }
}
