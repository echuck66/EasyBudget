using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class EditCheckingTabBarController : UITabBarController
    {
        public BankAccountViewModel Account { get; set; }

        public EditCheckingTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}