﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Playercontroller controller;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Playercontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));//速度判断
        anim.SetBool("jump", controller.isJump);
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", controller.isGround);
    }
}
