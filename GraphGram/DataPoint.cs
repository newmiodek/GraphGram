namespace GraphGram;
public class DataPoint {
    private readonly double x;
    private readonly double y;
    private readonly double uncertaintyX;
    private readonly double uncertaintyY;

    public DataPoint(double x, double y, double uncertaintyX, double uncertaintyY) {
        this.x = x;
        this.y = y;
        this.uncertaintyX = uncertaintyX;
        this.uncertaintyY = uncertaintyY;
    }

    public double GetX() {
        return x;
    }
    public double GetY() {
        return y;
    }
    public double GetUncertaintyX() {
        return uncertaintyX;
    }
    public double GetUncertaintyY() {
        return uncertaintyY;
    }
}
