using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
public class InputsHelicopter : MonoBehaviour
	{
		#region Singleton
		static InputsHelicopter _instance;
		static public bool isActive
		{
			get
			{
				return _instance != null;
			}
		}
		static public InputsHelicopter instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Object.FindObjectOfType(typeof(InputsHelicopter)) as InputsHelicopter;

					if (_instance == null)
					{
						GameObject go = new GameObject("InputsHelicopter");
						// DontDestroyOnLoad(go);
						_instance = go.AddComponent<InputsHelicopter>();
					}
				}
				return _instance;
			}
		}
		#endregion
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		//public bool aim;
		public bool fire1;
		public bool fire2;
		public bool fire3;
		public bool fire1Trigger;
		public bool fire2Trigger;
		public bool fire3Trigger;
		public bool Minimap;
		public bool flash;
		public bool on;
		public bool off;
		public bool crouch;
		public bool jump;
		public bool sprint;
		[Header("Active")]
		public bool active;
		public bool changeweapon;
		private PlayerInput _playerInput;
		public PlayerInput PlayerInput { get; }
		[Header("Movement Settings")]
		public bool analogMovement;
		[Header("Mouse Scroll")]
		public float mouseScrollY;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public Texture2D cursorText;

		[Header("Pause")]
		public bool Pause = false;
		bool timeScale = false;

		CursorMode cursorMode = CursorMode.ForceSoftware;
		Vector2 hotSpot = Vector2.zero;
		void Awake()
		{
			_playerInput = GetComponent<PlayerInput>();
			
			//Cursor.visible = false;
			SetCursorState(cursorLocked);
		}
		void cursorSet(Texture2D tex)
		{
			float xspot = tex.width * 0.25f;
			float yspot = tex.height * 0.25f;
			hotSpot = new Vector2(xspot, yspot);
			Cursor.SetCursor(tex, hotSpot, cursorMode);
		}
		//void OnMouseEnter()
		//{
		//	float xspot = cursorText.width * 0.25f;
		//	float yspot = cursorText.height * 0.25f;
		//	hotSpot = new Vector2(xspot, yspot);
		//	Cursor.SetCursor(cursorText, hotSpot, cursorMode);
		//}

		//void OnMouseExit()
		//{
		//	Cursor.SetCursor(null, Vector2.zero, cursorMode);
		//}
		void Start()
		{
			//cursorSet(cursorText);

			_playerInput.actions["Sprint"].started += ctx => Sprint(ctx);
			_playerInput.actions["Sprint"].performed += ctx => Sprint(ctx);
			_playerInput.actions["Sprint"].canceled += ctx => Sprint(ctx);

			//_playerInput.actions["Fire"].started += ctx => Fire(ctx,0);
			//_playerInput.actions["Fire"].performed += ctx => Fire(ctx, 0);
			//_playerInput.actions["Fire"].canceled += ctx => Fire(ctx, 0);

			_playerInput.actions["Fire1"].started += ctx => Fire(ctx, 1);
			_playerInput.actions["Fire1"].performed += ctx => Fire(ctx, 1);
			_playerInput.actions["Fire1"].canceled += ctx => Fire(ctx, 1);

			_playerInput.actions["Fire2"].started += ctx => Fire(ctx, 2);
			_playerInput.actions["Fire2"].performed += ctx => Fire(ctx, 2);
			_playerInput.actions["Fire2"].canceled += ctx => Fire(ctx, 2);

			_playerInput.actions["Fire3"].started += ctx => Fire(ctx, 3);
			_playerInput.actions["Fire3"].performed += ctx => Fire(ctx, 3);
			_playerInput.actions["Fire3"].canceled += ctx => Fire(ctx, 3);

			//_playerInput.actions["Flash"].started += ctx => Flash(ctx);
			//_playerInput.actions["Flash"].performed += ctx => Flash(ctx);
			//_playerInput.actions["Flash"].canceled += ctx => Flash(ctx);

			_playerInput.actions["Jump"].started += ctx => Jump(ctx);
			_playerInput.actions["Jump"].performed += ctx => Jump(ctx);
			_playerInput.actions["Jump"].canceled += ctx => Jump(ctx);

			_playerInput.actions["Crouch"].started += ctx => Crouch(ctx);
			_playerInput.actions["Crouch"].performed += ctx => Crouch(ctx);
			_playerInput.actions["Crouch"].canceled += ctx => Crouch(ctx);

			//_playerInput.actions["Aim"].started += ctx => Aim(ctx);
			//_playerInput.actions["Aim"].performed += ctx => Aim(ctx);
			//_playerInput.actions["Aim"].canceled += ctx => Aim(ctx);

			_playerInput.actions["ChangeWeapon"].started += ctx => ChangeWeaponInput(ctx);
			_playerInput.actions["ChangeWeapon"].performed += ctx => ChangeWeaponInput(ctx);
			_playerInput.actions["ChangeWeapon"].canceled += ctx => ChangeWeaponInput(ctx);

			_playerInput.actions["ZoomCamera"].performed += ctx => ScrollCamera(ctx);


			_playerInput.actions["On"].started += ctx => OnCtx(ctx);
			_playerInput.actions["On"].canceled += ctx => OnCtx(ctx);

			_playerInput.actions["Off"].started += ctx => OffCtx(ctx);
			_playerInput.actions["Off"].canceled += ctx => OffCtx(ctx);

			_playerInput.actions["Map"].started += ctx => ShowMinimapInput(ctx);
			//_playerInput.actions["Map"].performed += ctx => ShowMinimapInput(ctx);
			//_playerInput.actions["Map"].canceled += ctx => ShowMinimapInput(ctx);


		}
		void Update()
		{

			fire1Trigger = _playerInput.actions["Fire1"].triggered;
			fire2Trigger = _playerInput.actions["Fire2"].triggered;
			fire3Trigger = _playerInput.actions["Fire3"].triggered;

			flash =	_playerInput.actions["Flash"].triggered;

			Pause = _playerInput.actions["Pause"].triggered;

			

		}
		public string GetKeyNameAction(string action)
		{
			string key = _playerInput.actions[action].bindings[0].path;
			return "[" + key.Substring(key.IndexOf('/') + 1, 1).ToUpper() + "]";
		}
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}
		public void OnLook(InputValue value)
		{
			
				LookInput(value.Get<Vector2>());
			
		}
		public void OnLookGamePad(InputValue value)
		{
			
				LookInput(value.Get<Vector2>());
			
		}
		public void OnCtx(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.started)
				OnInput(true);
			else
				if (context.canceled)
				OnInput(false);
		}
		public void OffCtx(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.started)
				OffInput(true);
			else
				if (context.canceled)
				OffInput(false);
		}
		public void OnInput(bool newOnState)
		{
			if (!active) return;
			on = newOnState;
		}
		public void OffInput(bool newOffState)
		{
			if (!active) return;
			off = newOffState;
		}

		public void ChangeWeaponInput(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.started)
				ChangeWeaponInput(true);
			else
				if (context.canceled)
				ChangeWeaponInput(false);
		}
		public void ShowMinimapInput(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.started)
				Minimap = !Minimap; ;
			
		}
		public void ShowMinimapInput(bool newChangeWeaponState)
		{
			
		}
		public void ChangeWeaponInput(bool newChangeWeaponState)
		{
			changeweapon = newChangeWeaponState;
		}
		public void Sprint(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.performed)
				SprintInput(true);
			else
				if (context.canceled)
				SprintInput(false);

		}
		public void Flash(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.started)
				FlashInput(true);
			else
				if (context.canceled)
				FlashInput(false);

		}
		public void Fire(InputAction.CallbackContext context, int index)
		{
			if (!active) return;
			if (context.performed)
				FireInput(true, index);
			else
				if (context.canceled)
				FireInput(false, index);


		}
		public void Jump(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.performed)
				JumpInput(true);
			else
				if (context.canceled)
				JumpInput(false);
		}
		public void Crouch(InputAction.CallbackContext context)
		{
			if (!active) return;
			if (context.performed)
				CrouchInput(true);
			else
				if (context.canceled)
				CrouchInput(false);
		}
		void ScrollCamera(InputAction.CallbackContext context)
		{
			if (!active) return;
			mouseScrollY = context.ReadValue<float>();
		}
		public void MoveInput(Vector2 newMoveDirection)
		{
			if (!active)
			{
				move = Vector2.zero;
				return;
			}
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			if (!active)
			{
				look = Vector2.zero;
				return;
			}
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			if (!active) return;
			jump = newJumpState;
		}

		public void CrouchInput(bool newCrouchState)
		{
			if (!active) return;
			crouch = newCrouchState;
		}

		public void FlashInput(bool newFlashState)
		{
			if (!active) return;
			flash = newFlashState;
		}
		public void SprintInput(bool newSprintState)
		{
			if (!active) return;
			sprint = newSprintState;
		}
		void FireInput(bool newFireState, int index)
		{
			if (!active) return;
			switch (index)
			{

				case 1:
					fire1 = newFireState;
					break;
				case 2:
					fire2 = newFireState;
					break;
				case 3:
					fire3 = newFireState;
					break;
				default:
					break;
			}
		}
		public void Inactive()
		{
			active = false;
			//aim = false;
			//fire = false;
			fire1 = false;
			fire2 = false;
			fire3 = false;
			flash = false;
			jump = false;
			on = false;
			off = false;
			crouch = false;
			sprint = false;
			Pause = false;
		}
		public void Active()
		{
			active = true;
			//aim = false;
			//fire = false;
			fire1 = false;
			fire2 = false;
			fire3 = false;
			flash = false;
			jump = false;
			on = false;
			off = false;
			crouch = false;
			sprint = false;
			Pause = false;
		}
		private void OnApplicationFocus(bool hasFocus)
		{
			//Debug.Log("hasFocus: "+ hasFocus+"  cursorLocked: "+ cursorLocked);
			SetCursorState(cursorLocked);
		}
		public void SetCursorState(bool newState)
			{
				Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
				Cursor.visible = !newState;
			}
}

