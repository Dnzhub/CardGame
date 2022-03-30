using UnityEngine;
using DG.Tweening;
using TMPro;

public class GreenCard : MonoBehaviour,IInteractable
{
    public TextMeshProUGUI ValueText;
    int cardValue;
    
    private void Start()
    {
        cardValue = Random.Range(0, SpawnManager.instance._currentLevel) + 1;
        ValueText.text = cardValue.ToString();

    }
    public void Interact(PlayerManager playerManager)
    {
        playerManager._currentHealth += cardValue;              
    }

    private void OnEnable()
    {
        transform.DOScale(1f, 1f);
    }
   
}
