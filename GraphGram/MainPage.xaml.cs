namespace GraphGram;

public partial class MainPage : ContentPage {

    private Entry[,] entryTable;

    private GraphicsView xHeaderGraphicsView;
    private GraphicsView yHeaderGraphicsView;

    private Entry xHeaderEntry;
    private Entry yHeaderEntry;

    private bool goWithErrorBoxes;

    private bool drawSteepestLine;
    private bool drawLeastSteepLine;
    private bool drawBestFitLine;
    private SigFigsOrDecPoints sfdp;
    private int precision;  // 1000 means auto

    public MainPage() {

        InitializeComponent();
        goWithErrorBoxes = false;

        drawSteepestLine = false;
        drawLeastSteepLine = false;
        drawBestFitLine = true;
        sfdp = SigFigsOrDecPoints.SF;
        precision = 1000;

        UpdateGraphPreferences();

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

    private void UpdateGraphPreferences() {
        ((GraphingArea)GraphingAreaView.Drawable).PassPreferences(
            drawSteepestLine,
            drawLeastSteepLine,
            drawBestFitLine,
            sfdp,
            precision);
        GraphingAreaView.Invalidate();
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
            errorBarsBoxesToggle.Text = "Error Boxes\t";
        }
        else {
            goWithErrorBoxes = true;
            errorBarsBoxesToggle.Text = "Error Boxes\t\u2713";  // Check mark
        }
        UpdateGraph();
    }

    private void SteepestLineFlyoutClicked(object sender, EventArgs eventArgs) {
        if(drawSteepestLine) {
            drawSteepestLine = false;
            steepestLineDrawToggle.Text = "Steepest Line\t";
        }
        else {
            drawSteepestLine = true;
            steepestLineDrawToggle.Text = "Steepest Line\t\u2713";  // Check mark
        }
        UpdateGraphPreferences();
    }

    private void LeastSteepLineFlyoutClicked(object sender, EventArgs eventArgs) {
        if(drawLeastSteepLine) {
            drawLeastSteepLine = false;
            leastSteepLineDrawToggle.Text = "Least Steep Line\t";
        }
        else {
            drawLeastSteepLine = true;
            leastSteepLineDrawToggle.Text = "Least Steep Line\t\u2713"; // Check mark
        }
        UpdateGraphPreferences();
    }

    private void BestFitLineFlyoutClicked(object sender, EventArgs eventArgs) {
        if(drawBestFitLine) {
            drawBestFitLine = false;
            bestFitLineDrawToggle.Text = "Line of Best Fit\t";
        }
        else {
            drawBestFitLine = true;
            bestFitLineDrawToggle.Text = "Line of Best Fit\t\u2713";  // Check mark
        }
        UpdateGraphPreferences();
    }

    private async void ChoosePrecisionClicked(object sender, EventArgs eventArgs) {
        bool autoORcustom = precision == 1000; // Auto if true, custom if false
        string response = await DisplayActionSheet(
            "Choose Mode", "CANCEL", null,
            "Auto" + (autoORcustom ? " (Current)" : ""),
            "Custom" + (autoORcustom ? "" : " (Current)"));
        if(response == null) return;
        else if(response == "Auto") {
            sfdp = SigFigsOrDecPoints.SF;
            precision = 1000;
        }
        else if(response == "Custom" || response == "Custom (Current)") {
            string response2 = await DisplayActionSheet(
                "Choose Precision Style", "CANCEL", null,
                "Significant Figures" + (response == "Custom (Current)" && sfdp == SigFigsOrDecPoints.SF ? " (Current)" : ""),
                "Decimal Points" + (response == "Custom (Current)" && sfdp == SigFigsOrDecPoints.DP ? " (Current)" : ""));
            if(response2 == null) return;
            else if(response2 == "Significant Figures" || response2 == "Significant Figures (Current)") {
                string response3 = await DisplayPromptAsync(
                    "Significant Figures", "Enter the Number of Significant Figures",
                    cancel: "CANCEL", maxLength: 2);
                if(response3 == null) return;
                int sf;
                if(int.TryParse(response3, out sf) && sf > 0) {
                    sfdp = SigFigsOrDecPoints.SF;
                    precision = sf;
                    UpdateGraphPreferences();
                    await DisplayAlert("Changes Applied", "Output precision has been changed to " + sf.ToString() + " significant figures.", "OK");
                }
                else {
                    await DisplayAlert("Bad Input", "Bad input has been entered. The number of significant figures has to be a whole, positive number.", "OK");
                }
            }
            else if(response2 == "Decimal Points" || response2 == "Decimal Points (Current)") {
                string response3 = await DisplayPromptAsync(
                    "Decimal Points", "Enter the Number of Decimal Points",
                    cancel: "CANCEL", maxLength: 2);
                if(response3 == null) return;
                int dp;
                if(int.TryParse(response3, out dp)) {
                    sfdp = SigFigsOrDecPoints.DP;
                    precision = dp;
                    UpdateGraphPreferences();
                    await DisplayAlert("Changes Applied", "Output precision has been changed to " + dp.ToString() + " decimal points.", "OK");
                }
                else {
                    await DisplayAlert("Bad Input", "Bad input has been entered. The number of decimal points has to be a number.", "OK");
                }
            }
        }
    }
}
