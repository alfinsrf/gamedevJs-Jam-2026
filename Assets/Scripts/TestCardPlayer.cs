using UnityEngine;

public class TestCardPlayer : MonoBehaviour
{
    public void AttemptToPlayFirstCard()
    {
        if (CardManager.Instance.Hand.Count > 0)
        {            
            CardData cardToPlay = CardManager.Instance.Hand[0];
            bool success = CardManager.Instance.PlayCard(cardToPlay, 15, 10);

            if (success)
                Debug.Log($"Successfully played {cardToPlay.CardName}!");
        }
    }
}