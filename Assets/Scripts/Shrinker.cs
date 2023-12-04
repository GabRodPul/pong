using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// m. Añade cualquier otra idea que se te ocurra
/// Power-Up que guarda la escala Y original de un objeto
/// y lo encoge por _target segundos.
/// </summary>
public class Shrinker : MonoBehaviour
{
    float   _target = 3f;
    float   _start, _yOgScale;

    void Start()
    {
        _start = Time.time;

        _yOgScale = gameObject.transform.localScale.y;
        float newY = _yOgScale / 2;
        
        gameObject.transform.localScale = new(
            gameObject.transform.localScale.x,
            newY,
            gameObject.transform.localScale.z
        );

        // Center object
        gameObject.transform.position += new Vector3(0, newY/2, 0);
    }

    void Update()
    {
        // Reset and detach when timer reached
        if ( Time.time-_start > _target )
        {
            gameObject.transform.localScale = new(
                gameObject.transform.localScale.x,
                _yOgScale,
                gameObject.transform.localScale.z
            );

            Destroy(this);
            Debug.Log(_start);
        }
    }
}
