using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/*
 * This class is for handling a lot of the operations needed inorder to communicate with a file
 * Likely this class will not need to be edited to be used in the system
 */
public class FileHandler
{
    private string dataPath = "";
    private string fileName = "";

    private bool useEncryption = false; //Tracks whether encryption is being used or not
    private readonly string encryptionCode = "Hey you, you are finally awake";  //key to create the encryption with

    /*
     * Constructor for the FileHandler
     * DP is the path to access the location where the file is stored
     * FN is the name the file will be stored as
     * Encryption is a bool for whether Encryption is used or not (should be used in final product but toggling can be useful for debugging)
     */
    public FileHandler(string DP, string FN, bool Encryption)
    {
        dataPath = DP;
        fileName = FN;
        useEncryption = Encryption;
    }

    /*
     * used to load in data from the information class
     * returns the information class with all the data loaded into it from the file
     */
    public GameData Load()
    {
        string fullPath = Path.Combine(dataPath, fileName); //creates the full file path needed inorder to obtain the file
        GameData loadedData = null; 

        //checks if the file has been created already, if it has not returns a null object
        if (File.Exists(fullPath))  
        {
            try
            {
                string dataToLoad = ""; //uses filestream to read in all the data
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //if encryption is being used the data being loaded will be sent to the EncryptDecrypt function
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                
                //once the data has been loaded it it is converted from a json back to a gameData object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured trying to load data to a file to " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    /*
     * used to save data from the information class onto a file
     * takes the information class object in as input
     */
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataPath, fileName); //creates the full file path needed inorder to obtain the file
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);    //converts the data to json to be stored

            //if encryption is being used, sends the data to be stored to EncryptDecrypt
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //uses FileStream inorder to write the data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error occured trying to save data to a file to " + fullPath + "\n" + e);
        }
    }

    /*
     * Uses an XOR operation inorder to convert the provided string between being encrypted and decrypted (either can be sent and it will switch between the two)
     * Takes in a string of the data to be converted
     * Returns a string of the converted data
     */
    private string EncryptDecrypt(String data)
    {
        string modifiedData = "";

        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCode[i % encryptionCode.Length]);
        }

        return modifiedData;

    }




}