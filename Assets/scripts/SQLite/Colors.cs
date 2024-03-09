using System;
using System.Collections.Generic;

[Serializable]
public class Colors
{
    public int IdColor { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Hex { get; set; } = string.Empty;

    public static List<Colors> newColorEN = new List<Colors>
    {
        new Colors { IdColor = 1, Name = "RED",Hex = "#FF0000" },
        new Colors { IdColor = 2, Name = "BLUE", Hex = "#0000FF" },
        new Colors { IdColor = 3, Name = "GREEN", Hex = "#3C8434" },
        new Colors { IdColor = 4, Name = "YELLOW", Hex = "#FFFF00" },
        new Colors { IdColor = 5, Name = "BLACK", Hex = "#000000" },
        new Colors { IdColor = 6, Name = "WHITE", Hex = "#FFFFFF" },
        new Colors { IdColor = 7, Name = "PURPLE", Hex = "#800080" },
        new Colors { IdColor = 8, Name = "ORANGE", Hex = "#FFA500" },
        new Colors { IdColor = 9, Name = "PINK", Hex = "#FFC0CB" },
        new Colors { IdColor = 10, Name = "BROWN", Hex = "#A52A2A" },
        new Colors { IdColor = 11, Name = "AQUA", Hex = "#00FFFF" },
        new Colors { IdColor = 12, Name = "ORANGE RED", Hex = "#FF4500" },
        new Colors { IdColor = 13, Name = "SILVER", Hex = "#C0C0C0" },
        new Colors { IdColor = 14, Name = "GRAY", Hex = "#808080" },
        new Colors { IdColor = 15, Name = "TURQUOISE", Hex = "#40E0D0" },
        new Colors { IdColor = 16, Name = "VIOLET", Hex = "#EE82EE" },
        new Colors { IdColor = 17, Name = "DARK RED", Hex = "#8B0000" },
        new Colors { IdColor = 18, Name = "NAVI BLUE", Hex = "#000080" },
        new Colors { IdColor = 19, Name = "LIME GREEN", Hex = "#00FF00" },
        new Colors { IdColor = 20, Name = "MUSTARD YELLOW", Hex = "#FFDB58" },
        new Colors { IdColor = 21, Name = "CORAL ORANGE", Hex = "#FF6F61" },
        new Colors { IdColor = 22, Name = "DARK PURPLE", Hex = "#4B0082" },
        new Colors { IdColor = 23, Name = "LIGTH BROWN", Hex = "#D2B48C" },
        new Colors { IdColor = 24, Name = "LIGHT BLUE", Hex = "#ADD8E6" },
        new Colors { IdColor = 25, Name = "LIGHT YELLOW", Hex = "#FFFFE0" },
        new Colors { IdColor = 26, Name = "LIGHT GREEN", Hex = "#90EE90" },
        new Colors { IdColor = 27, Name = "MUSTRARD", Hex = "#FFDB58" },
        new Colors { IdColor = 28, Name = "CORAL", Hex = "#FF6F61" },
        new Colors { IdColor = 29, Name = "GOLD", Hex = "#FFD700" },
        new Colors { IdColor = 30, Name = "MIDNIGHT BLUE", Hex = "#191970" },
        new Colors { IdColor = 31, Name = "SKY BLUE", Hex = "#87CEEB" },
        new Colors { IdColor = 32, Name = "CREAM", Hex = "#FFFDD0" },
        new Colors { IdColor = 33, Name = "CRIMSON", Hex = "#DC143C" },
    };

    public static List<Colors> newColorES = new List<Colors>
    {
        new Colors { IdColor = 1, Name = "ROJO",Hex = "#FF0000" },
        new Colors { IdColor = 2, Name = "AZUL", Hex = "#0000FF" },
        new Colors { IdColor = 3, Name = "VERDE", Hex = "#3C8434" },
        new Colors { IdColor = 4, Name = "AMARILLO", Hex = "#FFFF00" },
        new Colors { IdColor = 5, Name = "NEGRO", Hex = "#000000" },
        new Colors { IdColor = 6, Name = "BLANCO", Hex = "#FFFFFF" },
        new Colors { IdColor = 7, Name = "MORADO", Hex = "#800080" },
        new Colors { IdColor = 8, Name = "TOMATE", Hex = "#FFA500" },
        new Colors { IdColor = 9, Name = "ROSADO", Hex = "#FFC0CB" },
        new Colors { IdColor = 10, Name = "CAFE", Hex = "#A52A2A" },
        new Colors { IdColor = 11, Name = "AQUA", Hex = "#00FFFF" },
        new Colors { IdColor = 12, Name = "NARANJA ROJO", Hex = "#FF4500" },
        new Colors { IdColor = 13, Name = "PLOMO", Hex = "#C0C0C0" },
        new Colors { IdColor = 14, Name = "GRIS", Hex = "#808080" },
        new Colors { IdColor = 15, Name = "TURQUEZA", Hex = "#40E0D0" },
        new Colors { IdColor = 16, Name = "VIOLETA", Hex = "#EE82EE" },
        new Colors { IdColor = 17, Name = "ROJO OSCURO", Hex = "#8B0000" },
        new Colors { IdColor = 18, Name = "AZUL MARINO", Hex = "#000080" },
        new Colors { IdColor = 19, Name = "VERDE LIMA", Hex = "#00FF00" },
        new Colors { IdColor = 20, Name = "AMARILLO MOSTAZA", Hex = "#FFDB58" },
        new Colors { IdColor = 21, Name = "NARANJA CORAL", Hex = "#FF6F61" },
        new Colors { IdColor = 22, Name = "BLANCO HUESO", Hex = "#E3D2B3" },
        new Colors { IdColor = 23, Name = "CAFE CLARO", Hex = "#D2B48C" },
        new Colors { IdColor = 24, Name = "AZUL CLARO", Hex = "#ADD8E6" },
        new Colors { IdColor = 25, Name = "AMARILLO CLARO", Hex = "#FFFFE0" },
        new Colors { IdColor = 26, Name = "VERDE CLARO", Hex = "#90EE90" },
        new Colors { IdColor = 27, Name = "MOSTAZA", Hex = "#FFDB58" },
        new Colors { IdColor = 28, Name = "CORAL", Hex = "#FF6F61" },
        new Colors { IdColor = 29, Name = "DORADO", Hex = "#FFD700" },
        new Colors { IdColor = 30, Name = "AZUL MEDIANOCHE", Hex = "#191970" },
        new Colors { IdColor = 31, Name = "CELESTE", Hex = "#87CEEB" },
        new Colors { IdColor = 32, Name = "CREAMA", Hex = "#FFFDD0" },
        new Colors { IdColor = 33, Name = "CARMESI", Hex = "#DC143C" },
    };
}
