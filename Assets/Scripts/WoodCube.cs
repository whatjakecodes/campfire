using System;
using UnityEditor;
using UnityEngine;

public class WoodCube : MonoBehaviour
{
    [Range(10, 1000)] [SerializeField] private float m_WaitTimeToChangeTemp = 10;
    [SerializeField] private FixedJoint m_LeftJoint;
    [SerializeField] private FixedJoint m_RightJoint;


    public float Temperature => _temperature;

    public bool IsCombusted { get; set; }
    public float Energy { get; set; }
    public float BurnRate { get; set; }
    public float FlashPointTemp { get; set; }
    public float IgnitionTemp { get; set; }
    public float CombustionTemp { get; set; }


    public WoodCube LeftNeighbor => _leftNeighbor;

    public WoodCube RightNeighbor => _rightNeighbor;

    public Joint LeftJoint => m_LeftJoint;
    public Joint RightJoint => m_RightJoint;
    public Rigidbody Rigidbody => _rigidBody;

    public event Action<WoodCube> OnDisintegration;
    public event Action<WoodCube> OnTemperatureChange;

    private WoodCube _leftNeighbor;
    private WoodCube _rightNeighbor;

    private float _flashPointTemp = 300;
    private float _ignitionTemp = 301;
    private float _combustionTemp = 401;
    private float _maxTemp = 700;

    private float _previousTemp;
    private float _targetTemp;
    private float _elapsedTime;
    private float _temperature;
    private bool shouldDisintegrate;
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
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

    private void FixedUpdate()
    {
        if (shouldDisintegrate)
        {
            shouldDisintegrate = false;
            OnDisintegration?.Invoke(this);

            if (LeftNeighbor && LeftNeighbor.LeftJoint && LeftNeighbor.LeftJoint.connectedBody == Rigidbody)
            {
                LeftNeighbor.DestroyRightJoint();
            }

            if (RightNeighbor && RightNeighbor.LeftJoint && RightNeighbor.LeftJoint.connectedBody == Rigidbody)
            {
                RightNeighbor.DestroyLeftJoint();
            }

            Destroy(gameObject);
        }
    }

    public void DestroyRightJoint()
    {
        Destroy(m_RightJoint);
        m_RightJoint = null;
    }
    
    public void DestroyLeftJoint()
    {
        Destroy(m_LeftJoint);
        m_LeftJoint = null;
    }

    public void QueuePhysicsDisintegration()
    {
        shouldDisintegrate = true;
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

    private void HandleNeighborTemperatureChange(WoodCube section)
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

    public void SetRightNeighbor(WoodCube value)
    {
        if (_rightNeighbor)
        {
            _rightNeighbor.OnTemperatureChange -= HandleNeighborTemperatureChange;
        }

        _rightNeighbor = value;
        _rightNeighbor.OnTemperatureChange += HandleNeighborTemperatureChange;
    }

    public void SetLeftNeighbor(WoodCube value)
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