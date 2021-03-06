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

using Android.App;
using Android.Widget;
using Android.OS;
using EasyBudget.Business;
using System.IO;

namespace EasyBudget.Android
{
    [Activity(Label = "EasyBudget.Android", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; LoadDataTest(); };
        }

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            var assetMgr = this.Assets;

            string destinationDBFile = FileAccessHelper.GetLocalFilePath("dbEasyBudget.sqlite");

            using (var asset = assetMgr.Open("dbEasyBudget.sqlite"))
            {
                using (var dest = File.Create(destinationDBFile))
                {
                    asset.CopyTo(dest);
                }
            }
        }
        protected void LoadDataTest()
        {
            
            EasyBudgetDataService ds = new EasyBudgetDataService(FileAccessHelper.GetLocalFilePath("dbEasyBudget.sqlite"));

            if (ds != null)
            {
                var categoriesVM = ds.GetBudgetCategoriesViewModelAsync().Result;

                int categoryCount = categoriesVM.BudgetCategoryVMs.Count;
            }
        }


    }
}

