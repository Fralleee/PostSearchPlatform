using CareersFralle.Data;
using CareersFralle.Models;

namespace CareersFralle.Repository;

public interface IClicksRepository
{
    Task<Click> RecordClick(Click click, CancellationToken cancellationToken);
}

public class ClicksRepository : IClicksRepository
{
    private readonly DataContext _context;

    public ClicksRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Click> RecordClick(Click click, CancellationToken cancellationToken)
    {
        if (click == null)
        {
            throw new ArgumentNullException(nameof(click));
        }

        _context.Click.Add(click);
        await _context.SaveChangesAsync(cancellationToken);
        return click;
    }
}
