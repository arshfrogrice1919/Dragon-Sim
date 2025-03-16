using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float flightSpeed = 5f;
    [SerializeField] private float verticalSpeed = 2f;
    [SerializeField] private GameObject fireEffectPrefab;
    [SerializeField] private Transform firePoint;
    
    private FixedJoystick fixedJoystick;
    private Rigidbody rigidBody;
    private Animator animator;
    private PlaceOnPlaneNewInputSystem placeOnPlaneScript;
    private bool isFlying = false;

    private void OnEnable()
    {
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        placeOnPlaneScript = FindObjectOfType<PlaceOnPlaneNewInputSystem>();
    }

    private void FixedUpdate()
    {
        float xval = fixedJoystick.Horizontal;
        float yval = fixedJoystick.Vertical;
        bool isMoving = Mathf.Abs(xval) > 0.1f || Mathf.Abs(yval) > 0.1f;

        if (placeOnPlaneScript != null)
            placeOnPlaneScript.enabled = !isMoving;

        Vector3 movement;

        if (isFlying)
        {
            movement = new Vector3(xval, yval * verticalSpeed, 0);
            rigidBody.linearVelocity = movement * flightSpeed;

            animator.SetBool("isFlying", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            movement = new Vector3(xval, 0, yval);
            rigidBody.linearVelocity = movement * speed;

            animator.SetBool("isFlying", false);
            animator.SetBool("isWalking", isMoving);
        }

        if (isMoving)
        {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                Mathf.Atan2(xval, yval) * Mathf.Rad2Deg,
                transform.eulerAngles.z
            );
        }
    }

    public void ToggleFlightMode()
    {
        isFlying = !isFlying;
        Debug.Log("Fly Button Pressed! isFlying = " + isFlying);

        if (isFlying)
        {
            //rigidBody.useGravity = false;
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            animator.SetBool("isFlying", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            //rigidBody.useGravity = true;
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.constraints = RigidbodyConstraints.None;

            animator.SetBool("isFlying", false);
            animator.SetBool("isWalking", false);
        }
    }

    public void FireAttack()
    {
        Debug.Log("Fire Attack!");
        
        if (isFlying)
        {
            animator.SetTrigger("FlyFlameAttack");
        }   
        else
        {
            animator.SetTrigger("FlameAttack");
        }

        GameObject fire = Instantiate(fireEffectPrefab, firePoint.position, firePoint.rotation);
        Destroy(fire, 1f);
        StartCoroutine(ResetFireAnimation());
    }
    private IEnumerator ResetFireAnimation()
    {
    yield return new WaitForSeconds(0.7f);
    animator.ResetTrigger("FireAttack"); 
    animator.SetTrigger("Idle"); 
    } 
}




