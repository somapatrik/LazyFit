﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace LazyFit.Models
{
    public partial class ActionSquare : ObservableObject
    {
        [ObservableProperty]
        private string _ActionName;

        [ObservableProperty]
        private string _Color;

        [ObservableProperty]
        private DateTime _Time;

        [ObservableProperty]
        private bool _IsBad;
    }
}