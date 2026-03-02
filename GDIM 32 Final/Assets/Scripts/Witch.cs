using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Walking,
    Talking,
    Interacting
}

public class Witch : MonoBehaviour
{
    PlayerState _currentActivity;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] public Animator animator;
    float turnSmoothVelocity;
    void Start()
    {



       _currentActivity = PlayerState.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentActivity == PlayerState.Walking)
        {
            animator.SetBool("Talking", false);
            animator.SetBool("Interacting", false);


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




        }else if (_currentActivity == PlayerState.Talking)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Talking", true);
        }else if (_currentActivity == PlayerState.Interacting)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Interacting", true);
        }
    }


}
