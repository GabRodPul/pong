// #define PONG_TWO_PLAYERS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
#if PONG_TWO_PLAYERS
    [SerializeField]
    KeyCode upKey        = KeyCode.UpArrow,
            downKey      = KeyCode.DownArrow,
            spinLeftKey  = KeyCode.LeftArrow,
            spinRightKey = KeyCode.RightArrow;
#endif

    [SerializeField]
    public float speed = 1f, spinStr = 10f;

    // Store RB2D to not call GetComponent everytime
    [SerializeField]
    public Rigidbody2D rb2d;

    float _angVelReset, _spinAxis;
    public bool _spin = false;

    void Start()
    {
#if !PONG_TWO_PLAYERS
        gameObject.name = Global.Data.NameP1;
#endif
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.angularDrag = .1f;
        _angVelReset = spinStr * 4.2f;
        _spinAxis = 0f;
    }
    void Update()
    {
#if PONG_TWO_PLAYERS
        GetComponent<Rigidbody2D>().velocity = speed switch
        {
            _ when Input.GetKey(upKey)   => Vector3.up   * speed,
            _ when Input.GetKey(downKey) => Vector3.down * speed,
            _ => Vector3.zero,
        };
#else
        rb2d.velocity = Input.GetAxisRaw("Vertical") switch
        {
             1f => speed * Vector2.up,
            -1f => speed * Vector2.down,
              _ => Vector2.zero
        };
#endif
        // Stop spin when a certain ang. vel. is reached
        if ((rb2d.angularVelocity > 0 && rb2d.angularVelocity <=  _angVelReset)
        ||  (rb2d.angularVelocity < 0 && rb2d.angularVelocity >= -_angVelReset))
        {
            rb2d.angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
            _spinAxis = 0f;
        }

        // Make the paddle spin if it isn't already and the Horizontal Axis
        // got any input.
        // From the left paddle's perspective, it'll spin to the same direction.
        if (rb2d.angularVelocity == 0)
            _spinAxis = -Input.GetAxisRaw("Horizontal");
        
        _spin = rb2d.angularVelocity == 0 && _spinAxis != 0f;
    }

    void FixedUpdate()
    {
        if (_spin) rb2d.AddTorque(spinStr * _spinAxis);
    }
}
