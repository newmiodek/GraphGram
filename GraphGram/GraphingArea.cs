namespace GraphGram;

public class GraphingArea : IDrawable {

    private float[,] dataTable;
    private bool isInputValid = true;

    public void PassDataTable(Entry[,] entryTable) {
        // Parse the text from entryTable to represent it as floats
        float[,] parsedDataTable = new float[entryTable.GetLength(1), 4];
        bool parseSucceded = true;
        for (int i = 0; i < parsedDataTable.GetLength(0); i++) {
            for (int j = 0; j < 4; j++) {
                // The deal with these locals is the same as in MainPage's constructor
                int localJ = 1 * j;
                int localI = 1 * i;
                if (!float.TryParse(entryTable[localJ, localI].Text, out parsedDataTable[localI, localJ])) {
                    parseSucceded = false;
                    break;
                }
            }
            if (!parseSucceded) break;
        }
        if (parseSucceded) {
            dataTable = parsedDataTable;
        }
        isInputValid = parseSucceded;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        if (dataTable != null) {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine(dataTable[0, 0], dataTable[0, 1], dataTable[1, 0], dataTable[1, 1]);
        }
        if (!isInputValid) {
            canvas.FontColor = Colors.Red;
            canvas.FontSize = 18;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.DrawString("Invalid input", 100, 100, HorizontalAlignment.Left);
        }
    }
}
