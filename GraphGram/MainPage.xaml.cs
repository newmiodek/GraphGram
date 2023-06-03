using System.Diagnostics;

namespace GraphGram;

public partial class MainPage : ContentPage {

	private readonly static int defaultRowCount = 40;
    private Entry[,] entryTable = new Entry[4, defaultRowCount];

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
                int localJ = 1 * j;
                int localI = 1 * i;
                Debug.WriteLine("Assigning to entry at " + localJ.ToString() + " " + localI.ToString());
                entryTable[localJ, localI] = new Entry {
                    ReturnCommand = new Command(() => { 
                        Debug.WriteLine(localJ);
                        Debug.WriteLine(localI);
                        GraphingArea.Add(new Label { Text = entryTable[localJ, localI].Text });
                    })
                };
                DataTable.Add(new Border {
                    Stroke = Colors.White,
                    StrokeThickness = 1,
                    Content = entryTable[localJ, localI]
                }, localJ + 1, localI);
            }
        }
	}
	
}
