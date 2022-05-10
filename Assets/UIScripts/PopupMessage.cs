using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    public static float animDuration = 1f;

    public static float holdDurationSeconds = 5f;

    private static float holdUntil = 0f;

    private static bool active = false;

    private static Vector3 initialPosition;

    private static GameObject parentGameObject;

    private static TMPro.TextMeshProUGUI titleText;

    private static TMPro.TextMeshProUGUI bodyText;

    void Awake() 
    {
        PopupMessage.initialPosition = gameObject.transform.position;
        PopupMessage.titleText = gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        PopupMessage.bodyText = gameObject.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        PopupMessage.parentGameObject = gameObject;
    }

    // Start is called before the first frame update
    public static void CreatePopupMessage(string title, string body)
    {

        if (PopupMessage.active) {
            PopupMessage.parentGameObject.transform.position = PopupMessage.initialPosition;
        }

        PopupMessage.active = true;
        PopupMessage.holdUntil = DateTime.Now.Second + PopupMessage.holdDurationSeconds;

        //set text
        PopupMessage.titleText.text = title;
        PopupMessage.bodyText.text = body;

        LeanTween.moveLocalX(parentGameObject, -20f, PopupMessage.animDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (PopupMessage.active && DateTime.Now.Second > PopupMessage.holdUntil) {
            PopupMessage.active = false;
            //reset the position.
            gameObject.transform.position = PopupMessage.initialPosition;
        }
    }
}
