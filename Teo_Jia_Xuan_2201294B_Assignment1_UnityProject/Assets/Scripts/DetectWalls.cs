using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWalls : MonoBehaviour
{
    public LayerMask environment;
    public Transform playerTransform;
    public GameObject objectToHide;
    public float distance;
    public Renderer objectToHideRenderer;
    public List<GameObject> objectsToHideList = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        //ensures that a line is specifically drawn from the camera to the player
        Vector3 direction = playerTransform.position - transform.position;
        float distanceFromCameraToPlayer = direction.magnitude;
        Vector3 normalizedDirection = direction.normalized;
        if (Physics.Raycast(transform.position, normalizedDirection, out RaycastHit hit, distanceFromCameraToPlayer,  environment))
        {
            objectToHide = hit.collider.gameObject;
            objectsToHideList.Add(objectToHide);
            objectToHideRenderer = objectToHide.GetComponent<Renderer>();
            if (objectToHideRenderer != null)
            {
                objectToHideRenderer.enabled = false;
            }
        }
        else
        {
            foreach (GameObject obj in objectsToHideList)
            {
                Renderer objRenderer = obj.GetComponent<Renderer>();
                if (objRenderer != null)
                {
                    objRenderer.enabled = true;
                }
            }
            objectsToHideList.Clear();
        }
    }
}
