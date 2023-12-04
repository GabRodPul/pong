using GRP.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    static readonly Vector2 upLeft    = Vector2.up   + Vector2.left;
    static readonly Vector2 upRight   = Vector2.up   + Vector2.right;
    static readonly Vector2 downLeft  = Vector2.down + Vector2.left;
    static readonly Vector2 downRight = Vector2.down + Vector2.right;

    GameManager _gameManager;

    [SerializeField]
    float speed = 1f;
    float _startingSpeed;

    [SerializeField]
    float speedMultiplier = 1f;

    [SerializeField]
    float respawnOffset = 10f;

    [SerializeField]
    GameObject paddleP1, paddleP2;

    Rigidbody2D rb2d;

    // In case I want to add powerups
    GameObject _lastPaddle;
    int _hits;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = _gameManager is null
                     ? Global.Data.FindFirstMonoSingle<GameManager>()
                     : _gameManager;

        rb2d = GetComponent<Rigidbody2D>();



        _startingSpeed = speed;
        Spawn();
    }
    
    /// <summary>
    /// Resets ball's speed and randomizes diagonal movement.
    /// </summary>
    void Spawn()
    {
        speed = _startingSpeed;
        transform.position = Vector2.zero;

        rb2d.velocity = speed * 
            UnityEngine.Random.Range(1, 5) switch
        {
            1 => upLeft,
            2 => upRight,
            3 => downLeft,
            _ => downRight
        };

        _hits = 0;
    }

    void Update()
    {
        // Add score, wait for the ball to reach the offset before respawning
        // P2 scored
        if (paddleP1.transform.position.x - respawnOffset > transform.position.x)
        {
            _gameManager.AddScore(false);
            Spawn();
        } else
        // P1 scored
        if (paddleP2.transform.position.x + respawnOffset < transform.position.x)
        {
            _gameManager.AddScore(true);
            Spawn();
        }

        // Make sure ball keeps moving horizontally
        if (Mathf.Abs(rb2d.velocity.x) < _startingSpeed) 
        {
            rb2d.velocity = new(
                _startingSpeed * Math.Sign(rb2d.velocity.x),
                rb2d.velocity.y
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case Global.Layers.Paddle:
                _lastPaddle = collision.gameObject;
                rb2d.velocity *= speedMultiplier;

                // Just like in Mario & Luigi!
                _gameManager.PlayRank(
                    Mathf.Clamp(++_hits / 2, 1, _gameManager.rankAudios.Length) - 1
                );
                break;
        
            default: break;
        }
    }

    /// <summary>
    ///  m. Añade cualquier otra idea que se te ocurra
    ///  Encoge al otro jugador
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Shrinker")) return;

        var target = _lastPaddle == paddleP1 ? paddleP2 : paddleP1;

        // Attach Shrinker only when that player hasn't been shrinked already
        if (!target.TryGetComponent<Shrinker>(out _))
            target.AddComponent<Shrinker>();

        Destroy(other.gameObject);
    }

    // Lo quito para dejarlo mejor cómo lo dijiste tú. Por ahora.
#if false
    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.layer)
        {
            case Global.Layers.Goal:
                bool goal1 = collider.gameObject.name.Contains("1");

                // Redirect left or right; OnCollisionExit2D
                GetComponent<Rigidbody2D>().velocity =
                    _exitSpeed * (goal1 ? Vector2.left : Vector2.right);

                _gameManager.AddScore(goal1);
                break;

            default: break;
        }    
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        switch (collider.gameObject.layer)
        {
            case Global.Layers.Goal:
                Spawn();
                break;

            default: break;
        }
    }
#endif
}
