using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace VL
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        public InputHandler inputHandler;
        public PlayerLocomotion playerLocomotion;
        private int vertical;
        private int horizontal;
        public bool canRotate;

        public void Initialize()
        {
            animator = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            var v = verticalMovement switch
            {
                > 0 and < 0.55f => 0.5f,
                > 0.55f => 1,
                < 0 and > -0.55f => -0.5f,
                < -0.55f => -1,
                _ => 0
            };
            
            var h = horizontalMovement switch
            {
                > 0 and < 0.55f => 0.5f,
                > 0.55f => 1,
                < 0 and > -0.55f => -0.5f,
                < -0.55f => -1,
                _ => 0
            };
            
            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnimation, 0.2f);
        }
        
        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (inputHandler.isInteracting == false) return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidBody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidBody.velocity = velocity;
        }
    }
    
}
