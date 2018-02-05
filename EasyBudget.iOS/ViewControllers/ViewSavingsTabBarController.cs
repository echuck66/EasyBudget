using Foundation;
using System;
using UIKit;
using EasyBudget.Models.DataModels;

namespace EasyBudget.iOS
{
    public partial class ViewSavingsTabBarController : UITabBarController
    {
        public SavingsAccount Account { get; set; }

        public ViewSavingsTabBarController (IntPtr handle) : base (handle)
        {
        }
    }
}