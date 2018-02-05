using Foundation;
using System;
using UIKit;

namespace EasyBudget.iOS
{
    public partial class CheckRegisterTableViewController : UITableViewController
    {
        public Guid AccountId { get; set; }

        public CheckRegisterTableViewController (IntPtr handle) : base (handle)
        {
        }
    }
}