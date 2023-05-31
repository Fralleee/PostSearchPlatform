using CareersFralle.Models;
using CareersFralle.Repository;
using Nest;

namespace CareersFralle.Services;

public interface IPostsService
{
    Task<IEnumerable<Post>> GetPosts(CancellationToken cancellationToken);
    Task<Post?> GetPost(int id, CancellationToken cancellationToken);
    Task<Post> CreatePost(Post post, CancellationToken cancellationToken);
    Task<IEnumerable<Post>> Generate(int count, CancellationToken cancellationToken);
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

    public async Task<IEnumerable<Post>> GetPosts(CancellationToken cancellationToken) => await _repository.GetPosts(cancellationToken);

    public async Task<Post?> GetPost(int id, CancellationToken cancellationToken) => await _repository.GetPost(id, cancellationToken);

    public async Task<Post> CreatePost(Post post, CancellationToken cancellationToken)
    {
        var createPostTask = _repository.CreatePost(post, cancellationToken);
        var indexDocumentTask = _elasticClient.IndexDocumentAsync(post);

        await Task.WhenAll(createPostTask, indexDocumentTask);

        return await createPostTask;
    }

    public async Task<IEnumerable<Post>> Generate(int count, CancellationToken cancellationToken)
    {
        var posts = Enumerable.Range(0, count).Select(_ => GeneratePost()).ToList();
        var result = await _repository.AddPosts(posts, cancellationToken);
        var indexTasks = result.Select(post => _elasticClient.IndexDocumentAsync(post));
        await Task.WhenAll(indexTasks);

        return result;
    }

    private Post GeneratePost()
    {
        return new Post(Faker.Name.FullName(), Faker.Lorem.Sentence(), Faker.Internet.DomainName(), Faker.Internet.Url());
    }
}
