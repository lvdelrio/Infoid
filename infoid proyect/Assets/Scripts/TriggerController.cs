using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("s")){
            animator.SetBool("bool_falling_fast",true);
            Debug.Log("weon q pija ctm weon inutil esta wea es mas que true");

        }if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("bool_falling_fast", false);
        }
        
    }
}
