//
//  Copyright 2018  CrawfordNET Solutions, LLC
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using UIKit;
using EasyBudget.Models.DataModels;
using EasyBudget.Models;

namespace EasyBudget.iOS.Models
{
    public class CategoryTypePickerModel : UIPickerViewModel
    {

        public string[] categoryTypes = new string[] {
            BudgetCategoryType.Income.ToString(),
            BudgetCategoryType.Expense.ToString()
        };

        public BudgetCategoryType SelectedType { get; set; }

        public CategoryTypePickerModel()
        {
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return categoryTypes.Length;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (component == 0)
                return categoryTypes[row];
            else
                return row.ToString();
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (pickerView.SelectedRowInComponent(0) == 0){
                this.SelectedType = BudgetCategoryType.Income;
            }
            else 
            {
                this.SelectedType = BudgetCategoryType.Expense;
            }
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            return 280f;
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 40f;
        }
    }
}
