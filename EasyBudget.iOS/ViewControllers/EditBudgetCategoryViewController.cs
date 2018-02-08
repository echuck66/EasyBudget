using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.iOS.Models;

namespace EasyBudget.iOS
{
    public partial class EditBudgetCategoryViewController : UIViewController
    {

        public BudgetCategory Category { get; set; }

        public EditBudgetCategoryViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (this.Category != null)
            {
                txtCategoryName.Text = Category.categoryName;
                txtCategoryDescription.Text = Category.description;
            }
            categoryTypePicker.Model = new CategoryTypePickerModel();

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidLoad()
        {
            
        }
    }
}