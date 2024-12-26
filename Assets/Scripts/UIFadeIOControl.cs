using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeIOControl : MonoBehaviour
{
    public enum Status
    {
        Idel,
        FadeIn,
        FadeOut
    }
    public Status status;
    private Animator animator;
    void Start()
    {
        status = Status.Idel;
        animator = gameObject.GetComponent<Animator>();
    }


    public void SetFadeIn()
    {
        if(!(status == Status.FadeIn))
        {
            status = Status.FadeIn;
            animator.SetBool("Fade In",true);
            animator.SetBool("Fade Out",false);
            Debug.Log("Fade in");
        }   
    }

    public void SetFadeOut()
    {
        if(!(status == Status.FadeOut))
        {
            status = Status.FadeOut;
            animator.SetBool("Fade In",false);
            animator.SetBool("Fade Out",true);
            Debug.Log("Fade Out");
        }
    }
}
