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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoriesViewModel : BaseViewModel
    {

        public ICollection<BudgetCategoryViewModel> BudgetCategoryVMs { get; set; }

        public BudgetCategoriesViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetCategoryVMs = new List<BudgetCategoryViewModel>();
        }

        internal async Task LoadBudgetCategoriesAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    foreach (var category in _results.Results)
                    {
                        var vm = new BudgetCategoryViewModel(this.dbFilePath);
                        await vm.PopulateVMAsync(category);
                        this.BudgetCategoryVMs.Add(vm);
                    }
                }
                else
                {
                    if (_results.WorkException != null)
                    {
                        WriteErrorCondition(_results.WorkException);
                    }
                    else if (!string.IsNullOrEmpty(_results.Message))
                    {
                        WriteErrorCondition(_results.Message);
                    }
                    else
                    {
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

    }

}
