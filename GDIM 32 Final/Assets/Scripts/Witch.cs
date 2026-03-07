using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WitchState{Walking, Idle}
public class Witch : MonoBehaviour
{
    WitchState _currentActivity;

    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] public Animator animator;
    float turnSmoothVelocity;
    void Start()
    {
        PlayerInteractor.Instance.PickupCollectable += Pickup;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        UpdateBehavior();
            
    }

    void UpdateState()
    {
        if (!DialogueManager.Instance.IsDialogueActive)
        {
            _currentActivity = WitchState.Walking;
        }
        else
        {
            _currentActivity = WitchState.Idle;
        }
    }
    void UpdateBehavior()
    {
        switch (_currentActivity)
        {
            case WitchState.Walking:
                Walking();
                break;

            case WitchState.Idle:
                Idle();
                break;

            default:
                break;
        }
    }

    private void Idle()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Talking", false);
    }

    private void Pickup()
    {
        animator.SetTrigger("Interacting");
    }

    private void Walking() {
        animator.SetBool("Talking", false);
        //movement stuff
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(- direction.x, - direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                controller.Move(direction * speed * Time.deltaTime);
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);

        }    

    }



}
