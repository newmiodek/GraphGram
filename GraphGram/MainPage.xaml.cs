using System.Diagnostics;

namespace GraphGram;

public partial class MainPage : ContentPage {

    private readonly static int defaultRowCount = 2;
    private Entry[,] entryTable;

    private GraphicsView xHeaderGraphicsView;
    private GraphicsView yHeaderGraphicsView;

    private Entry xHeaderEntry;
    private Entry yHeaderEntry;

    public MainPage() {
        InitializeComponent();
        // <Creating the data table>
        entryTable = new Entry[defaultRowCount, 4];
        for(int i = 0; i < defaultRowCount; i++) {
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

            for(int j = 0; j < 4; j++) {
                /* Local equivalents of j and i are made here
                 * because when j and i were passed directly
                 * the program behaved as if they were passed
                 * by reference which caused errors
                 */
                int localJ = 1 * j;
                int localI = 1 * i;

                entryTable[localI, localJ] = new Entry();

                // It's best to use only one of those two. Maybe let the user choose which one?
                // <Choose one>
                entryTable[localI, localJ].ReturnCommand = new Command(UpdateGraph);
                // entryTable[localJ, localI].TextChanged += (sender, e) => { UpdateGraph(); };
                // </Choose one>

                entryTable[localI, localJ].SetAppThemeColor(Entry.TextColorProperty, Colors.Black, Colors.White);

                Border entryBorder = new Border {
                    StrokeThickness = 1,
                    Padding = -6,
                    Content = entryTable[localI, localJ]
                };
                entryBorder.SetAppThemeColor(Border.StrokeProperty, Colors.Black, Colors.White);

                DataTable.Add(entryBorder, localJ + 1, localI);
            }
        }
        // </Creating the data table>

        // <Implementing custom headers>
        xHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide { Text = "x" } };
        xHeaderGraphicsView.StartInteraction += (sender, e) => { xHeaderGraphicsViewClicked(sender, e); };
        xBorder.Content = xHeaderGraphicsView;
        xHeaderEntry = new Entry { Text = ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).Text };
        xHeaderEntry.ReturnCommand = new Command(xHeaderEntryClicked);

        yHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide { Text = "y" } };
        yHeaderGraphicsView.StartInteraction += (sender, e) => { yHeaderGraphicsViewClicked(sender, e); };
        yBorder.Content = yHeaderGraphicsView;
        yHeaderEntry = new Entry { Text = ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).Text };
        yHeaderEntry.ReturnCommand = new Command(yHeaderEntryClicked);
        // </Implementing custom headers>
    }

    private void UpdateGraph() {
        ((GraphingArea)GraphingAreaView.Drawable).PassDataTable(entryTable);
        GraphingAreaView.Invalidate();
    }

    private void xHeaderGraphicsViewClicked(object sender, EventArgs e) {
        xBorder.Content = xHeaderEntry;
    }
    private void yHeaderGraphicsViewClicked(object sender, EventArgs e) {
        yBorder.Content = yHeaderEntry;
    }
    private void xHeaderEntryClicked() {
        ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).Text = xHeaderEntry.Text;
        xBorder.Content = xHeaderGraphicsView;
        ((GraphingArea)GraphingAreaView.Drawable).SetXAxisTitle(xHeaderEntry.Text);
        GraphingAreaView.Invalidate();
    }
    private void yHeaderEntryClicked() {
        ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).Text = yHeaderEntry.Text;
        yBorder.Content = yHeaderGraphicsView;
        ((GraphingArea)GraphingAreaView.Drawable).SetYAxisTitle(yHeaderEntry.Text);
        GraphingAreaView.Invalidate();
    }
}
