using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class ViewCheckingTabBarController : UITabBarController
    {
        public CheckingAccount Account { get; set; }

        public ViewCheckingTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}