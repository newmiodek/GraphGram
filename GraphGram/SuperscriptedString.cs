namespace GraphGram;
public class SuperscriptedString {
    private readonly SuperscriptedSegment[] supString;  // Never assign to this!

    public SuperscriptedString(string str, ICanvas canvas, float fontsize) {
        if(str.Length < 1 || str.StartsWith('^')) {
            SuperscriptedSegment[] simpleString = new SuperscriptedSegment[1];
            simpleString[0] = new SuperscriptedSegment(str, false);
            this.supString = simpleString;
            return;
        }

        List<SuperscriptedSegment> supString = new List<SuperscriptedSegment>();
        supString.Add(new SuperscriptedSegment(str[0].ToString(), false));

        for(int i = 1; i < str.Length; i++) {
            if(supString[^1].IsSuperscript()) {
                if(supString[^1].GetText().Length == 0) {
                    if(str[i] != '(') {
                        SuperscriptedSegment[] simpleString = new SuperscriptedSegment[1];
                        simpleString[0] = new SuperscriptedSegment(str, false);
                        this.supString = simpleString;
                        return;
                    }
                    supString[^1].Append("\u200B"); // Zero-width space
                    continue;
                }
                if(str[i] == ')') {
                    supString.Add(new SuperscriptedSegment("", false));
                    continue;
                }
            }
            else {  // If not in superscript
                if(str[i] == '^') {
                    supString.Add(new SuperscriptedSegment("", true));
                    continue;
                }
            }
            supString[^1].Append(str[i].ToString());
        }
        this.supString = supString.ToArray();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect, float fontsize) {
        canvas.FontColor = Application.Current.RequestedTheme == AppTheme.Light ? Colors.Black : Colors.White;

        SuperscriptedStringSize size = this.GetSize(canvas, fontsize);

        float horizontalPosition = dirtyRect.Left + dirtyRect.Width / 2f - size.GetTotalWidth() / 2f;

        for(int i = 0; i < this.supString.Length; i++) {
            if(this.supString[i].IsSuperscript()) {
                canvas.Font = Constants.SUPERSCRIPT_FONT;
                canvas.FontSize = fontsize * Constants.SUPERSCRIPT_RATIO;
                canvas.DrawString(this.supString[i].GetText(), horizontalPosition, dirtyRect.Top + dirtyRect.Height / 2f + fontsize / 2f - fontsize * Constants.SUPERSCRIPT_RATIO, HorizontalAlignment.Left);
            }
            else {
                canvas.Font = Constants.FONT;
                canvas.FontSize = fontsize;
                canvas.DrawString(this.supString[i].GetText(), horizontalPosition, dirtyRect.Top + dirtyRect.Height / 2f + fontsize / 2f, HorizontalAlignment.Left);
            }
            horizontalPosition += size.GetSingleWidth(i) + Constants.SUPERSCRIPT_SEPARATION;
        }
    }

    public SuperscriptedSegment GetAtIndex(int index) {
        return this.supString[index].Clone();
    }

    public SuperscriptedStringSize GetSize(ICanvas canvas, float fontsize) {
        return new SuperscriptedStringSize(canvas, this.supString, fontsize);
    }
}
