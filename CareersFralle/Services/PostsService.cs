using CareersFralle.Models;
using CareersFralle.Repository;

namespace CareersFralle.Services
{
    public interface IPostsService
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post?> GetPost(int id);
        Task<Post> CreatePost(Post post);
        Task<IEnumerable<Post>> Generate(int count);
    }

    public class PostsService : IPostsService
    {

        private readonly IPostsRepository _repository;
        public PostsService(IPostsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Post>> GetPosts() => await _repository.GetPosts();

        public async Task<Post?> GetPost(int id) => await _repository.GetPost(id);

        public async Task<Post> CreatePost(Post post) => await _repository.CreatePost(post);

        public async Task<IEnumerable<Post>> Generate(int count = 20)
        {
            var posts = new List<Post>();
            for (int i = 0; i < count; i++)
            {
                posts.Add(GeneratePost());
            }

            return await _repository.AddPosts(posts);
        }

        private Post GeneratePost()
        {
            return new Post(Faker.Name.FullName(), Faker.Lorem.Sentence(), Faker.Internet.DomainName(), Faker.Internet.Url());
        }
    }
}
