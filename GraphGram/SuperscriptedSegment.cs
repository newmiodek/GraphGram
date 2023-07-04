namespace GraphGram;
public class SuperscriptedSegment {
    private string text;
    private readonly bool isSuperscript;

    public SuperscriptedSegment(string text, bool isSuperscript) {
        this.text = text;
        this.isSuperscript = isSuperscript;
    }

    public string GetText() {
        return text;
    }

    public void Append(string fragment) {
        text += fragment;
    }

    public bool IsSuperscript() {
        return isSuperscript;
    }

    public SuperscriptedSegment Clone() {
        return new SuperscriptedSegment(text, isSuperscript);
    }
}
