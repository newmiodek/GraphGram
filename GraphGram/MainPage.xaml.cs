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

        InitializeComponent();
        // <Creating the data table>
        entryTable = new Entry[defaultRowCount, 4];

        for(int i = 0; i < defaultRowCount; i++) {
            DataTable.AddRowDefinition(new RowDefinition((double)Resources["cellHeight"]));

            Label rowNumberLabel = new Label {
                Text = (i + 1).ToString(),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            rowNumberLabel.SetAppThemeColor(Label.TextColorProperty, Colors.Black, Colors.White);
            rowNumberLabel.SetAppThemeColor(Label.BackgroundColorProperty, Colors.White, Colors.Black);

            BoxView rowNumberLabelBackground = new BoxView();
            rowNumberLabelBackground.SetAppThemeColor(BoxView.ColorProperty, Colors.White, Colors.Black);

            DataTable.Add(rowNumberLabelBackground, 0, i);
            DataTable.Add(rowNumberLabel, 0, i);

            for(int j = 0; j < 4; j++) {
                /* Local equivalents of j and i are made here
                 * because when j and i were passed directly
                 * the program behaved as if they were passed
                 * by reference which caused errors
                 */
                int localJ = 1 * j;
                int localI = 1 * i;

                entryTable[localI, localJ] = new Entry();
                entryTable[localI, localJ].SetAppThemeColor(Entry.BackgroundColorProperty, Colors.White, Colors.Black);
                entryTable[localI, localJ].SetAppThemeColor(Entry.TextColorProperty, Colors.Black, Colors.White);

                // It's best to use only one of those two. Maybe let the user choose which one?

                // <Choose one>

                entryTable[localI, localJ].ReturnCommand = new Command(UpdateGraph);

                // entryTable[localJ, localI].TextChanged += (sender, e) => { UpdateGraph(); };

                // </Choose one>

                ContentView entryContentView = new ContentView {
                    Content = entryTable[localI, localJ]
                };
                entryContentView.SetAppThemeColor(ContentView.BackgroundColorProperty, Colors.White, Colors.Black);

                DataTable.Add(entryContentView, localJ + 1, localI);
            }
        }
        // </Creating the data table>

        // <Implementing custom headers>
        xHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide { Text = "x" } };
        xHeaderGraphicsView.StartInteraction += (sender, e) => { SwitchXHeaderToEntry(sender, e); };
        xHeaderContentView.Content = xHeaderGraphicsView;
        xHeaderEntry = new Entry { Text = ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).Text };
        xHeaderEntry.ReturnCommand = new Command(SwitchXHeaderToGraphicsView);

        yHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide { Text = "y" } };
        yHeaderGraphicsView.StartInteraction += (sender, e) => { SwitchYHeaderToEntry(sender, e); };
        yHeaderContentView.Content = yHeaderGraphicsView;
        yHeaderEntry = new Entry { Text = ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).Text };
        yHeaderEntry.ReturnCommand = new Command(SwitchYHeaderToGraphicsView);
        // </Implementing custom headers>

        Application.Current.RequestedThemeChanged += (sender, eventArgs) => {
            xHeaderGraphicsView.Invalidate();
            yHeaderGraphicsView.Invalidate();
        };
    }

    private void UpdateGraph() {
        ((GraphingArea)GraphingAreaView.Drawable).PassDataTable(entryTable);
        GraphingAreaView.Invalidate();
    }

    private void SwitchXHeaderToEntry(object sender, EventArgs e) {
        xHeaderContentView.Content = xHeaderEntry;
    }
    private void SwitchYHeaderToEntry(object sender, EventArgs e) {
        yHeaderContentView.Content = yHeaderEntry;
    }
    private void SwitchXHeaderToGraphicsView() {
        ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).Text = xHeaderEntry.Text;
        xHeaderContentView.Content = xHeaderGraphicsView;
        ((GraphingArea)GraphingAreaView.Drawable).SetXAxisTitle(xHeaderEntry.Text);
        GraphingAreaView.Invalidate();
    }
    private void SwitchYHeaderToGraphicsView() {
        ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).Text = yHeaderEntry.Text;
        yHeaderContentView.Content = yHeaderGraphicsView;
        ((GraphingArea)GraphingAreaView.Drawable).SetYAxisTitle(yHeaderEntry.Text);
        GraphingAreaView.Invalidate();
    }
}
