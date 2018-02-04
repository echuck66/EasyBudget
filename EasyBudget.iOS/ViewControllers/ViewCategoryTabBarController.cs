using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class ViewCategoryTabBarController : UITabBarController
    {
        public BudgetCategory Category { get; set; }

        public ViewCategoryTabBarController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            if (this.Category != null)
            {
                UIViewController[] controllers = this.ViewControllers;
                int vcCount = controllers != null ? controllers.Length : 0;
                if (vcCount > 0)
                {
                    var vcCategory = controllers[0];
                    if (vcCategory != null)
                    {
                        (vcCategory as ViewBudgetCategoryViewController).Category = this.Category;
                    }
                }
            }
        }
    }
}