// GENERATED AUTOMATICALLY FROM 'Assets/EnvironmentSettings/PlayerInput/ApologuePlayerInput_Actions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ApologuePlayerInput_Actions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ApologuePlayerInput_Actions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ApologuePlayerInput_Actions"",
    ""maps"": [
        {
            ""name"": ""PlayerSword"",
            ""id"": ""9efd54f0-1b44-4ca7-9f94-03fe840281cb"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""dd45aca9-dcbf-416a-842f-a64bd752367c"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""ed689dca-fb89-4070-aa8c-c19afacaca25"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""93254501-c3d7-4ffd-9f83-c185690f0664"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""DoubleJump"",
                    ""type"": ""Button"",
                    ""id"": ""93f4d19c-4695-40be-9b06-8ad0aaa8efda"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ArrowKeysMovement"",
                    ""type"": ""Value"",
                    ""id"": ""b0b788bc-774c-415f-8d99-daf56626ad7d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""f6b52529-fb49-4e10-a1a3-eb56379a087a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slide"",
                    ""type"": ""Button"",
                    ""id"": ""9d964a69-039b-4573-a7b4-2660749e1bbb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwordLightAttack1"",
                    ""type"": ""Button"",
                    ""id"": ""ef4d892d-eec5-4c1c-baee-601dafbd0144"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""SwordLightAttack2"",
                    ""type"": ""Button"",
                    ""id"": ""ac3b5176-f9ad-4457-8bba-b908371a6bcb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.2)""
                },
                {
                    ""name"": ""SwordLightAttack3"",
                    ""type"": ""Button"",
                    ""id"": ""955b36eb-5eb6-43bb-b5f4-db8244cd416f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwordMediumAttack1"",
                    ""type"": ""Button"",
                    ""id"": ""afdd4df5-cf88-48d1-bbdb-0e242500567c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwordMediumAttack2"",
                    ""type"": ""Button"",
                    ""id"": ""3b08ee2b-e3e8-4875-9810-16d6ab57b80d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwordHeavyAttack1"",
                    ""type"": ""Button"",
                    ""id"": ""f146e3c3-18b9-4386-960d-f139e6cba0c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwordHeavyAttack2"",
                    ""type"": ""Button"",
                    ""id"": ""970987a5-ce4f-448c-a21b-d3635e5e0477"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Parry"",
                    ""type"": ""Button"",
                    ""id"": ""b3d4f02b-27cf-41ab-8b08-e5bd07aca939"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""c601e726-fa8e-44d3-9f41-de718346d404"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""add300a1-1007-4ad0-8a33-c284c52a1ed5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeStance"",
                    ""type"": ""Button"",
                    ""id"": ""d6f82a61-5dbb-4bce-a452-824fef1b83f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpawnBox"",
                    ""type"": ""Button"",
                    ""id"": ""d6eb7056-ec67-43e7-b642-f5d522736d20"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e1733bec-3511-4644-b32e-a773563f3023"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3578041a-b68a-4e5d-851a-7137ba576818"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a079324e-2391-4e7d-b9a7-109a896d83ad"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""DoubleJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""4b08e400-8a65-4a90-813e-5d60baee1f9a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKeysMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""685c739c-900b-4c83-837d-4effc520743a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ArrowKeysMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""50e3ec7a-94ad-492a-9ad8-02dacf3c905a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ArrowKeysMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""40d3d53b-d58f-411b-847f-9030e891bfbb"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""33c2fcc0-504a-403b-a0fc-923d018f501e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b1ab90b5-ec55-43c6-907c-0fa92bfdd043"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5bfca69a-d0bc-4af2-bd7b-7c94887d2e15"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""SwordLightAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""d206bd45-0d38-4991-8469-5955b44071f2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bf1bb3bd-9747-4d3c-8c2d-be79540807a9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""22fd10f5-3a32-4937-a8bd-4bed93448c28"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b4290461-9ac0-40e1-9a0c-caa91bf65b2d"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""0930cb13-338e-48da-8c91-641b0fecfdee"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""fc70bc25-f7a5-4d25-ba37-16f7493e06b1"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4659b4a8-c3d8-467b-aa3e-15483a76b891"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1585635-7e85-4449-ad6a-50c85bfa0c41"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b115e7b4-f478-4114-b230-6b463073be21"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2dad380a-4a64-4b30-a416-34a497624e4f"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51368483-62da-4237-9bd8-df172445961d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82f1b9a4-e0b4-4722-938a-4eb63ba280bf"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SwordLightAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b989db4f-4c27-47b0-b0ab-267bf6402858"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19675dd3-22ad-4461-8616-7958996fa75f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DoubleJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59fbd1c9-50d1-4cfb-b59d-1924c2da07cb"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6da8e162-c692-46d5-84d3-d3759490153b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""a7c30f7f-eea0-4ba7-be27-724adf9c1942"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""200d20aa-15be-425f-b4d3-16cb5ae90e27"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dbceb039-b719-4388-9ba3-8c45c91bf1ba"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5de8491d-6776-4371-9f0c-1bcefdea17e3"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch;Keyboard&Mouse"",
                    ""action"": ""SwordLightAttack3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3414c92e-b3ff-44dd-8ea3-d016bc297c0a"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""SwordLightAttack3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bbf7b43-b02e-46e3-910d-2336e01d37d2"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""SwordMediumAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""692d0d41-5965-4d23-87ab-fc4daf08ec3f"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwordMediumAttack1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""5c8adcd3-0c92-49e3-9c15-94f7b970a9ea"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""SwordMediumAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""3de34b7a-ef6d-4a53-aded-f2080a6699d5"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""SwordMediumAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6e9d59c2-10dd-49b3-9c8c-6559474c3d1e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""SwordHeavyAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7358fec-365f-41b7-9ea8-c08a52aaccec"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Gamepad"",
                    ""action"": ""SwordHeavyAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""025ae365-8e3d-4e39-a644-b705cf134ea6"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch;Keyboard&Mouse"",
                    ""action"": ""SwordHeavyAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cbc5c89b-bb6f-4128-a9dd-e56aa90ee082"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""SwordLightAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""086b4341-8b1b-4ab4-840b-f6ab62f18db1"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""SwordLightAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4e2b6e7-38e4-47b0-bc7d-afb14d54beb7"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""ChangeStance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4691acd0-7345-4bb6-8190-ea0be572bba7"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Gamepad"",
                    ""action"": ""ChangeStance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bc2de8a-c92e-40e4-80f2-e5779abe1fa6"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Gamepad"",
                    ""action"": ""SwordHeavyAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd6c7594-c6cb-4fa7-a5ff-58574696c8b0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""SwordMediumAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81e9aeec-0375-43b4-a8d0-6ea92dde15c8"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""SwordMediumAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""257a4f3f-9cc0-4ee7-9063-ddb927369d62"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Gamepad;Joystick;Touch"",
                    ""action"": ""SpawnBox"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerAxe"",
            ""id"": ""2f7abf69-ac54-4265-9aed-1f47de51fc74"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a32b8ea6-f0e8-4d7e-8cdd-02a118c9634b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""fcc7cc3f-8987-4291-bf72-7a677b578306"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""d0be4aff-2cef-44c5-b765-9200d316a65c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""DoubleJump"",
                    ""type"": ""Button"",
                    ""id"": ""d8e625a7-2729-426c-9c02-b9c8bdef4c48"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ArrowKeysMovement"",
                    ""type"": ""Value"",
                    ""id"": ""92c46a66-1a74-4331-a347-5b8d4cad7af4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""8b4c4076-5075-46c7-8b2e-2aa87d50f5d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slide"",
                    ""type"": ""Button"",
                    ""id"": ""749a0acc-7c73-4e2f-8e3d-3633fe745298"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AxeLightAttack1"",
                    ""type"": ""Button"",
                    ""id"": ""bfd626f9-8e02-45a7-9a9d-d1537870fead"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""AxeLightAttack2"",
                    ""type"": ""Button"",
                    ""id"": ""5882b2f5-2b61-49ef-9ef9-18c5bb5891d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.2)""
                },
                {
                    ""name"": ""AxeLightAttack3"",
                    ""type"": ""Button"",
                    ""id"": ""553c1807-016b-4bd0-a159-3f34911a9c1d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.2)""
                },
                {
                    ""name"": ""AxeMediumAttack1"",
                    ""type"": ""Button"",
                    ""id"": ""a54bdfff-f83e-4d4c-8c85-8af804bed289"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AxeMediumAttack2"",
                    ""type"": ""Button"",
                    ""id"": ""6a39e79a-6976-47b4-acf3-f2ba2e456bdb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AxeHeavyAttack1"",
                    ""type"": ""Button"",
                    ""id"": ""ff381f7d-7700-40c0-b209-cfad80dccb84"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""AxeHeavyAttack2"",
                    ""type"": ""Button"",
                    ""id"": ""09de757d-a59a-4b98-a12f-15fad79bf279"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Parry"",
                    ""type"": ""Button"",
                    ""id"": ""bbf6a93f-056a-49e7-be06-fd7e096d9860"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""b7e946ab-2e71-4f8c-82eb-c0657f64cdd6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""1ce52841-08c6-4b29-a8ec-05b93660b27d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeStance"",
                    ""type"": ""Button"",
                    ""id"": ""5f108d97-9aeb-4e3e-a36c-ac43af9d5095"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpawnBox"",
                    ""type"": ""Button"",
                    ""id"": ""1fb5b9a2-f76c-4dfe-bf0d-be4a87f7a701"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""383f9304-6ec5-430b-8c00-010ee9761769"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""942e2d19-52b1-443c-87fa-3ec59ecfd401"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48b34af7-c5f4-4ad7-828c-4702149420ab"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""DoubleJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""d2d4c22e-f4de-4539-b7c3-07444fe23c8c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArrowKeysMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3eb86231-8239-4c5d-9065-52d5dc6ee580"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ArrowKeysMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9c2b6132-c1ff-4f1e-82ce-c464cdce2ab9"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ArrowKeysMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d99f05e6-0a24-44f5-9d84-dc973b365b4f"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""86aaec35-8713-4dc3-866a-e92f1868d204"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""54f173d7-f373-4ea3-a03f-ce51fa2b089c"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""6659573f-8e15-407d-b15e-345c3389bd46"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""80b94291-9f6e-4396-854b-64cae6dd7dc9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""397ce5b6-a767-4eed-ba27-ab08c0549f54"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d48d0b5e-5736-4d3e-bdcd-f56121c41bd1"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""055ee06d-ab5d-4a86-91c6-3e316ef7fb49"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""04437947-1272-47b8-b4cc-4810cc525835"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""35cdba5a-aac2-4ff9-af58-387d2539ee91"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba910931-da5e-4048-a140-375fd15aed9c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2fc756ac-559f-4a33-821a-4ac7a42ba5f9"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b442067-37c8-43b9-beff-c3b040bada26"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6e7383f-159f-484b-8d1e-b155ce1d23e2"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f95e166b-c200-4c2a-81b0-4bcd5687cda0"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc013bb5-b43c-4c1a-97b4-1b0e0f704ba9"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DoubleJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d98b36a7-2326-4bfb-87c7-4eb6fe102aa3"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9e600c3-0f12-4cad-a8a9-3cd6be8327e9"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""8e5c71b0-cd4b-46a0-a7a6-7c826d9e2a4b"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""39f0e775-4d6e-4572-8f50-f66226fa3800"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d4e95b02-bf15-40d4-81d5-d75a0d5d7027"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""811a0a13-00f0-4020-aab9-afff8a9f2424"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch;Keyboard&Mouse"",
                    ""action"": ""AxeHeavyAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e257af42-6698-4458-bbaf-f989fc8b47d4"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""AxeHeavyAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bcd036ce-4ad0-4521-8acf-cb60c587059d"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""AxeLightAttack3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""227dd359-f9d2-4c2a-9a03-84f47a942b38"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""AxeLightAttack3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbebf523-2ceb-4ca1-9506-14bff60e445f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch;Keyboard&Mouse"",
                    ""action"": ""AxeLightAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fadd814-e133-4702-b8c7-6b2c012a81e1"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""AxeMediumAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""dd6d7363-5d83-49a4-be14-c32c0ca0de20"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AxeMediumAttack1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""f0d1e50f-8f82-47ba-8887-3b0dbd0d0bd8"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""AxeMediumAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""a424c4b0-e96f-49e6-964f-9be31cd9273f"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""AxeMediumAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e1a6bcc0-58d7-481a-9568-cad5e0c28848"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""AxeHeavyAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93f84f23-830a-43a9-b77e-197c0ef03154"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""AxeHeavyAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e40f759-d8cb-4b66-999b-cf207c82a050"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""AxeLightAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80475bd9-14db-4d3d-a5f2-0329260a7b2e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""AxeMediumAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3f2c7fd-7ccc-4351-af7b-0ccccc3fd68c"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AxeMediumAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14324afb-d771-4c25-b5ff-6248ca0d5caf"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""ChangeStance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ce05418-353c-4dc5-b7ad-ea1433de8094"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""ChangeStance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cc0fe48-f817-4a1a-a38c-b5c3b1bc6f18"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AxeLightAttack2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fb4d69f-1905-4597-b942-a20c04911012"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AxeLightAttack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cda3c51-4667-4de6-b501-c14817b1fd35"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Gamepad;Joystick;Touch"",
                    ""action"": ""SpawnBox"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""0959e841-d23d-47fb-83c9-d7027644d8a4"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""278aeb76-f9db-4342-9333-7b8090cc9168"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""f2689c45-1bde-4b24-b48d-5ee6da27b843"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""f374ebff-0f86-41fa-ae44-5d1b0625a4a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""43639f39-ee52-41a4-883e-d14507f902bf"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6904ecb5-8f7e-4e72-8a9e-772e7689de50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a20139c7-b242-4f8a-9ce4-eeb83a129476"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b14a1174-011c-47c6-bee8-039924a1ca25"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9446c863-284d-411c-aa4c-c82839d71c15"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d929127d-36b4-4f91-b2f3-53fd6d912a66"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b18ba8f0-5d4c-4f34-b0ac-ede8bb36c3a9"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""b5ae94a2-a6b0-4a38-8d81-1449ad822d1c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""809f371f-c5e2-4e7a-83a1-d867598f40dd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""14a5d6e8-4aaf-4119-a9ef-34b8c2c548bf"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9144cbe6-05e1-4687-a6d7-24f99d23dd81"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2db08d65-c5fb-421b-983f-c71163608d67"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""58748904-2ea9-4a80-8579-b500e6a76df8"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8ba04515-75aa-45de-966d-393d9bbd1c14"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""712e721c-bdfb-4b23-a86c-a0d9fcfea921"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcd248ae-a788-4676-a12e-f4d81205600b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1f04d9bc-c50b-41a1-bfcc-afb75475ec20"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fb8277d4-c5cd-4663-9dc7-ee3f0b506d90"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""e25d9774-381c-4a61-b47c-7b6b299ad9f9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3db53b26-6601-41be-9887-63ac74e79d19"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0cb3e13e-3d90-4178-8ae6-d9c5501d653f"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0392d399-f6dd-4c82-8062-c1e9c0d34835"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""942a66d9-d42f-43d6-8d70-ecb4ba5363bc"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""ff527021-f211-4c02-933e-5976594c46ed"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""563fbfdd-0f09-408d-aa75-8642c4f08ef0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb480147-c587-4a33-85ed-eb0ab9942c43"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2bf42165-60bc-42ca-8072-8c13ab40239b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""85d264ad-e0a0-4565-b7ff-1a37edde51ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""74214943-c580-44e4-98eb-ad7eebe17902"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cea9b045-a000-445b-95b8-0c171af70a3b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8607c725-d935-4808-84b1-8354e29bab63"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4cda81dc-9edd-4e03-9d7c-a71a14345d0b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9e92bb26-7e3b-4ec4-b06b-3c8f8e498ddc"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82627dcc-3b13-4ba9-841d-e4b746d6553e"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c52c8e0b-8179-41d3-b8a1-d149033bbe86"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1394cbc-336e-44ce-9ea8-6007ed6193f7"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5693e57a-238a-46ed-b5ae-e64e6e574302"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4faf7dc9-b979-4210-aa8c-e808e1ef89f5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d66d5ba-88d7-48e6-b1cd-198bbfef7ace"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47c2a644-3ebc-4dae-a106-589b7ca75b59"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch;Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb9e6b34-44bf-4381-ac63-5aa15d19f677"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38c99815-14ea-4617-8627-164d27641299"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24066f69-da47-44f3-a07e-0015fb02eb2e"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c191405-5738-4d4b-a523-c6a301dbf754"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7236c0d9-6ca3-47cf-a6ee-a97f5b59ea77"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23e01e3a-f935-4948-8d8b-9bcac77714fb"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""398fd1f7-264a-4c0c-901d-dde16a994a11"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""PauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02f40af5-d055-4864-a3f5-e6b24e4b7ee7"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Gamepad"",
                    ""action"": ""PauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerSword
        m_PlayerSword = asset.FindActionMap("PlayerSword", throwIfNotFound: true);
        m_PlayerSword_Move = m_PlayerSword.FindAction("Move", throwIfNotFound: true);
        m_PlayerSword_Jump = m_PlayerSword.FindAction("Jump", throwIfNotFound: true);
        m_PlayerSword_Dash = m_PlayerSword.FindAction("Dash", throwIfNotFound: true);
        m_PlayerSword_DoubleJump = m_PlayerSword.FindAction("DoubleJump", throwIfNotFound: true);
        m_PlayerSword_ArrowKeysMovement = m_PlayerSword.FindAction("ArrowKeysMovement", throwIfNotFound: true);
        m_PlayerSword_Crouch = m_PlayerSword.FindAction("Crouch", throwIfNotFound: true);
        m_PlayerSword_Slide = m_PlayerSword.FindAction("Slide", throwIfNotFound: true);
        m_PlayerSword_SwordLightAttack1 = m_PlayerSword.FindAction("SwordLightAttack1", throwIfNotFound: true);
        m_PlayerSword_SwordLightAttack2 = m_PlayerSword.FindAction("SwordLightAttack2", throwIfNotFound: true);
        m_PlayerSword_SwordLightAttack3 = m_PlayerSword.FindAction("SwordLightAttack3", throwIfNotFound: true);
        m_PlayerSword_SwordMediumAttack1 = m_PlayerSword.FindAction("SwordMediumAttack1", throwIfNotFound: true);
        m_PlayerSword_SwordMediumAttack2 = m_PlayerSword.FindAction("SwordMediumAttack2", throwIfNotFound: true);
        m_PlayerSword_SwordHeavyAttack1 = m_PlayerSword.FindAction("SwordHeavyAttack1", throwIfNotFound: true);
        m_PlayerSword_SwordHeavyAttack2 = m_PlayerSword.FindAction("SwordHeavyAttack2", throwIfNotFound: true);
        m_PlayerSword_Parry = m_PlayerSword.FindAction("Parry", throwIfNotFound: true);
        m_PlayerSword_PauseGame = m_PlayerSword.FindAction("PauseGame", throwIfNotFound: true);
        m_PlayerSword_Interact = m_PlayerSword.FindAction("Interact", throwIfNotFound: true);
        m_PlayerSword_ChangeStance = m_PlayerSword.FindAction("ChangeStance", throwIfNotFound: true);
        m_PlayerSword_SpawnBox = m_PlayerSword.FindAction("SpawnBox", throwIfNotFound: true);
        // PlayerAxe
        m_PlayerAxe = asset.FindActionMap("PlayerAxe", throwIfNotFound: true);
        m_PlayerAxe_Move = m_PlayerAxe.FindAction("Move", throwIfNotFound: true);
        m_PlayerAxe_Jump = m_PlayerAxe.FindAction("Jump", throwIfNotFound: true);
        m_PlayerAxe_Dash = m_PlayerAxe.FindAction("Dash", throwIfNotFound: true);
        m_PlayerAxe_DoubleJump = m_PlayerAxe.FindAction("DoubleJump", throwIfNotFound: true);
        m_PlayerAxe_ArrowKeysMovement = m_PlayerAxe.FindAction("ArrowKeysMovement", throwIfNotFound: true);
        m_PlayerAxe_Crouch = m_PlayerAxe.FindAction("Crouch", throwIfNotFound: true);
        m_PlayerAxe_Slide = m_PlayerAxe.FindAction("Slide", throwIfNotFound: true);
        m_PlayerAxe_AxeLightAttack1 = m_PlayerAxe.FindAction("AxeLightAttack1", throwIfNotFound: true);
        m_PlayerAxe_AxeLightAttack2 = m_PlayerAxe.FindAction("AxeLightAttack2", throwIfNotFound: true);
        m_PlayerAxe_AxeLightAttack3 = m_PlayerAxe.FindAction("AxeLightAttack3", throwIfNotFound: true);
        m_PlayerAxe_AxeMediumAttack1 = m_PlayerAxe.FindAction("AxeMediumAttack1", throwIfNotFound: true);
        m_PlayerAxe_AxeMediumAttack2 = m_PlayerAxe.FindAction("AxeMediumAttack2", throwIfNotFound: true);
        m_PlayerAxe_AxeHeavyAttack1 = m_PlayerAxe.FindAction("AxeHeavyAttack1", throwIfNotFound: true);
        m_PlayerAxe_AxeHeavyAttack2 = m_PlayerAxe.FindAction("AxeHeavyAttack2", throwIfNotFound: true);
        m_PlayerAxe_Parry = m_PlayerAxe.FindAction("Parry", throwIfNotFound: true);
        m_PlayerAxe_PauseGame = m_PlayerAxe.FindAction("PauseGame", throwIfNotFound: true);
        m_PlayerAxe_Interact = m_PlayerAxe.FindAction("Interact", throwIfNotFound: true);
        m_PlayerAxe_ChangeStance = m_PlayerAxe.FindAction("ChangeStance", throwIfNotFound: true);
        m_PlayerAxe_SpawnBox = m_PlayerAxe.FindAction("SpawnBox", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
        m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        m_UI_PauseMenu = m_UI.FindAction("PauseMenu", throwIfNotFound: true);
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

    // PlayerSword
    private readonly InputActionMap m_PlayerSword;
    private IPlayerSwordActions m_PlayerSwordActionsCallbackInterface;
    private readonly InputAction m_PlayerSword_Move;
    private readonly InputAction m_PlayerSword_Jump;
    private readonly InputAction m_PlayerSword_Dash;
    private readonly InputAction m_PlayerSword_DoubleJump;
    private readonly InputAction m_PlayerSword_ArrowKeysMovement;
    private readonly InputAction m_PlayerSword_Crouch;
    private readonly InputAction m_PlayerSword_Slide;
    private readonly InputAction m_PlayerSword_SwordLightAttack1;
    private readonly InputAction m_PlayerSword_SwordLightAttack2;
    private readonly InputAction m_PlayerSword_SwordLightAttack3;
    private readonly InputAction m_PlayerSword_SwordMediumAttack1;
    private readonly InputAction m_PlayerSword_SwordMediumAttack2;
    private readonly InputAction m_PlayerSword_SwordHeavyAttack1;
    private readonly InputAction m_PlayerSword_SwordHeavyAttack2;
    private readonly InputAction m_PlayerSword_Parry;
    private readonly InputAction m_PlayerSword_PauseGame;
    private readonly InputAction m_PlayerSword_Interact;
    private readonly InputAction m_PlayerSword_ChangeStance;
    private readonly InputAction m_PlayerSword_SpawnBox;
    public struct PlayerSwordActions
    {
        private @ApologuePlayerInput_Actions m_Wrapper;
        public PlayerSwordActions(@ApologuePlayerInput_Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerSword_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerSword_Jump;
        public InputAction @Dash => m_Wrapper.m_PlayerSword_Dash;
        public InputAction @DoubleJump => m_Wrapper.m_PlayerSword_DoubleJump;
        public InputAction @ArrowKeysMovement => m_Wrapper.m_PlayerSword_ArrowKeysMovement;
        public InputAction @Crouch => m_Wrapper.m_PlayerSword_Crouch;
        public InputAction @Slide => m_Wrapper.m_PlayerSword_Slide;
        public InputAction @SwordLightAttack1 => m_Wrapper.m_PlayerSword_SwordLightAttack1;
        public InputAction @SwordLightAttack2 => m_Wrapper.m_PlayerSword_SwordLightAttack2;
        public InputAction @SwordLightAttack3 => m_Wrapper.m_PlayerSword_SwordLightAttack3;
        public InputAction @SwordMediumAttack1 => m_Wrapper.m_PlayerSword_SwordMediumAttack1;
        public InputAction @SwordMediumAttack2 => m_Wrapper.m_PlayerSword_SwordMediumAttack2;
        public InputAction @SwordHeavyAttack1 => m_Wrapper.m_PlayerSword_SwordHeavyAttack1;
        public InputAction @SwordHeavyAttack2 => m_Wrapper.m_PlayerSword_SwordHeavyAttack2;
        public InputAction @Parry => m_Wrapper.m_PlayerSword_Parry;
        public InputAction @PauseGame => m_Wrapper.m_PlayerSword_PauseGame;
        public InputAction @Interact => m_Wrapper.m_PlayerSword_Interact;
        public InputAction @ChangeStance => m_Wrapper.m_PlayerSword_ChangeStance;
        public InputAction @SpawnBox => m_Wrapper.m_PlayerSword_SpawnBox;
        public InputActionMap Get() { return m_Wrapper.m_PlayerSword; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerSwordActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerSwordActions instance)
        {
            if (m_Wrapper.m_PlayerSwordActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnJump;
                @Dash.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnDash;
                @DoubleJump.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnDoubleJump;
                @DoubleJump.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnDoubleJump;
                @DoubleJump.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnDoubleJump;
                @ArrowKeysMovement.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnArrowKeysMovement;
                @ArrowKeysMovement.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnArrowKeysMovement;
                @ArrowKeysMovement.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnArrowKeysMovement;
                @Crouch.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnCrouch;
                @Slide.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSlide;
                @Slide.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSlide;
                @Slide.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSlide;
                @SwordLightAttack1.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack1;
                @SwordLightAttack1.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack1;
                @SwordLightAttack1.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack1;
                @SwordLightAttack2.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack2;
                @SwordLightAttack2.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack2;
                @SwordLightAttack2.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack2;
                @SwordLightAttack3.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack3;
                @SwordLightAttack3.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack3;
                @SwordLightAttack3.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordLightAttack3;
                @SwordMediumAttack1.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordMediumAttack1;
                @SwordMediumAttack1.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordMediumAttack1;
                @SwordMediumAttack1.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordMediumAttack1;
                @SwordMediumAttack2.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordMediumAttack2;
                @SwordMediumAttack2.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordMediumAttack2;
                @SwordMediumAttack2.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordMediumAttack2;
                @SwordHeavyAttack1.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordHeavyAttack1;
                @SwordHeavyAttack1.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordHeavyAttack1;
                @SwordHeavyAttack1.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordHeavyAttack1;
                @SwordHeavyAttack2.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordHeavyAttack2;
                @SwordHeavyAttack2.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordHeavyAttack2;
                @SwordHeavyAttack2.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSwordHeavyAttack2;
                @Parry.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnParry;
                @Parry.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnParry;
                @Parry.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnParry;
                @PauseGame.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnPauseGame;
                @Interact.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnInteract;
                @ChangeStance.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnChangeStance;
                @ChangeStance.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnChangeStance;
                @ChangeStance.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnChangeStance;
                @SpawnBox.started -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSpawnBox;
                @SpawnBox.performed -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSpawnBox;
                @SpawnBox.canceled -= m_Wrapper.m_PlayerSwordActionsCallbackInterface.OnSpawnBox;
            }
            m_Wrapper.m_PlayerSwordActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @DoubleJump.started += instance.OnDoubleJump;
                @DoubleJump.performed += instance.OnDoubleJump;
                @DoubleJump.canceled += instance.OnDoubleJump;
                @ArrowKeysMovement.started += instance.OnArrowKeysMovement;
                @ArrowKeysMovement.performed += instance.OnArrowKeysMovement;
                @ArrowKeysMovement.canceled += instance.OnArrowKeysMovement;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Slide.started += instance.OnSlide;
                @Slide.performed += instance.OnSlide;
                @Slide.canceled += instance.OnSlide;
                @SwordLightAttack1.started += instance.OnSwordLightAttack1;
                @SwordLightAttack1.performed += instance.OnSwordLightAttack1;
                @SwordLightAttack1.canceled += instance.OnSwordLightAttack1;
                @SwordLightAttack2.started += instance.OnSwordLightAttack2;
                @SwordLightAttack2.performed += instance.OnSwordLightAttack2;
                @SwordLightAttack2.canceled += instance.OnSwordLightAttack2;
                @SwordLightAttack3.started += instance.OnSwordLightAttack3;
                @SwordLightAttack3.performed += instance.OnSwordLightAttack3;
                @SwordLightAttack3.canceled += instance.OnSwordLightAttack3;
                @SwordMediumAttack1.started += instance.OnSwordMediumAttack1;
                @SwordMediumAttack1.performed += instance.OnSwordMediumAttack1;
                @SwordMediumAttack1.canceled += instance.OnSwordMediumAttack1;
                @SwordMediumAttack2.started += instance.OnSwordMediumAttack2;
                @SwordMediumAttack2.performed += instance.OnSwordMediumAttack2;
                @SwordMediumAttack2.canceled += instance.OnSwordMediumAttack2;
                @SwordHeavyAttack1.started += instance.OnSwordHeavyAttack1;
                @SwordHeavyAttack1.performed += instance.OnSwordHeavyAttack1;
                @SwordHeavyAttack1.canceled += instance.OnSwordHeavyAttack1;
                @SwordHeavyAttack2.started += instance.OnSwordHeavyAttack2;
                @SwordHeavyAttack2.performed += instance.OnSwordHeavyAttack2;
                @SwordHeavyAttack2.canceled += instance.OnSwordHeavyAttack2;
                @Parry.started += instance.OnParry;
                @Parry.performed += instance.OnParry;
                @Parry.canceled += instance.OnParry;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @ChangeStance.started += instance.OnChangeStance;
                @ChangeStance.performed += instance.OnChangeStance;
                @ChangeStance.canceled += instance.OnChangeStance;
                @SpawnBox.started += instance.OnSpawnBox;
                @SpawnBox.performed += instance.OnSpawnBox;
                @SpawnBox.canceled += instance.OnSpawnBox;
            }
        }
    }
    public PlayerSwordActions @PlayerSword => new PlayerSwordActions(this);

    // PlayerAxe
    private readonly InputActionMap m_PlayerAxe;
    private IPlayerAxeActions m_PlayerAxeActionsCallbackInterface;
    private readonly InputAction m_PlayerAxe_Move;
    private readonly InputAction m_PlayerAxe_Jump;
    private readonly InputAction m_PlayerAxe_Dash;
    private readonly InputAction m_PlayerAxe_DoubleJump;
    private readonly InputAction m_PlayerAxe_ArrowKeysMovement;
    private readonly InputAction m_PlayerAxe_Crouch;
    private readonly InputAction m_PlayerAxe_Slide;
    private readonly InputAction m_PlayerAxe_AxeLightAttack1;
    private readonly InputAction m_PlayerAxe_AxeLightAttack2;
    private readonly InputAction m_PlayerAxe_AxeLightAttack3;
    private readonly InputAction m_PlayerAxe_AxeMediumAttack1;
    private readonly InputAction m_PlayerAxe_AxeMediumAttack2;
    private readonly InputAction m_PlayerAxe_AxeHeavyAttack1;
    private readonly InputAction m_PlayerAxe_AxeHeavyAttack2;
    private readonly InputAction m_PlayerAxe_Parry;
    private readonly InputAction m_PlayerAxe_PauseGame;
    private readonly InputAction m_PlayerAxe_Interact;
    private readonly InputAction m_PlayerAxe_ChangeStance;
    private readonly InputAction m_PlayerAxe_SpawnBox;
    public struct PlayerAxeActions
    {
        private @ApologuePlayerInput_Actions m_Wrapper;
        public PlayerAxeActions(@ApologuePlayerInput_Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerAxe_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerAxe_Jump;
        public InputAction @Dash => m_Wrapper.m_PlayerAxe_Dash;
        public InputAction @DoubleJump => m_Wrapper.m_PlayerAxe_DoubleJump;
        public InputAction @ArrowKeysMovement => m_Wrapper.m_PlayerAxe_ArrowKeysMovement;
        public InputAction @Crouch => m_Wrapper.m_PlayerAxe_Crouch;
        public InputAction @Slide => m_Wrapper.m_PlayerAxe_Slide;
        public InputAction @AxeLightAttack1 => m_Wrapper.m_PlayerAxe_AxeLightAttack1;
        public InputAction @AxeLightAttack2 => m_Wrapper.m_PlayerAxe_AxeLightAttack2;
        public InputAction @AxeLightAttack3 => m_Wrapper.m_PlayerAxe_AxeLightAttack3;
        public InputAction @AxeMediumAttack1 => m_Wrapper.m_PlayerAxe_AxeMediumAttack1;
        public InputAction @AxeMediumAttack2 => m_Wrapper.m_PlayerAxe_AxeMediumAttack2;
        public InputAction @AxeHeavyAttack1 => m_Wrapper.m_PlayerAxe_AxeHeavyAttack1;
        public InputAction @AxeHeavyAttack2 => m_Wrapper.m_PlayerAxe_AxeHeavyAttack2;
        public InputAction @Parry => m_Wrapper.m_PlayerAxe_Parry;
        public InputAction @PauseGame => m_Wrapper.m_PlayerAxe_PauseGame;
        public InputAction @Interact => m_Wrapper.m_PlayerAxe_Interact;
        public InputAction @ChangeStance => m_Wrapper.m_PlayerAxe_ChangeStance;
        public InputAction @SpawnBox => m_Wrapper.m_PlayerAxe_SpawnBox;
        public InputActionMap Get() { return m_Wrapper.m_PlayerAxe; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerAxeActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerAxeActions instance)
        {
            if (m_Wrapper.m_PlayerAxeActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnJump;
                @Dash.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnDash;
                @DoubleJump.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnDoubleJump;
                @DoubleJump.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnDoubleJump;
                @DoubleJump.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnDoubleJump;
                @ArrowKeysMovement.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnArrowKeysMovement;
                @ArrowKeysMovement.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnArrowKeysMovement;
                @ArrowKeysMovement.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnArrowKeysMovement;
                @Crouch.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnCrouch;
                @Slide.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnSlide;
                @Slide.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnSlide;
                @Slide.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnSlide;
                @AxeLightAttack1.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack1;
                @AxeLightAttack1.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack1;
                @AxeLightAttack1.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack1;
                @AxeLightAttack2.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack2;
                @AxeLightAttack2.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack2;
                @AxeLightAttack2.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack2;
                @AxeLightAttack3.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack3;
                @AxeLightAttack3.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack3;
                @AxeLightAttack3.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeLightAttack3;
                @AxeMediumAttack1.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeMediumAttack1;
                @AxeMediumAttack1.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeMediumAttack1;
                @AxeMediumAttack1.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeMediumAttack1;
                @AxeMediumAttack2.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeMediumAttack2;
                @AxeMediumAttack2.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeMediumAttack2;
                @AxeMediumAttack2.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeMediumAttack2;
                @AxeHeavyAttack1.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeHeavyAttack1;
                @AxeHeavyAttack1.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeHeavyAttack1;
                @AxeHeavyAttack1.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeHeavyAttack1;
                @AxeHeavyAttack2.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeHeavyAttack2;
                @AxeHeavyAttack2.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeHeavyAttack2;
                @AxeHeavyAttack2.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnAxeHeavyAttack2;
                @Parry.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnParry;
                @Parry.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnParry;
                @Parry.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnParry;
                @PauseGame.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnPauseGame;
                @Interact.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnInteract;
                @ChangeStance.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnChangeStance;
                @ChangeStance.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnChangeStance;
                @ChangeStance.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnChangeStance;
                @SpawnBox.started -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnSpawnBox;
                @SpawnBox.performed -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnSpawnBox;
                @SpawnBox.canceled -= m_Wrapper.m_PlayerAxeActionsCallbackInterface.OnSpawnBox;
            }
            m_Wrapper.m_PlayerAxeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @DoubleJump.started += instance.OnDoubleJump;
                @DoubleJump.performed += instance.OnDoubleJump;
                @DoubleJump.canceled += instance.OnDoubleJump;
                @ArrowKeysMovement.started += instance.OnArrowKeysMovement;
                @ArrowKeysMovement.performed += instance.OnArrowKeysMovement;
                @ArrowKeysMovement.canceled += instance.OnArrowKeysMovement;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Slide.started += instance.OnSlide;
                @Slide.performed += instance.OnSlide;
                @Slide.canceled += instance.OnSlide;
                @AxeLightAttack1.started += instance.OnAxeLightAttack1;
                @AxeLightAttack1.performed += instance.OnAxeLightAttack1;
                @AxeLightAttack1.canceled += instance.OnAxeLightAttack1;
                @AxeLightAttack2.started += instance.OnAxeLightAttack2;
                @AxeLightAttack2.performed += instance.OnAxeLightAttack2;
                @AxeLightAttack2.canceled += instance.OnAxeLightAttack2;
                @AxeLightAttack3.started += instance.OnAxeLightAttack3;
                @AxeLightAttack3.performed += instance.OnAxeLightAttack3;
                @AxeLightAttack3.canceled += instance.OnAxeLightAttack3;
                @AxeMediumAttack1.started += instance.OnAxeMediumAttack1;
                @AxeMediumAttack1.performed += instance.OnAxeMediumAttack1;
                @AxeMediumAttack1.canceled += instance.OnAxeMediumAttack1;
                @AxeMediumAttack2.started += instance.OnAxeMediumAttack2;
                @AxeMediumAttack2.performed += instance.OnAxeMediumAttack2;
                @AxeMediumAttack2.canceled += instance.OnAxeMediumAttack2;
                @AxeHeavyAttack1.started += instance.OnAxeHeavyAttack1;
                @AxeHeavyAttack1.performed += instance.OnAxeHeavyAttack1;
                @AxeHeavyAttack1.canceled += instance.OnAxeHeavyAttack1;
                @AxeHeavyAttack2.started += instance.OnAxeHeavyAttack2;
                @AxeHeavyAttack2.performed += instance.OnAxeHeavyAttack2;
                @AxeHeavyAttack2.canceled += instance.OnAxeHeavyAttack2;
                @Parry.started += instance.OnParry;
                @Parry.performed += instance.OnParry;
                @Parry.canceled += instance.OnParry;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @ChangeStance.started += instance.OnChangeStance;
                @ChangeStance.performed += instance.OnChangeStance;
                @ChangeStance.canceled += instance.OnChangeStance;
                @SpawnBox.started += instance.OnSpawnBox;
                @SpawnBox.performed += instance.OnSpawnBox;
                @SpawnBox.canceled += instance.OnSpawnBox;
            }
        }
    }
    public PlayerAxeActions @PlayerAxe => new PlayerAxeActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_RightClick;
    private readonly InputAction m_UI_TrackedDevicePosition;
    private readonly InputAction m_UI_TrackedDeviceOrientation;
    private readonly InputAction m_UI_PauseMenu;
    public struct UIActions
    {
        private @ApologuePlayerInput_Actions m_Wrapper;
        public UIActions(@ApologuePlayerInput_Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;
        public InputAction @PauseMenu => m_Wrapper.m_UI_PauseMenu;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @TrackedDevicePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @PauseMenu.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPauseMenu;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
                @PauseMenu.started += instance.OnPauseMenu;
                @PauseMenu.performed += instance.OnPauseMenu;
                @PauseMenu.canceled += instance.OnPauseMenu;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IPlayerSwordActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnDoubleJump(InputAction.CallbackContext context);
        void OnArrowKeysMovement(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
        void OnSwordLightAttack1(InputAction.CallbackContext context);
        void OnSwordLightAttack2(InputAction.CallbackContext context);
        void OnSwordLightAttack3(InputAction.CallbackContext context);
        void OnSwordMediumAttack1(InputAction.CallbackContext context);
        void OnSwordMediumAttack2(InputAction.CallbackContext context);
        void OnSwordHeavyAttack1(InputAction.CallbackContext context);
        void OnSwordHeavyAttack2(InputAction.CallbackContext context);
        void OnParry(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnChangeStance(InputAction.CallbackContext context);
        void OnSpawnBox(InputAction.CallbackContext context);
    }
    public interface IPlayerAxeActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnDoubleJump(InputAction.CallbackContext context);
        void OnArrowKeysMovement(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
        void OnAxeLightAttack1(InputAction.CallbackContext context);
        void OnAxeLightAttack2(InputAction.CallbackContext context);
        void OnAxeLightAttack3(InputAction.CallbackContext context);
        void OnAxeMediumAttack1(InputAction.CallbackContext context);
        void OnAxeMediumAttack2(InputAction.CallbackContext context);
        void OnAxeHeavyAttack1(InputAction.CallbackContext context);
        void OnAxeHeavyAttack2(InputAction.CallbackContext context);
        void OnParry(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnChangeStance(InputAction.CallbackContext context);
        void OnSpawnBox(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
        void OnPauseMenu(InputAction.CallbackContext context);
    }
}
