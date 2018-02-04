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
            //this.LabelCategoryName.Text = Category?.categoryName;

        }

        public override void ViewDidLoad()
        {
            if (this.Category != null)
            {
                this.LabelCategoryName.Text = Category?.categoryName;
            }
        }
    }
}