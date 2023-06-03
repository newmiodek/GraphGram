namespace GraphGram;

public partial class MainPage : ContentPage {

	private readonly static int defaultRowCount = 40;

	public MainPage() {
		InitializeComponent();
		for (int i=0; i<defaultRowCount; i++) {
			DataTable.AddRowDefinition(new RowDefinition(30));
            DataTable.Add(new Border {
                Stroke = Colors.White,
                StrokeThickness = 1,
                Content = new Label {
                    Text = (i + 1).ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            }, 0, i);
            for (int j = 0; j < 4; j++) {
                DataTable.Add(new Border {
                    Stroke = Colors.White,
                    StrokeThickness = 1,
                    Content = new Label {
                        Text = "1234",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }, j + 1, i);
            }
        }
	}
	
}
