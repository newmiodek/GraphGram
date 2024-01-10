namespace GraphGram;
public class LineData {
    private Line leastSteepLine;
    private Line steepestLine;
    private Line lineOfBestFit;
    private double gradientUncertainty;
    private double yInterceptUncertainty;
    private int[] outliers; // Here outliers are represented by their zero-based indices

    public LineData(DataPoint[] dataPoints, bool goWithErrorBoxes) {
        LinePermutation[] lines;
        if(goWithErrorBoxes) {
            lines = CalculateAllLinesBoxes(dataPoints);
        }
        else {
            lines = CalculateAllLinesBars(dataPoints);
        }

        List<List<LinePermutation>> outlierCategories = new List<List<LinePermutation>>();
        for(int i = 0; i < dataPoints.Length - 1; i++) {
            outlierCategories.Add(new List<LinePermutation>());
        }
        for(int i = 0; i < lines.GetLength(0); i++) {
            int tempOutlierCount = lines[i].GetOutlierCount();
            outlierCategories[tempOutlierCount].Add(lines[i]);
        }
        // extract the steep and the least steep lines with the least outliers
        double steepGradient = 0,
               steepYIntercept = 0,
               leastSteepGradient = 0,
               leastSteepYIntercept = 0;
        
        for(int i = 0; i < outlierCategories.Count; i++) {
            if(outlierCategories[i].Count == 0) continue;

            List<OutlierPermutation> outlierPermutations = new List<OutlierPermutation>();

            for(int j = 0; j < outlierCategories[i].Count; j++) {
                OutlierPermutation permutation = new OutlierPermutation(outlierCategories[i][j].GetIntersections());

                bool isThere = false;
                for(int k = 0; k < outlierPermutations.Count; k++) {
                    if(outlierPermutations[k].AreIntersectionsEqual(permutation.GetIntersections())) {
                        isThere = true;
                        outlierPermutations[k].IncrementOccurrences();
                        break;
                    }
                }
                if(!isThere) outlierPermutations.Add(permutation);
            }

            int maxOccurencesIndex = 0;
            for(int j = 1; j < outlierPermutations.Count; j++) {
                if(outlierPermutations[j].GetOccurrences() > outlierPermutations[maxOccurencesIndex].GetOccurrences()) {
                    maxOccurencesIndex = j;
                }
            }

            outliers = new int[i];
            for(int j = 0, k = 0; k < i; j++) {
                if(!outlierPermutations[maxOccurencesIndex].GetIntersections()[j]) {
                    outliers[k] = j;
                    k++;
                }
            }

            steepGradient = double.MinValue;
            steepYIntercept = double.MaxValue;
            leastSteepGradient = double.MaxValue;
            leastSteepYIntercept = double.MinValue;
            for(int j = 0; j < outlierCategories[i].Count; j++) {
                if(outlierPermutations[maxOccurencesIndex].AreIntersectionsEqual(outlierCategories[i][j].GetIntersections())) {
                    if(outlierCategories[i][j].GetGradient() > steepGradient) {
                        steepGradient = outlierCategories[i][j].GetGradient();
                        steepYIntercept = outlierCategories[i][j].GetYIntercept();
                    }
                    if(outlierCategories[i][j].GetGradient() < leastSteepGradient) {
                        leastSteepGradient = outlierCategories[i][j].GetGradient();
                        leastSteepYIntercept = outlierCategories[i][j].GetYIntercept();
                    }
                }
            }

            break;
        }

        leastSteepLine = new Line(leastSteepGradient, leastSteepYIntercept);
        steepestLine = new Line(steepGradient, steepYIntercept);
        lineOfBestFit = new Line((leastSteepGradient + steepGradient) / 2.0, (leastSteepYIntercept + steepYIntercept) / 2.0);
        gradientUncertainty = Math.Abs((steepGradient - leastSteepGradient) / 2.0);
        yInterceptUncertainty = Math.Abs((steepYIntercept - leastSteepYIntercept) / 2.0);
    }

    private LinePermutation[] CalculateAllLinesBoxes(DataPoint[] dataPoints) {
        LinePermutation[] lines = new LinePermutation[dataPoints.Length * (dataPoints.Length - 1)];

        for(int i = 0; i < dataPoints.Length - 1; i++) {
            for(int j = i + 1; j < dataPoints.Length; j++) {
                lines[Order(dataPoints.Length, i, j) * 2] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX() + dataPoints[i].GetUncertaintyX(),
                    dataPoints[i].GetY() - dataPoints[i].GetUncertaintyY(),
                    dataPoints[j].GetX() - dataPoints[j].GetUncertaintyX(),
                    dataPoints[j].GetY() + dataPoints[j].GetUncertaintyY(),
                    i, j, true);

                lines[Order(dataPoints.Length, i, j) * 2 + 1] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX() - dataPoints[i].GetUncertaintyX(),
                    dataPoints[i].GetY() + dataPoints[i].GetUncertaintyY(),
                    dataPoints[j].GetX() + dataPoints[j].GetUncertaintyX(),
                    dataPoints[j].GetY() - dataPoints[j].GetUncertaintyY(),
                    i, j, true);
            }
        }
        return lines;
    }

    private LinePermutation[] CalculateAllLinesBars(DataPoint[] dataPoints) {
        LinePermutation[] lines = new LinePermutation[dataPoints.Length * (dataPoints.Length - 1) * 4];
        for(int i = 0; i < dataPoints.Length - 1; i++) {
            for(int j = i + 1; j < dataPoints.Length; j++) {
                lines[Order(dataPoints.Length, i, j) * 8 + 0] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX(),
                    dataPoints[i].GetY() - dataPoints[i].GetUncertaintyY(),
                    dataPoints[j].GetX(),
                    dataPoints[j].GetY() + dataPoints[j].GetUncertaintyY(),
                    i, j, false);
                lines[Order(dataPoints.Length, i, j) * 8 + 1] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX() + dataPoints[i].GetUncertaintyX(),
                    dataPoints[i].GetY(),
                    dataPoints[j].GetX(),
                    dataPoints[j].GetY() + dataPoints[j].GetUncertaintyY(),
                    i, j, false);
                lines[Order(dataPoints.Length, i, j) * 8 + 2] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX(),
                    dataPoints[i].GetY() - dataPoints[i].GetUncertaintyY(),
                    dataPoints[j].GetX() - dataPoints[j].GetUncertaintyX(),
                    dataPoints[j].GetY(),
                    i, j, false);
                lines[Order(dataPoints.Length, i, j) * 8 + 3] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX() + dataPoints[i].GetUncertaintyX(),
                    dataPoints[i].GetY(),
                    dataPoints[j].GetX() - dataPoints[j].GetUncertaintyX(),
                    dataPoints[j].GetY(),
                    i, j, false);
                lines[Order(dataPoints.Length, i, j) * 8 + 4] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX(),
                    dataPoints[i].GetY() + dataPoints[i].GetUncertaintyY(),
                    dataPoints[j].GetX(),
                    dataPoints[j].GetY() - dataPoints[j].GetUncertaintyY(),
                    i, j, false);
                lines[Order(dataPoints.Length, i, j) * 8 + 5] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX(),
                    dataPoints[i].GetY() + dataPoints[i].GetUncertaintyY(),
                    dataPoints[j].GetX() + dataPoints[j].GetUncertaintyX(),
                    dataPoints[j].GetY(),
                    i, j, false);
                lines[Order(dataPoints.Length, i, j) * 8 + 6] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX() - dataPoints[i].GetUncertaintyX(),
                    dataPoints[i].GetY(),
                    dataPoints[j].GetX(),
                    dataPoints[j].GetY() - dataPoints[j].GetUncertaintyY(),
                    i, j, false);
                lines[Order(dataPoints.Length, i, j) * 8 + 7] = CalculateLine(
                    dataPoints,
                    dataPoints[i].GetX() - dataPoints[i].GetUncertaintyX(),
                    dataPoints[i].GetY(),
                    dataPoints[j].GetX() + dataPoints[j].GetUncertaintyX(),
                    dataPoints[j].GetY(),
                    i, j, false);
            }
        }
        return lines;
    }

    private LinePermutation CalculateLine(DataPoint[] dataPoints, double firstX, double firstY, double secondX, double secondY, int iFirst, int iSecond, bool goWithErrorBoxes) {
        double grad = CalculateGradient(firstX, firstY, secondX, secondY);
        double yint = CalculateIntercept(grad, firstX, firstY);

        bool[] intersects;
        if(goWithErrorBoxes) {
            intersects = IntersectionTestBoxes(dataPoints, grad, yint);
        }
        else {
            intersects = IntersectionTestBars(dataPoints, grad, yint);
        }

        LinePermutation line = new LinePermutation(grad, yint, 0, intersects);

        line.SetSingleIntersection(iFirst, true);
        line.SetSingleIntersection(iSecond, true);

        for(int i = line.GetIntersections().Length - 1; i > -1; i--) {
            if(!line.GetSingleIntersection(i)) {
                line.IncrementOutlierCount();
            }
        }
        return line;
    }

    private bool[] IntersectionTestBoxes(DataPoint[] dataPoints, double gradient, double intercept) {
        bool[] intersections = new bool[dataPoints.Length];
        for(int i = 0; i < intersections.Length; i++) intersections[i] = true;

        for(int i = 0; i < intersections.Length; i++) {
            double dpIntercept;
            /* dpIntercept is the value of x or y (depending on which side
            of the box is being tested) at which the given line intersects
            a line which contains the side of the box being tested;
            if the value is between the x or y values of the box's sides
            then it intercepts the box */

            dpIntercept = (dataPoints[i].GetY() + dataPoints[i].GetUncertaintyY() - intercept) / gradient;
            if(dpIntercept >= dataPoints[i].GetX() - dataPoints[i].GetUncertaintyX()
               && dpIntercept <= dataPoints[i].GetX() + dataPoints[i].GetUncertaintyX()) {
                continue;
            }

            dpIntercept = (dataPoints[i].GetY() - dataPoints[i].GetUncertaintyY() - intercept) / gradient;
            if(dpIntercept >= dataPoints[i].GetX() - dataPoints[i].GetUncertaintyX()
                && dpIntercept <= dataPoints[i].GetX() + dataPoints[i].GetUncertaintyX()) {
                continue;
            }

            dpIntercept = gradient * (dataPoints[i].GetX() + dataPoints[i].GetUncertaintyX()) + intercept;
            if(dpIntercept >= dataPoints[i].GetY() - dataPoints[i].GetUncertaintyY()
                && dpIntercept <= dataPoints[i].GetY() + dataPoints[i].GetUncertaintyY()) {
                continue;
            }

            dpIntercept = gradient * (dataPoints[i].GetX() - dataPoints[i].GetUncertaintyX()) + intercept;
            if(dpIntercept >= dataPoints[i].GetY() - dataPoints[i].GetUncertaintyY()
                && dpIntercept <= dataPoints[i].GetY() + dataPoints[i].GetUncertaintyY()) {
                continue;
            }

            intersections[i] = false;
        }
        return intersections;
    }

    private bool[] IntersectionTestBars(DataPoint[] dataPoints, double gradient, double intercept) {
        bool[] intersections = new bool[dataPoints.Length];
        for(int i = 0; i < intersections.Length; i++) intersections[i] = true;

        for(int i = 0; i < intersections.Length; i++) {
            double dp_intercept;

            dp_intercept = (dataPoints[i].GetY() - intercept) / gradient;
            if(dp_intercept >= dataPoints[i].GetX() - dataPoints[i].GetUncertaintyX()
                && dp_intercept <= dataPoints[i].GetX() + dataPoints[i].GetUncertaintyX()) {
                continue;
            }

            dp_intercept = gradient * dataPoints[i].GetX() + intercept;
            if(dp_intercept >= dataPoints[i].GetY() - dataPoints[i].GetUncertaintyY()
                && dp_intercept <= dataPoints[i].GetY() + dataPoints[i].GetUncertaintyY()) {
                continue;
            }

            intersections[i] = false;
        }
        return intersections;
    }

    private static int Order(int n, int x, int y) {
        return (n * (n - 1) - (n - x) * (n - x - 1)) / 2 + y - x - 1;
    }

    private double CalculateGradient(double firstX, double firstY, double secondX, double secondY) {
        return (secondY - firstY) / (secondX - firstX);
    }

    private double CalculateIntercept(double gradient, double x, double y) {
        return y - gradient * x;
    }

    public Line GetLeastSteepLine() {
        return leastSteepLine;
    }
    public Line GetSteepestLine() {
        return steepestLine;
    }
    public Line GetLineOfBestFit() {
        return lineOfBestFit;
    }
    public double GetGradientUncertainty() {
        return gradientUncertainty;
    }
    public double GetYInterceptUncertainty() {
        return yInterceptUncertainty;
    }
    public int[] GetOutliers() {
        return outliers;
    }

    public override string ToString() {
        string toStringed = "LineData {\n"
                          + "Least Steep Line: " + leastSteepLine.ToString() + "\n"
                          + "Steepest Line: " + steepestLine.ToString() + "\n"
                          + "Line of Best Fit: " + lineOfBestFit.ToString() + "\n"
                          + "Gradient Uncertainty: " + gradientUncertainty.ToString() + "\n"
                          + "Y-Intercept Uncertainty: " + yInterceptUncertainty.ToString() + "\n"
                          + "Outliers: " + outliers.ToString() + "\n"
                          + "}";


        return toStringed;
    }
}
