using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface ICategoryRepository
    {
        List<Domain.Category> GetAll();
    }
}
