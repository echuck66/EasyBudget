//
//  Copyright 2018  CrawfordNET Solutions, LLC
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using Foundation;
using System;
using UIKit;
using EasyBudget.Business;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;
using System.Linq;
using EasyBudget.Models;

namespace EasyBudget.iOS
{
    /// <summary>
    /// Bank accounts table view controller.
    /// UI design contained in Main.storyboard and instantiated via the Storyboard
    /// </summary>
    public partial class BankAccountsTableViewController : UITableViewController
    {
        EasyBudgetDataService ds;
        BankAccountsViewSource vs;

        public BankAccountsTableViewController (IntPtr handle) : base (handle)
        {
            string dbFileName = "dbEasyBudget.sqlite";
            string dbFilePath = FileAccessHelper.GetLocalFilePath(dbFileName);
            ds = new EasyBudgetDataService(dbFilePath);
        }

        /// <summary>
        /// Called once the view is loaded.
        /// This method is called asynchronously after the controller is loaded. The TableViewSource is set
        /// via Factory Pattern. [Call to static async method BudgetItemsViewSource.CreateAsync(...) ]
        /// Following assignment of the TableViewSource property, the selected and edit events are registered
        /// for the TableViewSource instance.
        /// </summary>
        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.vs = await BankAccountsViewSource.CreateAsync(this, ds);
            this.TableView.Source = this.vs;
            if (this.TableView.Source != null)
            {
                (this.TableView.Source as BankAccountsViewSource).ItemSelected += OnItemSelected;
                (this.TableView.Source as BankAccountsViewSource).ItemEdit += OnItemEdit;
            }
            this.TableView.ReloadData();
        }

        #region Event Handlers 

        /// <summary>
        /// Event handler for BudgetCategory selection. Used to pass the selected BudgetCategory
        /// to the ViewController via the controlling UITabBarController
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnItemSelected(object sender, BankAccountEventArgs e)
        {
            var item = e.Account;
            if (item.accountType == BankAccountType.Checking)
            {
                var categoryTBC = this.Storyboard.InstantiateViewController("ViewCheckingTabBarController") as ViewCheckingTabBarController;
                categoryTBC.Account = item as CheckingAccount;
                this.NavigationController.PushViewController(categoryTBC, true);
            }
            else
            {
                var categoryTBC = this.Storyboard.InstantiateViewController("ViewSavingsTabBarController") as ViewSavingsTabBarController;
                categoryTBC.Account = item as SavingsAccount;
                this.NavigationController.PushViewController(categoryTBC, true);
            }
        }

        /// <summary>
        /// Event handler for BudgetItem editing and creation. Used to pass the selected BudgetItem
        /// to the UIViewController via the controlling UITabBarController, or simply to notify the edit UIViewController
        /// that a new BudgetItem is requested (when e.Category == null).
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnItemEdit(object sender, BankAccountEventArgs e)
        {
            var item = e.Account;
            if (item.accountType == BankAccountType.Checking)
            {
                var checkingTBC = this.Storyboard.InstantiateViewController("EditCheckingTabBarController") as EditCheckingTabBarController;
                checkingTBC.Account = item as CheckingAccount;
                this.NavigationController.PushViewController(checkingTBC, true);
            }
            else
            {
                var savingsTBC = this.Storyboard.InstantiateViewController("EditSavingsTabBarController") as EditSavingsTabBarController;
                savingsTBC.Account = item as SavingsAccount;
                this.NavigationController.PushViewController(savingsTBC, true);
            }

        }

        #endregion
    }


    /// <summary>
    /// Bank accounts view source. 
    /// Combines the UITableViewDataSource and UITableViewDelegate into a single class
    /// inherits from UITableViewSource
    /// </summary>
    public class BankAccountsViewSource : UITableViewSource
    {
        UITableViewController controller;
        EasyBudgetDataService ds;
        BankAccountsVM vmodel;

        public IGrouping<string, BankAccount>[] grouping { get; set; }

        static string CELL_ID = "BankAccountCellId";

        private BankAccountsViewSource(UITableViewController controller)
        {
            this.controller = controller;
        }


        /// <summary>
        /// Creates the BankAccountsViewSource obj instance asynchronously.
        /// Used to handle loading of underlying source data asynchronously because
        /// constructors cannot be called asynchronously.
        /// This is part of a Factory Pattern.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="controller">Controller.</param>
        /// <param name="dataService">Data service.</param>
        public static async Task<BankAccountsViewSource> CreateAsync(UITableViewController controller, EasyBudgetDataService dataService)
        {
            var tableViewSource = new BankAccountsViewSource(controller);
            tableViewSource.ds = dataService;
            //await dataService.EnsureSystemItemsExistAsync();
            await tableViewSource.GetViewModelAsync(dataService);
            return tableViewSource;
        }

        /// <summary>
        /// Gets the view model and groups the data.
        /// </summary>
        /// <returns>The data async.</returns>
        /// <param name="dataService">Data service.</param>
        private async Task GetViewModelAsync(EasyBudgetDataService dataService)
        {
            var vm = await dataService.GetBankAccountsViewModelAsync();
            this.vmodel = vm;
            this.grouping = (from a in this.vmodel.BankAccountVMs
                             orderby a.accountType ascending
                             group a by a.accountType.ToString() into g
                             select g).ToArray();
        }

        /// <summary>
        /// Gets the cell and dequeues it for reuse, or creates a new one in case 
        /// one doesn't already exist.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CELL_ID, indexPath);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Value1, CELL_ID);
            }

            if (cell.ImageView.Image != null)
            {
                cell.ImageView.Image.Dispose();
            }

            var account = this.grouping[indexPath.Section].ElementAt(indexPath.Row);
            string titleText = string.Empty;
            decimal currentBalance = 0;

            if (account.accountType == BankAccountType.Checking)
            {
                cell.ImageView.Image = UIImage.FromFile("check-book-22.png");
                titleText = account.bankName;
                currentBalance = account.currentBalance;
            }
            else
            {
                cell.ImageView.Image = UIImage.FromFile("coins-22.png");
                titleText = account.bankName;
                currentBalance = account.currentBalance;
            }
            string subTitleText = string.Format("{0:C}", currentBalance);

            cell.TextLabel.Text = titleText;
            cell.DetailTextLabel.Text = subTitleText;

            return cell;
        }

        /// <summary>
        /// Titles for header.
        /// </summary>
        /// <returns>The for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return this.grouping[section].Key;
        }

        /// <summary>
        /// Titles for footer.
        /// </summary>
        /// <returns>The for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override string TitleForFooter(UITableView tableView, nint section)
        {
            //return base.TitleForFooter(tableView, section);
            var sumCurrentBalances = (from g in this.grouping[section] select g.currentBalance).Sum();
            string footerText = "Total " + string.Format("{0:C}", sumCurrentBalances);
            return footerText;
        }

        /// <summary>
        /// Numbers the of sections.
        /// </summary>
        /// <returns>The of sections.</returns>
        /// <param name="tableView">Table view.</param>
        public override nint NumberOfSections(UITableView tableView)
        {
            return this.grouping.Length;
        }

        /// <summary>
        /// Rowes the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.grouping[section].Count();
        }

        /// <summary>
        /// Row Actions - this is where the Edit / Delete buttons are created and 
        /// the click events are registered.
        /// </summary>
        /// <returns>The actions for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction editButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Default,
                "Edit",
                delegate
                {
                    BankAccount item = grouping[indexPath.Section].ElementAt(indexPath.Row);
                    var e = new BankAccountEventArgs();
                    e.Account = item;
                    OnItemEdit(e);
                });
            UITableViewRowAction deleteButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Default,
                "Delete",
                delegate
                {
                    Console.WriteLine("Call to delete item triggered");
                    // TODO add logic to delete the category

                });

            // set the edit button's background color to orange:
            editButton.BackgroundColor = UIColor.Orange;

            // Locate the source item and determine if it can be deleted or not:
            var itm = this.grouping[indexPath.Section].ElementAt(indexPath.Row);
            UITableViewRowAction[] rowActions;
            if (!itm.CanDelete)
            {
                rowActions = new UITableViewRowAction[] { editButton };
            }
            else
            {
                rowActions = new UITableViewRowAction[] { editButton, deleteButton };
            }

            return rowActions;
        }

        /// <summary>
        /// Determins if the row can be edited or not.
        /// 
        /// Not sure yet, but I don't think this method is needed.
        /// </summary>
        /// <returns><c>true</c>, if edit row was caned, <c>false</c> otherwise.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            //return true;
            return this.grouping[indexPath.Section].ElementAt(indexPath.Row).CanEdit;
        }

        /// <summary>
        /// This method is called whenever the user selects a row in the UITableView
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            BankAccount item = grouping[indexPath.Section].ElementAt(indexPath.Row);
            var e = new BankAccountEventArgs();
            e.Account = item;
            OnItemSelected(e);
        }

        #region Category Selection Event Handling
        /// <summary>
        /// Category selected event handler.
        /// </summary>
        public delegate void BankAccountSelectedEventHandler(Object sender, BankAccountEventArgs e);
        /// <summary>
        /// Occurs when category selected.
        /// </summary>
        public event BankAccountSelectedEventHandler ItemSelected;
        /// <summary>
        /// Raise the event
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnItemSelected(BankAccountEventArgs e)
        {
            BankAccountSelectedEventHandler handler = ItemSelected;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region Category Edit Event Handling
        /// <summary>
        /// Category edit event handler.
        /// </summary>
        public delegate void BankAccountEditEventHandler(Object sender, BankAccountEventArgs e);
        /// <summary>
        /// Occurs when category edit.
        /// </summary>
        public event BankAccountEditEventHandler ItemEdit;
        /// <summary>
        /// Raise the event
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnItemEdit(BankAccountEventArgs e)
        {
            BankAccountEditEventHandler handler = ItemEdit;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

    }

    /// <summary>
    /// BankAccount selected event arguments.
    /// Used to pass the Account to view or edit controller
    /// </summary>
    public class BankAccountEventArgs : EventArgs
    {
        public BankAccount Account { get; set; }
    }
}