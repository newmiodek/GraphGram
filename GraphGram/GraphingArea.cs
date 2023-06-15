namespace GraphGram;

public class GraphingArea : IDrawable {

    private double[,] dataTable;
    private bool isInputValid = true;

    public void PassDataTable(Entry[,] entryTable) {
        // Parse the text from entryTable to represent it as floats
        double[,] parsedDataTable = new double[entryTable.GetLength(0), 4];
        bool parseSucceded = true;
        for (int i = 0; i < parsedDataTable.GetLength(0); i++) {
            for (int j = 0; j < 4; j++) {
                // The deal with these locals is the same as in MainPage's constructor
                int localJ = 1 * j;
                int localI = 1 * i;
                if (!double.TryParse(entryTable[localI, localJ].Text, out parsedDataTable[localI, localJ])) {
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
        canvas.StrokeColor = Colors.White;
        // <x axis>
        // main line
        canvas.DrawLine(dirtyRect.Left, dirtyRect.Bottom * 0.9f, dirtyRect.Right, dirtyRect.Bottom * 0.9f);
        // arrow point
        canvas.DrawLine(dirtyRect.Right, dirtyRect.Bottom * 0.9f, dirtyRect.Right - 10, dirtyRect.Bottom * 0.9f - 10);
        canvas.DrawLine(dirtyRect.Right, dirtyRect.Bottom * 0.9f, dirtyRect.Right - 10, dirtyRect.Bottom * 0.9f + 10);
        // </x axis>

        // <y axis>
        // main line
        canvas.DrawLine(dirtyRect.Right * 0.1f, dirtyRect.Bottom, dirtyRect.Right * 0.1f, dirtyRect.Top);
        // arrow point
        canvas.DrawLine(dirtyRect.Right * 0.1f, dirtyRect.Top, dirtyRect.Right * 0.1f - 10, dirtyRect.Top + 10);
        canvas.DrawLine(dirtyRect.Right * 0.1f, dirtyRect.Top, dirtyRect.Right * 0.1f + 10, dirtyRect.Top + 10);
        // </y axis>

        if (dataTable != null) {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine((float)dataTable[0, 0], (float)dataTable[0, 1], (float)dataTable[1, 0], (float)dataTable[1, 1]);
        }
        if (!isInputValid) {
            canvas.FontColor = Colors.Red;
            canvas.FontSize = 18;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.DrawString("Invalid input", 100, 100, HorizontalAlignment.Left);
        }
    }
}
