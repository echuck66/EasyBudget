using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.iOS.Models;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class EditBudgetCategoryViewController : UIViewController
    {

        public BudgetCategoryViewModel Category { get; set; }

        public EditBudgetCategoryViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var pickerModel = new CategoryTypePickerModel();
            int typeSelection; // = Category.categoryType == EasyBudget.Models.BudgetCategoryType.Income ? 0 : 1;
            bool enableTypePicker = Category == null;
            if (this.Category != null)
            {
                typeSelection = Category.CategoryType == EasyBudget.Models.BudgetCategoryType.Income ? 0 : 1;
                txtCategoryName.Text = Category.Name;
                txtCategoryDescription.Text = Category.Description;
                pickerModel.SelectedType = Category.CategoryType;
                categoryTypePicker.Select(typeSelection, 0, true);
            }
            
            categoryTypePicker.Model = pickerModel;
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