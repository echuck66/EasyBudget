using Foundation;
using System;
using UIKit;
using EasyBudget.Business;
using System.Threading.Tasks;

namespace EasyBudget.iOS
{
    public partial class BudgetCategoriesTableViewController : UITableViewController
    {
        EasyBudgetDataService ds;
        public BudgetCategoriesTableViewController (IntPtr handle) : base (handle)
        {

            string dbFileName = "dbEasyBudget";
            string dbFilePath = FileAccessHelper.GetLocalFilePath(dbFileName);
            ds = new EasyBudgetDataService(dbFilePath);

        }

        public async override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.TableView.Source = await BudgetCategoriesViewSource.CreateAsync(this, ds);

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

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return vmodel.BudgetCategories.Count;
        }
    }
}