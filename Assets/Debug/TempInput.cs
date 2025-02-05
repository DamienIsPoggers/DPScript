//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Debug/TempInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TempInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TempInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TempInput"",
    ""maps"": [
        {
            ""name"": ""D"",
            ""id"": ""e8e8b08a-4532-43ed-82fd-cdb6d0a542c9"",
            ""actions"": [
                {
                    ""name"": ""D"",
                    ""type"": ""Button"",
                    ""id"": ""299a6c4c-fa58-4b31-9f94-0092b4bd4d50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b258d502-9c27-4e05-9f9c-291318cfc084"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // D
        m_D = asset.FindActionMap("D", throwIfNotFound: true);
        m_D_D = m_D.FindAction("D", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // D
    private readonly InputActionMap m_D;
    private List<IDActions> m_DActionsCallbackInterfaces = new List<IDActions>();
    private readonly InputAction m_D_D;
    public struct DActions
    {
        private @TempInput m_Wrapper;
        public DActions(@TempInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @D => m_Wrapper.m_D_D;
        public InputActionMap Get() { return m_Wrapper.m_D; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DActions set) { return set.Get(); }
        public void AddCallbacks(IDActions instance)
        {
            if (instance == null || m_Wrapper.m_DActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DActionsCallbackInterfaces.Add(instance);
            @D.started += instance.OnD;
            @D.performed += instance.OnD;
            @D.canceled += instance.OnD;
        }

        private void UnregisterCallbacks(IDActions instance)
        {
            @D.started -= instance.OnD;
            @D.performed -= instance.OnD;
            @D.canceled -= instance.OnD;
        }

        public void RemoveCallbacks(IDActions instance)
        {
            if (m_Wrapper.m_DActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDActions instance)
        {
            foreach (var item in m_Wrapper.m_DActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DActions @D => new DActions(this);
    public interface IDActions
    {
        void OnD(InputAction.CallbackContext context);
    }
}
