using System.Diagnostics;

namespace GraphGram;

public partial class MainPage : ContentPage {

    private readonly static int defaultRowCount = 2;
    private Entry[,] entryTable;

    public MainPage() {
        InitializeComponent();
        // <Creating the data table>
        entryTable = new Entry[4, defaultRowCount];
        for (int i = 0; i < defaultRowCount; i++) {
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
                /* Local equivalents of j and i are made here
                 * because when j and i were passed directly
                 * the program behaved as if they were passed
                 * by reference, which broke its functionality
                 */
                int localJ = 1 * j;
                int localI = 1 * i;

                entryTable[localJ, localI] = new Entry {
                    ReturnCommand = new Command(UpdateGraph)
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
        // </Creating the data table>
    }

    private void UpdateGraph() {
        // Parse the text from entryTable to represent it as doubles
        float[,] parsedDataTable = new float[4, entryTable.GetLength(1)];
        bool parseSucceded = true;
        for (int i = 0; i < parsedDataTable.GetLength(1); i++) {
            for (int j = 0; j < 4; j++) {
                // The deal with these locals is the same as above
                int localJ = 1 * j;
                int localI = 1 * i;
                if (!float.TryParse(entryTable[localJ, localI].Text, out parsedDataTable[localJ, localI])) {
                    parseSucceded = false;
                    break;
                }
            }
            if (!parseSucceded) break;
        }

        GraphingArea graphingAreaDrawable = (GraphingArea)GraphingAreaView.Drawable;

        if (parseSucceded) {
            graphingAreaDrawable.DataTable = parsedDataTable;
        }

        graphingAreaDrawable.IsInputValid = parseSucceded;

        GraphingAreaView.Invalidate();
    }
}
