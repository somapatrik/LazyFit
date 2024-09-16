
using LazyFit.ViewModels.MoodViewModels;
using Microcharts;

namespace LazyFit.Views.Mood;

public partial class MoodChartView : ContentView
{
    public static readonly BindableProperty FromProperty = BindableProperty.Create(nameof(From), typeof(DateTime), typeof(MoodChartView), propertyChanged: OnFromToPropertyChanged);
    public static readonly BindableProperty ToProperty = BindableProperty.Create(nameof(To), typeof(DateTime), typeof(MoodChartView), propertyChanged: OnFromToPropertyChanged);
    public static readonly BindableProperty ChartProperty = BindableProperty.Create(nameof(Chart), typeof(Chart), typeof(MoodChartView), propertyChanged: OnFromToPropertyChanged);

    public Chart Chart
    {
        get => (Chart)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

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

    private static void OnFromToPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (MoodChartView)bindable;
        var viewModel = view.BindingContext as MoodChartViewModel;

        if (viewModel != null)
        {
            viewModel.FromDate = view.From;
            viewModel.ToDate = view.To;
            view.Chart = viewModel.ChartObject;
        }
    }

    public MoodChartView()
	{
		InitializeComponent();
        BindingContext = new MoodChartViewModel();
	}
}