using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Models;
using LazyFit.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels.Reports
{
    public partial class ActionDayViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _ActionDate;

        [ObservableProperty]
        private ObservableCollection<ActionSquare> _Actions = new ObservableCollection<ActionSquare>();

        public ActionDayViewModel(ActionSquareDate actionSquare) 
        {
            ActionDate = actionSquare.Time;
            actionSquare.Actions.ForEach(Actions.Add);
        }

        [RelayCommand]
        private async Task DeleteAction(object action)
        {
            await DB.Database.DeleteAsync(action);

        }
    }
}
