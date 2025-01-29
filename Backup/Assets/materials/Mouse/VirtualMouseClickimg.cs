using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class VirtualMouseClickimg : MonoBehaviour
{
    [SerializeField] private Sprite defaultCursor;
    [SerializeField] private Sprite clickCursor;
    [SerializeField] private Image cursorImage;     // האובייקט UI מסוג Image שמשמש בתור העכבר

    [SerializeField] private InputActionReference clickAction; // Input Action ללחיצה


    void OnEnable()
    {
        // הפעלת ה-Input Action
        clickAction.action.Enable();
    }

    void OnDisable()
    {
        // כיבוי ה-Input Action
        clickAction.action.Disable();
    }

    void Update()
    {
        // בדיקה אם ה-Input Action בלחיצה
        if (clickAction.action.IsPressed())
        {
            cursorImage.sprite = clickCursor; // החלפת תמונה בזמן לחיצה
        }
        else
        {
            cursorImage.sprite = defaultCursor; // חזרה לתמונת ברירת מחדל
        }
    }
}
