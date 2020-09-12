using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveSpeed;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");

        Vector2 moveVals = new Vector2(hori,verti);
        if (hori != 0 || verti != 0) transform.Translate(moveVals.normalized * Time.deltaTime * moveSpeed);

        animator.SetFloat("SpeedX", hori);
        animator.SetBool("SpeedY", verti != 0);
    }
}
