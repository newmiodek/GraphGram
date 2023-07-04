namespace GraphGram;
public class SuperscriptedStringSize {
    public float[] Widths { get; private set; }
    public float TotalWidth { get; private set; }

    public SuperscriptedStringSize(ICanvas canvas, SuperscriptedSegment[] supString, float fontSize) {
        TotalWidth = 0f;

        if(supString == null) {
            Widths = null;
            return;
        }

        Widths = new float[supString.Length];

        for(int i = 0; i < supString.Length; i++) {
            if(supString[i].IsSuperscript()) {
                Widths[i] = canvas.GetStringSize(supString[i].GetText(), Constants.SUPERSCRIPT_FONT, fontSize * Constants.SUPERSCRIPT_RATIO).Width;
            }
            else {
                Widths[i] = canvas.GetStringSize(supString[i].GetText(), Constants.FONT, fontSize).Width;
            }

            if(i != 0) TotalWidth += Constants.SUPERSCRIPT_SEPARATION;
            TotalWidth += Widths[i];
        }
    }
}
