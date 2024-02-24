using System.Collections.Generic;
using LiteDB;

public class City
{
    [BsonId]
    public ObjectId IdCity { get; set; }
    public string Name { get; set; } = string.Empty;


    public static List<City> newCity = new List<City>
    {
        new City { Name = "NEW YORK" },
        new City { Name = "LOS ANGELES" },
        new City { Name = "CHICAGO" },
        new City { Name = "HUSTON" },
        new City { Name = "PHOENIX" },
        new City { Name = "PHILADELPHIA" },
        new City { Name = "SAN ANTONIO" },
        new City { Name = "SAN DIEGO" },
        new City { Name = "DALLAS" },
        new City { Name = "BOSTON" }
    };
}
