using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class ViewBudgetCategoryViewController : UIViewController
    {
        public BudgetCategory Category { get; set; }

        public ViewBudgetCategoryViewController()
        {
            
        }

        public ViewBudgetCategoryViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            if (this.Category != null)
            {
                this.lblCategoryName.Text = Category?.categoryName;
                lblCategoryDescription.Text = Category?.description;
            }
        }
    }
}