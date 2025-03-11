using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

[RequireComponent(typeof(CharacterController))]
public class NewVirtualMouse : MonoBehaviour
{
    [SerializeField] private float MouseSpeed = 1500.0f;
    private CharacterController controller;
    private Vector2 movementInput = Vector2.zero;

    //assigned in line if(canvasRectTransform != null)
    [SerializeField] private RectTransform canvasRectTransform;

    //ui 
    [SerializeField] private Sprite defaultCursor;
    [SerializeField] private Sprite clickCursor;
    public GameObject Child;
    //ui index
    public int index = 0;
    public int getindex() { return index; }
    private bool UiClicked = false;

    public bool Clicked = false;
    public bool getClicked() { return Clicked; }


    //saved button index
    public int buttonIndex;
    private ButtonScript BoxStatus;
    public int getbuttonIndex() { return buttonIndex; }

    [SerializeField] public LayerMask layerMask;

    [Header("selected character")]
    public bool selected;
    public bool confirmed;
    public bool getSelected() { return selected; }
    public bool getConfirmed() { return confirmed; }

    private void Start()
    {
        //set player index
        PlayerInput playerInput = GetComponentInParent<PlayerInput>();
        index = playerInput.playerIndex + 1;

        //character controller 
        controller = gameObject.GetComponent<CharacterController>();

        //canvas for Clamp()
        if (canvasRectTransform == null)
        {
            // Find the Canvas in the scene and get its RectTransform
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                canvasRectTransform = canvas.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogError("Canvas not found in the scene!");
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed) // Trigger only when the button is pressed once
        {
            //Debug.LogError("Ouch you clicked me!");
            
            Clicked = true;
            StartCoroutine(SpawnPrefabsWithDelay(0.025f));
        }
        else if (context.canceled) // Reset on release (optional)
        {
            Clicked = false;
        }

            UiClicked = context.action.triggered;
        if (UiClicked)
            Child.GetComponent<Image>().sprite = clickCursor; // שינוי תמונת ה-Child
        else
            Child.GetComponent<Image>().sprite = defaultCursor; // שינוי תמונת ה-Child
    }

    public void OnEscClick(InputAction.CallbackContext context)
    {
        selected = false;
    }
    
    //public void confirmClick(InputAction.CallbackContext context)
    //{
    //    if (selected)
    //    {
    //        confirmed = true;
    //    }

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Mouse on button");

    //    if (other.CompareTag("Button Char"))
    //    {
    //        if (Clicked)
    //        {
    //            Debug.Log("Button clicked! SEND INDEX");
    //        }
    //    }
    //}
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("in side Mouse button");

        if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if (Clicked)
            {
                BoxStatus = GameObject.FindObjectOfType<ButtonScript>();
                if (other.TryGetComponent<ButtonScript>(out BoxStatus))
                {
                    buttonIndex = BoxStatus.getButtonIndex();
                    selected = true;
                }
            }
        }

    }

    void Update()
    {
        if (canvasRectTransform != null)
        {
            //movment
            Vector3 move = new Vector3(movementInput.x, movementInput.y, 0);
            Vector3 newPosition = transform.position + (move * Time.deltaTime * MouseSpeed);

            // חישוב גבולות ה-Canvas
            Vector3[] corners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(corners);
            float minX = corners[0].x;
            float maxX = corners[2].x;
            float minY = corners[0].y;
            float maxY = corners[2].y;

            // הגבלת התזוזה לגבולות ה-Canvas
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            // חישוב הוקטור התזוזה הסופי
            Vector3 finalMove = newPosition - transform.position;
            controller.Move(finalMove);

            // Debug - להדפסת המיקום והגבולות
            //Debug.Log($"Position: {transform.position}, Bounds: X({minX},{maxX}), Y({minY},{maxY})");
        }

        if (Clicked)
        {
            //Debug.Log("Clicked!");

        }
    }

    private IEnumerator SpawnPrefabsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Clicked = false;

    }
}