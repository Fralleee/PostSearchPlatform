using CareersFralle.Data;
using CareersFralle.Models;

namespace CareersFralle.Repository
{
    public interface IClicksRepository
    {
        Task<Click> RecordClick(Click click);
    }

    public class ClicksRepository : IClicksRepository
    {
        private readonly DataContext _context;
        public ClicksRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Click> RecordClick(Click click)
        {
            await _context.Click.AddAsync(click);
            await _context.SaveChangesAsync();
            return click;
        }
    }
}
