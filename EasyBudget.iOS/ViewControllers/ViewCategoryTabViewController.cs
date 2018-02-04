using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class ViewCategoryTabViewController : UITabBarController
    {
        public BudgetCategory Category { get; set; }

        public ViewCategoryTabViewController (IntPtr handle) : base (handle)
        {
        }
    }
}