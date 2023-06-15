namespace GraphGram;

public class GraphingArea : IDrawable {

    private double[,] dataTable;
    private bool isInputValid = true;

    // consider changing them to floats
    private double minX = 0;
    private double maxX = 1;
    private double minY = 0;
    private double maxY = 1;

    public void PassDataTable(Entry[,] entryTable) {
        // Parse the text from entryTable to represent it as floats
        double[,] parsedDataTable = new double[entryTable.GetLength(0), 4];
        bool parseSucceded = true;
        minX = double.MaxValue;
        maxX = double.MinValue;
        minY = double.MaxValue;
        maxY = double.MinValue;
        for (int i = 0; i < parsedDataTable.GetLength(0); i++) {
            for (int j = 0; j < 4; j++) {
                if (!double.TryParse(entryTable[i, j].Text, out parsedDataTable[i, j])) {
                    parseSucceded = false;
                    break;
                }
                if((j == 3 || j == 4) && parsedDataTable[i, j] < 0) {
                    parseSucceded = false;
                    break;
                }
            }
            if (!parseSucceded) break;

            minX = parsedDataTable[i, 0] - parsedDataTable[i, 2] < minX
                ? parsedDataTable[i, 0] - parsedDataTable[i, 2]
                : minX;
            maxX = parsedDataTable[i, 0] + parsedDataTable[i, 2] > maxX
                ? parsedDataTable[i, 0] + parsedDataTable[i, 2]
                : maxX;
            minY = parsedDataTable[i, 1] - parsedDataTable[i, 3] < minY
                ? parsedDataTable[i, 1] - parsedDataTable[i, 3]
                : minY;
            maxY = parsedDataTable[i, 1] + parsedDataTable[i, 3] > maxY
                ? parsedDataTable[i, 1] + parsedDataTable[i, 3]
                : maxY;
        }
        if (parseSucceded) {
            dataTable = parsedDataTable;
        }
        isInputValid = parseSucceded;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        DrawAxes(canvas, dirtyRect);

        if (!isInputValid) {
            canvas.FontColor = Colors.Red;
            canvas.FontSize = 18;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.DrawString("Invalid input", 100, 100, HorizontalAlignment.Left);
            return;
        }

        if (dataTable != null) {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine((float)dataTable[0, 0], (float)dataTable[0, 1], (float)dataTable[1, 0], (float)dataTable[1, 1]);
        }

    }

    private void DrawAxes(ICanvas canvas, RectF dirtyRect) {
        float xAxisVerticalPosition;
        if (minY >= 0) {
            xAxisVerticalPosition = dirtyRect.Bottom * 0.9f;
        }
        else if (minY < 0 && maxY > 0) {
            xAxisVerticalPosition = dirtyRect.Bottom * (0.8f * (float)(maxY / (maxY - minY)) + 0.1f);
        }
        else {
            xAxisVerticalPosition = dirtyRect.Bottom * 0.1f;
        }

        float yAxisHorizontalPosition;
        if (minX >= 0) {
            yAxisHorizontalPosition = dirtyRect.Right * 0.1f;
        }
        else if (minX < 0 && maxX > 0) {
            yAxisHorizontalPosition = dirtyRect.Right * (0.8f * (float)(-minX / (maxX - minX)) + 0.1f);
        }
        else {
            yAxisHorizontalPosition = dirtyRect.Right * 0.9f;
        }

        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 1;
        // <x axis>
        canvas.DrawLine(dirtyRect.Left, xAxisVerticalPosition, dirtyRect.Right, xAxisVerticalPosition);             // main line
        canvas.DrawLine(dirtyRect.Right, xAxisVerticalPosition, dirtyRect.Right - 10, xAxisVerticalPosition - 10);  // arrow
        canvas.DrawLine(dirtyRect.Right, xAxisVerticalPosition, dirtyRect.Right - 10, xAxisVerticalPosition + 10);  // arrow
        // </x axis>

        // <y axis>
        canvas.DrawLine(yAxisHorizontalPosition, dirtyRect.Bottom, yAxisHorizontalPosition, dirtyRect.Top);           // main line
        canvas.DrawLine(yAxisHorizontalPosition, dirtyRect.Top, yAxisHorizontalPosition - 10, dirtyRect.Top + 10);    // arrow
        canvas.DrawLine(yAxisHorizontalPosition, dirtyRect.Top, yAxisHorizontalPosition + 10, dirtyRect.Top + 10);    // arrow
        // </y axis>
    }
}
