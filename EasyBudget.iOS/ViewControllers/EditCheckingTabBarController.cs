using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class EditCheckingTabBarController : UITabBarController
    {
        public CheckingAccount Account { get; set; }

        public EditCheckingTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}