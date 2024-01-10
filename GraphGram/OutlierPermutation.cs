namespace GraphGram;
public class OutlierPermutation {
    private bool[] intersections;
    private int occurrences;

    public OutlierPermutation(bool[] intersections) {
        this.intersections = intersections;
        occurrences = 1;
    }

    public bool[] GetIntersections() {
        return intersections;
    }

    public int GetOccurrences() {
        return occurrences;
    }

    public void IncrementOccurrences() {
        occurrences++;
    }

    public bool AreIntersectionsEqual(bool[] other) {   // You could do this with hashes
        if(intersections.Length != other.Length) return false;
        for(int i = 0; i < intersections.Length; i++) {
            if(intersections[i] != other[i]) return false;
        }
        return true;
    }
}
