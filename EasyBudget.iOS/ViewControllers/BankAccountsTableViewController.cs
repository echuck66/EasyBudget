using Foundation;
using System;
using UIKit;

namespace EasyBudget.iOS.ViewControllers
{
    public partial class BankAccountsTableViewController : UITableViewController
    {
        public BankAccountsTableViewController (IntPtr handle) : base (handle)
        {
        }
    }

    public class BankAccountsViewSource : UITableViewSource
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