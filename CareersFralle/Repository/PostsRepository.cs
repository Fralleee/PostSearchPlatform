using CareersFralle.Data;
using CareersFralle.Models;
using Microsoft.EntityFrameworkCore;

namespace CareersFralle.Repository
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post?> GetPost(int id);
        Task<Post> CreatePost(Post post);
        Task<IEnumerable<Post>> AddPosts(IEnumerable<Post> posts);
    }

    public class PostsRepository : IPostsRepository
    {
        private readonly DataContext _context;
        public PostsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _context.Post.ToListAsync();
        }

        public async Task<Post?> GetPost(int id)
        {
            return await _context.Post.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Post> CreatePost(Post post)
        {
            await _context.Post.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<IEnumerable<Post>> AddPosts(IEnumerable<Post> posts)
        {
            await _context.Post.AddRangeAsync(posts);
            await _context.SaveChangesAsync();
            return posts;
        }
    }
}
