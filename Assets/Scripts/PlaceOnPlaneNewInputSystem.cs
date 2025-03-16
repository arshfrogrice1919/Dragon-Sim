using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlaneNewInputSystem : MonoBehaviour
{
    [SerializeField] private GameObject dragonPrefab;
    
    private GameObject dragonInstance;
    private TouchControls controls;
    private bool isPressed;
    private ARRaycastManager aRRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool isJoystickActive = false;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        controls = new TouchControls();
        controls.control.touch.performed += _ => isPressed = true;
        controls.control.touch.canceled += _ => isPressed = false;
    }

    private void Start()
    {
        SpawnDragon();
    }

    void Update()
    {
        if (isJoystickActive || Pointer.current == null || !isPressed)
            return;

        var touchPosition = Pointer.current.position.ReadValue();

        if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            dragonInstance.transform.position = hitPose.position;
            dragonInstance.transform.rotation = hitPose.rotation;

            Vector3 lookPos = Camera.main.transform.position - dragonInstance.transform.position;
            lookPos.y = 0;
            dragonInstance.transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }

    private void SpawnDragon()
    {
        if (dragonInstance == null)
        {
            dragonInstance = Instantiate(dragonPrefab, Vector3.zero, Quaternion.identity);
            dragonInstance.GetComponent<Animator>().Play("Idle");

            DragonController dragonController = dragonInstance.GetComponent<DragonController>();

            if (dragonController != null)
            {
                Button flyButton = GameObject.Find("FlyButton").GetComponent<Button>();
                flyButton.onClick.RemoveAllListeners();
                flyButton.onClick.AddListener(dragonController.ToggleFlightMode);

                Button fireButton = GameObject.Find("FireButton").GetComponent<Button>();
                fireButton.onClick.RemoveAllListeners();
                fireButton.onClick.AddListener(dragonController.FireAttack);
            }
        }
    }

    private void OnEnable() => controls.control.Enable();
    private void OnDisable() => controls.control.Disable();
}



