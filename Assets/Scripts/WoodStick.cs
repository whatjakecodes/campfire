using UnityEngine;

public class WoodStick : MonoBehaviour
{
    // degrees Celsius
    // flash point = 300
    // ignition point = 400
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
    private WoodStickSection[] _sections;

    private void Awake()
    {
        _sections = GetComponentsInChildren<WoodStickSection>();
    }

    void Start()
    {
        for (var i = 0; i < _sections.Length; i++)
        {
            var section = _sections[i];
            section.InitTemperature(m_StartingTemperature);
            section.IsCombusted = false;
            section.Energy = m_TotalEnergy / _sections.Length;
            section.BurnRate = m_BurnRate;

            section.FlashPointTemp = _flashPointTemp;
            section.IgnitionTemp = _ignitionTemp;
            section.CombustionTemp = _combustionTemp;

            var leftIndex = i - 1;
            if (leftIndex >= 0 && leftIndex < _sections.Length)
            {
                section.SetLeftNeighbor(_sections[leftIndex]);
            }

            var rightIndex = i + 1;
            if (rightIndex >= 0 && rightIndex < _sections.Length)
            {
                section.SetRightNeighbor(_sections[rightIndex]);
            }
        }

        // light first chunk
        _sections[0].IsCombusted = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}