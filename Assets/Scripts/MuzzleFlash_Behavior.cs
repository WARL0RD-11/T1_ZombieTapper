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

    public void PlayAnimation()
    {
        //anim.Play("MuzzleFlash_Fire");
        //anim.Play();
        animator.Play(0, -1, 0f);
    }

}
