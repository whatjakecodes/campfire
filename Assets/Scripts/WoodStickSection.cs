using System;
using UnityEditor;
using UnityEngine;

public class WoodStickSection : MonoBehaviour
{
    [Range(10, 1000)] [SerializeField] private float m_WaitTimeToChangeTemp = 10;

    public float Temperature => _temperature;

    public bool IsCombusted { get; set; }
    public float Energy { get; set; }
    public float BurnRate { get; set; }
    public float FlashPointTemp { get; set; }
    public float IgnitionTemp { get; set; }
    public float CombustionTemp { get; set; }


    public WoodStickSection LeftNeighbor => _leftNeighbor;
    public WoodStickSection RightNeighbor => _rightNeighbor;

    public event Action<WoodStickSection> OnTemperatureChange;

    private WoodStickSection _leftNeighbor;
    private WoodStickSection _rightNeighbor;

    private float _flashPointTemp = 300;
    private float _ignitionTemp = 301;
    private float _combustionTemp = 401;
    private float _maxTemp = 700;

    private float _previousTemp;
    private float _targetTemp;
    private float _elapsedTime;
    private float _temperature;

    void Update()
    {
        if (IsCombusted)
        {
            UpdateCombustion();
        }
        else if (_targetTemp > 0)
        {
            UpdateNonCombustion();
        }

        _elapsedTime += Time.deltaTime;
    }

    public void InitTemperature(float initialTemperature)
    {
        _previousTemp = initialTemperature;
        _temperature = initialTemperature;
        _targetTemp = initialTemperature;
    }

    private void UpdateNonCombustion()
    {
        var nextTemp = Mathf.Lerp(_previousTemp, _targetTemp, _elapsedTime / m_WaitTimeToChangeTemp);

        _temperature = nextTemp;

        if (_temperature > _flashPointTemp)
        {
            // show vapor
        }

        if (_temperature > _ignitionTemp)
        {
            IsCombusted = true;
            _targetTemp = _maxTemp;
        }
    }

    private void UpdateCombustion()
    {
    }

    private void HandleNeighborTemperatureChange(WoodStickSection section)
    {
        var potentialTargetTemp = 0f;

        if (_leftNeighbor)
        {
            potentialTargetTemp = _leftNeighbor.Temperature;
        }

        if (_rightNeighbor)
        {
            potentialTargetTemp += _rightNeighbor.Temperature;
        }

        if (potentialTargetTemp > _temperature)
        {
            _targetTemp = potentialTargetTemp;
            _elapsedTime = 0;
        }
    }

    public void SetRightNeighbor(WoodStickSection value)
    {
        if (_rightNeighbor)
        {
            _rightNeighbor.OnTemperatureChange -= HandleNeighborTemperatureChange;
        }

        _rightNeighbor = value;
        _rightNeighbor.OnTemperatureChange += HandleNeighborTemperatureChange;
    }

    public void SetLeftNeighbor(WoodStickSection value)
    {
        if (_leftNeighbor)
        {
            _leftNeighbor.OnTemperatureChange -= HandleNeighborTemperatureChange;
        }

        _leftNeighbor = value;
        _leftNeighbor.OnTemperatureChange += HandleNeighborTemperatureChange;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Gizmos.draw
        var guiStyle = new GUIStyle
        {
            fontSize = 10
        };

        Handles.Label(transform.position, $"{(int)_temperature}c", guiStyle);
    }
#endif
}