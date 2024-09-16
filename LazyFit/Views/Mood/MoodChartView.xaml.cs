namespace LazyFit.Views.Mood;

public partial class MoodChartView : ContentView
{
    public static readonly BindableProperty FromProperty = BindableProperty.Create(nameof(From), typeof(DateTime), typeof(MoodChartView));
    public static readonly BindableProperty ToProperty = BindableProperty.Create(nameof(To), typeof(DateTime), typeof(MoodChartView));
    public DateTime From
    {
        get => (DateTime)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public DateTime To
    {
        get => (DateTime)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public MoodChartView()
	{
		InitializeComponent();
	}
}