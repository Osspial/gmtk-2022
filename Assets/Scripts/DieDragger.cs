using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DieDragger : MonoBehaviour
{
    private List<Die> grabbedDice = new List<Die>();
    public Collider dragPlane;
	private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
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
                if (dieClicked.StartDrag(hit))
                {
                    grabbedDice.Add(dieClicked);
					source.Play();
                }
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
