using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector2 destination;
    public float speed;
    float timeNeededToReachDestination;
    float timeElapsed;
    float startTime;
    float percentage;
    Vector2 startingPos;
    public float waveAmplitude;
    public int waveFrequency;
    public Vector2 startingSize;
 
    // Start is called before the first frame update
    void Start()
    {
        timeNeededToReachDestination = (Vector2.Distance(transform.position, destination)) / speed;
        timeElapsed = 0;
        startingPos = transform.position;

       
    }

    // Update is called once per frame
    void Update()
    {
        //update the time elapsed
        timeElapsed += Time.deltaTime;
        
        if (Vector2.Distance(transform.position, destination) >= .01f)
        {
            //calculate percentage along wave
            percentage = timeElapsed / timeNeededToReachDestination;

            //get a vector that is your next destination along path you are moving to
            Vector2 heading = Vector2.Lerp(startingPos, destination, percentage);

            //get the perpindicular vector
            Vector2 perp = GetPerpindicularVector(heading).normalized;

            //multiply the perpindicular vector by the sin of the distance from the destination
            Vector2 newPerp = waveAmplitude * perp * Mathf.Sin(percentage * waveFrequency * Mathf.PI * 2);

            //move toward the destination with wAvEs
            transform.position = heading + newPerp;

            //make the object smaller over time
            transform.localScale = Vector2.Lerp(startingSize, Vector2.zero, percentage);

            //make the object dissolve over time
            GetComponent<SpriteRenderer>().material.SetFloat("_Fade", 1 - percentage);

        }
        else
        {
            Destroy(gameObject);
        }


    }

    //gets the vector perpindicular to the one given
    Vector2 GetPerpindicularVector(Vector2 vec)
    {
        return new Vector2(vec.y, -1 * vec.x);
    }

    
}
