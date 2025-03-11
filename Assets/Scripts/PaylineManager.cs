using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaylineManager
{
    private int[,] Paylines { get; } = new int[,]
    {
        {0, 1, 2, 3, 4},    // 1. row
        {5, 6, 7, 8, 9},    // 2. row
        {10, 11, 12, 13, 14}, // 3. row
        {0, 6, 12, 8, 4},   // V shape
        {10, 6, 2, 8, 14},  // Inverted V
        {0, 1, 7, 13, 14},
        {10, 11, 7, 3, 4},
        {5, 1, 2, 8, 9},
        {5, 12, 13, 8, 9},
        {0, 6, 7, 3, 4}
    };

    private Dictionary<string, int> Paytable { get; } = new Dictionary<string, int>
    {
        {"RedSevenRedSeven", 1}, {"RedSevenRedSevenRedSeven", 5}, {"RedSevenRedSevenRedSevenRedSeven", 25}, {"RedSevenRedSevenRedSevenRedSevenRedSeven", 500},
        {"GrapesGrapesGrapes", 4}, {"GrapesGrapesGrapesGrapes", 12}, {"GrapesGrapesGrapesGrapesGrapes", 70},
        {"WatermelonWatermelonWatermelon", 4}, {"WatermelonWatermelonWatermelonWatermelon", 12}, {"WatermelonWatermelonWatermelonWatermelonWatermelon", 70},
        {"OrangeOrangeOrange", 1}, {"OrangeOrangeOrangeOrange", 3}, {"OrangeOrangeOrangeOrangeOrange", 15},
        {"LemonLemonLemon", 1}, {"LemonLemonLemonLemon", 3}, {"LemonLemonLemonLemonLemon", 15},
        {"CherryCherryCherry", 1}, {"CherryCherryCherryCherry", 3}, {"CherryCherryCherryCherryCherry", 15},
        {"BellBellBell", 2}, {"BellBellBellBell", 4}, {"BellBellBellBellBell", 20},
        {"PeachPeachPeach", 1}, {"PeachPeachPeachPeach", 3}, {"PeachPeachPeachPeachPeach", 15},
        {"DiamondDiamondDiamond", 5}, {"DiamondDiamondDiamondDiamond", 20}, {"DiamondDiamondDiamondDiamondDiamond", 100},
        {"StarStarStar", 20}
    };

    public int CalculateWinnings(string[] reels, int bet)
    {
        int totalWin = 0;
        HashSet<int> matchedPaylines = new HashSet<int>();

        for (int i = 0; i < Paylines.GetLength(0); i++)
        {
            Debug.Log($"Checking Payline {i + 1}:");
            List<string> lineSymbols = new List<string>();
            for (int j = 0; j < 5; j++)
            {
                lineSymbols.Add(reels[Paylines[i, j]]);
            }
            Debug.Log("Symbols in payline: " + string.Join(" ", lineSymbols));

            if (!IsValidStartingSymbol(lineSymbols[0]))
            {
                Debug.Log("No valid start symbol on reel 1. Skipping this payline.");
                continue;
            }

            string bestMatch = FindBestMatch(lineSymbols);
            Debug.Log("Best matching sequence: " + bestMatch);

            if (!string.IsNullOrEmpty(bestMatch) && Paytable.ContainsKey(bestMatch) && !matchedPaylines.Contains(i))
            {
                int win = Paytable[bestMatch] * bet;
                totalWin += win;
                matchedPaylines.Add(i);
                Debug.Log($"Payline {i + 1}: {bestMatch} -> Win: {win} units");
            }
        }
        return totalWin;
    }

    private bool IsValidStartingSymbol(string symbol)
    {
        return symbol != "Crown" && symbol != "Diamond" && symbol != "Star";
    }

    private string FindBestMatch(List<string> lineSymbols)
    {
        List<string> possibleMatches = new List<string>();
        string baseSymbol = lineSymbols[0];

        if (baseSymbol == "Crown") return "";

        string currentMatch = "";
        for (int j = 0; j < lineSymbols.Count; j++)
        {
            if (lineSymbols[j] == baseSymbol || lineSymbols[j] == "Crown")
            {
                currentMatch += baseSymbol;
            }
            else break;
        }
        possibleMatches.Add(currentMatch);

        return possibleMatches.OrderByDescending(x => x.Length).FirstOrDefault() ?? "";
    }
}
