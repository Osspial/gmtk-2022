using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Interface that will be used on any class that will make use of saveed data will be made with
 * This class should not be edited
 */
public interface saveableInterface
{
    //When implemented will read the appropriete GameData to the object that implements this interface
    void loadData(GameData data);

    //When implemented will write the appropriete GameData to the provided GameData object (GameData is automatically passed by ref)
    void saveData(GameData data);

}
