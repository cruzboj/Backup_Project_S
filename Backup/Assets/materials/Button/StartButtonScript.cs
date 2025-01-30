using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public int[] buttonIndex = new int[10];
    private Image buttonImage;
    public int indexRecived = -1;

    //new idea 
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

    //cluade
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
        NewVirtualMouse[] allMice = GameObject.FindObjectsOfType<NewVirtualMouse>();

        foreach (NewVirtualMouse mouse in allMice)
        {
            if (mouse != null)
            {
                int playerIndex = mouse.getindex() - 1;

                // Store/update this player's data
                if (playerIndex >= 0 && playerIndex < buttonIndex.Length)
                {
                    playerIndices[playerIndex] = mouse.getbuttonIndex();
                    playerClickStatus[playerIndex] = mouse.getClicked();

                    // Update the buttonIndex array
                    buttonIndex[playerIndex] = mouse.getbuttonIndex();
                }
            }
        }

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


    private void OnTriggerStay(Collider other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            mouseStatus = GameObject.FindObjectOfType<NewVirtualMouse>();
            //NewVirtualMouse currentMouse = other.GetComponent<NewVirtualMouse>();

            //Debug.Log("Mouse inside me nnya!");
            indexRecived = mouseStatus.getindex(); //not importent just for testing
            buttonImage.color = new Color(0f, 0f, 0f);
            
            if (mouseStatus.getClicked())
            //if(currentMouse != null)
            //if (NewVirtualMouse.getClicked())

            {

                //for (int i = 0; i < 10; i++) //buttonIndex[i] != null
                //{
                //    if (buttonIndex[i] != -1)
                //    {
                        StartCoroutine(SpawnPrefabsWithDelay(1f));
                        //Prefab = availablePrefabs[buttonIndex[i]];
                        //Instantiate(Prefab, Vector3.zero, Quaternion.identity);
                //    }
                
                //}
            //HashSet<int> spawnedIndices = new HashSet<int>(); // 

            //for (int i = 0; i < buttonIndex.Length; i++)
            //{
            //    if (buttonIndex[i] > 0 && !spawnedIndices.Contains(buttonIndex[i]))
            //    {
            //        int prefabIndex = buttonIndex[i] - 1;

            //        if (prefabIndex > 0 && prefabIndex < availablePrefabs.Count)
            //        {
            //            GameObject Prefab = availablePrefabs[buttonIndex[i]];
            //            Instantiate(Prefab, Vector3.zero, Quaternion.identity);

            //            spawnedIndices.Add(buttonIndex[i]); // 
            //        }
            //    }
            //}
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("Mouse out");
            buttonImage.color = new Color(0f, 255f, 0f); // #00FF00
        }
        //mouseStatus = null;
    }

    //private IEnumerator SpawnPrefabsWithDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    //HashSet<int> spawnedIndices = new HashSet<int>(); // למנוע כפילויות
    //    //for (int i = 0; i < buttonIndex.Length; i++)
    //    //{
    //    //    if (buttonIndex[i] > 0 && !spawnedIndices.Contains(buttonIndex[i]))
    //    //    {
    //    //        int prefabIndex = buttonIndex[i] - 1;

    //    //        if (prefabIndex > 0 && prefabIndex < availablePrefabs.Count)
    //    //        {
    //    //            GameObject Prefab = availablePrefabs[buttonIndex[i]];
    //    //            Instantiate(Prefab, Vector3.zero, Quaternion.identity);

    //    //            spawnedIndices.Add(buttonIndex[i]); // מונע כפילויות
    //    //        }
    //    //    }
    //    //}

    //    for (int i = 0; i < 10; i++) //buttonIndex[i] != null
    //    {
    //        if (buttonIndex[i] != -1)
    //        {
    //            Prefab = availablePrefabs[buttonIndex[i]];
    //            yield return new WaitForSeconds(delay); // מחכה לפני הספאון

    //            //new
    //            //GameObject spawnedPrefab = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
    //            //DontDestroyOnLoad(spawnedPrefab);

    //            GameObject spawnedPrefab = Instantiate(availablePrefabs[buttonIndex[i]], Vector3.zero, Quaternion.identity);

    //            // העבר את ה-Prefab ל-DontDestroyOnLoad
    //            DontDestroyOnLoad(spawnedPrefab);
    //        }
    //    }
    //    //new
    //    yield return new WaitForSeconds(10f);
    //    SceneManager.LoadScene(scene2Name);
    //}
    private IEnumerator SpawnPrefabsWithDelay(float delay)
    {
        spawnedPrefabs.Clear();
        yield return new WaitForSeconds(delay);

        // וודא שיש לנו הורה קבוע
        if (persistentParent == null)
        {
            persistentParent = new GameObject("PersistentParent");
            DontDestroyOnLoad(persistentParent);
        }

        for (int i = 0; i < 10; i++)
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

    
}
