﻿//
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
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [SQLite.Table("BudgetCategory")]
    public class BudgetCategory : BaseObject
    {
        public BudgetCategory()
        {
        }

        [MaxLength(250), Unique]
        public string categoryName { get; set; }

        public string description { get; set; }

        public decimal budgetAmount { get; set; }

        public bool systemCategory { get; set; }

        public bool userSelected { get; set; }

        public AppIcon categoryIcon { get; set; }

        public BudgetCategoryType categoryType { get; set; }
    }
}
