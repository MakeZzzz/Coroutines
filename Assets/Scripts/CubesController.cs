using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubesController : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] private GameObject _startposition;
    [SerializeField] private float _numberOfCubes;
    [SerializeField] private float _cubesLine;
    [SerializeField] private float _stepBetweenCubes;
    [SerializeField] private float _timeBetweenAppearances;
    [SerializeField] private float _colorChangeInterval;
    [SerializeField] private float _colorChangeTime;
    
    private Color[] _colors = { Color.black, Color.clear, Color.gray, Color.red, Color.yellow, Color.green,Color.magenta, Color.cyan};
    private List<GameObject> _cubes = new List<GameObject>();
    
    void Start()
    {
        StartCoroutine(StartCubesSpawn());
    }

    private IEnumerator StartCubesSpawn()
    {
        var startPosition = _startposition.transform.position;
        for (int i = 0; i < _numberOfCubes/_cubesLine; i++)
        {
            for (int k = 0; k < _cubesLine; k++)
            {
                var cube = Instantiate(_cube);
                cube.transform.position = startPosition;
                startPosition.z -= _stepBetweenCubes;
                _cubes.Add(cube);
                yield return new WaitForSeconds(_timeBetweenAppearances);
            }
            startPosition.x -= _stepBetweenCubes;
            startPosition.z = _startposition.transform.position.z;
        }
    }
    private IEnumerator CubesColorChange()
    {
        var color = Random.Range(0, _colors.Length-1);
        for (int i = 0; i < _cubes.Count - 1; i++)
        {
            var currentTime = 0f;
            var cubeColor = _cubes[i].GetComponent<Renderer>().material.color;
            while (currentTime <= _colorChangeTime)
            {
                _cubes[i].GetComponent<Renderer>().material.color = Color.Lerp(cubeColor, _colors[color],  currentTime/_colorChangeTime);
                currentTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(_colorChangeInterval);
        }
    }
    [UsedImplicitly]
    public void StartChangeColor()
    {
        StartCoroutine(CubesColorChange());
    }
}
