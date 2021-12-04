using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public float transitionSpeed = 0.1f;

    Player player;
    Animator animator;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        Inventory.instance.onSwitch += SwitchWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent = controller.velocity.magnitude / player.playerSpeed;
        animator.SetFloat("speedPercent", speedPercent, transitionSpeed, Time.deltaTime);
    }

    void SwitchWeapon(int i)
    {
        animator.SetInteger("switchWeapon", i);
    }
}
