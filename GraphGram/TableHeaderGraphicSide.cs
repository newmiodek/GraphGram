namespace GraphGram;
public class TableHeaderGraphicSide : IDrawable {

    private string text;
    private Pair<string, bool>[] supString;

    public void SetText(string text) {
        this.text = text;
        supString = GenerateSuperscripts(text);
    }
    public string GetText() {
        return text;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        DrawSuperscriptedString(canvas, dirtyRect, supString);
    }

    private static Pair<string, bool>[] GenerateSuperscripts(string str) {
        if(str.Length < 1 || str.StartsWith('^')) {
            Pair<string, bool>[] simpleString = new Pair<string, bool>[1];
            simpleString[0] = new Pair<string, bool>(str, false);
            return simpleString;
        }
        /* A Pair's string is a part of the input string and the bool
         * determines whether it in a superscript (bool = true)
         * or not (bool = false)
         */
        List<Pair<string, bool>> supString = new List<Pair<string, bool>>();
        supString.Add(new Pair<string, bool>(str[0].ToString(), false));

        for(int i = 1; i < str.Length; i++) {
            if(supString[^1].second) {  // If in superscript
                if(supString[^1].first.Length == 0) {
                    if(str[i] != '(') {
                        Pair<string, bool>[] simpleString = new Pair<string, bool>[1];
                        simpleString[0] = new Pair<string, bool>(str, false);
                        return simpleString;
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
        return supString.ToArray();
    }

    private static void DrawSuperscriptedString(ICanvas canvas, RectF dirtyRect, Pair<string, bool>[] supString) {
        if(supString == null) {
            return;
        }

        canvas.FontColor = Application.Current.RequestedTheme == AppTheme.Light ? Colors.Black : Colors.White;

        SuperscriptedStringSize size = new SuperscriptedStringSize(canvas, supString, Constants.TABLE_FONT_SIZE);

        float horizontalPosition = dirtyRect.Width / 2f - size.TotalWidth / 2f;

        for(int i = 0; i < supString.Length; i++) {
            if(supString[i].second) {  // If in superscript
                canvas.Font = Constants.SUPERSCRIPT_FONT;
                canvas.FontSize = Constants.TABLE_FONT_SIZE * Constants.SUPERSCRIPT_RATIO;
                SizeF smallFontSize = canvas.GetStringSize(supString[i].first, Constants.SUPERSCRIPT_FONT, Constants.TABLE_FONT_SIZE * Constants.SUPERSCRIPT_RATIO);
                canvas.DrawString(supString[i].first, horizontalPosition, dirtyRect.Height / 2f + Constants.TABLE_FONT_SIZE / 2f - Constants.TABLE_FONT_SIZE * Constants.SUPERSCRIPT_RATIO, HorizontalAlignment.Left);
                horizontalPosition += smallFontSize.Width;
            }
            else {  // If not in superscript
                canvas.Font = Constants.FONT;
                canvas.FontSize = Constants.TABLE_FONT_SIZE;
                canvas.DrawString(supString[i].first, horizontalPosition, dirtyRect.Height / 2f + Constants.TABLE_FONT_SIZE / 2f, HorizontalAlignment.Left);
                horizontalPosition += canvas.GetStringSize(supString[i].first, Constants.FONT, Constants.TABLE_FONT_SIZE).Width;
            }
            horizontalPosition += Constants.SUPERSCRIPT_SEPARATION;
        }
    }
}
