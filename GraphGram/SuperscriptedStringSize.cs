namespace GraphGram;
public class SuperscriptedStringSize {
    private float[] widths;
    private float totalWidth;

    public SuperscriptedStringSize(ICanvas canvas, SuperscriptedSegment[] supString, float fontSize) {
        totalWidth = 0f;

        if(supString == null) {
            widths = null;
            return;
        }

        widths = new float[supString.Length];

        for(int i = 0; i < supString.Length; i++) {
            if(supString[i].IsSuperscript()) {
                widths[i] = canvas.GetStringSize(supString[i].GetText(), Constants.SUPERSCRIPT_FONT, fontSize * Constants.SUPERSCRIPT_RATIO).Width;
            }
            else {
                widths[i] = canvas.GetStringSize(supString[i].GetText(), Constants.FONT, fontSize).Width;
            }

            if(i != 0) totalWidth += Constants.SUPERSCRIPT_SEPARATION;
            totalWidth += widths[i];
        }
    }

    public float GetSingleWidth(int index) {
        return widths[index];
    }
    public float GetTotalWidth() {
        return totalWidth;
    }
}
