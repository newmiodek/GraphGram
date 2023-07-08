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

		WeakReferenceMessenger.Default.Register<UpdateGraphResultsMessage>(this, (r, m) => {
			gradientEntry.Text = m.Value.GetGradient();
			yInterceptEntry.Text = m.Value.GetYIntercept();
		});
	}
}
