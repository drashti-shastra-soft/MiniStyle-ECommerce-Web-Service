using System.Collections.Generic;
using System;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface ICategoryService
    {
        List<Domain.Category> GetAll();
    }

}
