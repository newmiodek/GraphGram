using Font = Microsoft.Maui.Graphics.Font;

namespace GraphGram;
public class TableHeaderGraphicSide : IDrawable {

    private static readonly Font FONT = Font.Default;
    private static readonly float FONTSIZE = 15f;

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
        if(str.Length < 1 || str.StartsWith('^')) return null;
        /* A Pair's string is a part of the input string and the bool
         * determines whether it in a superscript (bool = true)
         * or not (bool = false)
         */
        List<Pair<string, bool>> supString = new List<Pair<string, bool>>();
        supString.Add(new Pair<string, bool>(str[0].ToString(), false));

        for(int i = 1; i < str.Length; i++) {
            if(supString[^1].second) {  // If in superscript
                if(supString[^1].first.Length == 0) {
                    if(str[i] != '(') return null;
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
        canvas.FontColor = Application.Current.RequestedTheme == AppTheme.Light ? Colors.Black : Colors.White;
        canvas.Font = FONT;

        if(supString == null) {

            return;
        }

        SuperscriptedStringSize size = new SuperscriptedStringSize(canvas, supString, FONTSIZE);

        float horizontalPosition = dirtyRect.Width / 2f - size.TotalWidth / 2f;

        for(int i = 0; i < supString.Length; i++) {
            if(supString[i].second) {  // If in superscript
                canvas.FontSize = FONTSIZE / 2f;
                SizeF smallFontSize = canvas.GetStringSize(supString[i].first, FONT, FONTSIZE / 2f);
                canvas.DrawString(supString[i].first, horizontalPosition, dirtyRect.Height / 2f + size.Height / 2f - smallFontSize.Height, HorizontalAlignment.Left);
                horizontalPosition += smallFontSize.Width;
            }
            else {  // If not in superscript
                canvas.FontSize = FONTSIZE;
                canvas.DrawString(supString[i].first, horizontalPosition, dirtyRect.Height / 2f + size.Height / 2f, HorizontalAlignment.Left);
                horizontalPosition += canvas.GetStringSize(supString[i].first, FONT, FONTSIZE).Width;
            }
            horizontalPosition += 3f;
        }
    }
}
