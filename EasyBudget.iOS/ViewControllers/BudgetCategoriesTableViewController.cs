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

        //private async void OnNewExpenseCategoryClicked(Object sender, EventArgs e)
        //{
        //    BudgetCategory cat = new BudgetCategory();
        //    await viewSource.AddNewExpenseCategory();
        //    //this.TableView.InsertRows(0, UITableViewRowAnimation.Automatic);
        //    viewSource.WillBeginTableEditing(this.TableView);
        //}

        //private async void OnNewIncomeCategoryClicked(Object sender, EventArgs e)
        //{
        //    BudgetCategory cat = new BudgetCategory();
        //    await viewSource.AddNewIncomeCategory();
        //    //this.TableView.InsertRows(0, UITableViewRowAnimation.Automatic);
        //    viewSource.WillBeginTableEditing(this.TableView);
        //}

        private async void OnNewCategoryClicked(Object sender, EventArgs e)
        {
            // TODO ad logic to change VCs to Add New Budget Category item
        }

        public BudgetCategoriesTableViewController (IntPtr handle) : base (handle)
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
            this.TableView.Source = await BudgetCategoriesViewSource.CreateAsync(this, ds);
            this.TableView.ReloadData();
        }

    }

    public class BudgetCategoriesViewSource : UITableViewSource
    {   
        UITableViewController controller;
        EasyBudgetDataService ds;
        BudgetCategoriesVM vmodel;
        IGrouping<string, BudgetCategory>[] grouping;
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
            grouping = (from c in this.vmodel.BudgetCategories
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

            var category = grouping[indexPath.Section].ElementAt(indexPath.Row);
            string titleText = category.categoryName;
            decimal budgetedAmount = category.budgetAmount;
            string subTitleText = "Budgeted Amount " + string.Format("{0:C}", budgetedAmount);

            cell.TextLabel.Text = titleText;
            cell.DetailTextLabel.Text = subTitleText;

            return cell;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return grouping[section].Key;
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            //return base.TitleForFooter(tableView, section);
            var sumBudgetedAmounts = (from g in grouping[section] select g.budgetAmount).Sum();
            string footerText = "Total Budgeted " + string.Format("{0:C}", sumBudgetedAmounts);
            return footerText;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return grouping.Length;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return grouping[section].Count();
        }
    
        //public async Task AddNewExpenseCategory()
        //{
        //    BudgetCategory newCategory = new BudgetCategory();
        //    newCategory.categoryName = "New Expense Category";
        //    newCategory.categoryType = Models.BudgetCategoryType.Expense;
        //    await Task.Run(() => this.vmodel.BudgetCategories.Add(newCategory));
        //    //grouping = (from w in vmodel.BudgetCategories
        //                //orderby w.categoryType ascending
        //                //group w by w.categoryType.ToString() into g
        //                //select g).ToArray();
        //}

        //public async Task AddNewIncomeCategory()
        //{
        //    BudgetCategory newCategory = new BudgetCategory();
        //    newCategory.categoryName = "New Income Category";
        //    newCategory.categoryType = Models.BudgetCategoryType.Income;
        //    await Task.Run(() => this.vmodel.BudgetCategories.Add(newCategory));
        //    //grouping = (from w in vmodel.BudgetCategories
        //                //orderby w.categoryType ascending
        //                //group w by w.categoryType.ToString() into g
        //                //select g).ToArray();
        //}

        //public override UISwipeActionsConfiguration GetLeadingSwipeActionsConfiguration(UITableView tableView, NSIndexPath indexPath)
        //{
        //    //return base.GetLeadingSwipeActionsConfiguration(tableView, indexPath);
        //    //UIContextualActions
        //    var definitionAction = ContextualDefinitionAction(indexPath.Row);
        //    var flagAction = ContextualFlagAction(indexPath.Row);

        //    //UISwipeActionsConfiguration
        //    var leadingSwipe = UISwipeActionsConfiguration.FromActions(new UIContextualAction[] { flagAction, definitionAction });

        //    leadingSwipe.PerformsFirstActionWithFullSwipe = false;

        //    return leadingSwipe;
        //}

        public UIContextualAction ContextualFlagAction(int row)
        {
            var action = UIContextualAction.FromContextualActionStyle
                            (UIContextualActionStyle.Normal,
                                "Flag",
                                (FlagAction, view, success) => {
                                    //var alertController = UIAlertController.Create($"Report {words[row]}?", "", UIAlertControllerStyle.Alert);
                                    //alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                                    //alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Destructive, null));
                                    
                                    //PresentViewController(alertController, true, null);

                                    success(true);
                                });

            action.Image = UIImage.FromFile("feedback.png");
            action.BackgroundColor = UIColor.Blue;

            return action;
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction editButton = UITableViewRowAction.Create(
                UITableViewRowActionStyle.Default,
                "Edit",
                delegate {
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

            // set the edit button's background color to green:
            editButton.BackgroundColor = UIColor.Green;

            // Locate the source item and determine if it can be deleted or not:
            var itm = grouping[indexPath.Section].ElementAt(indexPath.Row);
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

        //public async override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        //{
        //    //base.CommitEditingStyle(tableView, editingStyle, indexPath);
        //    switch (editingStyle)
        //    {
        //        case UITableViewCellEditingStyle.Delete:
        //            // remove the item from the underlying data source
        //            var itm = grouping[indexPath.Section].ElementAt(indexPath.Row);
        //            //if (await ds.DeleteBudgetCategoryAsync(itm))
        //            //{
        //            //    await GetViewModelAsync(this.ds);
        //            //    // delete the row from the table
        //            //    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
        //            //}
        //            break;
        //        case UITableViewCellEditingStyle.Insert:

        //            break;
        //        case UITableViewCellEditingStyle.None:

        //            break;
        //    }
        //}

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            //return true;
            return grouping[indexPath.Section].ElementAt(indexPath.Row).CanEdit;
        }

        //public void WillBeginTableEditing(UITableView tableView)
        //{
        //    tableView.BeginUpdates();
        //    tableView.InsertRows(new NSIndexPath[] {
        //        NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
        //    }, UITableViewRowAnimation.Fade);
        //    tableView.EndUpdates(); 
        //}

    }
}