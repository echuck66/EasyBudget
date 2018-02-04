using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class EditCategoryTabViewController : UITabBarController
    {
        public BudgetCategory Category { get; set; }

        public EditCategoryTabViewController (IntPtr handle) : base (handle)
        {
            
        }

        //public async override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();
        //}
    }
}