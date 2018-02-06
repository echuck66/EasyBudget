using Foundation;
using System;
using UIKit;

namespace EasyBudget.iOS
{
    public partial class CheckRegisterTableViewController : UITableViewController
    {
        public int AccountId { get; set; }

        public CheckRegisterTableViewController (IntPtr handle) : base (handle)
        {
        }
    }
}