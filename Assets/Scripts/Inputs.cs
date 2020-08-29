// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Hider"",
            ""id"": ""3b8fc6e0-996e-41c0-b54c-6573ebb4dddc"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""4ca3732d-3f2f-43b5-a43d-1e6b6bba5903"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Speak"",
                    ""type"": ""Button"",
                    ""id"": ""3a5960c3-6892-4cbd-a0f7-02feeb038add"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""fd1d6dbb-fa42-4c49-a284-d4c0a86266bb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""47050aa1-8678-4227-90db-402a3e2ed403"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""61eddc2c-bd38-4da4-9174-c172d7fe6c05"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""414c0868-d9ce-41db-b24b-7b335045afd9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5befa9a2-1397-415b-a2ee-70648df5309f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""da268d14-7ea9-445f-a097-16e7473074c3"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Speak"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Hider
        m_Hider = asset.FindActionMap("Hider", throwIfNotFound: true);
        m_Hider_Movement = m_Hider.FindAction("Movement", throwIfNotFound: true);
        m_Hider_Speak = m_Hider.FindAction("Speak", throwIfNotFound: true);
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

    // Hider
    private readonly InputActionMap m_Hider;
    private IHiderActions m_HiderActionsCallbackInterface;
    private readonly InputAction m_Hider_Movement;
    private readonly InputAction m_Hider_Speak;
    public struct HiderActions
    {
        private @Inputs m_Wrapper;
        public HiderActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Hider_Movement;
        public InputAction @Speak => m_Wrapper.m_Hider_Speak;
        public InputActionMap Get() { return m_Wrapper.m_Hider; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HiderActions set) { return set.Get(); }
        public void SetCallbacks(IHiderActions instance)
        {
            if (m_Wrapper.m_HiderActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_HiderActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_HiderActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_HiderActionsCallbackInterface.OnMovement;
                @Speak.started -= m_Wrapper.m_HiderActionsCallbackInterface.OnSpeak;
                @Speak.performed -= m_Wrapper.m_HiderActionsCallbackInterface.OnSpeak;
                @Speak.canceled -= m_Wrapper.m_HiderActionsCallbackInterface.OnSpeak;
            }
            m_Wrapper.m_HiderActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Speak.started += instance.OnSpeak;
                @Speak.performed += instance.OnSpeak;
                @Speak.canceled += instance.OnSpeak;
            }
        }
    }
    public HiderActions @Hider => new HiderActions(this);
    public interface IHiderActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnSpeak(InputAction.CallbackContext context);
    }
}
