using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

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

    public MainPage() {

        InitializeComponent();
        goWithErrorBoxes = false;
        drawSteepestLine = false;
        drawLeastSteepLine = false;
        drawBestFitLine = true;

        // <Creating the data table>
        entryTable = new Entry[Constants.DEFAULT_ROW_COUNT, 4];

        for(int i = 0; i < Constants.DEFAULT_ROW_COUNT; i++) {
            dataTable.AddRowDefinition(new RowDefinition((double)Resources["cellHeight"]));

            Label rowNumberLabel = new Label {
                Text = (i + 1).ToString(),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            rowNumberLabel.SetAppThemeColor(Label.TextColorProperty, Colors.Black, Colors.White);
            rowNumberLabel.SetAppThemeColor(Label.BackgroundColorProperty, Colors.White, Colors.Black);

            BoxView rowNumberLabelBackground = new BoxView();
            rowNumberLabelBackground.SetAppThemeColor(BoxView.ColorProperty, Colors.White, Colors.Black);

            dataTable.Add(rowNumberLabelBackground, 0, i);
            dataTable.Add(rowNumberLabel, 0, i);

            for(int j = 0; j < 4; j++) {
                /* Local equivalents of j and i are made here
                 * because when j and i were passed directly
                 * the program behaved as if they were passed
                 * by reference which caused errors
                 */
                int localJ = 1 * j;
                int localI = 1 * i;

                entryTable[localI, localJ] = new Entry { IsSpellCheckEnabled = false, IsTextPredictionEnabled = false};
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

                dataTable.Add(entryContentView, localJ + 1, localI);
            }
        }
        // </Creating the data table>

        // <Implementing custom headers>
        xHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide() };
        ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).SetText("x");
        xHeaderGraphicsView.StartInteraction += SwitchXHeaderToEntry;
        xHeaderContentView.Content = xHeaderGraphicsView;
        xHeaderEntry = new Entry {
            Text = ((TableHeaderGraphicSide)xHeaderGraphicsView.Drawable).GetText(),
            IsSpellCheckEnabled = false,
            IsTextPredictionEnabled = false
        };
        xHeaderEntry.ReturnCommand = new Command(SwitchXHeaderToGraphicsView);

        yHeaderGraphicsView = new GraphicsView { Drawable = new TableHeaderGraphicSide() };
        ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).SetText("y");
        yHeaderGraphicsView.StartInteraction += SwitchYHeaderToEntry;
        yHeaderContentView.Content = yHeaderGraphicsView;
        yHeaderEntry = new Entry {
            Text = ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).GetText(),
            IsSpellCheckEnabled = false,
            IsTextPredictionEnabled = false
        };
        yHeaderEntry.ReturnCommand = new Command(SwitchYHeaderToGraphicsView);

        ((TableHeaderGraphicSide)xUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394x");
        ((TableHeaderGraphicSide)yUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394y");
        // </Implementing custom headers>

        Application.Current.RequestedThemeChanged += (sender, eventArgs) => {
            graphingAreaView.Invalidate();
            xHeaderGraphicsView.Invalidate();
            yHeaderGraphicsView.Invalidate();
            xUncertaintyHeaderGraphicsView.Invalidate();
            yUncertaintyHeaderGraphicsView.Invalidate();
        };

        WeakReferenceMessenger.Default.Register<RequestMessage<GraphResults>>(this, (r, m) => {
            m.Reply(((GraphingArea)graphingAreaView.Drawable).GetGraphResults());
        });

        WeakReferenceMessenger.Default.Register<ImportDataMessage>(this, (r, m) => {
            for(int i = 0; i < m.Value.GetLength(0); i++) {
                for(int j = 0; j < 4; j++) {
                    string new_text;
                    if(m.Value[i, j] == null) {
                        new_text = "";
                    }
                    else {
                        new_text = m.Value[i, j].ToString();
                    }
                    entryTable[i, j].Text = new_text;
                }
            }
            UpdateGraph();
        });

        WeakReferenceMessenger.Default.Register<RequestMessage<string>>(this, (r, m) => {
            string for_export = "";
            for(int i = 0; i < Constants.DEFAULT_ROW_COUNT; i++) {
                float next_num;
                string line = "";

                for(int j = 0; j < 3; j++) {
                    if(!float.TryParse(entryTable[i, j].Text, out next_num)) {
                        break;
                    }
                    line += next_num.ToString() + "\t";
                }
                if(!float.TryParse(entryTable[i, 3].Text, out next_num)) {
                    break;
                }
                line += next_num.ToString() + "\n";

                for_export += line;
            }
            m.Reply(for_export);
        });
    }

    private void UpdateGraph() {
        GraphingArea graphingArea = (GraphingArea)graphingAreaView.Drawable;
        graphingArea.PassDataTable(entryTable, goWithErrorBoxes);
        graphingAreaView.Invalidate();

        WeakReferenceMessenger.Default.Send(new UpdateGraphResultsMessage(graphingArea.GetGraphResults()));
    }

    private void UpdateGraphPreferences() {
        ((GraphingArea)graphingAreaView.Drawable).PassPreferences(
            drawSteepestLine,
            drawLeastSteepLine,
            drawBestFitLine);
        graphingAreaView.Invalidate();
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
        ((GraphingArea)graphingAreaView.Drawable).SetXAxisTitle(xHeaderEntry.Text);
        graphingAreaView.Invalidate();

        ((TableHeaderGraphicSide)xUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394" + xHeaderEntry.Text);
        xUncertaintyHeaderGraphicsView.Invalidate();
    }
    private void SwitchYHeaderToGraphicsView() {
        ((TableHeaderGraphicSide)yHeaderGraphicsView.Drawable).SetText(yHeaderEntry.Text);
        yHeaderContentView.Content = yHeaderGraphicsView;
        ((GraphingArea)graphingAreaView.Drawable).SetYAxisTitle(yHeaderEntry.Text);
        graphingAreaView.Invalidate();

        ((TableHeaderGraphicSide)yUncertaintyHeaderGraphicsView.Drawable).SetText("\u0394" + yHeaderEntry.Text);
        yUncertaintyHeaderGraphicsView.Invalidate();
    }

    private void CheckBoxClicked(object sender, CheckedChangedEventArgs e) {
        CheckBox checkBoxSender = (CheckBox)sender;
        if(checkBoxSender == bestFitLineCheckBox) {
            drawBestFitLine = e.Value;
            UpdateGraphPreferences();
        }
        else if(checkBoxSender == steepestLineCheckBox) {
            drawSteepestLine = e.Value;
            UpdateGraphPreferences();
        }
        else if(checkBoxSender == leastSteepLineCheckBox) {
            drawLeastSteepLine = e.Value;
            UpdateGraphPreferences();
        }
        else if(checkBoxSender == errorBoxesCheckBox) {
            goWithErrorBoxes = e.Value;
            UpdateGraph();
        }
    }

}
