using System.Collections.Generic;

public class Colors
{
    public int IdColor { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Hex { get; set; } = string.Empty;

    public static List<Colors> newColor = new List<Colors>
    {
        new Colors { IdColor = 1, Name = "RED",Hex = "#FF0000" },
        new Colors { IdColor = 2, Name = "BLUE", Hex = "#0000FF" },
        new Colors { IdColor = 3, Name = "GREEN", Hex = "#00FF00" },
        new Colors { IdColor = 4, Name = "YELLOW", Hex = "#FFFF00" },
        new Colors { IdColor = 5, Name = "BLACK", Hex = "#000000" },
        new Colors { IdColor = 6, Name = "WHITE", Hex = "#FFFFFF" },
        new Colors { IdColor = 7, Name = "PURPLE", Hex = "#800080" },
        new Colors { IdColor = 8, Name = "ORANGE", Hex = "#FFA500" },
        new Colors { IdColor = 9, Name = "PINK", Hex = "#FFC0CB" },
        new Colors { IdColor = 10, Name = "BROWN", Hex = "#A52A2A" }
    };
}
