namespace GraphGram;
class TableHeaderGraphicSide : IDrawable {

    public string Text { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        canvas.FontColor = Colors.White;
        canvas.FontSize = 15;
        canvas.DrawString(Text, 35, 18, HorizontalAlignment.Center);
    }

}
