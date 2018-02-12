using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class EditCategoryTabBarController : UITabBarController
    {
        public BudgetCategoryViewModel Category { get; set; }

        public EditCategoryTabBarController (IntPtr handle) : base (handle)
        {
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("New", UIBarButtonItemStyle.Plain, OnNewItemClicked);
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
                var vcItems = controllers[1];
                if (vcItems != null && this.Category != null)
                {
                    (vcItems as BudgetItemsTableViewController).CategoryId = this.Category.CategoryId;
                }
            }
        }

        /// <summary>
        /// Event handler for New button in Navigation Bar. Used to notify
        /// the ViewController via the controlling UITabBarController
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnNewItemClicked(Object sender, EventArgs e)
        {
            var budgetItemVC = this.Storyboard.InstantiateViewController("EditBudgetItemViewController");
            this.NavigationController.PushViewController(budgetItemVC, true);
        }
    }
}