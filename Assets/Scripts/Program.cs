using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        BankrollManager bankroll = new BankrollManager(100);
        ReelManager reelManager = new ReelManager();
        PaylineManager paylineManager = new PaylineManager();
        bool inMenu = false;

        while (true)
        {
            if (!inMenu)
            {
                Console.Clear();
                Console.WriteLine($"💰 Bankroll: {bankroll.Bankroll} | 🎲 Tét: {bankroll.CurrentBet}");
                Console.WriteLine("\nENTER - Pörgetés | M - Menü");

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    if (bankroll.CurrentBet <= 0)
                    {
                        Console.WriteLine("\n❌ Nincs tét beállítva!");
                        continue;
                    }

                    bankroll.DeductBet();
                    string[] reels = reelManager.SpinReels();
                    reelManager.PrintGrid(reels);
                    int winnings = paylineManager.CalculateWinnings(reels, bankroll.CurrentBet);

                    if (winnings > 0)
                    {
                        bankroll.AddWinnings(winnings);
                        Console.WriteLine($"\n🎉 Nyeremény: {winnings} egység!");
                    }
                    else
                    {
                        Console.WriteLine("\n😢 Nincs nyeremény!");
                    }

                    Console.WriteLine("\nNyomj egy gombot a folytatáshoz...");
                    Console.ReadKey();
                }
                else if (key.Key == ConsoleKey.M)
                {
                    inMenu = true;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("===== MENÜ =====");
                Console.WriteLine("1. Tét beállítása");
                Console.WriteLine("2. Pénz befizetése");
                Console.WriteLine("3. Pénz kivétele");
                Console.WriteLine("4. Teszt mód indítása");
                Console.WriteLine("5. Vissza a játékhoz");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Új tét: ");
                        if (int.TryParse(Console.ReadLine(), out int bet))
                            bankroll.SetBet(bet);
                        break;
                    case "2":
                        Console.Write("Befizetés: ");
                        if (int.TryParse(Console.ReadLine(), out int deposit))
                            bankroll.AddFunds(deposit);
                        break;
                    case "3":
                        Console.Write("Kivétel: ");
                        if (int.TryParse(Console.ReadLine(), out int withdraw))
                            bankroll.WithdrawFunds(withdraw);
                        break;
                    case "4":
                        StartTestMode(bankroll, reelManager, paylineManager);
                        break;
                    case "5":
                        inMenu = false;
                        break;
                }
            }
        }
    }

    static void StartTestMode(BankrollManager bankroll, ReelManager reelManager, PaylineManager paylineManager)
    {
        Console.Clear();
        Console.WriteLine("===== TESZT MÓD =====");

        // Alapértelmezett iterációszám: 1,000
        int defaultIterations = 100000;
        Console.Write($"Iterációk száma (alapértelmezett: {defaultIterations}): ");
        string input = Console.ReadLine();
        int iterations = string.IsNullOrEmpty(input) ? defaultIterations : int.Parse(input);

        // Bankroll beállítása 1,000-re
        bankroll.AddFunds(1000 - bankroll.Bankroll); // Mindig 1,000-ről indul
        int initialBankroll = bankroll.Bankroll;
        int currentBet = bankroll.CurrentBet;

        Console.WriteLine($"\nKezdeti bankroll: {initialBankroll}");
        Console.WriteLine($"Tét: {currentBet}");
        Console.WriteLine($"Iterációk: {iterations}");
        Console.WriteLine("\nNyomj egy gombot a teszt indításához...");
        Console.ReadKey();

        int totalBet = 0;
        int totalWinnings = 0;

        for (int i = 0; i < iterations; i++)
        {
            /*if (bankroll.Bankroll <= 0)
            {
                Console.WriteLine($"\n❌ Bankroll elfogyott {i + 1} iteráció után.");
                break;
            }*/

            int bet = bankroll.CurrentBet;
            totalBet += bet;
            bankroll.DeductBet();

            string[] reels = reelManager.SpinReels();
            int winnings = paylineManager.CalculateWinnings(reels, bet);
            totalWinnings += winnings;

            if (winnings > 0)
            {
                bankroll.AddWinnings(winnings);
            }

            if (i % 1000 == 0) // Progressz jelzés minden 1,000 iteráció után
            {
                Console.WriteLine($"Iteráció: {i + 1} | Bankroll: {bankroll.Bankroll}");
            }
        }

        int finalBankroll = bankroll.Bankroll;
        double rtp = (double)totalWinnings / totalBet * 100;

        Console.WriteLine($"\nTeszt befejezve. Végső bankroll: {finalBankroll}");
        Console.WriteLine($"Összes tét: {totalBet}");
        Console.WriteLine($"Összes nyeremény: {totalWinnings}");
        Console.WriteLine($"RTP: {rtp:F2}%");

        // Eredmények mentése CSV fájlba
        LogTestResults(iterations, initialBankroll, finalBankroll, totalBet, totalWinnings, rtp);

        Console.WriteLine("\nNyomj egy gombot a visszatéréshez...");
        Console.ReadKey();
    }

    static void LogTestResults(int iterations, int initialBankroll, int finalBankroll, int totalBet, int totalWinnings, double rtp)
    {
        string csvFilePath = "test_results.csv";
        bool fileExists = File.Exists(csvFilePath);

        using (StreamWriter writer = new StreamWriter(csvFilePath, append: true))
        {
            // Ha a fájl nem létezik, írjuk be a fejlécet
            if (!fileExists)
            {
                writer.WriteLine("Iterációk;Kezdeti Bankroll;Végső Bankroll;Összes Tét;Összes Nyeremény;RTP (%)");
            }

            // Új sor hozzáadása
            writer.WriteLine($"{iterations};{initialBankroll};{finalBankroll};{totalBet};{totalWinnings};{rtp:F2}");
        }

        Console.WriteLine($"\n✅ Eredmények mentve a következő fájlba: {csvFilePath}");
    }
}