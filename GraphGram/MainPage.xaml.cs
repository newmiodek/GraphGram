using System.Diagnostics;

namespace GraphGram;

public partial class MainPage : ContentPage {

    private readonly static int defaultRowCount = 40;
    private Entry[,] entryTable;

    private GraphicsView xHeaderGraphicsView;
    private GraphicsView yHeaderGraphicsView;

    private Entry xHeaderEntry;
    private Entry yHeaderEntry;

    public MainPage() {

        // TODO: Give proper names to things and add theming

        InitializeComponent();
        // <Creating the data table>
        entryTable = new Entry[defaultRowCount, 4];
        for(int i = 0; i < defaultRowCount; i++) {
            DataTable.AddRowDefinition(new RowDefinition((double)this.Resources["CellHeight"]));

            Label rowNumberLabel = new Label {
                Text = (i + 1).ToString(),
                TextColor = Colors.White,
                BackgroundColor = Colors.Black,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            BoxView rowNumberLabelBorder = new BoxView {
                Color = Colors.Black
            };

            DataTable.Add(rowNumberLabelBorder, 0, i);
            DataTable.Add(rowNumberLabel, 0, i);

            for(int j = 0; j < 4; j++) {
                /* Local equivalents of j and i are made here
                 * because when j and i were passed directly
                 * the program behaved as if they were passed
                 * by reference which caused errors
                 */
                int localJ = 1 * j;
                int localI = 1 * i;

                entryTable[localI, localJ] = new Entry {
                    BackgroundColor = Colors.Black
                };

                // It's best to use only one of those two. Maybe let the user choose which one?
                // <Choose one>
                entryTable[localI, localJ].ReturnCommand = new Command(UpdateGraph);
                // entryTable[localJ, localI].TextChanged += (sender, e) => { UpdateGraph(); };
                // </Choose one>

                ContentView entryBorder = new ContentView {
                    BackgroundColor = Colors.Black,
                    Content = entryTable[localI, localJ]
                };

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
