using System;
using System.Collections.Generic;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class DeleteCheckingAccountResults : UnitOfWorkResults<bool>
    {
        public Guid AccountId { get; set; }

        public CheckingAccount Account { get; set; }

        public DeleteCheckingAccountResults()
        {
        }
    }
}
