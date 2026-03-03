using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class EnableHouseUnlock : MonoBehaviour
{
    public InputActionReference unlockAction;
    public GameObject blinkingText;
    public GameObject textCanvas;
    public GameObject houseObject;
    public float blinkInterval = 0.5f;
    private bool isPlayerInside = false;
    private Coroutine blinkingCoroutine;

    private void OnEnable()
    {
        unlockAction.action.Enable();
        unlockAction.action.performed += onUnlockHouse;
    }

    private void OnDisable()
    {
        unlockAction.action.performed -= onUnlockHouse;
        unlockAction.action.Disable();
    }

    private void onUnlockHouse(InputAction.CallbackContext context)
    {
        if (isPlayerInside)
        {
            houseObject.SetActive(true);
            textCanvas.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            blinkingCoroutine = StartCoroutine(BlinkText());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (blinkingCoroutine != null)
            {
                StopCoroutine(blinkingCoroutine);
                blinkingCoroutine = null;
            }
            blinkingText.SetActive(false);
        }
    }

    private IEnumerator BlinkText()
    {
        while (isPlayerInside)
        {
            blinkingText.SetActive(!blinkingText.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}