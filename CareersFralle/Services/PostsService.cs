using CareersFralle.Models;
using CareersFralle.Repository;
using Nest;

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
        private readonly IElasticClient _elasticClient;
        public PostsService(IPostsRepository repository, IElasticClient elasticClient)
        {
            _repository = repository;
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<Post>> GetPosts() => await _repository.GetPosts();

        public async Task<Post?> GetPost(int id) => await _repository.GetPost(id);

        public async Task<Post> CreatePost(Post post)
        {
            var result = await _repository.CreatePost(post);
            await _elasticClient.IndexDocumentAsync(post);
            return result;
        }

        public async Task<IEnumerable<Post>> Generate(int count = 20)
        {
            var posts = new List<Post>();
            for (int i = 0; i < count; i++)
            {
                posts.Add(GeneratePost());
            }

            var result = await _repository.AddPosts(posts);
            foreach (var post in result)
            {
                await _elasticClient.IndexDocumentAsync(post);
            }

            return result;
        }

        private Post GeneratePost()
        {
            return new Post(Faker.Name.FullName(), Faker.Lorem.Sentence(), Faker.Internet.DomainName(), Faker.Internet.Url());
        }
    }
}
