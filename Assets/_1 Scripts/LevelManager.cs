using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    /*
     * Internal class for level definition
     * Includes the actual level prefab and how many slimes the player can use
     */
    [Serializable]
    public class LevelDef
    {
        [SerializeField] public GameObject levelPrefab;
        [SerializeField] public int slimeCount;
    }

    [SerializeField] private LevelDef[] levelDefinitions;
    [SerializeField] private Transform playerSpawnPosition;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int remainingSlimes;
    [SerializeField] private int currentLevelIndex;

    private GameObject currentPlayerInstance;
    private GameObject currentLevelInstance;

    public void LoadLevel(int levelIndex)
    {
        //Destroy existing player in exists
        if (currentPlayerInstance != null)
            Destroy(currentPlayerInstance);

        //Destroy existing level if exists
        if (currentLevelInstance != null)
            Destroy(currentLevelInstance);

        currentLevelIndex = levelIndex;

        //Instantiate a basic player
        currentPlayerInstance = Instantiate(playerPrefab);

        //Place the player in the correct spot on the map
        currentPlayerInstance.transform.position = playerSpawnPosition.position;

        //Instantiate the level passed as argument
        currentLevelInstance = Instantiate(levelDefinitions[currentLevelIndex].levelPrefab);

        //Set number of slimes the player can use
        remainingSlimes = levelDefinitions[currentLevelIndex].slimeCount;
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex);
    }
}
