using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class QuickTimeBinding
{
    [SerializeField] private InputActionReference m_bindingInputActionReference = null;
    
    public InputActionReference BindingInputActionReference 
    { 
        get { return m_bindingInputActionReference; }
    }
}
