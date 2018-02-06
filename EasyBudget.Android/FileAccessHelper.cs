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
using Android.Content.Res;

namespace EasyBudget.Android
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            //string path = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //string 
            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            //if (!System.IO.Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}

            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Copy("dbEasyBudget.sqlite", path);
            }

            //return System.IO.Path.Combine(path, filename);
            return path;
        }
    }
}
