using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRaycast : MonoBehaviour
{
    //variables
    [SerializeField] float maxRayDistance = 100f;
    [SerializeField] float maxGrabDistance = 10f;

    public LayerMask activeLayers;

    bool isGrabed;
    Rigidbody grabbedObject;
    [SerializeField] float forceIntensity = 500;    

    private void Start()
    {
        isGrabed = false;
    }

    void Update()
    {//debug line so I can track where the raycast would appear
        Debug.DrawRay(gameObject.transform.position, transform.forward * maxRayDistance, Color.red);

            //shoot when left click
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
            
        //grab/ungrab when right click
        if (Input.GetMouseButtonDown(1)) 
        {
            CheckForGrab();
        }        
    }

    //shoot method
    void Shoot()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward * maxRayDistance);

        if (Physics.Raycast(ray, out hit, maxRayDistance, activeLayers))
        {
            if (hit.collider.tag == "hitable")
            {
                Destroy(hit.collider.gameObject);
                Debug.Log("You shot " + hit.collider.name);
            }
            else
            {
                Debug.Log("You tried to shoot " + hit.collider.name);
            }
        }
    }

    //this method checks to see if what the player is aiming at is being grabed or not
    void CheckForGrab()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward * maxGrabDistance);

        if (Physics.Raycast(ray, out hit, maxRayDistance, activeLayers))
        {
            if (hit.collider.tag == "grabable")
            {
                if (!isGrabed)
                {
                    Grab(hit, ray);
                }
                else if ((isGrabed))
                {
                    Ungrab(hit, ray);
                }
            }
        }
    }
    
    //if CheckForGrab comes out false this method gets called
    void Grab(RaycastHit hit, Ray ray)
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, activeLayers))
        {
            grabbedObject = hit.rigidbody;
            grabbedObject.isKinematic = true;
            grabbedObject.transform.SetParent(gameObject.transform);
            isGrabed = true;
        }
    }

    //if CheckForGrab comes out true this method gets called
    void Ungrab(RaycastHit hit, Ray ray)
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, activeLayers))
        {
            grabbedObject.transform.parent = null;
            grabbedObject.isKinematic = false;
            isGrabed = false;
        }
    }
    
}
