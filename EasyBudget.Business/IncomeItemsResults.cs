using System;
using System.Collections.Generic;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class IncomeItemsResults : UnitOfWorkResults<ICollection<IncomeItem>>
    {
        public IncomeItemsResults()
        {
        }
    }
}
