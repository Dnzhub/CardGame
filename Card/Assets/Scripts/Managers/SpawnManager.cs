using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance = null;

    [Header("Prefabs")]
    public GameObject RedPrefab;
    public GameObject GreenPrefab;
    public Transform CardsParent;
    [Space]
    public List<Transform> SpawnPoints = new List<Transform>();

    [Header("Attributes")]
    public int _currentLevel;
    private int maxRandomness = 5;
 

    private void Awake()
    {
        #region SÝNGLETON
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        #endregion
    }
    void Start()
    {
        _currentLevel = 1;
        SpawnPoints[0].gameObject.SetActive(false);

        foreach (Transform point in SpawnPoints)
        {
            if(point.gameObject.activeInHierarchy)
            {
                SpawnCard(point);
            }        
        }       
    }

    public void SpawnCard(Transform position)
    {
        int randomDrop;
        if (_currentLevel <= maxRandomness) //If current level smaller than %50 chance
        {
            randomDrop = _currentLevel;
        }
        else
        {
            randomDrop = maxRandomness;  //fix it to %50 
        }

        if (Random.value > randomDrop * 0.1f)
        {
            GameObject GreenCard = Instantiate(GreenPrefab, position.position, Quaternion.identity);
            GreenCard.transform.SetParent(CardsParent.transform);           
            
        }
        else
        {
            GameObject RedCard = Instantiate(RedPrefab,position.position,Quaternion.identity);
            RedCard.transform.SetParent(CardsParent.transform);

        }
       
    }

}
