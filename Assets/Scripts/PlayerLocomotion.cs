using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private Transform cameraObject;
        private InputHandler inputHandler;
        private Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;

        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public Rigidbody rigidBody;
        public GameObject normalCamera;

        [Header("Stats")]
        [SerializeField]
        private float movementSpeed = 5;
        [SerializeField]
        private float rotationSpeed = 10;   
        
        // Start is called before the first frame update
        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();
            
        }

        public void Update()
        {
            float delta = Time.deltaTime;
            
            inputHandler.TickInput(delta);
            HandleMovement(delta);
            HandleRollingAndSprinting(delta);

        }

        #region Movement

        private Vector3 normalVector;
        private Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDirection = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;
            
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = myTransform.forward;
            }

            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();

            float speed = movementSpeed;
            moveDirection *= speed;
            moveDirection.y = 0;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidBody.velocity = projectedVelocity;
            
            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

         public void HandleRollingAndSprinting(float delta)
         {
             if (animatorHandler.animator.GetBool("isInteracting")) return;
        
             if (inputHandler.rollFlag)
             {
                 moveDirection = cameraObject.forward * inputHandler.vertical;
                 moveDirection += cameraObject.right * inputHandler.horizontal;
        
                 if (inputHandler.moveAmount > 0)
                 {
                     animatorHandler.PlayTargetAnimation("Roll", true);
                     moveDirection.y = 0;
                     Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                     myTransform.rotation = rollRotation;
                 }
                 else
                 {
                     animatorHandler.PlayTargetAnimation("Backstep", true);
                 }
             }
        }

        #endregion
    }
    
}
