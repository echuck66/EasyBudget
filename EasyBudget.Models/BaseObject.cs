using System;
using SQLite;

namespace EasyBudget.Models
{
    public class BaseObject
    {
        [PrimaryKey]
        public Guid id { get; set; }

        public DateTime dateCreated { get; set; }

        public DateTime dateModified { get; set; }

        public BaseObject()
        {
            this.id = Guid.NewGuid();
        }
    }
}
