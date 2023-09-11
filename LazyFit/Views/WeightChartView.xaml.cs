using LazyFit.ViewModels;

namespace LazyFit.Views;

public partial class WeightChartView : ContentView
{
    //public static readonly BindableProperty DateFromProperty = BindableProperty.Create(
    //    nameof(DateFrom),
    //    typeof(DateTime),
    //    typeof(WeightChartView));

    //public DateTime DateFrom
    //{
    //    get => (DateTime)GetValue(DateFromProperty);
    //    set => SetValue(DateFromProperty, value);
    //}

    WeightChartViewModel _viewModel;

    public WeightChartView()
	{
		InitializeComponent();
        BindingContext = _viewModel = new WeightChartViewModel(DateTime.Now, DateTime.Now);
	}
}