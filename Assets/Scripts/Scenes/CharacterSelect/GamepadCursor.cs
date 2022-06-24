using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    public PlayerInput playerInput;
    public RectTransform cursorTransorm;
    public float cursorSpeed = 1000f;
    public RectTransform canvasRectTransform;
    public Camera mainCamera;
    public Canvas canvas;
    private float cursorPadding;

    private Mouse virtualMouse;
    private bool previousMouseState;


    private void Start()
    {
        cursorPadding = cursorTransorm.sizeDelta[0] / 2.5f;
    }
    private void OnEnable()
    {
        mainCamera = Camera.main;

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransorm != null)
        {
            Vector2 cursorPosition = cursorTransorm.anchoredPosition;
            InputState.Change(virtualMouse.position, cursorPosition);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null) return;
        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;


        // clamp to screen
        newPosition.x = Mathf.Clamp(newPosition.x, cursorPadding, Screen.width - cursorPadding);
        newPosition.y = Mathf.Clamp(newPosition.y, cursorPadding, Screen.height - cursorPadding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        if (previousMouseState != Gamepad.current.buttonSouth.isPressed)
        {
            bool isSouthPressed = Gamepad.current.buttonSouth.IsPressed();
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, isSouthPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = isSouthPressed;
        }

        AnchorCursor(newPosition);
    }

    // Weird shit to make sure the cursor scales with different camera stuff
    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode
        == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);

        cursorTransorm.anchoredPosition = anchoredPosition;
    }


}
