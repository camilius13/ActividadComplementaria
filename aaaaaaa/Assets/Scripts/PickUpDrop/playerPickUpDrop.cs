


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
    public float throwForce = 6f;




    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {


            if (objectGrabbable == null)
            {
                //sin objeto en la mano
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {

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
        else if (objectGrabbable != null && Input.GetKeyDown(KeyCode.Mouse0))
        {

            objectGrabbable.Throw(throwForce);
            objectGrabbable = null;


        }
    }



}



