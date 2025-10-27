using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPickUpDrop : MonoBehaviour
{

    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    
    private objectGrabbable objectGrabbable;
    public float pickupDistance = 2f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                //sin objeto en la mano
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        Debug.Log(raycastHit.transform.name);
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }
            else
            {
                //objeto en la mano
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
            
        }
    }


}
