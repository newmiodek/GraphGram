using Font = Microsoft.Maui.Graphics.Font;

namespace GraphGram;
public class SuperscriptedStringSize {
    public float[] Widths { get; private set; }
    public float Height { get; private set; }
    public float TotalWidth { get; private set; }

    public SuperscriptedStringSize(ICanvas canvas, Pair<string, bool>[] supString, float fontSize) {
        Height = 0f;
        TotalWidth = 0f;

        if(supString == null) {
            Widths = null;
            return;
        }

        Widths = new float[supString.Length];

        for(int i = 0; i < supString.Length; i++) {
            if(supString[i].second) {  // If in superscript
                Widths[i] = canvas.GetStringSize(supString[i].first, Font.Default, fontSize * Constants.SUPERSCRIPT_RATIO).Width;
            }
            else {  // If not in superscript
                SizeF tempSizeF = canvas.GetStringSize(supString[i].first, Font.Default, fontSize);
                Widths[i] = tempSizeF.Width;
                Height = Math.Max(Height, tempSizeF.Height);
            }

            if(i != 0) TotalWidth += Constants.SUPERSCRIPT_SEPARATION;
            TotalWidth += Widths[i];
        }
    }
}
