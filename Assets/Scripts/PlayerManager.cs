using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler inputHandler;
        private Animator animator;
        
        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            inputHandler.isInteracting = animator.GetBool("isInteracting");
            inputHandler.rollFlag = false;
        }
    }
    
}
