using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodLog : MonoBehaviour
{
    // degrees Celsius
    // flash point = 300
    // autoignition point = 400
    // combustion = 400 - 700
    [Range(250, 300)] [SerializeField] private float _flashPointTemp = 300;
    [Range(301, 400)] [SerializeField] private float _ignitionTemp = 301;
    [Range(401, 700)] [SerializeField] private float _combustionTemp = 401;

    [Range(30, 700)] [SerializeField] private float m_StartingTemperature = 300;

    [Range(10_000, 100_000)] [SerializeField]
    private float m_TotalEnergy = 10_000;

    [SerializeField] private float m_BurnRate;

    [SerializeField] private Color m_Color = Color.yellow;
    [SerializeField] private Color m_BurntColor = Color.black;

    private float _currentTemperature;
    private List<WoodCube> _sections;

    private void Awake()
    {
        _sections = GetComponentsInChildren<WoodCube>().ToList();
    }

    void Start()
    {
        for (var i = 0; i < _sections.Count; i++)
        {
            var section = _sections[i];
            section.OnDisintegration += RemoveCubeFromLog;
            section.InitTemperature(m_StartingTemperature);
            section.IsCombusted = false;
            section.Energy = m_TotalEnergy / _sections.Count;
            section.BurnRate = m_BurnRate;

            section.FlashPointTemp = _flashPointTemp;
            section.IgnitionTemp = _ignitionTemp;
            section.CombustionTemp = _combustionTemp;

            var leftIndex = i - 1;
            if (leftIndex >= 0 && leftIndex < _sections.Count)
            {
                section.SetLeftNeighbor(_sections[leftIndex]);
                section.LeftJoint.connectedBody = _sections[leftIndex].Rigidbody;
            }
            else
            {
                section.DestroyLeftJoint();
            }

            var rightIndex = i + 1;
            if (rightIndex >= 0 && rightIndex < _sections.Count)
            {
                section.SetRightNeighbor(_sections[rightIndex]);
                section.RightJoint.connectedBody = _sections[rightIndex].Rigidbody;
            }
            else
            {
                section.DestroyRightJoint();
            }
        }

        // light first chunk
        _sections[0].IsCombusted = true;
    }


    private void RemoveCubeFromLog(WoodCube destroyedCube)
    {
        destroyedCube.OnDisintegration -= RemoveCubeFromLog;
        _sections.Remove(destroyedCube);
    }
}