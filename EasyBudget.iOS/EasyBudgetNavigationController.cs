using Foundation;
using System;
using UIKit;

namespace EasyBudget.iOS
{
    public partial class EasyBudgetNavigationController : UINavigationController
    {
        public EasyBudgetNavigationController (IntPtr handle) : base (handle)
        {
            
        }

        public override void AddChildViewController(UIViewController childController)
        {
            base.AddChildViewController(childController);
        }

        public override UIViewController PopViewController(bool animated)
        {
            return base.PopViewController(animated);
        }

        public override UIViewController[] PopToRootViewController(bool animated)
        {
            return base.PopToRootViewController(animated);
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);
        }
    }
}