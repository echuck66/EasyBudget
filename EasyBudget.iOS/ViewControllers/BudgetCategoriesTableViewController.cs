using Foundation;
using System;
using UIKit;
using EasyBudget.Business;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class BudgetCategoriesTableViewController : UITableViewController
    {
        BudgetCategoriesViewSource viewSource;

        private async void OnNewExpenseCategoryClicked(Object sender, EventArgs e)
        {
            BudgetCategory cat = new BudgetCategory();
            await viewSource.AddNewCategory();
            //this.TableView.InsertRows(0, UITableViewRowAnimation.Automatic);
            viewSource.WillBeginTableEditing(this.TableView);

        }
        private async void OnNewIncomeCategoryClicked(Object sender, EventArgs e)
        {
            BudgetCategory cat = new BudgetCategory();
            await viewSource.AddNewCategory();
            //this.TableView.InsertRows(0, UITableViewRowAnimation.Automatic);
            viewSource.WillBeginTableEditing(this.TableView);

        }
        EasyBudgetDataService ds;
        public BudgetCategoriesTableViewController (IntPtr handle) : base (handle)
        {

            string dbFileName = "dbEasyBudget";
            string dbFilePath = FileAccessHelper.GetLocalFilePath(dbFileName);
            ds = new EasyBudgetDataService(dbFilePath);
            this.NavigationItem.RightBarButtonItems = new[] { new UIBarButtonItem("+Exp", UIBarButtonItemStyle.Plain, OnNewExpenseCategoryClicked), new UIBarButtonItem("+Inc", UIBarButtonItemStyle.Plain, OnNewIncomeCategoryClicked) };
        }

        public async override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            viewSource = await BudgetCategoriesViewSource.CreateAsync(this, ds);
            this.TableView.Source = viewSource;
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.TableView.Source = await BudgetCategoriesViewSource.CreateAsync(this, ds);
        }

    }

    public class BudgetCategoriesViewSource : UITableViewSource
    {   
        UITableViewController controller;
        BudgetCategoriesVM vmodel;

        static string CELL_ID = "BudgetCategoryCellId";

        private BudgetCategoriesViewSource(UITableViewController controller)
        {
            this.controller = controller;
        }

        public static async Task<BudgetCategoriesViewSource> CreateAsync(UITableViewController controller, EasyBudgetDataService dataService)
        {
            var tableViewSource = new BudgetCategoriesViewSource(controller);
            await tableViewSource.GetViewModelAsync(dataService);
            return tableViewSource;
        }

        private async Task GetViewModelAsync(EasyBudgetDataService dataService)
        {
            var vm = await dataService.GetBudgetCategoriesViewModelAsync();
            this.vmodel = vm;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CELL_ID, indexPath);

            if (cell.ImageView.Image != null)
            {
                cell.ImageView.Image.Dispose();
            }

            cell.TextLabel.Text = vmodel.BudgetCategories[indexPath.Row].categoryName;
            cell.DetailTextLabel.Text = "Total Budgeted $0.00";

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return vmodel.BudgetCategories.Count;
        }
    
        public async Task AddNewCategory()
        {
            BudgetCategory newCategory = new BudgetCategory();
            newCategory.categoryName = "New Category";
            newCategory.categoryType = Models.BudgetCategoryType.Expense;
            await Task.Run(() => this.vmodel.BudgetCategories.Add(newCategory));

        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public nint GetNewRowIndex()
        {
            return this.vmodel.BudgetCategories.Count;
        }

        public void WillBeginTableEditing(UITableView tableView)
        {
            tableView.BeginUpdates();
            tableView.InsertRows(new NSIndexPath[] {
                NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
            }, UITableViewRowAnimation.Fade);
            tableView.EndUpdates(); 
        }

    }
}