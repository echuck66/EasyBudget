using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class ViewCheckingTabBarController : UITabBarController
    {
        public BankAccountViewModel Account { get; set; }

        public ViewCheckingTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}