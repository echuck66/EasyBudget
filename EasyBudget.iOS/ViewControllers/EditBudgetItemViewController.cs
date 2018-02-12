using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class EditBudgetItemViewController : UIViewController
    {
        public BudgetItemViewModel BudgetItem { get; set; }

        public EditBudgetItemViewController (IntPtr handle) : base (handle)
        {
            
        }
    }
}