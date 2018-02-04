using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class ViewBudgetItemViewController : UIViewController
    {
        public BudgetItem BudgetItem { get; set; }

        public ViewBudgetItemViewController (IntPtr handle) : base (handle)
        {
        }
    }
}