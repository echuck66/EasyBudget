using System;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public abstract class UnitOfWorkResults<T>
    {
        public UnitOfWorkResults()
        {

        }

        public T Results { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public Exception WorkException { get; set; }
    }
}
