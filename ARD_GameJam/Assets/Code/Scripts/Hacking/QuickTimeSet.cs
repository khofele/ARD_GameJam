using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuickTimeSet", menuName = "QTE/Quick Time Set")]
public class QuickTimeSet : ScriptableObject
{
    [SerializeField] private int m_minAmountChosenBindings = 1;
    [SerializeField] private int m_maxAmountChosenBindings = 5;

    [SerializeField] private List<QuickTimeBinding> m_bindings = new List<QuickTimeBinding>();

    public int MinAmountChosenBindings
    {
        get { return m_minAmountChosenBindings; }
    }

    public int MaxAmountChosenBindings
    {
        get { return m_maxAmountChosenBindings; }
    }

    public List<QuickTimeBinding> Bindings
    {
        get { return m_bindings; }
    }
}
