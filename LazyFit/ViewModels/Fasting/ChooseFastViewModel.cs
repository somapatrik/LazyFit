﻿using LazyFit.Models;
using LazyFit.Services;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LazyFit.ViewModels.Fasting
{

    class ChooseFastViewModel : PrimeViewModel
    {
        private ObservableCollection<FastingOption> _FastingOptions;
        public ObservableCollection<FastingOption> FastingOptions { get => _FastingOptions; set => SetProperty(ref _FastingOptions, value); }

        public ICommand StartFast { get; private set; }

        public bool OptionSelected { get; private set; }

        FastService FastService;

        public ChooseFastViewModel()
        {

            FastService = new FastService();

            FastingOptions = new ObservableCollection<FastingOption>()
            {
                new FastingOption(4, "4 hours", "Also known as pussyfast..."),
                new FastingOption(6, "6 hours", "Good for you"),
                new FastingOption(8, "8 hours", "Real deal"),
                new FastingOption(10, "10 hours", "You love to suffer...good"),
                new FastingOption(12, "12 hours", "Do you really want to do it?"),
                new FastingOption(16, "16 hours", "Wouldn´t recommend it"),
                new FastingOption(18, "18 hours", "Why?"),
                new FastingOption(24, "Full day", "Now you´re just showing off")
            };

            StartFast = new Command(StartFastHandler);

        }

        private async void StartFastHandler(object selectedFast)
        {
            FastingOption option = (FastingOption)selectedFast;
            await FastService.StartFast(option.Hours);

            OptionSelected = true;
            await MopupService.Instance.PopAsync();
        }
    }
}
