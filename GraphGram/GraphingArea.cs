namespace GraphGram;

public class GraphingArea : IDrawable {

    private double[,] dataTable;
    private DataPoint[] dataPoints;
    private LineData lineData;

    private bool isInitiated = false;
    private bool isInputValid = true;

    // consider changing them to floats
    private double minX = 0.0;
    private double maxX = 100.0;
    private double minY = 0.0;
    private double maxY = 100.0;

    private float xSpacingValue = 10f;
    private float ySpacingValue = 10f;

    private double xRange = 100.0;
    private double yRange = 100.0;

    private string xTitleString = "x";
    private string yTitleString = "y";
    private SuperscriptedString xTitleSupString;
    private SuperscriptedString yTitleSupString;
    private bool isXTitleSupStringUpToDate = false;
    private bool isYTitleSupStringUpToDate = false;

    public void PassDataTable(Entry[,] entryTable) {
        // Parse the text from entryTable to represent it as floats
        double[,] parsedDataTable = new double[entryTable.GetLength(0), 4];
        int validRows = 0;
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
            if(!parseSucceded) {
                validRows = i;
                break;
            }

            minX = Math.Min(parsedDataTable[i, 0] - parsedDataTable[i, 2], minX);
            maxX = Math.Max(parsedDataTable[i, 0] + parsedDataTable[i, 2], maxX);
            minY = Math.Min(parsedDataTable[i, 1] - parsedDataTable[i, 3], minY);
            maxY = Math.Max(parsedDataTable[i, 1] + parsedDataTable[i, 3], maxY);
        }

        isInputValid = validRows >= 2;
        if(!isInputValid) { return; }

        isInitiated = true;

        if(parseSucceded) dataTable = parsedDataTable;
        else {
            dataTable = new double[validRows, 4];

            for(int i = 0; i < validRows; i++) {
                for(int j = 0; j < 4; j++) {
                    /* The deal with these locals is the same
                     * as in the constructor of MainPage
                     */
                    int localI = 1 * i;
                    int localJ = 1 * j;
                    dataTable[localI, localJ] = parsedDataTable[localI, localJ];
                }
            }
        }

        dataPoints = new DataPoint[dataTable.GetLength(0)];
        for(int i = 0; i < dataTable.GetLength(0); i++) {
            dataPoints[i] = new DataPoint(dataTable[i, 0], dataTable[i, 1], dataTable[i, 2], dataTable[i, 3]);
        }

        lineData = new LineData(dataPoints);

        xRange = Math.Max(0, maxX) - Math.Min(0, minX);
        double xMagnitude = Math.Log10(xRange);
        double xDecimalPart = xMagnitude - Math.Truncate(xMagnitude);
        if(xMagnitude > 0) {
            if(xDecimalPart < Constants.SPACING_THRESHOLD[1]) {
                xSpacingValue = (float)Math.Pow(10, Math.Floor(xMagnitude) - 1);
            }
            else if(Constants.SPACING_THRESHOLD[1] <= xDecimalPart && xDecimalPart < Constants.SPACING_THRESHOLD[2]) {
                xSpacingValue = (float)(2.0 * Math.Pow(10, Math.Floor(xMagnitude) - 1));
            }
            else if(Constants.SPACING_THRESHOLD[2] <= xDecimalPart && xDecimalPart < Constants.SPACING_THRESHOLD[5]) {
                xSpacingValue = (float)(5.0 * Math.Pow(10, Math.Floor(xMagnitude) - 1));
            }
            else {
                xSpacingValue = (float)Math.Pow(10, Math.Floor(xMagnitude));
            }
        }
        else {
            if(xDecimalPart == 0) {
                xSpacingValue = (float)Math.Pow(10, xMagnitude - 1.0);
            }
            else if(xDecimalPart > Constants.SPACING_THRESHOLD[-10]) {
                xSpacingValue = (float)Math.Pow(10, Math.Floor(xMagnitude));
            }
            else if(Constants.SPACING_THRESHOLD[-10] >= xDecimalPart && xDecimalPart > Constants.SPACING_THRESHOLD[-5]) {
                xSpacingValue = (float)(5.0 * Math.Pow(10, Math.Floor(xMagnitude) - 1.0));
            }
            else if(Constants.SPACING_THRESHOLD[-5] >= xDecimalPart && xDecimalPart > Constants.SPACING_THRESHOLD[-2]) {
                xSpacingValue = (float)(2.0 * Math.Pow(10, Math.Floor(xMagnitude) - 1.0));
            }
            else {
                xSpacingValue = (float)Math.Pow(10, Math.Floor(xMagnitude) - 1.0);
            }
        }

        yRange = Math.Max(0, maxY) - Math.Min(0, minY);
        double yMagnitude = Math.Log10(yRange);
        double yDecimalPart = yMagnitude - Math.Truncate(yMagnitude);
        if(yMagnitude > 0) {
            if(yDecimalPart < Constants.SPACING_THRESHOLD[1]) {
                ySpacingValue = (float)Math.Pow(10, Math.Floor(yMagnitude) - 1.0);
            }
            else if(Constants.SPACING_THRESHOLD[1] <= yDecimalPart && yDecimalPart < Constants.SPACING_THRESHOLD[2]) {
                ySpacingValue = (float)(2.0 * Math.Pow(10, Math.Floor(yMagnitude) - 1.0));
            }
            else if(Constants.SPACING_THRESHOLD[2] <= yDecimalPart && yDecimalPart < Constants.SPACING_THRESHOLD[5]) {
                ySpacingValue = (float)(5.0 * Math.Pow(10, Math.Floor(yMagnitude) - 1.0));
            }
            else {
                ySpacingValue = (float)Math.Pow(10, Math.Floor(yMagnitude));
            }
        }
        else {
            if(yDecimalPart == 0) {
                ySpacingValue = (float)Math.Pow(10, yMagnitude - 1.0);
            }
            else if(yDecimalPart > Constants.SPACING_THRESHOLD[-10]) {
                ySpacingValue = (float)Math.Pow(10, Math.Floor(yMagnitude));
            }
            else if(Constants.SPACING_THRESHOLD[-10] >= yDecimalPart && yDecimalPart > Constants.SPACING_THRESHOLD[-5]) {
                ySpacingValue = (float)(5.0 * Math.Pow(10, Math.Floor(yMagnitude) - 1.0));
            }
            else if(Constants.SPACING_THRESHOLD[-5] >= yDecimalPart && yDecimalPart > Constants.SPACING_THRESHOLD[-2]) {
                ySpacingValue = (float)(2.0 * Math.Pow(10, Math.Floor(yMagnitude) - 1.0));
            }
            else {
                ySpacingValue = (float)Math.Pow(10, Math.Floor(yMagnitude) - 1.0);
            }
        }
    }

    public void SetXAxisTitle(string xTitle) {
        this.xTitleString = xTitle;
        isXTitleSupStringUpToDate = false;
    }
    public void SetYAxisTitle(string yTitle) {
        this.yTitleString = yTitle;
        isYTitleSupStringUpToDate = false;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        // <Calculations>
        float OriginX = CalculateOriginX(dirtyRect);
        float OriginY = CalculateOriginY(dirtyRect);

        float xSpacingPixels = dirtyRect.Width  * (1f - 2f * Constants.PADDING) / (float)Math.Ceiling(xRange / xSpacingValue);
        float ySpacingPixels = dirtyRect.Height * (1f - 2f * Constants.PADDING) / (float)Math.Ceiling(yRange / ySpacingValue);
        // </Calculations>

        DrawGrid(canvas, dirtyRect, OriginX, OriginY, xSpacingPixels, ySpacingPixels);
        DrawXAxis(canvas, dirtyRect, OriginY);
        DrawYAxis(canvas, dirtyRect, OriginX);

        if(isInputValid && isInitiated) {
            DrawDataPoints(canvas, dirtyRect, dataPoints, OriginX, OriginY, xSpacingPixels, ySpacingPixels);
            DrawLine(canvas, dirtyRect, lineData.GetLeastSteepLine(), Colors.Red, OriginX, OriginY, xSpacingPixels, ySpacingPixels);
            DrawLine(canvas, dirtyRect, lineData.GetSteepestLine(), Colors.Green, OriginX, OriginY, xSpacingPixels, ySpacingPixels);
            DrawLine(canvas, dirtyRect, lineData.GetLineOfBestFit(), Application.Current.RequestedTheme == AppTheme.Light ? Colors.Black : Colors.White, OriginX, OriginY, xSpacingPixels, ySpacingPixels);
        }

        DrawAxisMarks(canvas, dirtyRect, OriginX, OriginY, xSpacingPixels, ySpacingPixels, xSpacingValue, ySpacingValue);
        DrawAxisTitles(canvas, dirtyRect, OriginX, OriginY);

        if(!isInputValid) {
            canvas.FontColor = Colors.Red;
            canvas.FontSize = Constants.GRAPHING_AREA_FONT_SIZE;
            canvas.Font = Constants.FONT;
            canvas.DrawString("Invalid input", 100f, 100f, HorizontalAlignment.Left);
            return;
        }

    }

    public string GetGradient() {
        if(isInputValid && isInitiated) {
            return lineData.GetLineOfBestFit().GetGradient().ToString()
                 + " \u00B1 "
                 + lineData.GetGradientUncertainty().ToString();
        }
        return "-- \u00B1 --";
    }

    public string GetYIntercept() {
        if(isInputValid && isInitiated) {
            return lineData.GetLineOfBestFit().GetYIntercept().ToString()
                 + " \u00B1 "
                 + lineData.GetYInterceptUncertainty().ToString();
        }
        return "-- \u00B1 --";
    }

    private float CalculateOriginX(RectF dirtyRect) {
        float OriginX;
        if(minX >= 0)                   OriginX = dirtyRect.Right * Constants.PADDING;
        else if(minX < 0 && maxX > 0)   OriginX = dirtyRect.Right * 
                                            ((1f - 2f * Constants.PADDING) * (float)(-minX / (maxX - minX)) + Constants.PADDING);
        else                            OriginX = dirtyRect.Right * (1f - Constants.PADDING);
        return OriginX;
    }

    private float CalculateOriginY(RectF dirtyRect) {
        float OriginY;
        if(minY >= 0)                   OriginY = dirtyRect.Bottom * (1f - Constants.PADDING);
        else if(minY < 0 && maxY > 0)   OriginY = dirtyRect.Bottom *
                                            ((1f - 2f * Constants.PADDING) * (float)(maxY / (maxY - minY)) + Constants.PADDING);
        else                            OriginY = dirtyRect.Bottom * Constants.PADDING;
        return OriginY;
    }

    private void DrawXAxis(ICanvas canvas, RectF dirtyRect, float OriginY) {
        canvas.StrokeColor = 
            Application.Current.RequestedTheme == AppTheme.Light
            ? Colors.Black
            : Colors.White;
        canvas.StrokeSize = 1;

        canvas.DrawLine(dirtyRect.Left, OriginY, dirtyRect.Right, OriginY);             // main line
        canvas.DrawLine(dirtyRect.Right, OriginY, dirtyRect.Right - 10f, OriginY - 10f);  // arrow
        canvas.DrawLine(dirtyRect.Right, OriginY, dirtyRect.Right - 10f, OriginY + 10f);  // arrow
    }

    private void DrawYAxis(ICanvas canvas, RectF dirtyRect, float OriginX) {
        canvas.StrokeColor = 
            Application.Current.RequestedTheme == AppTheme.Light
            ? Colors.Black
            : Colors.White;
        canvas.StrokeSize = 1f;

        canvas.DrawLine(OriginX, dirtyRect.Bottom, OriginX, dirtyRect.Top);           // main line
        canvas.DrawLine(OriginX, dirtyRect.Top, OriginX - 10f, dirtyRect.Top + 10f);    // arrow
        canvas.DrawLine(OriginX, dirtyRect.Top, OriginX + 10f, dirtyRect.Top + 10f);    // arrow
    }

    private void DrawGrid(ICanvas canvas, RectF dirtyRect, float OriginX, float OriginY, float xSpacingPixels, float ySpacingPixels) {
        canvas.StrokeColor =
            Application.Current.RequestedTheme == AppTheme.Light
            ? Color.FromRgb(0xB0, 0xB0, 0xB0)
            : Color.FromRgb(0x30, 0x30, 0x30);


            //Color.FromRgb(0x30, 0x30, 0x30);
        canvas.StrokeSize = 1f;

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
        canvas.Font = Constants.FONT;
        canvas.FontColor =
            Application.Current.RequestedTheme == AppTheme.Light
            ? Colors.Black
            : Colors.White;
        canvas.FontSize = Constants.GRAPHING_AREA_FONT_SIZE;

        SizeF fontDimensions = canvas.GetStringSize("0", Constants.FONT, Constants.GRAPHING_AREA_FONT_SIZE);

        float markValue = -xSpacingValue;
        // Marks on the X AXIS to the LEFT of the origin
        for(float x = OriginX - xSpacingPixels; x >= dirtyRect.Left; x -= xSpacingPixels) {
            canvas.DrawString(markValue.ToString(), x - 250f, OriginY + fontDimensions.Height, 500f, 100f, HorizontalAlignment.Center, VerticalAlignment.Top);
            markValue -= xSpacingValue;
        }

        markValue = xSpacingValue;
        // Marks on the X AXIS to the RIGHT of the origin
        for(float x = OriginX + xSpacingPixels; x <= dirtyRect.Right; x += xSpacingPixels) {
            canvas.DrawString(markValue.ToString(), x - 250f, OriginY + fontDimensions.Height, 500f, 100f, HorizontalAlignment.Center, VerticalAlignment.Top);
            markValue += xSpacingValue;
        }

        markValue = -ySpacingValue;
        // Marks on the Y AXIS BELOW the origin
        for(float y = OriginY + ySpacingPixels; y <= dirtyRect.Bottom; y += ySpacingPixels) {
            canvas.DrawString(markValue.ToString(), OriginX - 500f, y - fontDimensions.Height, 500f - fontDimensions.Width, 100f, HorizontalAlignment.Right, VerticalAlignment.Top);
            markValue -= ySpacingValue;
        }

        markValue = ySpacingValue;
        // Marks on the Y AXIS ABOVE the origin
        for(float y = OriginY - ySpacingPixels; y >= dirtyRect.Top; y -= ySpacingPixels) {
            canvas.DrawString(markValue.ToString(), OriginX - 500f, y - fontDimensions.Height, 500f - fontDimensions.Width, 100f, HorizontalAlignment.Right, VerticalAlignment.Top);
            markValue += ySpacingValue;
        }

        canvas.DrawString("0", OriginX + fontDimensions.Width * 0.5f, OriginY + fontDimensions.Height, 100f, 100f, HorizontalAlignment.Left, VerticalAlignment.Top);         // x axis zero
        canvas.DrawString("0", OriginX - 500f, OriginY - fontDimensions.Height * 2f, 500f - fontDimensions.Width, 100f, HorizontalAlignment.Right, VerticalAlignment.Top); // y axis zero
    }

    private void DrawAxisTitles(ICanvas canvas, RectF dirtyRect, float OriginX, float OriginY) {
        canvas.FontColor =
            Application.Current.RequestedTheme == AppTheme.Light
            ? Colors.Black
            : Colors.White;

        if(!isXTitleSupStringUpToDate) {
            xTitleSupString = new SuperscriptedString(xTitleString, canvas, Constants.GRAPHING_AREA_FONT_SIZE);
        }
        if(!isYTitleSupStringUpToDate) {
            yTitleSupString = new SuperscriptedString(yTitleString, canvas, Constants.GRAPHING_AREA_FONT_SIZE);
        }

        SuperscriptedStringSize xTitleSupStringSize = xTitleSupString.GetSize(canvas, Constants.GRAPHING_AREA_FONT_SIZE);
        SuperscriptedStringSize yTitleSupStringSize = yTitleSupString.GetSize(canvas, Constants.GRAPHING_AREA_FONT_SIZE);

        RectF xTitleDirtyRect = new RectF(
            dirtyRect.Right - Constants.GRAPHING_AREA_FONT_SIZE - xTitleSupStringSize.TotalWidth,
            OriginY - Constants.GRAPHING_AREA_FONT_SIZE * 3f,
            xTitleSupStringSize.TotalWidth,
            Constants.GRAPHING_AREA_FONT_SIZE * 3f);

        RectF yTitleDirtyRect = new RectF(
            OriginX + Constants.GRAPHING_AREA_FONT_SIZE,
            dirtyRect.Top,
            yTitleSupStringSize.TotalWidth,
            Constants.GRAPHING_AREA_FONT_SIZE * 3f);

        xTitleSupString.Draw(canvas, xTitleDirtyRect, Constants.GRAPHING_AREA_FONT_SIZE);
        yTitleSupString.Draw(canvas, yTitleDirtyRect, Constants.GRAPHING_AREA_FONT_SIZE);

        // canvas.DrawString(xTitleString, dirtyRect.Right - Constants.GRAPHING_AREA_FONT_SIZE, OriginY - Constants.GRAPHING_AREA_FONT_SIZE, HorizontalAlignment.Right);
        // canvas.DrawString(yTitleString, OriginX + Constants.GRAPHING_AREA_FONT_SIZE, dirtyRect.Top + Constants.GRAPHING_AREA_FONT_SIZE, HorizontalAlignment.Left);
    }

    private void DrawLine(ICanvas canvas, RectF dirtyRect, Line line, Color color, float OriginX, float OriginY, float xSpacingPixels, float ySpacingPixels) {
        float spacingRatio = xSpacingValue * ySpacingPixels / (xSpacingPixels * ySpacingValue);

        // These values are defined in terms of pixels
        float gradient = (float)line.GetGradient() * spacingRatio;
        float yIntercept = (float)line.GetYIntercept() * ySpacingPixels / ySpacingValue;

        canvas.StrokeSize = 3;
        canvas.StrokeColor = color;
        canvas.DrawLine(dirtyRect.Left, OriginY - ((dirtyRect.Left - OriginX) * gradient + yIntercept), dirtyRect.Right, OriginY - ((dirtyRect.Right - OriginX) * gradient + yIntercept));
    }

    private void DrawDataPoints(ICanvas canvas, RectF dirtyRect, DataPoint[] dataPoints, float OriginX, float OriginY, float xSpacingPixels, float ySpacingPixels) {
        float xScale = xSpacingPixels / xSpacingValue;
        float yScale = ySpacingPixels / ySpacingValue;
        canvas.StrokeColor =
            Application.Current.RequestedTheme == AppTheme.Light
            ? Colors.Black
            : Colors.White;
        canvas.StrokeSize = 1;
        for(int i = 0; i < dataPoints.Length; i++) {
            float xPixels = OriginX + (float)dataPoints[i].GetX() * xScale;
            float yPixels = OriginY - (float)dataPoints[i].GetY() * yScale;
            float uncXPixels = (float)dataPoints[i].GetUncertaintyX() * xScale;
            float uncYPixels = (float)dataPoints[i].GetUncertaintyY() * yScale;

            canvas.DrawCircle(xPixels, yPixels, 2);

            canvas.DrawLine(xPixels - uncXPixels, yPixels, xPixels + uncXPixels, yPixels);
            canvas.DrawLine(xPixels, yPixels - uncYPixels, xPixels, yPixels + uncYPixels);

            canvas.DrawLine(xPixels - uncXPixels, yPixels - 3f, xPixels - uncXPixels, yPixels + 3f);
            canvas.DrawLine(xPixels + uncXPixels, yPixels - 3f, xPixels + uncXPixels, yPixels + 3f);
            canvas.DrawLine(xPixels - 3f, yPixels - uncYPixels, xPixels + 3f, yPixels - uncYPixels);
            canvas.DrawLine(xPixels - 3f, yPixels + uncYPixels, xPixels + 3f, yPixels + uncYPixels);
        }
    }
}
