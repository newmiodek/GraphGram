namespace GraphGram;
public class OutlierPermutation {
    private bool[] intersections;
    private int occurences;

    public OutlierPermutation(bool[] intersections) {
        this.intersections = intersections;
        occurences = 1;
    }

    public bool[] GetIntersections() {
        return intersections;
    }

    public int GetOccurences() {
        return occurences;
    }

    public void IncrementOccurences() {
        occurences++;
    }

    public bool AreIntersectionsEqual(bool[] other) {   // You could do this with hashes
        if(intersections.Length != other.Length) return false;
        for(int i = 0; i < intersections.Length; i++) {
            if(intersections[i] != other[i]) return false;
        }
        return true;
    }
}
