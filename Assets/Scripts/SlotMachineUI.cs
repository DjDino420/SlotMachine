using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; // Required for directory operations

public class SlotMachineUI : MonoBehaviour
{
    public ReelManager reelManager;
    public BankrollManager bankrollManager;
    public PaylineManager paylineManager;
    public Image[] slotImages; // Array of 15 Image components for the grid
    public Text bankrollText;
    public Text betText;
    public Text winText;

    // Mapping between symbol names and file names (with .png extension)
    private Dictionary<string, string> symbolToFileName = new Dictionary<string, string>
    {
        {"Cherry", "cherry.png"},
        {"Lemon", "lemon.png"},
        {"Orange", "orange.png"},
        {"Peach", "peach.png"},
        {"Bell", "bell.png"},
        {"Grapes", "grapes.png"},
        {"Watermelon", "watermelon.png"},
        {"RedSeven", "seven.png"},
        {"Diamond", "diamond.png"},
        {"Star", "star.png"},
        {"Crown", "crown.png"}
    };

    void Start()
    {
        Debug.Log("SlotMachineUI: Start method called.");
        PrintDetectedFiles();
        reelManager = new ReelManager();
        bankrollManager = new BankrollManager(100);
        paylineManager = new PaylineManager();
        UpdateUI();
    }

    void PrintDetectedFiles()
    {
        string resourcesPath = Application.dataPath + "/Resources/Symbols";
        Debug.Log($"Resources path: {resourcesPath}");
        if (Directory.Exists(resourcesPath))
        {
            foreach (string file in Directory.GetFiles(resourcesPath))
            {
                Debug.Log($"Detected file: {Path.GetFileName(file)}");
            }
        }
        else
        {
            Debug.LogError($"Directory not found: {resourcesPath}");
        }
    }

    public void SpinReels()
    {
        if (bankrollManager.CurrentBet <= 0 || bankrollManager.Bankroll < bankrollManager.CurrentBet)
        {
            Debug.LogWarning("Not enough funds to spin!");
            return;
        }

        bankrollManager.DeductBet();
        string[] reels = reelManager.SpinReels();
        for (int i = 0; i < reels.Length; i++)
        {
            if (symbolToFileName.TryGetValue(reels[i], out string fileName))
            {
                Sprite symbolSprite = Resources.Load<Sprite>("Symbols/" + Path.GetFileNameWithoutExtension(fileName));
                if (symbolSprite != null)
                {
                    slotImages[i].sprite = symbolSprite;
                }
                else
                {
                    Debug.LogError($"Failed to load sprite: {fileName}");
                }
            }
            else
            {
                Debug.LogError($"No file name found for symbol: {reels[i]}");
            }
        }
        PrintGridDebug(reels);
        int winnings = paylineManager.CalculateWinnings(reels, bankrollManager.CurrentBet);
        bankrollManager.AddWinnings(winnings);
        UpdateUI();
        winText.text = winnings > 0 ? $"You won {winnings} units!" : "No win this time!";
    }

    void UpdateUI()
    {
        bankrollText.text = $"Bankroll: {bankrollManager.Bankroll}";
        betText.text = $"Bet: {bankrollManager.CurrentBet}";
    }

    public void PrintGridDebug(string[] reels)
    {
        Debug.Log("-------------------------");
        for (int i = 0; i < 15; i += 5)
        {
            Debug.Log($"| {reels[i]} | {reels[i + 1]} | {reels[i + 2]} | {reels[i + 3]} | {reels[i + 4]} |");
            Debug.Log("-------------------------");
        }
    }
}
