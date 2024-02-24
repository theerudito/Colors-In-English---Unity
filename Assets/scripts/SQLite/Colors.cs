using System.Collections.Generic;

public class Colors
{
    public int IdColor { get; set; }
    public string Name { get; set; } = string.Empty;

    public static List<Colors> newColor = new List<Colors>
    {
        new Colors { IdColor = 1, Name = "RED"},
        new Colors { IdColor = 2, Name = "BLUE" },
        new Colors { IdColor = 3, Name = "GREEN" },
        new Colors { IdColor = 4, Name = "YELLOW" },
        new Colors { IdColor = 5, Name = "BLACK" },
        new Colors { IdColor = 6, Name = "WHITE" },
        new Colors { IdColor = 7, Name = "PURPLE" },
        new Colors { IdColor = 8, Name = "ORANGE" },
        new Colors { IdColor = 9, Name = "PINK" },
        new Colors { IdColor = 10, Name = "BROWN" }
    };
}
