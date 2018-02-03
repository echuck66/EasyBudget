// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace EasyBudget.iOS
{
    [Register ("ViewBudgetCategoryViewController")]
    partial class ViewBudgetCategoryViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelCategoryName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LabelCategoryName != null) {
                LabelCategoryName.Dispose ();
                LabelCategoryName = null;
            }
        }
    }
}