namespace GraphGram;

public class GraphingArea : IDrawable {

    private double[,] dataTable;
    private bool isInputValid = true;

    // consider changing them to floats
    private double minX = 0;
    private double maxX = 100;
    private double minY = 0;
    private double maxY = 100;

    private static readonly float PADDING = 0.075f;
    private static readonly float FONTSIZE = 18;

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
        // <Calculations>
        float OriginX = CalculateOriginX(dirtyRect);
        float OriginY = CalculateOriginY(dirtyRect);

        float xSpacingValue = (float)Math.Ceiling((maxX - minX) / 10.0);    // Device-independent
        float ySpacingValue = (float)Math.Ceiling((maxY - minY) / 10.0);    // Device-independent

        float xSpacingPixels = dirtyRect.Width * (1f - 2f * PADDING) / 10f;
        float ySpacingPixels = dirtyRect.Height * (1f - 2f * PADDING) / 10f;
        // </Calculations>

        DrawGrid(canvas, dirtyRect, OriginX, OriginY, xSpacingPixels, ySpacingPixels);
        DrawXAxis(canvas, dirtyRect, OriginY);
        DrawYAxis(canvas, dirtyRect, OriginX);
        DrawAxisMarks(canvas, dirtyRect, OriginX, OriginY, xSpacingPixels, ySpacingPixels, xSpacingValue, ySpacingValue);

        if(!isInputValid) {
            canvas.FontColor = Colors.Red;
            canvas.FontSize = 18;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.DrawString("Invalid input", 100, 100, HorizontalAlignment.Left);
            return;
        }

    }

    private float CalculateOriginX(RectF dirtyRect) {
        float OriginX;
        if(minX >= 0) {
            OriginX = dirtyRect.Right * PADDING;
        }
        else if(minX < 0 && maxX > 0) {
            OriginX = dirtyRect.Right * ((1f - 2f * PADDING) * (float)(-minX / (maxX - minX)) + PADDING);
        }
        else {
            OriginX = dirtyRect.Right * (1f - PADDING);
        }
        return OriginX;
    }

    private float CalculateOriginY(RectF dirtyRect) {
        float OriginY;
        if(minY >= 0) {
            OriginY = dirtyRect.Bottom * (1f - PADDING);
        }
        else if(minY < 0 && maxY > 0) {
            OriginY = dirtyRect.Bottom * ((1f - 2f * PADDING) * (float)(maxY / (maxY - minY)) + PADDING);
        }
        else {
            OriginY = dirtyRect.Bottom * PADDING;
        }
        return OriginY;
    }

    private void DrawXAxis(ICanvas canvas, RectF dirtyRect, float OriginY) {
        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 1;
        canvas.DrawLine(dirtyRect.Left, OriginY, dirtyRect.Right, OriginY);             // main line
        canvas.DrawLine(dirtyRect.Right, OriginY, dirtyRect.Right - 10, OriginY - 10);  // arrow
        canvas.DrawLine(dirtyRect.Right, OriginY, dirtyRect.Right - 10, OriginY + 10);  // arrow
    }

    private void DrawYAxis(ICanvas canvas, RectF dirtyRect, float OriginX) {
        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 1;
        canvas.DrawLine(OriginX, dirtyRect.Bottom, OriginX, dirtyRect.Top);           // main line
        canvas.DrawLine(OriginX, dirtyRect.Top, OriginX - 10, dirtyRect.Top + 10);    // arrow
        canvas.DrawLine(OriginX, dirtyRect.Top, OriginX + 10, dirtyRect.Top + 10);    // arrow
    }

    private void DrawGrid(ICanvas canvas, RectF dirtyRect, float OriginX, float OriginY, float xSpacingPixels, float ySpacingPixels) {
        canvas.StrokeColor = Color.FromRgb(0x30, 0x30, 0x30);
        canvas.StrokeSize = 1;
        
        // Vertical lines to the LEFT of the origin
        for(float x = OriginX - xSpacingPixels; x >= dirtyRect.Left; x -= xSpacingPixels) {
            canvas.DrawLine(x, dirtyRect.Top, x, dirtyRect.Bottom);
        }

        // Vertical lines to the RIGHT of the origin
        for(float x = OriginX + xSpacingPixels; x <= dirtyRect.Right; x += xSpacingPixels) {
            canvas.DrawLine(x, dirtyRect.Top, x, dirtyRect.Bottom);
        }

        // Horizontal lines ABOVE the origin
        for(float y = OriginY - ySpacingPixels; y >= dirtyRect.Top; y -= ySpacingPixels) {
            canvas.DrawLine(dirtyRect.Left, y, dirtyRect.Right, y);
        }

        // Horizontal lines BELOW the origin
        for(float y = OriginY + ySpacingPixels; y <= dirtyRect.Bottom; y += ySpacingPixels) {
            canvas.DrawLine(dirtyRect.Left, y, dirtyRect.Right, y);
        }
    }

    private void DrawAxisMarks(ICanvas canvas, RectF dirtyRect, float OriginX, float OriginY, float xSpacingPixels, float ySpacingPixels, float xSpacingValue, float ySpacingValue) {
        canvas.Font = Microsoft.Maui.Graphics.Font.Default;
        canvas.FontColor = Colors.White;
        canvas.FontSize = 18;

        float markValue = -xSpacingValue;
        // Marks on the X AXIS to the LEFT of the origin
        for(float x = OriginX - xSpacingPixels; x >= dirtyRect.Left; x -= xSpacingPixels) {
            canvas.DrawString(string.Format("{0}", markValue), x - 250f, OriginY + FONTSIZE, 500f, 100f, HorizontalAlignment.Center, VerticalAlignment.Top);
            markValue -= xSpacingValue;
        }

        markValue = xSpacingValue;
        // Marks on the X AXIS to the RIGHT of the origin
        for(float x = OriginX + xSpacingPixels; x <= dirtyRect.Right; x += xSpacingPixels) {
            canvas.DrawString(string.Format("{0}", markValue), x - 250f, OriginY + FONTSIZE, 500f, 100f, HorizontalAlignment.Center, VerticalAlignment.Top);
            markValue += xSpacingValue;
        }

        markValue = -ySpacingValue;
        // Marks on the Y AXIS BELOW the origin
        for(float y = OriginY + ySpacingPixels; y <= dirtyRect.Bottom; y += ySpacingPixels) {
            canvas.DrawString(string.Format("{0}", markValue), OriginX - 500f, y - FONTSIZE * 0.75f, 500f - FONTSIZE, 100f, HorizontalAlignment.Right, VerticalAlignment.Top);
            markValue -= ySpacingValue;
        }

        markValue = ySpacingValue;
        // Marks on the Y AXIS ABOVE the origin
        for(float y = OriginY - ySpacingPixels; y >= dirtyRect.Top; y -= ySpacingPixels) {
            canvas.DrawString(string.Format("{0}", markValue), OriginX - 500f, y - FONTSIZE * 0.75f, 500f - FONTSIZE, 100f, HorizontalAlignment.Right, VerticalAlignment.Top);
            markValue += ySpacingValue;
        }

        canvas.DrawString("0", OriginX + FONTSIZE * 0.5f, OriginY + FONTSIZE, 100f, 100f, HorizontalAlignment.Left, VerticalAlignment.Top);   // x axis zero
        canvas.DrawString("0", OriginX - 500f, OriginY - FONTSIZE * 1.5f, 500f - FONTSIZE, 100f, HorizontalAlignment.Right, VerticalAlignment.Top);   // y axis zero
    }
}
