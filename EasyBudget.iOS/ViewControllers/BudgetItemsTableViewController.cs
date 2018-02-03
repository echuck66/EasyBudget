using Foundation;
using System;
using UIKit;

namespace EasyBudget.iOS
{
    public partial class BudgetItemsTableViewController : UITableViewController
    {
        public BudgetItemsTableViewController (IntPtr handle) : base (handle)
        {
        }
    }


    public class BudgetItemsViewSource : UITableViewSource
    {
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            throw new NotImplementedException();
        }
    }
}