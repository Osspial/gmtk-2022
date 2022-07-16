using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * SaveDataManager will be attached to an otherwise empty gameobject that will be responsible for handling saving and loading data within the scene
 * This class is a singleton so there should not be more than one of this script within the scene it is in
 * Likely changes will need to be made based on how the save data is being handled. One example would be if multiple information classes were made
 */
public class SaveDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;   //Name that will be used for the file, visible in the editor. file extension and name can be set to anything
    [SerializeField] private bool useEncryption;    //Whether or not Encryption is used to store the data. Changeable within the unity editor

    private GameData gameData;  //The information class representing all the information being stored to a file
    private List<saveableInterface> savableObjects; //List of all objects that contain information that will be stored to a file
    private FileHandler fileHandler;    //The fileHandler that will handle interaction between the program and the file that is being used to store information


    public static SaveDataManager instance { get; private set; }    //The instance of this class, again there  should only be one in each scene

    /*Called when this object is loaded into the scene, checks if there is another instance of this class and throws an error if there is
     *then assigns itself as the instance
     */
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SaveDataManager is in the scene.");
        }
        instance = this;
    }


    /*Called before the first time update would be run,
     *Creates the filehandler providing it with the appropriete file path, file name, and if encryption is being used.
     *Then calls LoadGame
     */
    private void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName, useEncryption);
        savableObjects = findAllSavableObjects();   //finds all objects that use saved data and stores them in a list
        LoadGame(); 
    }

    /*
     * When the application is closed saves any data that needs to be saved
     */
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    /*
     * Creates a new instance of GameData
     */
    public void NewGame()
    {
        gameData = new GameData();
    }

    /*
     * Attempts to load the GameData stored on file
     * If there is no save file, will create a new one with default GameData values
     */
    public void LoadGame()
    {

        gameData = fileHandler.Load();  //Has the fileHandler load in game data from file if there is a file

        //Will create default gamedata if there is no file
        if(gameData == null)
        {
            Debug.Log("No data found, creating new data with default values");
            NewGame();
        }

        foreach(saveableInterface SO in savableObjects)
        {
            SO.loadData(gameData);  //Sends the GameData to each object in the scene that needs it
        }

    }

    /*
     * Checks every object that contains information that is saved to a file and saves the information to GameData, 
     * Then sends the data to the filehandler to convert it into a file
     */
    public void SaveGame()
    {
        //gives each object the gameData so it can add its respective information to it
        foreach (saveableInterface SO in savableObjects)
        {
            SO.saveData(gameData);  
        }

        //fileHandler takes the data and store it to the file
        fileHandler.Save(gameData);

    }

    /*
     * Finds all of the objects in the scene that contain data that would need to be saved
     * returns a list of all of these objects
     */
    private List<saveableInterface> findAllSavableObjects()
    {
        IEnumerable<saveableInterface> SO = FindObjectsOfType<MonoBehaviour>().OfType<saveableInterface>();
        return new List<saveableInterface>(SO);
    }

}
