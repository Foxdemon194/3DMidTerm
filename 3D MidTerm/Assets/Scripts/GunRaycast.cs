using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunRaycast : MonoBehaviour
{
    //variables
    [SerializeField] float maxRayDistance = 100f;
    [SerializeField] float maxGrabDistance = 10f;

    public LayerMask activeLayers;
    public LayerMask redDotLayer;

    bool isGrabed;
    Rigidbody grabbedObject;
    //[SerializeField] float forceIntensity = 500;   
    
    public Text objectName;
    public GameObject redDot;

    Color oldColor = Color.black;
    Renderer rendererHolder = null;

    private void Start()
    {
        isGrabed = false;
        objectName.text = "No Object is grabed";
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

    private void LateUpdate()
    {
        //using late update to handle colors and the position of the red dot
        Ray ray = new Ray(transform.position, transform.forward * maxRayDistance);
        if (rendererHolder != null) 
        {
            rendererHolder.material.color = oldColor;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, redDotLayer))
        {
            redDot.transform.position = hit.point;
            if (hit.collider.CompareTag("grabable"))
            {
                rendererHolder = hit.collider.GetComponent<Renderer>();
                oldColor = rendererHolder.material.color;
                rendererHolder.material.color = Color.black;
            }
            if (hit.collider.CompareTag("hitable"))
            {
                rendererHolder = hit.collider.GetComponent<Renderer>();
                oldColor = rendererHolder.material.color;
                rendererHolder.material.color = Color.white;
            }
        }
        else
        {
            redDot.transform.position = transform.forward * maxRayDistance;
        }
    }

    //shoot method; destroys breakable objects
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
    
    //if CheckForGrab comes out false this method gets called; grabs an object and takes its name
    void Grab(RaycastHit hit, Ray ray)
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, activeLayers))
        {
            objectName.text = (hit.collider.name);
            grabbedObject = hit.rigidbody;
            grabbedObject.isKinematic = true;
            grabbedObject.transform.SetParent(gameObject.transform);
            isGrabed = true;
        }
    }

    //if CheckForGrab comes out true this method gets called; grabs object and changes the grabed object text to "No Object is grabed"
    void Ungrab(RaycastHit hit, Ray ray)
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, activeLayers))
        {
            objectName.text = "No Object is grabed";
            grabbedObject.transform.parent = null;
            grabbedObject.isKinematic = false;
            isGrabed = false;
        }
    }
    
}
