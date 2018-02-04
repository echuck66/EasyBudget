using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class EditCategoryTabBarController : UITabBarController
    {
        public BudgetCategory Category { get; set; }

        public EditCategoryTabBarController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            UIViewController[] controllers = this.ViewControllers;
            int vcCount = controllers != null ? controllers.Length : 0;
            if (vcCount > 0)
            {
                var vcCategory = controllers[0];
                if (vcCategory != null)
                {
                    (vcCategory as EditBudgetCategoryViewController).Category = this.Category;
                }
            }
        }
    }
}