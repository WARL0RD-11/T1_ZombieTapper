using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash_Behavior : MonoBehaviour
{
    private Animation anim;
    private Animator animator;

    private void Start()
    {
       // anim = GetComponent<Animation>();

        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(int index)
    {
        //anim.Play("MuzzleFlash_Fire");
        //anim.Play();
        animator.SetBool("isShowing", true);
        animator.SetInteger("muzzleIndex", index);
        animator.Play(0, -1, 0f);
        //animator.SetBool("isShowing", false);
    }

    public void FlashGone()
    {
        animator.SetBool("isShowing", false);
    }
}
