using System.Data;

namespace GraphGram;

public class GraphingArea : IDrawable {

    public float[,] DataTable { get; set; }

    public bool IsInputValid { get; set; } = false;

    public void Draw(ICanvas canvas, RectF dirtyRect) {
        if (DataTable != null) {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 6;
            canvas.DrawLine(DataTable[0,0], DataTable[1,0], DataTable[0,1], DataTable[1,1]);
        }
        if (!IsInputValid) {
            canvas.FontColor = Colors.Red;
            canvas.FontSize = 18;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.DrawString("Invalid input", 100, 100, HorizontalAlignment.Left);
        }
    }
}
