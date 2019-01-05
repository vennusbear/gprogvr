using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour {

    private GameObject[] itemsInside;
    public LayerMask m_LayerMask;
    private Transform scanArea;
    bool m_Started;

	// Use this for initialization
	void Start () {
        m_Started = true;
        scanArea = transform.GetChild(0);
	}
	

    void GetInactiveInRadius()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(scanArea.position, scanArea.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_Started)
        {
            Gizmos.DrawWireCube(scanArea.position, scanArea.localScale / 2);
        }
    }
}
