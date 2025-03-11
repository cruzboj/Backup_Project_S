using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    //new solution
    [Header("Start Button Settings")]
    public Image imageComponent;

    public bool first_time_blueScreen = false;
    public bool hasConfimed = false;
    public bool allPlayersSelected = false;

    public GameObject DestroyMouse;

    [Header("BoxCollider")]
    public GameObject CharactersBoxColliders;

    [Header("Mouse ARRAY")]
    public int[] buttonIndex = new int[10];
    private Image buttonImage;
    public int indexRecived = -1;

    [SerializeField] private List<GameObject> availablePrefabs = new List<GameObject>();
    [SerializeField] private int currentPrefabIndex = 0;
    private NewVirtualMouse PlayerChooseIndex;

    private NewVirtualMouse mouseStatus = null;

    //new for - SpawnSelectedPrefab()
    private PlayerPrefabSelector spawnPrefab;
    public GameObject Prefab;
    public int[] getButtonIndex() { return buttonIndex; }

    // Dictionary to store mouse data per player
    private Dictionary<int, int> playerIndices = new Dictionary<int, int>();
    private Dictionary<int, bool> playerClickStatus = new Dictionary<int, bool>();

    [SerializeField] public LayerMask layerMask;

    //new fix
    private List<NewVirtualMouse> mouseList = new List<NewVirtualMouse>();

    [SerializeField] private string scene2Name = "DontDestroyOnLoad"; // שם הסצנה השנייה
    private static GameObject persistentParent;

    private List<GameObject> spawnedPrefabs = new List<GameObject>(); // רשימה חדשה לשמירת האובייקטים שנוצרו

    void Start()
    {
        if (persistentParent == null)
        {
            persistentParent = new GameObject("PersistentParent");
            DontDestroyOnLoad(persistentParent);
        }

        buttonImage = transform.GetChild(0).GetComponent<Image>();
        spawnPrefab = GetComponent<PlayerPrefabSelector>();
    }

    
    void Update()
    {
        if(hasConfimed) return;

        NewVirtualMouse[] allMice = GameObject.FindObjectsOfType<NewVirtualMouse>();

        foreach (NewVirtualMouse mouse in allMice)
        {
            if (mouse != null)
            {
                int playerIndex = mouse.getindex() - 1;

                //Debug.Log("in loop");
                //check if all players selected character
                if (!mouse.getSelected())
                {
                    //not all players sellected mouse
                    Debug.Log("allPlayersSelected = False");
                    //BoxCollider.GetComponent<BoxCollider>().enabled = false;
                    imageComponent.enabled = false;
                    allPlayersSelected = false;
                    CharactersBoxColliders.SetActive(true);
                    if (first_time_blueScreen)
                        first_time_blueScreen = false;
                    return;
                }
                else
                {
                    //StartCoroutine(Deley(0.5f));
                    //all players selected mouse
                    if (!first_time_blueScreen)
                    {
                        foreach (var currentMouse in allMice) //turn off all mouses
                        {
                            //Debug.Log("hasClicked = False");
                            currentMouse.Clicked = false;
                        }
                    }
                    first_time_blueScreen = true;

                    //BoxCollider.GetComponent<BoxCollider>().enabled = true;
                    imageComponent.enabled = true;
                    allPlayersSelected = true;
                    CharactersBoxColliders.SetActive(false);


                }

                // Store/update this player's data
                if (playerIndex >= 0 && playerIndex < buttonIndex.Length)
                {
                    playerIndices[playerIndex] = mouse.getbuttonIndex();
                    playerClickStatus[playerIndex] = mouse.getClicked();

                    // Update the buttonIndex array
                    buttonIndex[playerIndex] = mouse.getbuttonIndex();
                }

                if (mouse.Clicked && allPlayersSelected)
                {
                    Debug.Log("hasConfimed = TRUE");
                    StartCoroutine(SpawnPrefabsWithDelay(1f));
                    //break;
                    hasConfimed = true;
                }
            }

            
        }
        
        //if(allPlayersSelected)
        //    confirm_game();


        

        //mouseStatus = GameObject.FindObjectOfType<NewVirtualMouse>();

        //if (mouseStatus == null)
        //{
        //    Debug.LogError("NewVirtualMouse not found in the scene.");
        //    return;

        //}



        //indexRecived = mouseStatus.getindex() - 1;

        //if (indexRecived >= 0 && indexRecived < buttonIndex.Length)
        //{
        //    buttonIndex[indexRecived] = mouseStatus.getbuttonIndex();
        //}
        //else
        //{
        //    //Debug.LogWarning("indexRecived is out of bounds: " + indexRecived);
        //}
    }

    void confirm_game()
    {
        if (allPlayersSelected)
        {
            Debug.Log("entered confirm");
            //hasClicked = false;
            //allPlayersSelected = false;

            //StartCoroutine(SpawnPrefabsWithDelay(1f));
            //return;
        }
    }


    //private void OnTriggerStay(Collider other)
    //{
    //    if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
    //    {
    //        mouseStatus = GameObject.FindObjectOfType<NewVirtualMouse>();

    //        //Debug.Log("Mouse inside me nnya!");
    //        indexRecived = mouseStatus.getindex(); //not importent just for testing
    //        buttonImage.color = new Color(0f, 0f, 0f);

    //        if (mouseStatus.getClicked() && allPlayersSelected)
    //        {
    //            StartCoroutine(SpawnPrefabsWithDelay(1f));
    //        }
    //    }

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
    //    {
    //        Debug.Log("Mouse out");
    //        buttonImage.color = new Color(0f, 255f, 0f); // #00FF00
    //    }
    //    //mouseStatus = null;
    //}

    private IEnumerator SpawnPrefabsWithDelay(float delay)
    {
        spawnedPrefabs.Clear();
        Destroy(DestroyMouse);
        yield return new WaitForSeconds(delay);

        // וודא שיש לנו הורה קבוע
        if (persistentParent == null)
        {
            persistentParent = new GameObject("PersistentParent");
            DontDestroyOnLoad(persistentParent);
        }

        for (int i = 0; i < 6; i++)
        {
            if (buttonIndex[i] != -1)
            {
                Prefab = availablePrefabs[buttonIndex[i]];
                yield return new WaitForSeconds(delay);

                // יצירת הקלון והגדרתו כבן של ה-persistentParent
                GameObject spawnedPrefab = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
                spawnedPrefab.transform.SetParent(persistentParent.transform);
                spawnedPrefabs.Add(spawnedPrefab);
            }
        }

        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(scene2Name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject obj in spawnedPrefabs)
        {
            if (obj != null)
            {
                // שחרור מההורה והעברה לסצנה החדשה
                obj.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(obj, scene);
            }
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator Deley(float delay)
    {
        //Debug.Log("DELAY");
        yield return new WaitForSeconds(delay);

        //if(allPlayersSelected)
        hasConfimed = true;
        

        //BoxCollider.GetComponent<BoxCollider>().enabled = true;
        imageComponent.enabled = true;
        allPlayersSelected = true;
        CharactersBoxColliders.SetActive(false);
        //Debug.Log("after DELAY");
    }
}
