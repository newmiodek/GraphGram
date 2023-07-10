namespace GraphGram;
public class GraphResults {
    private string bestFitLineGradient;
    private string bestFitLineYIntercept;
    private string outliers;
    private string steepestGradient;
    private string steepestYIntercept;
    private string leastSteepGradient;
    private string leastSteepYIntercept;

    public GraphResults(string bestFitLineGradient, string bestFitLineYIntercept, string outliers, string steepestGradient, string steepestYIntercept, string leastSteepGradient, string leastSteepYIntercept) {
        this.bestFitLineGradient = bestFitLineGradient;
        this.bestFitLineYIntercept = bestFitLineYIntercept;
        this.outliers = outliers;
        this.steepestGradient = steepestGradient;
        this.steepestYIntercept = steepestYIntercept;
        this.leastSteepGradient = leastSteepGradient;
        this.leastSteepYIntercept = leastSteepYIntercept;
    }

    public string GetBestFitLineGradient() {
        return bestFitLineGradient;
    }

    public string GetBestFitLineYIntercept() {
        return bestFitLineYIntercept;
    }

    public string GetOutliers() {
        return outliers;
    }

    public string GetSteepestGradient() {
        return steepestGradient;
    }

    public string GetSteepestYIntercept() {
        return steepestYIntercept;
    }

    public string GetLeastSteepGradient() {
        return leastSteepGradient;
    }

    public string GetLeastSteepYIntercept() {
        return leastSteepYIntercept;
    }

    public override string ToString() {
        return "GraphResults: {\n"
            + "\tBest Fit Line's Gradient: " + bestFitLineGradient + ",\n"
            + "\tBest Fit Line's Y-Intercept: " + bestFitLineYIntercept + ",\n"
            + "\tOutliers: " + outliers + ",\n"
            + "\tSteepest Gradient: " + steepestGradient + ",\n"
            + "\tSteepest Y-Intercept: " + steepestYIntercept + ",\n"
            + "\tLeast Steep Gradient: " + leastSteepGradient + ",\n"
            + "\tLeast Steep Y-Intercept: " + leastSteepYIntercept + ",\n"
            + "}";
    }
}
