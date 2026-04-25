using UnityEngine;
using UnityEngine.InputSystem;

public class carInput : MonoBehaviour
{
    CarController carController;

    void Awake()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.y = 1;

        if (Keyboard.current.aKey.isPressed)
            inputVector.x = -1;
        else if (Keyboard.current.dKey.isPressed)
            inputVector.x = 1;

        carController.SetInputVector(inputVector);
    }
}