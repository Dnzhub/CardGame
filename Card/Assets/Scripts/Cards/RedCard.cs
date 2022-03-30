using UnityEngine;
using DG.Tweening;
using TMPro;

public class RedCard : MonoBehaviour,IInteractable
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
        
        playerManager._currentHealth = Mathf.Max(playerManager._currentHealth -= cardValue, 0); 
    }

    private void OnEnable()
    {
        transform.DOScale(1f, 1f);
    }
    
}
