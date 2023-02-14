using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRaycast : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] float maxRayDistance = 100f;
    public LayerMask activeLayers;

    float dist;
    bool isGrabed;
    Rigidbody grabbedObject;
    GameObject viewedObject;
    //[SerializeField] float grabRange;
    Rigidbody objectRB;
    [SerializeField] float forceIntensity = 500;

    private void Start()
    {
        isGrabed = false;
    }

    void Update()
    {
        Debug.DrawRay(gameObject.transform.position, transform.forward * maxRayDistance, Color.red);

        Ray ray = new Ray(transform.position, Vector3.forward);

        RaycastHit[] hits = Physics.RaycastAll(ray, maxRayDistance, activeLayers);

        if(Physics.Raycast(ray, out hit, maxRayDistance, activeLayers))
        {
            viewedObject = hit.collider.gameObject;
            dist = Vector3.Distance(viewedObject.transform.position, transform.position);
            dist = Mathf.Round(dist * 0.01f);
            Debug.Log(hit.collider.name + ": " + dist + ".");
            objectRB = viewedObject.GetComponent<Rigidbody>();
            //shoot when left click
            if (Input.GetMouseButton(0))
            {
                if (hit.collider.tag == "hitable")
                {
                    Destroy(viewedObject);
                    Debug.Log("You shot " + hit.collider.name);
                }
                else
                {
                    Debug.Log("You tried to shoot " + hit.collider.name);
                }

            }

            //grab when right click
            if (Input.GetMouseButton(1) && !isGrabed)
            {
                //Debug.Log("You tried to grab " + hit.collider.name);
                if (hit.collider.tag == "grabable")
                {
                    grabbedObject = hit.rigidbody;
                    grabbedObject.isKinematic = true;
                    grabbedObject.transform.SetParent(gameObject.transform);
                    isGrabed = true;
                }

            }
            else if((Input.GetMouseButton(1) && isGrabed))
            {
                grabbedObject.transform.parent = null;
                grabbedObject.isKinematic = false;
                isGrabed = false;
            }
        }
    }
}
