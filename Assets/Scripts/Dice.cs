using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dice
{
    public static int Roll(int numDice, int sizeDice)
    {
        int diceroll, total = 0;
        for (int i = 0; i < numDice; i++)
        {
            diceroll = Random.Range(1, sizeDice);
            total += diceroll;
            Debug.Log( $"Rolling {numDice}d{sizeDice} = {diceroll}: Total: {total}");
        }
        
        return total;
    }
}
