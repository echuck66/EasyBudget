using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class ViewSavingsTabBarController : UITabBarController
    {
        public BankAccountViewModel Account { get; set; }

        public ViewSavingsTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}