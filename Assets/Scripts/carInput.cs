using UnityEngine;
using UnityEngine.InputSystem;

public class carInput : MonoBehaviour
{

    CarController carController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        carController = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        if (Keyboard.current.aKey.isPressed)
            inputVector.x = -1;
        else if (Keyboard.current.dKey.isPressed)
            inputVector.x = 1;

        if (Keyboard.current.wKey.isPressed)
            inputVector.y = 1;
        else if (Keyboard.current.sKey.isPressed)
            inputVector.y = -1;

        carController.SetInputVector(inputVector);
    }
}
