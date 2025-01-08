using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI_Control
{

    public class UIFadeIOControl : MonoBehaviour
    {
        public enum Status
        {
            Idel,
            FadeIn,
            FadeOut
        }
        public Status status;
        public Canvas canvas;
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

        public void FadeInToOut(float sec)
        {
            StartCoroutine(fadeInToOut(sec));
        }

        public void FadeOutToIn(float sec)
        {
            StartCoroutine(fadeOutToIn(sec));
        }

        public IEnumerator fadeInToOut(float sec)
        {
            SetFadeIn();
            yield return new WaitForSeconds(sec);
            SetFadeOut();
            yield return new WaitForSeconds(1);
            canvas.sortingOrder = 0;
        }

        public IEnumerator fadeOutToIn(float sec)
        {
            SetFadeOut();
            yield return new WaitForSeconds(sec);
            SetFadeIn();
            yield return new WaitForSeconds(1);
            canvas.sortingOrder = 0;
        }
    }
}
