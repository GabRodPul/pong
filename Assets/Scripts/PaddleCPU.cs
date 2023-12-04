// #define PONG_TWO_PLAYERS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleCPU : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] float speed = 1f;

    void Start()
    {
#if !PONG_TWO_PLAYERS
        gameObject.name = Global.Data.NameCPU2;
#endif
    }

    void FixedUpdate()
    {
        if (transform.position.y < ball.transform.position.y)
            GetComponent<Rigidbody2D>().AddForce(speed * Vector2.up);
        else
        if (transform.position.y > ball.transform.position.y)
            GetComponent<Rigidbody2D>().AddForce(speed * Vector2.down);
    }
}
