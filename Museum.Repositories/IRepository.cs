﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Delete(object id);

        Task<IEnumerable<T>> GetAll();

        Task<T> GetByIdAsync(object id);

        T Insert(T obj);

        void Save();

        T Update(T obj);
    }
}