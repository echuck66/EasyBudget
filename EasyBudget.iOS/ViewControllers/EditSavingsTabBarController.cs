using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Business.ViewModels;

namespace EasyBudget.iOS
{
    public partial class EditSavingsTabBarController : UITabBarController
    {
        public BankAccountViewModel Account { get; set; }

        public EditSavingsTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}