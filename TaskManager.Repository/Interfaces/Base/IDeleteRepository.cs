﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Repository.Interfaces.Base
{
    public interface IDeleteRepository<T>
    {
        void Delete(T item);
    }
}
