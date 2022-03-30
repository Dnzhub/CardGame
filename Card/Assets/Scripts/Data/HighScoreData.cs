using UnityEngine;
using TMPro;


public class HighScoreData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
     
    void Start()
    {
        int highScore = PlayerPrefs.GetInt(PlayerManager.HighScoreKey, 0);
        highScoreText.text = "High Score: " + highScore.ToString();
    }
    
}
