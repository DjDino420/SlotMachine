using System;

public class BankrollManager
{
    private int bankroll;
    private int currentBet;

    public BankrollManager(int initialBankroll)
    {
        bankroll = initialBankroll;
        currentBet = 1;
    }

    public int Bankroll => bankroll;
    public int CurrentBet => currentBet;

    public bool SetBet(int bet)
    {
        if (bet <= 0 || bet > bankroll) return false;
        currentBet = bet;
        return true;
    }

    public void AddFunds(int amount) => bankroll += amount;
    public void WithdrawFunds(int amount) => bankroll -= amount;
    public void DeductBet() => bankroll -= currentBet;
    public void AddWinnings(int winnings) => bankroll += winnings;
}