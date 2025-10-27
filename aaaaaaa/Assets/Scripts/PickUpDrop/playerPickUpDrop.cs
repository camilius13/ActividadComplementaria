using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPickUpDrop : MonoBehaviour
{

    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    

    public float pickupDistance = 2f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            
            if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,out RaycastHit raycastHit, pickupDistance))
            {
                if(raycastHit.transform.TryGetComponent(out objectGrabbable objectGrabbable))
                {
                    objectGrabbable.Grab(objectGrabPointTransform);
                }
            }
        }
    }


}
