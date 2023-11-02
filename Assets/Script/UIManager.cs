using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CarController carController; // Pasangkan komponen CarController Anda di sini
    public Button turboButton;
    private bool isTurboButtonDown = false;
    private float turboButtonPressedTime = 0f;
    public float turboActivationTime = 2f;

    private void Update()
    {
        if (isTurboButtonDown)
        {
            turboButtonPressedTime += Time.deltaTime;
            if (turboButtonPressedTime >= turboActivationTime)
            {
                carController.ActivateTurbo(true);
            }
        }
    }

    public void OnTurboButtonPressed()
    {
        isTurboButtonDown = true;
    }

    public void OnTurboButtonReleased()
    {
        if (turboButtonPressedTime < turboActivationTime)
        {
            carController.ActivateTurbo(false);
        }
        isTurboButtonDown = false;
        turboButtonPressedTime = 0f;
    }
}
