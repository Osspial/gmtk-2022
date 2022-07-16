using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Information class that will be used to represent most of the data that needs to be saved throughout the game,
 * changes will need to be made to this in order to reflect the data we need to store
 */
[System.Serializable]
public class GameData 
{

    /*
     * List out any variables that need to be stored here, public is a good idea so that they can be easily accessed as stuff only is written/read from this class when creating or storing save data
     * Any variables added here should have default values in the constructor as the variables will likely be called by everything that is using the values to obtain their default values
     * 
     * Side note, more complex data types like dictionaries will cause problems, if you wanted to use stuff like that you would need to make a seperate info class
     * that is serialized so it can be converted to JSON properly
     */






    /*
     * Constructor for this data class, should be used for setting the default values of all of the variables listed above
     */
    public GameData()
    {
       
    }

}