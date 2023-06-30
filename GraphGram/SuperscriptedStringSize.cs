namespace GraphGram;
public class SuperscriptedStringSize {
    public float[] Widths { get; private set; }
    public float TotalWidth { get; private set; }

    public SuperscriptedStringSize(ICanvas canvas, Pair<string, bool>[] supString, float fontSize) {
        TotalWidth = 0f;

        if(supString == null) {
            Widths = null;
            return;
        }

        Widths = new float[supString.Length];

        for(int i = 0; i < supString.Length; i++) {
            if(supString[i].second) {  // If in superscript
                Widths[i] = canvas.GetStringSize(supString[i].first, Constants.SUPERSCRIPT_FONT, fontSize * Constants.SUPERSCRIPT_RATIO).Width;
            }
            else {  // If not in superscript
                SizeF tempSizeF = canvas.GetStringSize(supString[i].first, Constants.FONT, fontSize);
                Widths[i] = tempSizeF.Width;
            }

            if(i != 0) TotalWidth += Constants.SUPERSCRIPT_SEPARATION;
            TotalWidth += Widths[i];
        }
    }
}
