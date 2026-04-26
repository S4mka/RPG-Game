using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class AnimationController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    void Update()
    {
        bool Walking = animator.GetBool("Walk");
        bool isLeftMousePressed = Mouse.current.leftButton.isPressed;
        bool isRightMousePressed = Mouse.current.rightButton.isPressed;
        bool isWalkPressed1 = Keyboard.current.wKey.isPressed;
        bool isWalkPressed2 = Keyboard.current.aKey.isPressed;
        bool isWalkPressed3 = Keyboard.current.dKey.isPressed;
        bool isWalkPressed4 = Keyboard.current.sKey.isPressed;
       /* if (!Walking && isWalkPressed1)
            animator.SetBool("Walk", isWalkPressed);

        if (!isWalkPressed && Walking)
            animator.SetBool("Walk", isWalkPressed);*/

        if (Keyboard.current == null)
            animator.SetBool("Walk", isWalkPressed1);


        if (isWalkPressed2)
            animator.SetBool("Walk", isWalkPressed2);
        if (!isWalkPressed2)
            animator.SetBool("Walk", isWalkPressed2);

        if (isWalkPressed3)
            animator.SetBool("Walk", isWalkPressed3);


        if (isWalkPressed1)
                animator.SetBool("Walk", isWalkPressed1);


        if (Keyboard.current.sKey.isPressed)
            animator.SetBool("Walk", isWalkPressed4);


        if (isLeftMousePressed)
            animator.SetTrigger("PAttack");

        if (isRightMousePressed)
            animator.SetTrigger("MAttack");




    }
}