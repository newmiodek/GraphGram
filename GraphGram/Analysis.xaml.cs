using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GraphGram;

public partial class Analysis : ContentPage {
	public Analysis() {
		InitializeComponent();

		RequestMessage<GraphResults> gradientRequest = new RequestMessage<GraphResults>();
		WeakReferenceMessenger.Default.Send(gradientRequest);
		gradientEntry.Text = gradientRequest.Response.GetGradient();
		yInterceptEntry.Text = gradientRequest.Response.GetYIntercept();
		outlierIndicesEntry.Text = gradientRequest.Response.GetOutliers();

		WeakReferenceMessenger.Default.Register<UpdateGraphResultsMessage>(this, (r, m) => {
			gradientEntry.Text = m.Value.GetGradient();
			yInterceptEntry.Text = m.Value.GetYIntercept();
			outlierIndicesEntry.Text = m.Value.GetOutliers();
		});
	}
}
