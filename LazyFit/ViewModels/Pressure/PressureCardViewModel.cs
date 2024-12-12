
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Pressure;
using LazyFit.Services;

namespace LazyFit.ViewModels.Pressure
{
    class PressureCardViewModel : PrimeViewModel
    {
       private BloodPressure _BloodPressure;

        PressureService PressureService;

        public PressureCardViewModel(BloodPressure bloodPressure) 
       {
            PressureService = new PressureService();
            _BloodPressure = bloodPressure;
       }

        public async Task DeletePressure()
        {
            //await DB.DeleteItem(_BloodPressure);
            //WeakReferenceMessenger.Default.Send(new Messages.RefreshPressureCards(true));
        }

    }
}
