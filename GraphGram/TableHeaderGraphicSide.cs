namespace GraphGram;
public class TableHeaderGraphicSide : IDrawable {

    private string text;
    private SuperscriptedString supString;

    private bool isSupStringUpToDate = false;

    public void SetText(string text) {
        this.text = text;
        isSupStringUpToDate = false;
    }
    public string GetText() {
        return text;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        if(!isSupStringUpToDate) {
            supString = new SuperscriptedString(this.GetText(), canvas, Constants.TABLE_FONT_SIZE);
            isSupStringUpToDate = true;
        }
        supString.Draw(canvas, dirtyRect, Constants.TABLE_FONT_SIZE);
    }
}
