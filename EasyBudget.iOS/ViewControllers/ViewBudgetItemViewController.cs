using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class ViewBudgetItemViewController : UIViewController
    {
        public BudgetItemViewModel BudgetItem { get; set; }

        public ViewBudgetItemViewController (IntPtr handle) : base (handle)
        {
        }
    }
}