using System.Diagnostics;

namespace GraphGram;

public partial class MainPage : ContentPage {

	private readonly static int defaultRowCount = 40;
    private Entry[,] entryTable = new Entry[4, defaultRowCount];

	public MainPage() {
		InitializeComponent();
        Color cellEdgeColor = App.Current.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
		for (int i=0; i<defaultRowCount; i++) {
			DataTable.AddRowDefinition(new RowDefinition(30));

            Label rowNumberLabel = new Label {
                Text = (i + 1).ToString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            rowNumberLabel.SetAppThemeColor(Label.TextColorProperty, Colors.Black, Colors.White);

            Border rowNumberLabelBorder = new Border {
                StrokeThickness = 1,
                Content = rowNumberLabel
            };
            rowNumberLabelBorder.SetAppThemeColor(Border.StrokeProperty, Colors.Black, Colors.White);

            DataTable.Add(rowNumberLabelBorder, 0, i);

            for (int j = 0; j < 4; j++) {
                int localJ = 1 * j;
                int localI = 1 * i;

                entryTable[localJ, localI] = new Entry {
                    ReturnCommand = new Command(() => { 
                        GraphingArea.Add(new Label { Text = entryTable[localJ, localI].Text });
                    })
                };
                entryTable[localJ, localI].SetAppThemeColor(Entry.TextColorProperty, Colors.Black, Colors.White);

                Border entryBorder = new Border {
                    StrokeThickness = 1,
                    Padding = -6,
                    Content = entryTable[localJ, localI]
                };
                entryBorder.SetAppThemeColor(Border.StrokeProperty, Colors.Black, Colors.White);

                DataTable.Add(entryBorder, localJ + 1, localI);
            }
        }
	}
}
