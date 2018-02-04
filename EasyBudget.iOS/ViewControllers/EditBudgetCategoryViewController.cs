using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class EditBudgetCategoryViewController : UIViewController
    {

        public BudgetCategory Category { get; set; }

        public EditBudgetCategoryViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            if (this.Category != null)
            {
                TextCategoryName.Text = Category.categoryName;
                TextCategoryDescription.Text = Category.description;
            }
        }
    }
}