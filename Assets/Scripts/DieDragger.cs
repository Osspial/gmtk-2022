using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DieDragger : MonoBehaviour
{
    private List<Die> grabbedDice = new List<Die>();
    public Collider dragPlane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Assert.IsNotNull(dragPlane);

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit hit;
            Die dieClicked = null;
            if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, Die.DIE_LAYER))
            {
                dieClicked = hit.collider.GetComponent<Die>();
                Assert.IsNotNull(dieClicked);
                grabbedDice.Add(dieClicked);
                dieClicked.StartDrag(hit);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            foreach (Die die in grabbedDice)
            {
                die.EndDrag();
            }
            grabbedDice.Clear();
        }

        RaycastHit dragPlaneHit;
        if (dragPlane.Raycast(mouseRay, out dragPlaneHit, Mathf.Infinity)) {
            foreach (Die die in grabbedDice)
            {
                die.DragTo(dragPlaneHit.point);
            }
        }

    }
}
