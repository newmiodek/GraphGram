using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GraphGram;

public partial class Analysis : ContentPage {
	public Analysis() {
		InitializeComponent();

		RequestMessage<GraphResults> gradientRequest = new RequestMessage<GraphResults>();
		WeakReferenceMessenger.Default.Send(gradientRequest);
		gradientEntry.Text = gradientRequest.Response.GetBestFitLineGradient();
		yInterceptEntry.Text = gradientRequest.Response.GetBestFitLineYIntercept();
		outlierIndicesEntry.Text = gradientRequest.Response.GetOutliers();
		steepestGradientEntry.Text = gradientRequest.Response.GetSteepestGradient();
		steepestYInterceptEntry.Text = gradientRequest.Response.GetSteepestYIntercept();
		leastSteepGradientEntry.Text = gradientRequest.Response.GetLeastSteepGradient();
		leastSteepYInterceptEntry.Text = gradientRequest.Response.GetLeastSteepYIntercept();

		WeakReferenceMessenger.Default.Register<UpdateGraphResultsMessage>(this, (r, m) => {
			gradientEntry.Text = m.Value.GetBestFitLineGradient();
			yInterceptEntry.Text = m.Value.GetBestFitLineYIntercept();
			outlierIndicesEntry.Text = m.Value.GetOutliers();
			steepestGradientEntry.Text = m.Value.GetSteepestGradient();
			steepestYInterceptEntry.Text = m.Value.GetSteepestYIntercept();
			leastSteepGradientEntry.Text = m.Value.GetLeastSteepGradient();
			leastSteepYInterceptEntry.Text = m.Value.GetLeastSteepYIntercept();
		});
	}
}
