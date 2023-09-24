using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.ViewModels
{
    interface IResultModel
    {
        
        void ShowPage(int pageNum);

        void ShowOlderPage(int pageNum);

        void ShowNewerPage(int pageNum);

        void LoadResults();
    }
}
