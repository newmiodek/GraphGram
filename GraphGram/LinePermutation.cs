namespace GraphGram;
public class LinePermutation : Line {
    private int outlierCount;
    private bool[] intersections;
    
    public LinePermutation(double gradient, double yIntercept, int outlierCount, bool[] intersections) : base(gradient, yIntercept) {
        this.outlierCount = outlierCount;
        this.intersections = intersections;
    }

    public int GetOutlierCount() {
        return outlierCount;
    }

    public bool[] GetIntersections() {
        return intersections;
    }

    public void IncrementOutlierCount() {
        outlierCount++;
    }

    public void SetSingleIntersection(int index, bool isIntersecting) {
        intersections[index] = isIntersecting;
    }

    public bool GetSingleIntersection(int index) {
        return intersections[index];
    }
}
