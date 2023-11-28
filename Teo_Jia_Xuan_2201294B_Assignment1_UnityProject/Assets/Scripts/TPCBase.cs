using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGGE
{
    // The base class for all third-person camera controllers
    public abstract class TPCBase
    {
        protected Transform mCameraTransform;
        protected Transform mPlayerTransform;

        public Transform CameraTransform
        {
            get
            {
                return mCameraTransform;
            }
        }
        public Transform PlayerTransform
        {
            get
            {
                return mPlayerTransform;
            }
        }

        public TPCBase(Transform cameraTransform, Transform playerTransform)
        {
            mCameraTransform = cameraTransform;
            mPlayerTransform = playerTransform;
        }

    public void RepositionCamera()
    {
        //determines the vector used for the ray itself from the camera to player
        Vector3 direction = mPlayerTransform.position - mCameraTransform.position;
        //offset here to make sure camera ray doesnt clash with the floor
        direction.y += 0.2f;

        // Set up a layer mask to ignore the player layer
        //so the layermask includes everything but the player since we want to check for anything between the camera and playyer
        LayerMask layerMask = ~LayerMask.GetMask("Player");
        //float variable used here to draw a ray that is exactly from the camera to the player with Physics.Raycast
        float distanceFromCameraToPlayer = direction.magnitude;
        Vector3 normalizedDirection = direction.normalized;
        
        //checks to see if the line of sight between camera and player is broken
        if (Physics.Raycast(mCameraTransform.position, normalizedDirection, out RaycastHit hit, distanceFromCameraToPlayer, layerMask))
        {
            //checks the tag of the wall to see if its transparent
            string wallTag = hit.collider.gameObject.tag;
            //if the wall is not transparent, then move the camera.
            //done because the camera can still see the player even if theres a transparent wall
            //as we can still see it
            if(wallTag != "Transparent")
            {
                if(wallTag != "Ceiling")
                {
                    //offset used here to ensure that the camera's y position always stays at a specified amount dictated by the original offset
                    float offset = CameraConstants.CameraPositionOffset.y - hit.point.y;
                    //adds that offset to the exact position of the object the ray is collided into 
                    Vector3 newOffset = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
                    //set camera position to that position
                    mCameraTransform.position = newOffset;
                }
                else
                {
                    mCameraTransform.position = hit.point;
                }
                // Recalculate direction based on the updated camera position
                //makes sure that the direction is not somehow using the camera's old position
                direction = mPlayerTransform.position - mCameraTransform.position;
            }

        }
        //Debug statment used here to visualise the ray to some degree
        Debug.DrawRay(mCameraTransform.position, direction, Color.blue);
    }



        public abstract void Update();
    }
}
