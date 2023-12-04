using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager 
    : GRP.Unity.MonoSingle<GameManager>
{
    uint _scoreP1, _scoreP2;

    public uint ScoreP1
    {
        get => _scoreP1;
        set
        {
            _scoreP1 = value;
            scoreTextP1.text = _scoreP1.ToString();
        }
    }

    public uint ScoreP2
    {
        get => _scoreP2;
        set
        {
            _scoreP2 = value;
            scoreTextP2.text = _scoreP2.ToString();
        }
    }

    [SerializeField]
    TMP_Text scoreTextP1, scoreTextP2;

    [SerializeField] public AudioClip[] rankAudios;
    AudioSource _audioSource;

    [SerializeField] GameObject _shrinkerPrefab;
    [SerializeField] Transform _prefabsParent;
    float _shrinkSpawnTimer;
    const float _spawnEach = 1f;

    void Start()
    {
        ScoreP1 = 0;
        ScoreP2 = 0;
        _shrinkSpawnTimer = Time.time;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
    }

    /// <summary>
    ///  m. Añade cualquier otra idea que se te ocurra
    ///  Spawnea los Shrinker
    /// </summary>
    void Update()
    {
        if (Time.time - _shrinkSpawnTimer < _spawnEach
        || _prefabsParent.childCount != 0)
            return;

        _shrinkSpawnTimer = Time.time;

        Func<float, float, float> ensureUnit = (min, max) =>
        {
            float n = UnityEngine.Random.Range(min, max);
            
            if (n > -1f && n < 1f)
                n = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;

            return n;
        };

        float x = ensureUnit(-4f, 4f),
              y = ensureUnit(-3f, 3f);
            
        Instantiate(
            original: _shrinkerPrefab,
            parent:   _prefabsParent,
            position: new(x, y, 0),
            rotation: Quaternion.identity
        );
    }

    public void AddScore(bool p1)
    {
        if (p1) ScoreP1 += 1;
        else    ScoreP2 += 1;
    }

    /// <summary>
    ///  m. Añade cualquier otra idea que se te ocurra
    ///  Sistema de pases. Cuanto más se pase sin marcar un gol, más rango tiene el golpe,
    ///  y va sonando distinto.
    /// </summary>
    /// <param name="i"></param>
    public void PlayRank(int i)
    {
        Debug.Log($"[HIT INDEX]: {i}");
        _audioSource.Stop();
        _audioSource.clip = rankAudios[i];
        _audioSource.Play();
    }
}
