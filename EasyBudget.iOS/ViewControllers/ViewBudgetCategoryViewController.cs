using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class ViewBudgetCategoryViewController : UIViewController
    {
        public BudgetCategoryViewModel Category { get; set; }

        public ViewBudgetCategoryViewController()
        {
            
        }

        public ViewBudgetCategoryViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (this.Category != null)
            {
                this.lblCategoryName.Text = Category?.Name;
                lblCategoryDescription.Text = Category?.Description;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.Category = null;
            }
        }
    }
}