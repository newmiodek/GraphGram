namespace GraphGram;

public partial class MainPage : ContentPage {

    private Entry[,] entryTable;

    private GraphicsView xHeaderGraphicsView;
    private GraphicsView yHeaderGraphicsView;

    private Entry xHeaderEntry;
    private Entry yHeaderEntry;

    private bool goWithErrorBoxes;

    public MainPage() {

        InitializeComponent();
        goWithErrorBoxes = false;

        // <Creating the data table>
        entryTable = new Entry[Constants.DEFAULT_ROW_COUNT, 4];

        for(int i = 0; i < Constants.DEFAULT_ROW_COUNT; i++) {
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
        xHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide() };
        ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).SetText("x");
        xHeaderGraphicsView.StartInteraction += SwitchXHeaderToEntry;
        xHeaderContentView.Content = xHeaderGraphicsView;
        xHeaderEntry = new Entry { Text = ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).GetText() };
        xHeaderEntry.ReturnCommand = new Command(SwitchXHeaderToGraphicsView);

        yHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide() };
        ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).SetText("y");
        yHeaderGraphicsView.StartInteraction += SwitchYHeaderToEntry;
        yHeaderContentView.Content = yHeaderGraphicsView;
        yHeaderEntry = new Entry { Text = ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).GetText() };
        yHeaderEntry.ReturnCommand = new Command(SwitchYHeaderToGraphicsView);

        ((TableHeaderGraphicSide)xUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394x");
        ((TableHeaderGraphicSide)yUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394y");
        // </Implementing custom headers>

        Application.Current.RequestedThemeChanged += (sender, eventArgs) => {
            GraphingAreaView.Invalidate();
            xHeaderGraphicsView.Invalidate();
            yHeaderGraphicsView.Invalidate();
            xUncertaintyHeaderGraphicsView.Invalidate();
            yUncertaintyHeaderGraphicsView.Invalidate();
        };
    }

    private void UpdateGraph() {
        GraphingArea graphingArea = (GraphingArea)GraphingAreaView.Drawable;
        graphingArea.PassDataTable(entryTable, goWithErrorBoxes);
        GraphingAreaView.Invalidate();

        gradientOutput.Text = graphingArea.GetGradient();
        yInterceptOutput.Text = graphingArea.GetYIntercept();
    }

    private void SwitchXHeaderToEntry(object sender, EventArgs e) {
        xHeaderContentView.Content = xHeaderEntry;
    }
    private void SwitchYHeaderToEntry(object sender, EventArgs e) {
        yHeaderContentView.Content = yHeaderEntry;
    }
    private void SwitchXHeaderToGraphicsView() {
        ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).SetText(xHeaderEntry.Text);
        xHeaderContentView.Content = xHeaderGraphicsView;
        ((GraphingArea)GraphingAreaView.Drawable).SetXAxisTitle(xHeaderEntry.Text);
        GraphingAreaView.Invalidate();

        ((TableHeaderGraphicSide)xUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394" + xHeaderEntry.Text);
        xUncertaintyHeaderGraphicsView.Invalidate();
    }
    private void SwitchYHeaderToGraphicsView() {
        ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).SetText(yHeaderEntry.Text);
        yHeaderContentView.Content = yHeaderGraphicsView;
        ((GraphingArea)GraphingAreaView.Drawable).SetYAxisTitle(yHeaderEntry.Text);
        GraphingAreaView.Invalidate();

        ((TableHeaderGraphicSide)yUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394" + yHeaderEntry.Text);
        yUncertaintyHeaderGraphicsView.Invalidate();
    }

    private void ErrorBarsFlyoutClicked(object sender, EventArgs eventArgs) {
        if(goWithErrorBoxes) {
            goWithErrorBoxes = false;
            errorBarsBoxesToggle.Text = "Switch to error boxes";
        }
        else {
            goWithErrorBoxes = true;
            errorBarsBoxesToggle.Text = "Switch to error bars";
        }
        UpdateGraph();
    }
}
