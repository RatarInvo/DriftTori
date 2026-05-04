using UnityEngine;
using UnityEngine.InputSystem;

public class carInput : MonoBehaviour
{
    CarController carController;
    bool gameStarted = false;

    void Awake()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        if (!carController.carStarted)
            gameStarted = false;

        // Wait for W press to start the car
        if (!gameStarted)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                gameStarted = true;
                carController.StartCar();

                LevelTitleUI.Instance.HideTitle();
            }
            return; // Don't process any input until started
        }

        Vector2 inputVector = Vector2.zero;
        inputVector.y = 1;

        if (Keyboard.current.aKey.isPressed)
            inputVector.x = -1;
        else if (Keyboard.current.dKey.isPressed)
            inputVector.x = 1;
        else if (Keyboard.current.leftArrowKey.isPressed)
            inputVector.x = -1;
        else if (Keyboard.current.rightArrowKey.isPressed)
            inputVector.x = 1;

        carController.SetInputVector(inputVector);
    }
}