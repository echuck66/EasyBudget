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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
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
                    var vcItems = controllers[1];
                    if (vcItems != null && this.Category != null)
                    {
                        (vcItems as BudgetItemsTableViewController).CategoryId = this.Category.id;
                    }
                }

            }

            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("New", UIBarButtonItemStyle.Plain, OnNewItemClicked);
            this.NavigationItem.RightBarButtonItem.Enabled = false;
            this.ViewControllerSelected += OnTabSelected;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.ViewControllerSelected -= OnTabSelected;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.Category = null;
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

        private void OnTabSelected(object sender, UITabBarSelectionEventArgs e)
        {
            var tabBarItem = this.TabBar.SelectedItem;
            if (tabBarItem.Title.ToLower() == "budget items")
                this.NavigationItem.RightBarButtonItem.Enabled = true;
            else
                this.NavigationItem.RightBarButtonItem.Enabled = false;
        }
    }
}