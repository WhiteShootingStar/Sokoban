using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayer : MonoBehaviour
{
    public enum AnimClips
    {
        Idle,
        Walk,
        Push
    }
    Animator animator;
    public AnimClips dummyState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("AnimationIndex",(int)dummyState);
    }
}
