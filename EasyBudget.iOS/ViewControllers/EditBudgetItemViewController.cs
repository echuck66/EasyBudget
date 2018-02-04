using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class EditBudgetItemViewController : UIViewController
    {
        public BudgetItem BudgetItem { get; set; }

        public EditBudgetItemViewController (IntPtr handle) : base (handle)
        {
            
        }
    }
}