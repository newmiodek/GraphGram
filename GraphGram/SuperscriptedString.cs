namespace GraphGram;
public class SuperscriptedString {
    /* A Pair's string is a part of the input string and the bool
     * determines whether it in a superscript (bool = true)
     * or not (bool = false)
     */
    private readonly Pair<string, bool>[] supString;  // Never assign to this!

    public SuperscriptedString(string str, ICanvas canvas, float fontsize) {
        if(str.Length < 1 || str.StartsWith('^')) {
            Pair<string, bool>[] simpleString = new Pair<string, bool>[1];
            simpleString[0] = new Pair<string, bool>(str, false);
            this.supString = simpleString;
            return;
        }

        List<Pair<string, bool>> supString = new List<Pair<string, bool>>();
        supString.Add(new Pair<string, bool>(str[0].ToString(), false));

        for(int i = 1; i < str.Length; i++) {
            if(supString[^1].second) {  // If in superscript
                if(supString[^1].first.Length == 0) {
                    if(str[i] != '(') {
                        Pair<string, bool>[] simpleString = new Pair<string, bool>[1];
                        simpleString[0] = new Pair<string, bool>(str, false);
                        this.supString = simpleString;
                        return;
                    }
                    supString[^1].first += '\u200B';  // Zero-width space
                    continue;
                }
                if(str[i] == ')') {
                    supString.Add(new Pair<string, bool>("", false));
                    continue;
                }
            }
            else {  // If not in superscript
                if(str[i] == '^') {
                    supString.Add(new Pair<string, bool>("", true));
                    continue;
                }
            }
            supString[^1].first += str[i];
        }
        this.supString = supString.ToArray();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect, float fontsize) {
        canvas.FontColor = Application.Current.RequestedTheme == AppTheme.Light ? Colors.Black : Colors.White;

        SuperscriptedStringSize size = this.GetSize(canvas, fontsize);

        float horizontalPosition = dirtyRect.Left + dirtyRect.Width / 2f - size.TotalWidth / 2f;

        for(int i = 0; i < this.supString.Length; i++) {
            if(this.supString[i].second) {  // If in superscript
                canvas.Font = Constants.SUPERSCRIPT_FONT;
                canvas.FontSize = fontsize * Constants.SUPERSCRIPT_RATIO;
                canvas.DrawString(this.supString[i].first, horizontalPosition, dirtyRect.Top + dirtyRect.Height / 2f + fontsize / 2f - fontsize * Constants.SUPERSCRIPT_RATIO, HorizontalAlignment.Left);
            }
            else {  // If not in superscript
                canvas.Font = Constants.FONT;
                canvas.FontSize = fontsize;
                canvas.DrawString(this.supString[i].first, horizontalPosition, dirtyRect.Top + dirtyRect.Height / 2f + fontsize / 2f, HorizontalAlignment.Left);
            }
            horizontalPosition += size.Widths[i] + Constants.SUPERSCRIPT_SEPARATION;
        }
    }

    public Pair<string, bool> GetAtIndex(int index) {
        return new Pair<string, bool>(this.supString[index].first + "", !!this.supString[index].second);
    }

    public SuperscriptedStringSize GetSize(ICanvas canvas, float fontsize) {
        return new SuperscriptedStringSize(canvas, this.supString, fontsize);
    }
}
