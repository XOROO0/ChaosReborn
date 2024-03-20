using System;
using UnityEngine;

/// <summary>
/// The script added to an object that has been time slowed. 
/// Do not add this to something yourself.
/// </summary>
public class TimeSlowed : MonoBehaviour
{
    //The number of times to slow down the object. e.g. setting it to 0.5 would slow down the object by half/it would be going 50% as fast
    public float timeSlowFactor = 1f;
    private Vector3 beforeTimeSlowVelocity;
    private Vector3 beforeTimeSlowRotation;
    public float timeSlowTimer;
    public float timeSlowTimeOut = 5f;
    bool shouldHaveGravity = true;
    private Animator animator;
    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            //records whether the object used gravity before being time slowed
            shouldHaveGravity = GetComponent<Rigidbody>().useGravity;
            //disable gravity so the object doesn't get affected by normal gravity
            GetComponent<Rigidbody>().useGravity = false;
            //		print ("objects gravity " + shouldHaveGravity);
            beforeTimeSlowVelocity = GetComponent<Rigidbody>().velocity;
            //had problems with floats being NaN, so I put this in
            if (Single.IsNaN(beforeTimeSlowVelocity.x))
            {
                beforeTimeSlowVelocity = Vector3.zero;
            }
            beforeTimeSlowRotation = GetComponent<Rigidbody>().angularVelocity;
            if (Single.IsNaN(beforeTimeSlowRotation.x))
            {
                beforeTimeSlowRotation = Vector3.zero;
            }
            //slows down the velocity and rotation by the time slow factor
            GetComponent<Rigidbody>().velocity = beforeTimeSlowVelocity * timeSlowFactor;
            GetComponent<Rigidbody>().angularVelocity = beforeTimeSlowRotation * timeSlowFactor;
        }
    }

    private void OnDestroy()
    {
        print("Time slow script destroyed");
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = new Vector3(beforeTimeSlowVelocity.x, beforeTimeSlowVelocity.y, beforeTimeSlowVelocity.z);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(beforeTimeSlowRotation.x, beforeTimeSlowRotation.y, beforeTimeSlowRotation.z);
            GetComponent<Rigidbody>().useGravity = shouldHaveGravity;
        }
    }

    void Update()
    {

        timeSlowTimer += Time.deltaTime;

        if (timeSlowTimer > timeSlowTimeOut)
        {
            Destroy(this);
        }
        if (GetComponent<Rigidbody>())
        {
            if (!Vector3IsEqual(GetComponent<Rigidbody>().velocity / timeSlowFactor, beforeTimeSlowVelocity))
            {
                //any velocity applied to the object since last update gets added to the object. This is so you can boost the object's speed by a bunch when it finally is returned to normal speed.
                beforeTimeSlowVelocity += ((GetComponent<Rigidbody>().velocity / timeSlowFactor) - beforeTimeSlowVelocity) * timeSlowFactor;
                //Then set the velocity of the object to the new slowed speed.
                GetComponent<Rigidbody>().velocity = beforeTimeSlowVelocity * timeSlowFactor;
            }
            if (!Vector3IsEqual(GetComponent<Rigidbody>().angularVelocity / timeSlowFactor, beforeTimeSlowRotation))
            {
                //same as above, except for rotation
                beforeTimeSlowRotation += ((GetComponent<Rigidbody>().angularVelocity / timeSlowFactor) - beforeTimeSlowRotation) * timeSlowFactor * 0.5f;
                GetComponent<Rigidbody>().angularVelocity = beforeTimeSlowRotation * timeSlowFactor;
            }
            //Add the force of gravity modified by the time slow factor
            GetComponent<Rigidbody>().AddForce((Physics.gravity * timeSlowFactor) * GetComponent<Rigidbody>().mass);
        }
        //was going to affect animations, but didn't have time
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            animator.speed = timeSlowFactor * 40;
        }
    }

    // called when player wants to cancel all time slowed objects
    public void DestroyTimeSlowed()
    {
        Destroy(this);
    }

    //Like Vector3.IsEqual, but more lenient
    public bool Vector3IsEqual(Vector3 firstVector, Vector3 secondVector)
    {
        return (firstVector - secondVector).sqrMagnitude <= 0.001f;
    }
}
