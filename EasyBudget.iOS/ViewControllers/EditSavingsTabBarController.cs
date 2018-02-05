using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class EditSavingsTabBarController : UITabBarController
    {
        public SavingsAccount Account { get; set; }

        public EditSavingsTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}