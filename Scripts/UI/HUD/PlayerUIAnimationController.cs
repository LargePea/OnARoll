using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        if (animator != null)
        {
            animator.SetBool("Victory", true);
        }
    }
}
