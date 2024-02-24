using System.Collections.Generic;

public class Character
{
    public int IdCharacter { get; set; }
    public string Name { get; set; }
    public string Clan { get; set; }
    public int Age { get; set; }
    public string Avatar { get; set; }

    public static List<Character> characters = new List<Character>
    {
        new Character { IdCharacter = 1, Name = "NARUTO", Clan = "UZUMAKI", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img1&radius=50&backgroundColor=d1d4f9" },
        new Character { IdCharacter = 2, Name = "HINATA", Clan = "HYUGA", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img2&radius=50&backgroundColor=d1d4f9" },
        new Character { IdCharacter = 3, Name = "SASUKE", Clan = "UCHIHA", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img3&radius=50&backgroundColor=d1d4f9" },
        new Character { IdCharacter = 4, Name = "SAKURA", Clan = "HARUNO", Age = 15, Avatar = "https://api.dicebear.com/7.x/micah/png?seed=img4&radius=50&backgroundColor=d1d4f9" },
    };
}
