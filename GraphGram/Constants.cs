namespace GraphGram;
public static class Constants {
    public static readonly float TABLE_FONT_SIZE = 15f;
    public static readonly Microsoft.Maui.Graphics.Font FONT = new Microsoft.Maui.Graphics.Font("Default");
    public static readonly Microsoft.Maui.Graphics.Font SUPERSCRIPT_FONT = new Microsoft.Maui.Graphics.Font("Default", 475);
    public static readonly float SUPERSCRIPT_SEPARATION = 3f;
    public static readonly float SUPERSCRIPT_RATIO = 0.7f;
    public static readonly float GRAPHING_AREA_FONT_SIZE = 18f;
    public static readonly int DEFAULT_ROW_COUNT = 40;
    public static readonly float PADDING = 0.075f; // Expressed as a fraction of the graphing area's dimensions
    public static readonly Dictionary<int, double> SPACING_THRESHOLD =
        new Dictionary<int, double> {
            { -2, (-1 - Math.Log10(5)) / 2.0},                  // Halfway between -1 and -log(5)
            { -5, (-Math.Log10(5) - Math.Log10(2)) / 2.0},      // Halfway between -log(5) and -log(2)
            { -10, -Math.Log10(2.0) / 2.0},                     // Halfway between -log(2) and 0

            { 1, Math.Log10(2.0) / 2.0 },                       // Halfway between 0 and log(2)
            { 2, (Math.Log10(2.0) + Math.Log10(5.0)) / 2.0 },   // Halfway between log(2) and log(5)
            { 5, (Math.Log10(5) + 1.0) / 2.0}                   // Halfway between log(5) and 1
        };
}
