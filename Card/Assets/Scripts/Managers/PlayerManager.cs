using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [Header("EventSystem")]
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    [Space]
    
    [Header("UI")]
    public TextMeshProUGUI GameOverLevelText;
    public TextMeshProUGUI GameOverScoreText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverPanel;
    TextMeshProUGUI _scoreText;
    public const string HighScoreKey = "HighScore";
    [Space]
   

    [Header("Player Attributes")]
    public GameObject player;
    public int _currentHealth;    
    public float _movementDelay = 1f;
    int _currentStep = 0;
    int _pointCounter = 0;
    bool _isMoving = false;
    Transform _lastPosition;


    void Start()
    {
        _lastPosition = player.gameObject.transform;      
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
        _currentHealth = 1;
        _scoreText = player.GetComponentInChildren<TextMeshProUGUI>();
        GameOverPanel.SetActive(false);
     

    }

    void Update()
    {
        if (IsGameOver() && !_isMoving)
        {
            SetGameOver();
        }
        if (Input.GetMouseButtonDown(0) && !_isMoving)
        {
            TargetCardDetection();
        }
    }
    private void SetGameOver()
    {
        GameOverPanel.SetActive(true);
        GameOverLevelText.text = "LEVEL: " + SpawnManager.instance._currentLevel.ToString();
        GameOverScoreText.text = "SCORE: " + _pointCounter.ToString();
        
        //Get high score data
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (_pointCounter > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, _pointCounter);
        }
        DOTween.Clear(true); //Clear all tweens data when game over
    }
    private bool IsGameOver()
    {
        if (_currentHealth < 1 )
        {
            return true;
        }
        return false;
    }

    private void TargetCardDetection()
    {
        //Detect any moveable card around player
        RaycastHit2D hitRight = Physics2D.Raycast(player.transform.position, player.transform.right, 225);
        RaycastHit2D hitLeft = Physics2D.Raycast(player.transform.position, -player.transform.right, 225);
        RaycastHit2D hitUp = Physics2D.Raycast(player.transform.position, player.transform.up, 100);
        RaycastHit2D hitDown = Physics2D.Raycast(player.transform.position, -player.transform.up, 100);

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);


        foreach (RaycastResult result in results)
        {
            //Polymorph on Interactable items
            IInteractable interactable = result.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
                _scoreText.text = _currentHealth.ToString();
              
            }

            //If selected card player's right-left-up-down move it
            if (result.gameObject.transform == hitRight.transform || result.gameObject.transform == hitLeft.transform
                || result.gameObject.transform == hitUp.transform || result.gameObject.transform == hitDown.transform)
            {
                _isMoving = true;
                _currentStep++;
                _pointCounter++;
                ScoreText.text = "SCORE: " + _pointCounter.ToString();

                if (_currentStep >= 10)
                {
                    SpawnManager.instance._currentLevel += 1;
                    LevelText.text = "LEVEL: " + SpawnManager.instance._currentLevel.ToString();
                    _currentStep = 0;
                }
                HandleCardMovement(result);
            }
        }
    }
    private void HandleCardMovement(RaycastResult result)
    {
        player.transform.DOMove(result.gameObject.transform.position, _movementDelay).OnComplete(() => _isMoving = false);
        SpawnManager.instance.SpawnCard(_lastPosition);
        Destroy(result.gameObject);
        _lastPosition = player.gameObject.transform;

    }

   
}
