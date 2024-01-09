namespace LazyFit.Views.Pressure;

public partial class PressureCardView : ContentView
{
	public PressureCardView()
	{
		InitializeComponent();

#if ANDROID

		Microsoft.Maui.Handlers.BorderHandler.Mapper.AppendToMapping("longBorder", (handler, view) =>
		{
            handler.PlatformView.LongClick += PlatformView_LongClick;
		});

#endif
    }


#if ANDROID
    private void PlatformView_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
    {
        throw new NotImplementedException();
    }
#endif
}