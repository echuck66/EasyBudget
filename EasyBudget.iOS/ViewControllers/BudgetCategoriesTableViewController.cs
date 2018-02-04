using Foundation;
using System;
using UIKit;
using EasyBudget.Business;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;
using System.Linq;

namespace EasyBudget.iOS
{
    public partial class BudgetCategoriesTableViewController : UITableViewController
    {
        EasyBudgetDataService ds;
        BudgetCategoriesViewSource vs;

        private async void OnNewCategoryClicked(Object sender, EventArgs e)
        {
            var categoriesVC = this.Storyboard.InstantiateViewController("EditCategoryTabViewController");
            this.NavigationController.PushViewController(categoriesVC, true);
        }

        public BudgetCategoriesTableViewController(IntPtr handle) : base(handle)
        {

            string dbFileName = "dbEasyBudget";
            string dbFilePath = FileAccessHelper.GetLocalFilePath(dbFileName);
            ds = new EasyBudgetDataService(dbFilePath);
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("New", UIBarButtonItemStyle.Plain, OnNewCategoryClicked);
            //this.NavigationItem.RightBarButtonItems = new[] { new UIBarButtonItem("+Exp", UIBarButtonItemStyle.Plain, OnNewExpenseCategoryClicked), new UIBarButtonItem("+Inc", UIBarButtonItemStyle.Plain, OnNewIncomeCategoryClicked) };
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.vs = await BudgetCategoriesViewSource.CreateAsync(this, ds);
            this.TableView.Source = this.vs;
            //this.vs.CategorySelected += OnCategorySelected;
            if (this.TableView.Source != null)
            {
                (this.TableView.Source as BudgetCategoriesViewSource).CategorySelected += OnCategorySelected;
            }
            this.TableView.ReloadData();
        }

        protected virtual void OnCategorySelected(object sender, CategorySelectedEventArgs e)
        {
            var category = e.Category;
            var categoriesTVC = this.Storyboard.InstantiateViewController("ViewCategoryTabViewController");
            //categoriesTVC.Category = category;
            this.NavigationController.PushViewController(categoriesTVC, true);
        }
    }

    public class BudgetCategoriesViewSource : UITableViewSource
    {
        UITableViewController controller;
        EasyBudgetDataService ds;
        BudgetCategoriesVM vmodel;
        public IGrouping<string, BudgetCategory>[] grouping { get; set; }

        static string CELL_ID = "BudgetCategoryCellId";

        private BudgetCategoriesViewSource(UITableViewController controller)
        {
            this.controller = controller;
        }

        public static async Task<BudgetCategoriesViewSource> CreateAsync(UITableViewController controller, EasyBudgetDataService dataService)
        {
            var tableViewSource = new BudgetCategoriesViewSource(controller);
            tableViewSource.ds = dataService;
            await dataService.EnsureSystemItemsExistAsync();
            await tableViewSource.GetViewModelAsync(dataService);
            return tableViewSource;
        }

        private async Task GetViewModelAsync(EasyBudgetDataService dataService)
        {
            var vm = await dataService.GetBudgetCategoriesViewModelAsync();
            this.vmodel = vm;
            this.grouping = (from c in this.vmodel.BudgetCategories
                             orderby c.categoryType ascending
                             group c by c.categoryType.ToString() into g
                             select g).ToArray();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CELL_ID, indexPath);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CELL_ID);
            }

            if (cell.ImageView.Image != null)
            {
                cell.ImageView.Image.Dispose();
            }

            var category = this.grouping[indexPath.Section].ElementAt(indexPath.Row);
            string titleText = category.categoryName;
            decimal budgetedAmount = category.budgetAmount;
            string subTitleText = "Budgeted Amount " + string.Format("{0:C}", budgetedAmount);

            cell.TextLabel.Text = titleText;
            cell.DetailTextLabel.Text = subTitleText;

            return cell;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return this.grouping[section].Key;
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            //return base.TitleForFooter(tableView, section);
            var sumBudgetedAmounts = (from g in this.grouping[section] select g.budgetAmount).Sum();
            string footerText = "Total Budgeted " + string.Format("{0:C}", sumBudgetedAmounts);
            return footerText;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return this.grouping.Length;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.grouping[section].Count();
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction editButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Default,
                "Edit",
                delegate
                {
                    Console.WriteLine("Call to edit category triggered");
                    // TODO add logic to edit the category

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

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            //return true;
            return this.grouping[indexPath.Section].ElementAt(indexPath.Row).CanEdit;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            BudgetCategory category = grouping[indexPath.Section].ElementAt(indexPath.Row);
            var e = new CategorySelectedEventArgs();
            e.Category = category;
            OnCategorySelected(e);
        }

        public delegate void CategorySelectedEventHandler(Object sender, CategorySelectedEventArgs e);

        public event CategorySelectedEventHandler CategorySelected;

        protected virtual void OnCategorySelected(CategorySelectedEventArgs e)
        {
            CategorySelectedEventHandler handler = CategorySelected;

            if (handler != null)
            {
                handler(this, e);
            }

        }

    }

    public class CategorySelectedEventArgs : EventArgs
    {
        public BudgetCategory Category { get; set; }
    }
}