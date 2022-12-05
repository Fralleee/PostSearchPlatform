using CareersFralle.Models;
using CareersFralle.Repository;

namespace CareersFralle.Services
{
    public interface IClicksService
    {
        Task<Click> RecordClick(Click click);
    }

    public class ClicksService : IClicksService
    {
        private readonly IClicksRepository _repository;
        public ClicksService(IClicksRepository repository)
        {
            _repository = repository;
        }

        public async Task<Click> RecordClick(Click click)
        {
            return await _repository.RecordClick(click);
        }
    }
}
