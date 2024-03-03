using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;



[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : PressInputBase
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject MyShield;
    GameObject spawnedObject;
    bool isPressed;

    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    protected override void Awake()
    {
        base.Awake();
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    
    void Update()
    {
       
        if (Pointer.current == null || isPressed == false)
            return;

        
        var touchPosition = Pointer.current.position.ReadValue();

        
        if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(MyShield, hitPose.position, hitPose.rotation);
            }
            else
            { 
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;
            }

            Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;
            lookPos.y = 0;
            spawnedObject.transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }

    protected override void OnPress(Vector3 position) => isPressed = true;

    protected override void OnPressCancel() => isPressed = false;
}
