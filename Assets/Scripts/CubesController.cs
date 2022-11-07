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
        var color = Random.ColorHSV();
        for (int i = 0; i < _cubes.Count - 1; i++)
        {
            var cubeColor = _cubes[i].GetComponent<Renderer>();
            StartCoroutine(CubeColorChange(cubeColor, _colorChangeTime, color));
            yield return new WaitForSeconds(_colorChangeInterval);
        }
    }
    
    private IEnumerator CubeColorChange(Renderer cubeRenderer, float colorChangeTime, Color finalColor)
    {
        var startColor = cubeRenderer.material.color;
        var currentTime = 0f;
        while (currentTime < colorChangeTime)
        {
            var newColor = Color.Lerp(startColor, finalColor,  currentTime/colorChangeTime);
            cubeRenderer.material.color = newColor;
            currentTime += Time.deltaTime;
            yield return null;
        }
        cubeRenderer.material.color = finalColor;
    }
    
    [UsedImplicitly]
    public void StartChangeColor()
    {
        StartCoroutine(CubesColorChange());
    }
}
