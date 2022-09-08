
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnits : MonoBehaviour {

	public GameObject[] units;
	public GameObject unitPrefab;
	public int numUnits = 10;
	public Vector3 range = new Vector3(5,5,5);

    public bool seekGoal = true;
	public bool obedient = true;
	public bool willful = false;

	[Range(0, 200)]
	public int neighbourDistance = 50;

	[Range(0, 2)]
	public float maxforce = 0.5f;

	[Range(0, 5)]
  	public float maxvelocity = 2.0f;

 	void OnDrawGizmosSelected() 
 	{
	 	Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.transform.position, range*2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, 0.2f);

    }

	
	void Start () {
		units = new GameObject[numUnits];
		for(int i = 0; i < numUnits; i++)
		{
			Vector3 unitPos = new Vector3(Random.Range(-range.x,range.x),
											Random.Range(-range.y,range.y),
											Random.Range(0,0));
			units[i] = Instantiate(unitPrefab,this.transform.position + unitPos, Quaternion.identity) as GameObject;
			units[i].GetComponent<Unit>().manager = this.gameObject;
		}

	}
}
