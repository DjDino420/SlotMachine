using System;
using System.Collections.Generic;
using System.Linq;

public class SymbolManager
{
    private Random rng = new Random();

    // Normál szimbólumok és súlyok
    public Dictionary<string, int> NormalSymbols { get; } = new Dictionary<string, int>
    {
        {"Cherry", 35}, {"Lemon", 35}, {"Orange", 35}, {"Peach", 35},
        {"Bell", 25},
        {"Grapes", 20}, {"Watermelon", 20},
        {"RedSeven", 10}
    };

    // Speciális szimbólumok és súlyok
    public Dictionary<string, int> SpecialSymbols { get; } = new Dictionary<string, int>
    {
        {"Diamond", 4}, {"Star", 4}, {"Crown", 4} // Gyémánt, csillag, korona
    };

    // Véletlenszerű normál szimbólum generálása
    public string SpinNormalSymbol()
    {
        int totalWeight = NormalSymbols.Values.Sum();
        int randomValue = rng.Next(1, totalWeight + 1);
        int cumulative = 0;

        foreach (var symbol in NormalSymbols)
        {
            cumulative += symbol.Value;
            if (randomValue <= cumulative) return symbol.Key;
        }
        return "Unknown";
    }

    // Véletlenszerű speciális szimbólum generálása
    public string SpinSpecialSymbol()
    {
        int totalWeight = SpecialSymbols.Values.Sum();
        int randomValue = rng.Next(1, totalWeight + 1);
        int cumulative = 0;

        foreach (var symbol in SpecialSymbols)
        {
            cumulative += symbol.Value;
            if (randomValue <= cumulative) return symbol.Key;
        }
        return "Unknown";
    }
}