using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Services;
using System.Runtime.CompilerServices;

namespace LazyFit.ViewModels.Administration
{
    internal partial class InstanceInfoViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _InstanceId;

        [ObservableProperty]
        private string _AppVersion;

        InstanceInfoService InstanceInfoRepository;

        public InstanceInfoViewModel() 
        {
            InstanceInfoRepository = new InstanceInfoService();

            LoadInstance();
            LoadVersion();
        }

        private async void LoadInstance()
        {
            var instance = await InstanceInfoRepository.GetInstance();
            InstanceId = instance.InstanceId.ToString();
        }

        private void LoadVersion()
        {
            AppVersion = AppInfo.VersionString;
        }

        [RelayCommand]
        private async Task CreateEmail()
        {
            if (Email.Default.IsComposeSupported)
            {

                string subject = "LazyFit feedback";
                string body = "";
                string[] recipients = new[] { "soma.patrik@gmail.com" };

                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    BodyFormat = EmailBodyFormat.PlainText,
                    To = new List<string>(recipients)
                };

                await Email.Default.ComposeAsync(message);
            }
        }

    }
}
