
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Pressure;
using LazyFit.Services;

namespace LazyFit.ViewModels.Pressure
{
    class PressureCardViewModel : PrimeViewModel
    {
       private BloodPressure _BloodPressure;
       public PressureCardViewModel(BloodPressure bloodPressure) 
       {
            _BloodPressure = bloodPressure;
       }

        public async Task DeletePressure()
        {
            await DB.DeleteItem(_BloodPressure);
            WeakReferenceMessenger.Default.Send(new Messages.RefreshPressureCards(true));
        }

    }
}
