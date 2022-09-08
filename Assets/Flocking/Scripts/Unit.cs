using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public GameObject manager;
	public Vector2 location = Vector2.zero;
  	public Vector2 velocity;
  	Vector2 goalPos = Vector2.zero;
  	Vector2 currentForce;

	void Start () 
	{
		velocity = new Vector2(Random.Range(0.01f,0.1f),Random.Range(0.01f,0.1f));
		location = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
	}

	Vector2 seek(Vector2 target)
	{
	    return(target - location);
	}

	void applyForce(Vector2 f)
	{
		Vector3 force = new Vector3(f.x,f.y,0);
		if(force.magnitude > manager.GetComponent<AllUnits>().maxforce)
		{
			force = force.normalized;
			force *= manager.GetComponent<AllUnits>().maxforce;
		}
		this.GetComponent<Rigidbody2D>().AddForce(force);

		if(this.GetComponent<Rigidbody2D>().velocity.magnitude > manager.GetComponent<AllUnits>().maxvelocity)
		{
			this.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity.normalized;
			this.GetComponent<Rigidbody2D>().velocity *= manager.GetComponent<AllUnits>().maxvelocity;
		}

		Debug.DrawRay(this.transform.position, force, Color.white);
	}

	Vector2 align()
	{
		float neighbordist = manager.GetComponent<AllUnits>().neighbourDistance;
	    Vector2 sum = Vector2.zero;
	    int count = 0;
	    foreach (GameObject other in manager.GetComponent<AllUnits>().units) 
	    {
	      if(other == this.gameObject) continue;

	      float d = Vector2.Distance(location, other.GetComponent<Unit>().location);
	      
	      if (d < neighbordist) {
	        sum += other.GetComponent<Unit>().velocity;
	        count++;
	      }
	    }
	    if (count > 0) 
	    {
	      sum /= count;
	      Vector2 steer = sum - velocity;
	      return steer;
	    } 

	    return Vector2.zero;
	    
	}

	Vector2 cohesion()
	{
		float neighbordist = manager.GetComponent<AllUnits>().neighbourDistance;
	    Vector2 sum = Vector2.zero;  
	    int count = 0;
	    foreach (GameObject other in manager.GetComponent<AllUnits>().units) 
	    {
	      if(other == this.gameObject) continue;

	      float d = Vector2.Distance(location, other.GetComponent<Unit>().location);
	      if (d < neighbordist) 
	      {
	        sum += other.GetComponent<Unit>().location; 
	        count++;
	      }
	    }

	    if (count > 0) 
	    {
	      sum /= count;
	      return seek(sum);  
	    } 
	    
	    return Vector2.zero;
	   
	}

	void flock()
	{
		location = this.transform.position;
		velocity = this.GetComponent<Rigidbody2D>().velocity;

		if(manager.GetComponent<AllUnits>().obedient && Random.Range(0,50)<=1)
		{

		    Vector2 ali = align();      
		    Vector2 coh = cohesion();
		    Vector2 gl;
		    if(manager.GetComponent<AllUnits>().seekGoal) 
		    {
		    	gl = seek(goalPos);
		    	currentForce = gl + ali + coh;
		    }
		    else
		    	currentForce = ali + coh;

		    currentForce = currentForce.normalized;
		}  
		
		if(manager.GetComponent<AllUnits>().willful && Random.Range(0,50)<=1)
		{
			if(Random.Range(0,50) < 1) 
				currentForce = new Vector2(Random.Range(0.01f,0.1f),Random.Range(0.01f,0.1f));
		}  
		
		applyForce(currentForce);
	}


	void Update () 
	{
		flock();
		goalPos = manager.transform.position;
	}
}
