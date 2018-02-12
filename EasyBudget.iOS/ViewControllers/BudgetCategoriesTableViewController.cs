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
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    /// <summary>
    /// Budget categories table view controller.
    /// UI design contained in Main.storyboard and instantiated via the Storyboard
    /// </summary>
    public partial class BudgetCategoriesTableViewController : UITableViewController
    {
        EasyBudgetDataService ds;
        BudgetCategoriesViewSource vs;
        string dbFilePath;

        public BudgetCategoriesTableViewController(IntPtr handle) : base(handle)
        {

            string dbFileName = "dbEasyBudget.sqlite";
            dbFilePath = FileAccessHelper.GetLocalFilePath(dbFileName);
            //ds = new EasyBudgetDataService(dbFilePath);
            //this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("New", UIBarButtonItemStyle.Plain, OnNewCategoryClicked);

        }

        /// <summary>
        /// Load the view data and assign the even handlers
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public async override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ds = new EasyBudgetDataService(dbFilePath);
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("New", UIBarButtonItemStyle.Plain, OnNewCategoryClicked);

            this.vs = await BudgetCategoriesViewSource.CreateAsync(this, ds);
            this.TableView.Source = this.vs;
            if (this.TableView.Source != null)
            {
                (this.TableView.Source as BudgetCategoriesViewSource).CategorySelected += OnCategorySelected;
                (this.TableView.Source as BudgetCategoriesViewSource).CategoryEdit += OnCategoryEdit;
            }
            this.TableView.ReloadData();
        }

        /// <summary>
        /// Release the event handlers assigned in ViewWillAppear and break the references to ds and vs
        /// so the GC cleans everything up correctly
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            (this.TableView.Source as BudgetCategoriesViewSource).CategorySelected -= OnCategorySelected;
            (this.TableView.Source as BudgetCategoriesViewSource).CategoryEdit -= OnCategoryEdit;
            this.NavigationItem.RightBarButtonItem.Dispose();

            // Ask about this:
            //this.NavigationItem.RightBarButtonItem.Clicked -= OnNewCategoryClicked;

            //ds = null;
            this.ds = null;
            this.vs = null;
        }

        /// <summary>
        /// Called once the view is loaded.
        /// This method is called asynchronously after the controller is loaded. The TableViewSource is set
        /// via Factory Pattern. [Call to static async method BudgetCategoriesViewSource.CreateAsync(...) ]
        /// Following assignment of the TableViewSource property, the selected and edit events are registered
        /// for the TableViewSource instance.
        /// </summary>
        //public async override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();
        //    this.vs = await BudgetCategoriesViewSource.CreateAsync(this, ds);
        //    this.TableView.Source = this.vs;
        //    if (this.TableView.Source != null)
        //    {
        //        (this.TableView.Source as BudgetCategoriesViewSource).CategorySelected += OnCategorySelected;
        //        (this.TableView.Source as BudgetCategoriesViewSource).CategoryEdit += OnCategoryEdit;
        //    }
        //    this.TableView.ReloadData();
        //}


        #region Event Handlers 

        /// <summary>
        /// Event handler for New button in Navigation Bar. Used to notify
        /// the ViewController via the controlling UITabBarController
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnNewCategoryClicked(Object sender, EventArgs e)
        {
            var categoriesVC = this.Storyboard.InstantiateViewController("EditCategoryTabBarController");
            this.NavigationController.PushViewController(categoriesVC, true);
        }  

        /// <summary>
        /// Event handler for BudgetCategory selection. Used to pass the selected BudgetCategory
        /// to the ViewController via the controlling UITabBarController
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnCategorySelected(object sender, CategorySelectedEventArgs e)
        {
            var category = e.Category;
            var categoryTBC = this.Storyboard.InstantiateViewController("ViewCategoryTabBarController") as ViewCategoryTabBarController;
            categoryTBC.Category = category;
            this.NavigationController.PushViewController(categoryTBC, true);
        }

        /// <summary>
        /// Event handler for BudgetCategory editing and creation. Used to pass the selected BudgetCategory
        /// to the UIViewController via the controlling UITabBarController, or simply to notify the edit UIViewController
        /// that a new BudgetCategory is requested (when e.Category == null).
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnCategoryEdit(object sender, CategorySelectedEventArgs e)
        {
            var category = e.Category;
            var categoryTBC = this.Storyboard.InstantiateViewController("EditCategoryTabBarController") as EditCategoryTabBarController;
            categoryTBC.Category = category;
            this.NavigationController.PushViewController(categoryTBC, true);
        }

        #endregion

    }

    /// <summary>
    /// Budget categories view source. 
    /// Combines the UITableViewDataSource and UITableViewDelegate into a single class
    /// inherits from UITableViewSource
    /// </summary>
    public class BudgetCategoriesViewSource : UITableViewSource
    {
        UITableViewController controller;
        EasyBudgetDataService ds;
        BudgetCategoriesViewModel vmodel;
        public IGrouping<string, BudgetCategoryViewModel>[] grouping { get; set; }

        static string CELL_ID = "BudgetCategoryCellId";

        private BudgetCategoriesViewSource(UITableViewController controller)
        {
            this.controller = controller;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ds = null;
                vmodel = null;
                grouping = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates the BudgetCategoriesViewSource instance asynchronously.
        /// Used to handle loading of underlying source data asynchronously because
        /// constructors cannot be called asynchronously.
        /// This is part of a Factory Pattern.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="controller">Controller.</param>
        /// <param name="dataService">Data service.</param>
        public static async Task<BudgetCategoriesViewSource> CreateAsync(UITableViewController controller, EasyBudgetDataService dataService)
        {
            var tableViewSource = new BudgetCategoriesViewSource(controller);
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
            var vm = await dataService.GetBudgetCategoriesViewModelAsync();
            this.vmodel = vm;
            this.grouping = (from c in this.vmodel.BudgetCategoryVMs
                             orderby c.CategoryType ascending
                             group c by c.CategoryType.ToString() into g
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

            var category = this.grouping[indexPath.Section].ElementAt(indexPath.Row);
            string titleText = category.Name;
            decimal budgetedAmount = category.Amount;
            string subTitleText = string.Format("{0:C}", budgetedAmount);

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
            var sumBudgetedAmounts = (from g in this.grouping[section] select g.Amount).Sum();
            string footerText = "Total Budgeted " + string.Format("{0:C}", sumBudgetedAmounts);
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
        /// Rowses the in section.
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
                    BudgetCategoryViewModel category = grouping[indexPath.Section].ElementAt(indexPath.Row);
                    var e = new CategorySelectedEventArgs();
                    e.Category = category;
                    OnCategoryEdit(e);
                });
            UITableViewRowAction deleteButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Default,
                "Delete",
                delegate
                {
                    Console.WriteLine("Call to delete category triggered");
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
            BudgetCategoryViewModel category = grouping[indexPath.Section].ElementAt(indexPath.Row);
            var e = new CategorySelectedEventArgs();
            e.Category = category;
            OnCategorySelected(e);
        }

        #region Category Selection Event Handling

        /// <summary>
        /// Category selected event handler.
        /// </summary>
        public delegate void CategorySelectedEventHandler(Object sender, CategorySelectedEventArgs e);
        /// <summary>
        /// Occurs when category selected.
        /// </summary>
        public event CategorySelectedEventHandler CategorySelected;
        /// <summary>
        /// Raise the event
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnCategorySelected(CategorySelectedEventArgs e)
        {
            CategorySelectedEventHandler handler = CategorySelected;
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
        public delegate void CategoryEditEventHandler(Object sender, CategorySelectedEventArgs e);
        /// <summary>
        /// Occurs when category edit.
        /// </summary>
        public event CategoryEditEventHandler CategoryEdit;
        /// <summary>
        /// Raise the event
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnCategoryEdit(CategorySelectedEventArgs e)
        {
            CategoryEditEventHandler handler = CategoryEdit;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

    }

    /// <summary>
    /// Category selected event arguments.
    /// Used to pass the BudgetCategory selected to view or edit
    /// </summary>
    public class CategorySelectedEventArgs : EventArgs
    {
        public BudgetCategoryViewModel Category { get; set; }
    }
}