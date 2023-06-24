namespace GraphGram;
public class Line {
    private readonly double gradient;
    private readonly double yIntercept;

    public Line(double gradient, double yIntercept) {
        this.gradient = gradient;
        this.yIntercept = yIntercept;
    }

    public double GetGradient() {
        return gradient;
    }

    public double GetYIntercept() {
        return yIntercept;
    }
}
