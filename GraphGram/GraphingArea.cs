namespace GraphGram;

public class GraphingArea : IDrawable {

    private double[,] dataTable;
    private bool isInputValid = true;

    // consider changing them to floats
    private double minX = 0;
    private double maxX = 100;
    private double minY = 0;
    private double maxY = 100;

    public void PassDataTable(Entry[,] entryTable) {
        // Parse the text from entryTable to represent it as floats
        double[,] parsedDataTable = new double[entryTable.GetLength(0), 4];
        bool parseSucceded = true;
        minX = double.MaxValue;
        maxX = double.MinValue;
        minY = double.MaxValue;
        maxY = double.MinValue;
        for(int i = 0; i < parsedDataTable.GetLength(0); i++) {
            for(int j = 0; j < 4; j++) {
                if(!double.TryParse(entryTable[i, j].Text, out parsedDataTable[i, j])) {
                    parseSucceded = false;
                    break;
                }
                if((j == 3 || j == 4) && parsedDataTable[i, j] < 0) {
                    parseSucceded = false;
                    break;
                }
            }
            if(!parseSucceded) break;

            minX = Math.Min(parsedDataTable[i, 0] - parsedDataTable[i, 2], minX);
            maxX = Math.Max(parsedDataTable[i, 0] + parsedDataTable[i, 2], maxX);
            minY = Math.Min(parsedDataTable[i, 1] - parsedDataTable[i, 3], minY);
            maxY = Math.Max(parsedDataTable[i, 1] + parsedDataTable[i, 3], maxY);
        }
        if(parseSucceded) {
            dataTable = parsedDataTable;
        }
        isInputValid = parseSucceded;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        float xAxisVerticalPosition = CalculateXAxisPosition(dirtyRect);
        float yAxisHorizontalPosition = CalculateYAxisPosition(dirtyRect);
        DrawGrid(canvas, dirtyRect, xAxisVerticalPosition, yAxisHorizontalPosition);
        DrawXAxis(canvas, dirtyRect, xAxisVerticalPosition);
        DrawYAxis(canvas, dirtyRect, yAxisHorizontalPosition);

        if(!isInputValid) {
            canvas.FontColor = Colors.Red;
            canvas.FontSize = 18;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.DrawString("Invalid input", 100, 100, HorizontalAlignment.Left);
            return;
        }

        if(dataTable != null) {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine((float)dataTable[0, 0], (float)dataTable[0, 1], (float)dataTable[1, 0], (float)dataTable[1, 1]);
        }

    }

    private float CalculateXAxisPosition(RectF dirtyRect) {
        float xAxisVerticalPosition;
        if(minY >= 0) {
            xAxisVerticalPosition = dirtyRect.Bottom * 0.9f;
        }
        else if(minY < 0 && maxY > 0) {
            xAxisVerticalPosition = dirtyRect.Bottom * (0.8f * (float)(maxY / (maxY - minY)) + 0.1f);
        }
        else {
            xAxisVerticalPosition = dirtyRect.Bottom * 0.1f;
        }
        return xAxisVerticalPosition;
    }

    private float CalculateYAxisPosition(RectF dirtyRect) {
        float yAxisHorizontalPosition;
        if(minX >= 0) {
            yAxisHorizontalPosition = dirtyRect.Right * 0.1f;
        }
        else if(minX < 0 && maxX > 0) {
            yAxisHorizontalPosition = dirtyRect.Right * (0.8f * (float)(-minX / (maxX - minX)) + 0.1f);
        }
        else {
            yAxisHorizontalPosition = dirtyRect.Right * 0.9f;
        }
        return yAxisHorizontalPosition;
    }

    private void DrawXAxis(ICanvas canvas, RectF dirtyRect, float xAxisVerticalPosition) {
        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 1;
        canvas.DrawLine(dirtyRect.Left, xAxisVerticalPosition, dirtyRect.Right, xAxisVerticalPosition);             // main line
        canvas.DrawLine(dirtyRect.Right, xAxisVerticalPosition, dirtyRect.Right - 10, xAxisVerticalPosition - 10);  // arrow
        canvas.DrawLine(dirtyRect.Right, xAxisVerticalPosition, dirtyRect.Right - 10, xAxisVerticalPosition + 10);  // arrow
    }

    private void DrawYAxis(ICanvas canvas, RectF dirtyRect, float yAxisHorizontalPosition) {
        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 1;
        canvas.DrawLine(yAxisHorizontalPosition, dirtyRect.Bottom, yAxisHorizontalPosition, dirtyRect.Top);           // main line
        canvas.DrawLine(yAxisHorizontalPosition, dirtyRect.Top, yAxisHorizontalPosition - 10, dirtyRect.Top + 10);    // arrow
        canvas.DrawLine(yAxisHorizontalPosition, dirtyRect.Top, yAxisHorizontalPosition + 10, dirtyRect.Top + 10);    // arrow
    }

    private void DrawGrid(ICanvas canvas, RectF dirtyRect, float xAxisVerticalPosition, float yAxisHorizontalPosition) {
        // float xSpacingValue = (float)Math.Ceiling((maxX - minX) / 10.0);
        // float ySpacingValue = (float)Math.Ceiling((maxY - minY) / 10.0);

        float xSpacingPixels = dirtyRect.Width * 0.08f;     // (width * 0.8) / 10
        float ySpacingPixels = dirtyRect.Height * 0.08f;    // (height * 0.8) / 10
        
        canvas.StrokeColor = Color.FromRgb(0x30, 0x30, 0x30);
        canvas.StrokeSize = 1;
        
        // Vertical lines to the left of the origin
        for(float x = yAxisHorizontalPosition - xSpacingPixels; x >= dirtyRect.Left; x -= xSpacingPixels) {
            canvas.DrawLine(x, dirtyRect.Top, x, dirtyRect.Bottom);
        }

        // Vertical lines to the right of the origin
        for(float x = yAxisHorizontalPosition + xSpacingPixels; x <= dirtyRect.Right; x += xSpacingPixels) {
            canvas.DrawLine(x, dirtyRect.Top, x, dirtyRect.Bottom);
        }

        // Horizontal lines above the origin
        for(float y = xAxisVerticalPosition - ySpacingPixels; y >= dirtyRect.Top; y -= ySpacingPixels) {
            canvas.DrawLine(dirtyRect.Left, y, dirtyRect.Right, y);
        }

        // Horizontal lines below the origin
        for(float y = xAxisVerticalPosition + ySpacingPixels; y <= dirtyRect.Bottom; y += ySpacingPixels) {
            canvas.DrawLine(dirtyRect.Left, y, dirtyRect.Right, y);
        }
    }

}
