using System;
using System.Collections.Generic;

namespace DesignHubSite.Repositories
{

    public interface IRepository<T>
    {

        T Single(int id);

        List<T> All();

        void Create(T model);

        bool Delete(int id);


    }
}

