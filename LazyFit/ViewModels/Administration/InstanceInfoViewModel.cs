using CommunityToolkit.Mvvm.ComponentModel;
using LazyFit.Services;

namespace LazyFit.ViewModels.Administration
{
    internal partial class InstanceInfoViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _InstanceId;
        public InstanceInfoViewModel() 
        {
            LoadInstance();
        }

        private async void LoadInstance()
        {
            var instance = await InstanceInfoService.GetInstance();
            InstanceId = instance.InstanceId.ToString();
        }
    }
}
