namespace GraphGram;
public class GraphResults {
    private string gradient;
    private string yIntercept;
    private string outliers;

    public GraphResults(string gradient, string yIntercept, string outliers) {
        this.gradient = gradient;
        this.yIntercept = yIntercept;
        this.outliers = outliers;
    }

    public string GetGradient() {
        return gradient;
    }

    public string GetYIntercept() {
        return yIntercept;
    }

    public string GetOutliers() {
        return outliers;
    }

    public override string ToString() {
        return "GraphResults: {\n"
            + "\tGradient: " + gradient + ",\n"
            + "\tY-Intercept: " + yIntercept + ",\n"
            + "\tOutliers: " + outliers + ",\n"
            + "}";
    }
}
