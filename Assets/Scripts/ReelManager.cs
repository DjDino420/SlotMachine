using System;
using UnityEngine;
using Random = System.Random;

public class ReelManager
{
    private SymbolManager symbolManager = new SymbolManager();

    // Tárcsák pörgetése
    public string[] SpinReels()
    {
        string[] reels = new string[15];

        // Speciális szimbólumok generálása (Star, Diamond, Crown)
        GenerateSpecialSymbols(reels);

        // Normál szimbólumok generálása
        for (int i = 0; i < 15; i++)
        {
            if (reels[i] == null) // Ha nincs speciális szimbólum, akkor normál szimbólumot generálunk
            {
                reels[i] = symbolManager.SpinNormalSymbol();
            }
        }

        // Ellenőrizzük és kiterjesztjük a Crown szimbólumokat
        reels = CheckAndReplaceWilds(reels);

        return reels;
    }

    // Speciális szimbólumok generálása (Star, Diamond, Crown)
    private void GenerateSpecialSymbols(string[] reels)
    {
        Random rng = new Random();

        // Star generálása (csak 1x per reel)
        for (int col = 0; col < 5; col++)
        {
            if (rng.Next(0, 100) < 5) // 5% esély a Star-ra
            {
                int row = rng.Next(0, 3); // Véletlenszerű sor kiválasztása
                reels[col + row * 5] = "Star";
            }
        }

        // Diamond generálása (csak 1., 3. és 5. reel, és csak 1x per reel)
        for (int col = 0; col < 5; col += 2) // 1., 3. és 5. reel
        {
            if (rng.Next(0, 100) < 5) // 10% esély a Diamond-ra
            {
                int row = rng.Next(0, 3); // Véletlenszerű sor kiválasztása
                reels[col + row * 5] = "Diamond";
            }
        }

        // Crown generálása (csak 2., 3. és 4. reel, és csak 1x per reel)
        for (int col = 1; col < 4; col++) // 2., 3. és 4. reel
        {
            if (rng.Next(0, 1000) < 62) // 10% esély a Crown-ra
            {
                int row = rng.Next(0, 3); // Véletlenszerű sor kiválasztása
                reels[col + row * 5] = "Crown";
            }
        }
    }

    // Crown szimbólumok ellenőrzése és kiterjesztése
    private string[] CheckAndReplaceWilds(string[] reels)
    {
        // Minden oszlopban ellenőrizzük, hogy van-e Crown
        for (int col = 0; col < 5; col++)
        {
            bool hasWild = false;

            // Ellenőrizzük, hogy az oszlopban van-e Crown
            for (int row = 0; row < 3; row++)
            {
                if (reels[col + row * 5] == "Crown")
                {
                    hasWild = true;
                    break;
                }
            }

            // Ha van Crown az oszlopban, akkor az egész oszlop Crown lesz
            if (hasWild)
            {
                for (int row = 0; row < 3; row++)
                {
                    reels[col + row * 5] = "Crown";
                }
            }
        }

        return reels;
    }

    // Tárcsák megjelenítése
    public void PrintGrid(string[] reels)
    {
        Debug.Log("-------------------------");

        for (int i = 0; i < 15; i += 5)
        {
            Debug.Log($"| {reels[i]} | {reels[i + 1]} | {reels[i + 2]} | {reels[i + 3]} | {reels[i + 4]} |");
            Debug.Log("-------------------------");
        }
    }
}
