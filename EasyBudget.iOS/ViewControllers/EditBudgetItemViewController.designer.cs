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
    [Register ("EditBudgetItemViewController")]
    partial class EditBudgetItemViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCancel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCategoryName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView selectFrequency { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch switchRecurring { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtBudgetedAmount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtItemDescription { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCancel != null) {
                btnCancel.Dispose ();
                btnCancel = null;
            }

            if (btnSave != null) {
                btnSave.Dispose ();
                btnSave = null;
            }

            if (lblCategoryName != null) {
                lblCategoryName.Dispose ();
                lblCategoryName = null;
            }

            if (selectFrequency != null) {
                selectFrequency.Dispose ();
                selectFrequency = null;
            }

            if (switchRecurring != null) {
                switchRecurring.Dispose ();
                switchRecurring = null;
            }

            if (txtBudgetedAmount != null) {
                txtBudgetedAmount.Dispose ();
                txtBudgetedAmount = null;
            }

            if (txtItemDescription != null) {
                txtItemDescription.Dispose ();
                txtItemDescription = null;
            }
        }
    }
}