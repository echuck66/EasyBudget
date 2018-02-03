using Foundation;
using System;
using UIKit;

namespace EasyBudget.iOS
{
    public partial class MainMenuTableViewController : UITableViewController
    {
        public MainMenuTableViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            //TableView.ContentInset = new UIEdgeInsets(this.TopLayoutGuide.Length, 0, 0, 0);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            switch (indexPath.Row)
            {
                case 0:
                    var categoriesVC = this.Storyboard.InstantiateViewController("BudgetCategoriesTableViewController");
                    this.NavigationController.PushViewController(categoriesVC, true);
                    break;
                case 1:
                    var accountsVC = this.Storyboard.InstantiateViewController("BankAccountsTableViewController");
                    this.NavigationController.PushViewController(accountsVC, true);
                    break;
            }
        } 

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            //base.AccessoryButtonTapped(tableView, indexPath);
            switch (indexPath.Row)
            {
                case 0:
                    var categoriesVC = this.Storyboard.InstantiateViewController("BudgetCategoriesTableViewController");
                    this.NavigationController.PushViewController(categoriesVC, true);
                    break;
                case 1:
                    var accountsVC = this.Storyboard.InstantiateViewController("BankAccountsTableViewController");
                    this.NavigationController.PushViewController(accountsVC, true);
                    break;
            }
        }

        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    //base.RowSelected(tableView, indexPath);
        //    switch (indexPath.Row)
        //    {
        //        case 0:
        //            var categoriesVC = this.Storyboard.InstantiateViewController("BudgetCategoriesTableViewController");
        //            this.NavigationController.PushViewController(categoriesVC, false);
        //            break;
        //        case 1:
        //            var accountsVC = this.Storyboard.InstantiateViewController("BankAccountsTableViewController");
        //            this.NavigationController.PushViewController(accountsVC, false);
        //            break;
        //    }
        //}
    }
}