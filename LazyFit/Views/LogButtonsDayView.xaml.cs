using LazyFit.ViewModels;
using Mopups.Pages;
using System.ComponentModel.Design.Serialization;

namespace LazyFit.Views;

public partial class LogButtonsDayView : PopupPage
{
    private LogDayViewModel _viewModel;

    public LogButtonsDayView(DateTime _ActionDate)
	{
		InitializeComponent();
		BindingContext = _viewModel = new LogDayViewModel(_ActionDate);
	}



}