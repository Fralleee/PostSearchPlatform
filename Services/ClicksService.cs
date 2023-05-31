using PostSearchPlatform.Models;
using PostSearchPlatform.Repository;

namespace PostSearchPlatform.Services;

public interface IClicksService
{
    Task<Click> RecordClick(Click click, CancellationToken cancellationToken);
}

public class ClicksService : IClicksService
{
    private readonly IClicksRepository _repository;

    public ClicksService(IClicksRepository repository)
    {
        _repository = repository;
    }

    public async Task<Click> RecordClick(Click click, CancellationToken cancellationToken)
    {
        if (click == null)
        {
            throw new ArgumentNullException(nameof(click));
        }

        return await _repository.RecordClick(click, cancellationToken);
    }
}
