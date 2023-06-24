namespace GraphGram;
class TableHeaderGraphicSide : IDrawable {

    public string Text { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        canvas.FontColor = Application.Current.RequestedTheme == AppTheme.Light ? Colors.Black : Colors.White;
        canvas.FontSize = 15f;
        canvas.DrawString(Text, dirtyRect.Left, dirtyRect.Top, dirtyRect.Width, dirtyRect.Height, HorizontalAlignment.Center, VerticalAlignment.Center);
    }

}
