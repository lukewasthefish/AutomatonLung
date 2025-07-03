using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

[RequireComponent(typeof(InputManager))]
public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public GameObject bullet;

    public RenderTexture gameRenderTexture;
    //End inspector

    [HideInInspector] public string destinationScene;
    public int previousElevatorFloorNumber;
    [HideInInspector] public int destinationPlayerStart = 0; //Which playerstart to send the player to at the beginning of a level

    [HideInInspector] public ThirdPersonCharacterMovement thirdPersonCharacterMovement;

    [HideInInspector] public EnemySpawnManager enemySpawnManager;

    [HideInInspector] public int currentWeaponIndex = 0;

    //private variables
    private GameManager[] otherGameManager;

    private bool isRealGameManager = false;

    [HideInInspector]public ObjectPool bulletPool;

    [HideInInspector]public List<Collectible> allCollectiblesInCurrentScene = new List<Collectible>();

    //Save data
    [HideInInspector] public int chipCount = 0; //How many chips has the player collected accross the entire game?
    [HideInInspector] public float maxPlayerHealth = 10;
    [HideInInspector] public bool[] weaponsUnlocked = {true, false, false, false, false};
    public bool[,] wornClothing = new bool[NUM_CLOTHINGCATEGORIES, FASHION_ITEMS_PER_CATEGORY];
    public bool[,] clothingUnlocked = new bool[NUM_CLOTHINGCATEGORIES, FASHION_ITEMS_PER_CATEGORY];
    private bool hasSaveDataBeenInitialized = false;

    [HideInInspector]public GameObject fakeFog;

    //Debug / Cheat values
    [Header("Cheat code bools")]
    public bool flightMode = false;
    public bool invincible = false;
    public bool bobbleHead = false;
    public bool removeSky = false;
    public bool fasterFlight = false;
    public bool autocompleteMode = false;

    [HideInInspector]
    public bool canAchieveAchievements = true;

    [HideInInspector]public MusicPlayer currentMusicPlayer;

    private bool isPaused = false;

    private InputManager inputmanager;

    [HideInInspector] public PlayerCombat playerCombat;

    private string currentSaveDataFilePath = null;

    private const int NUM_CLOTHINGCATEGORIES = 6;
    private const int FASHION_ITEMS_PER_CATEGORY = 10;

    public SaveData CurrentSaveData { get; set; } = null;
    public int CurrentSaveFileIndex { get; set; } = 1;

    //Options
    [HideInInspector]public bool musicEnabled = true;
    [HideInInspector]public bool invertCameraVertical = false;
    [HideInInspector]public bool invertCameraHorizontal = false;
    public bool useOriginalResolution = false;
    
    [HideInInspector]
    public int DesiredScreenWidth = 1920;
    [HideInInspector]
    public int DesiredScreenHeight = 1080;

    [HideInInspector]
    public FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;

    [HideInInspector]
    public int framerate = 60; //Frames per second
    [HideInInspector]
    public int mouseSensitivity = 50;

    public ActualCameraIdentifier actualCameraPrefab;

    private int scenesLoadedAfterTitle = 0; //Increments as the game progresses. With this number we know that 0 = title menu.

    [HideInInspector]
    public BeetleFlight beetleFlight;

    [HideInInspector]
    public int desiredMonitor = -1;

    private void Awake()
    {
        if(GameManager.Instance)
            GameManager.Instance.Set3dsResolution();
        DetermineTrueGameManager();

        if(this != Instance)
        {
            return;
        }

        //First do 3ds resolution camera check
        ActualCameraIdentifier acl = FindObjectOfType<ActualCameraIdentifier>();
        if (!acl)
        {
            acl = Instantiate(actualCameraPrefab);
            acl.transform.position = Vector3.zero;
        }

        if (Application.targetFrameRate < 12) //Lowest possible framerate with hacking
        {
            //Another reason why I'm leaving this conditional here is because I want the game to recover in case a value of 0 somehow gets loaded from iffy saved options data
            UnityEngine.Debug.Log("12 is the limit for the lowest framerate of this program. Sorry if you were tying to set it to 11 or below.");
            Application.targetFrameRate = 60; //Putting it back to 60. 
        }

        this.inputmanager = this.GetComponent<InputManager>();
        this.allCollectiblesInCurrentScene = new List<Collectible>();

        if (!this.hasSaveDataBeenInitialized)
        {
            InitializeSaveData();
            this.hasSaveDataBeenInitialized = true;
        }

        this.enemySpawnManager = null;

        AssignMissingPlayerTransform();
        if (bulletPool == null)
        {
            CreateBullets();
        }

        if(this.desiredMonitor != -1)
            PlayerPrefs.SetInt("UnitySelectMonitor", this.desiredMonitor);
        allCollectiblesInCurrentScene.Clear();
    }

    private void Start()
    {
        if(this != Instance)
        {
            return;
        }

        GameManager.Instance.LoadOptions();
        GameManager.Instance.ScaleResolution();
    
        GameManager.Instance.Set3dsResolution();

        if(this.desiredMonitor != -1)
            PlayerPrefs.SetInt("UnitySelectMonitor", this.desiredMonitor);


        if (!this.hasSaveDataBeenInitialized)
        {
            InitializeSaveData();
            this.hasSaveDataBeenInitialized = true;
        }
    }

    public void Set3dsResolution()
    {
        if(this.desiredMonitor != -1)
            PlayerPrefs.SetInt("UnitySelectMonitor", this.desiredMonitor);

        UnityEngine.Debug.Log($"Use original resolution : {useOriginalResolution}");
        ActualCameraIdentifier actualCamera = FindObjectOfType<ActualCameraIdentifier>(true);

        if (!actualCamera)
        {
            return;
        }

        //Apply or remove original 3ds resolution here
        if (!useOriginalResolution)
        {
            actualCamera.GetComponent<Camera>().targetTexture = null;
            actualCamera.GetComponent<Camera>().enabled = false;

            Camera[] cameras = FindObjectsOfType<Camera>(true);
            foreach (Camera cam in cameras)
            {
                if (cam.GetComponent<DisableThisCameraLighting>() || cam.GetComponent<ActualCameraIdentifier>())
                {
                    continue;
                } else
                {
                    UnityEngine.Debug.Log($"{cam.name} had its target texture removed");
                    cam.targetTexture = null;
                }
            }
        } else
        {
            Camera[] cameras = FindObjectsOfType<Camera>(true);
            foreach (Camera cam in cameras)
            {
                if (cam.GetComponent<DisableThisCameraLighting>() || cam.GetComponent<ActualCameraIdentifier>())
                {
                    continue;
                }

                cam.targetTexture = gameRenderTexture;
            }

            actualCamera.GetComponent<Camera>().enabled = true;
        }

        if(this.desiredMonitor != -1)
            PlayerPrefs.SetInt("UnitySelectMonitor", this.desiredMonitor);
    }

    public InputManager GetPlayerInputManager()
    {
        return inputmanager;
    }

    public void ScaleResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        Resolution closestToCurrentResolution = resolutions[0];

        int lowestWidthDifference = Mathf.Abs( closestToCurrentResolution.width - Screen.width );
        int lowestHeightDifference = Mathf.Abs( closestToCurrentResolution.height - Screen.height );

        bool resolutionSupported = false;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].width == GameManager.Instance.DesiredScreenWidth && resolutions[i].height == GameManager.Instance.DesiredScreenHeight)
            {
                resolutionSupported = true;
            }

            int widthDifference = Mathf.Abs(resolutions[i].width - Screen.width);
            int heightDifference = Mathf.Abs(resolutions[i].height - Screen.height);

            if(widthDifference < lowestWidthDifference && heightDifference < lowestHeightDifference)
            {
                closestToCurrentResolution = resolutions[i];
            }
        }

        if (!resolutionSupported)
        {
            GameManager.Instance.DesiredScreenWidth = closestToCurrentResolution.width;
            GameManager.Instance.DesiredScreenHeight = closestToCurrentResolution.height;
        }

        //Steam Deck has its special case
        bool isOnSteamDeck = IsOnSteamDeck();
        if(isOnSteamDeck)
        {
            GameManager.Instance.DesiredScreenWidth = 1280;
            GameManager.Instance.DesiredScreenHeight = 800;
            GameManager.Instance.desiredMonitor = 0;
            GameManager.Instance.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        bool fullscreen = true; //UNITY BUG : if we create this field as a FullScreenMode enum and pass it in below, the game crashes. So, don't do that. Doing it this way for a reason.

        if(!isOnSteamDeck)
        {
            switch(fullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                fullscreen = true;
                break;
                case FullScreenMode.FullScreenWindow:
                fullscreen = true;
                break;
                case FullScreenMode.MaximizedWindow:
                fullscreen = true;
                break;
                case FullScreenMode.Windowed:
                fullscreen = false;
                break;
            }
        }
        if(this.desiredMonitor != -1)
            PlayerPrefs.SetInt("UnitySelectMonitor", this.desiredMonitor);

        Screen.SetResolution(DesiredScreenWidth, DesiredScreenHeight, fullscreen);

        if(this.desiredMonitor != -1)
            PlayerPrefs.SetInt("UnitySelectMonitor", this.desiredMonitor);
    }

    private void AssignMissingPlayerTransform()
    {
        GameManager.Instance.thirdPersonCharacterMovement = FindObjectOfType<ThirdPersonCharacterMovement>(true);
        GameManager.Instance.playerCombat = FindObjectOfType<PlayerCombat>(true);

        GameManager.Instance.beetleFlight = FindObjectOfType<BeetleFlight>(true);

        if (GameManager.Instance.beetleFlight)
        {
            if (fasterFlight)
            {
                GameManager.Instance.beetleFlight.topSpeed *= 10f;
            }
        }
    }

    /// <summary>
    /// Determines the true original GameManager instance and removes other GameManager objects from the scene.
    /// </summary>
    private void DetermineTrueGameManager(){
        otherGameManager = GameObject.FindObjectsOfType<GameManager>(true);

        for(int i = 0; i < otherGameManager.Length; i++){
            if (otherGameManager[i] != null && otherGameManager[i] != this && otherGameManager[i].isRealGameManager == false)
            {
                Destroy(otherGameManager[i].gameObject);
            }
        }

        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            UnityEngine.Debug.Log("GameManager Instance assigned");
            Instance = this;
            this.isRealGameManager = true;
        } else
        {
            Destroy(this.gameObject);
        }
    }

    private void CreateBullets(){
        bulletPool = this.gameObject.AddComponent<ObjectPool>();
        bulletPool.objectToPool = this.bullet;
        bulletPool.numberOfObjectInstances = 16; //HARDCODED number of bullets to create
    }

    private void InitializeSaveData()
    {
        CurrentSaveData = new SaveData();

        weaponsUnlocked = new bool[] { true, false, false, false, false };
    }

    public void LoadOptions()
    {
        SaveData optionsSaveData = OptionsFileHandler.LoadOptions();

        if (optionsSaveData == null) //Instead uses the options values as they are by default
        {
            UnityEngine.Debug.Log("Options does not exist yet");
            return;
        }

        UnityEngine.Debug.Log("Loading options into GameManager");

        if (optionsSaveData.ContainsKey("FPS"))
        {
            framerate = optionsSaveData.GetInt("FPS");
            //NOTE : Vsync in unity needs to be set to 0 for this framerate setting to be applied properly. Order matters.
            Application.targetFrameRate = framerate;
        }

        if(optionsSaveData.ContainsKey("vSyncCount"))
            QualitySettings.vSyncCount = optionsSaveData.GetInt("vSyncCount");

        if(optionsSaveData.ContainsKey("invertVertical"))
            invertCameraVertical = optionsSaveData.GetBool("invertVertical");

        if(optionsSaveData.ContainsKey("invertHorizontal"))
            invertCameraHorizontal = optionsSaveData.GetBool("invertHorizontal");

        if(optionsSaveData.ContainsKey("musicEnabled"))
            musicEnabled = optionsSaveData.GetBool("musicEnabled");

        if (optionsSaveData.ContainsKey("use3dsres"))
            useOriginalResolution = optionsSaveData.GetBool("use3dsres");

        if (optionsSaveData.ContainsKey("mouseSensitivity"))
        {
            mouseSensitivity = optionsSaveData.GetInt("mouseSensitivity");

            UnityEngine.Debug.Log($"Loaded mouseSensitivity : {mouseSensitivity}");
        }

        //SetWindowModeFromOptions
        if(optionsSaveData.ContainsKey("WindowMode"))
        {
            string windowModeString = optionsSaveData.GetString("WindowMode");

            //String to enum
            switch(windowModeString)
            {
                case "ExclusiveFullScreen":
                this.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

                case "FullScreenWindow":
                this.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

                case "Windowed":
                this.fullScreenMode = FullScreenMode.Windowed;
                break;

                case "MaximizedWindow":
                this.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            }
            
            Screen.fullScreenMode = this.fullScreenMode;
        }

        GameManager.Instance.SetLanguage();

        scenesLoadedAfterTitle++; 

    }

    public void LoadSaveData()
    {
        InitializeSaveData();

        currentSaveDataFilePath = "saves/save" + CurrentSaveFileIndex.ToString();
        CurrentSaveData = SaveFileUtils.LoadSaveFile(currentSaveDataFilePath);

        if (CurrentSaveData == null)
        {
            UnityEngine.Debug.Log("new SaveData object created");
            CurrentSaveData = new SaveData();
        }

        if (CurrentSaveData.ContainsKey("ChipCount"))
        {
            chipCount = CurrentSaveData.GetInt("ChipCount");
        } else
        {
            chipCount = 0;
        }

        if (CurrentSaveData.ContainsKey("destinationPlayerStart"))
        {
            destinationPlayerStart = CurrentSaveData.GetInt("destinationPlayerStart");
        } else
        {
            destinationPlayerStart = 0;
        }

        if (CurrentSaveData.ContainsKey("screenWidth"))
        {
            DesiredScreenWidth = CurrentSaveData.GetInt("screenWidth");
        }

        if (CurrentSaveData.ContainsKey("screenHeight"))
        {
            DesiredScreenHeight = CurrentSaveData.GetInt("screenHeight");
        }
        
        for (int i = 0; i < weaponsUnlocked.Length; i++)
        {
            string key =  "weaponUnlocked" + i;
            
            // UnityEngine.Debug.Log($"Loading weapon unlocked status at index {i} with value {CurrentSaveData.GetBool(key)}");
            weaponsUnlocked[i] = CurrentSaveData.GetBool(key);
        }

        weaponsUnlocked[0] = true; //Initial weapon must always be unlocked
        currentWeaponIndex = 0; //Initialize equipped weapon to default
    }

    public int GetChipCountFromSaveFileIndex(int saveFileIndex)
    {
        SaveData currentIndexSaveData = SaveFileUtils.LoadSaveFile("saves/save" + saveFileIndex.ToString());

        if (currentIndexSaveData != null)
        {
            return currentIndexSaveData.GetInt("ChipCount");
        }

        return 0;
    }

    private void CheckSteamAchievements()
    {
        if(!canAchieveAchievements || !SteamManager.Initialized)
        {
            return;
        }

        int weaponsUnlockedCount = 0;
        for(int i = 0; i < GameManager.Instance.weaponsUnlocked.Length; i++)
        {
            if(GameManager.Instance.weaponsUnlocked[i] == true)
            {
                weaponsUnlockedCount++;
            }
        }

        if(weaponsUnlockedCount >= 5 && Steamworks.SteamUserStats.GetAchievement("Achievement_AllWeapons", out bool achieved) && achieved)
        {
            Steamworks.SteamUserStats.SetAchievement("Achievement_AllWeapons");
        }

        if (GameManager.Instance.chipCount >= 210)
        {
            Steamworks.SteamUserStats.SetAchievement("Achievement_AllChips");
        }

        Steamworks.SteamUserStats.StoreStats();
    }

    private void Update()
    {
        if(this != Instance)
        {
            return;
        }

        if(!this.hasSaveDataBeenInitialized)
        {
            InitializeSaveData();
            this.hasSaveDataBeenInitialized = true;
        }
        

        //Enable / disable mouse cursor
        bool useMouseCursor = (GameManager.Instance.GetIsPaused() || GameManager.Instance.thirdPersonCharacterMovement == null) && !IsOnSteamDeck();
        Cursor.visible = useMouseCursor;
        if(useMouseCursor)
        {
            Cursor.lockState = CursorLockMode.None;
        } else 
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (thirdPersonCharacterMovement == null)
        {
            ThirdPersonCharacterMovement thirdPersonCharacterMovement = FindObjectOfType<ThirdPersonCharacterMovement>();

            if (SceneManager.GetActiveScene().name.Contains("Floor")) {
                string floorNumberString = "";

                string currentSceneName = SceneManager.GetActiveScene().name;
                for (int i = 0; i < currentSceneName.Length; i++) {
                    if (char.IsDigit(currentSceneName[i])) {
                        floorNumberString += currentSceneName[i];
                    }
                }

                this.previousElevatorFloorNumber = int.Parse(floorNumberString);
            }

            if (thirdPersonCharacterMovement)
            {
                GameManager.Instance.thirdPersonCharacterMovement = thirdPersonCharacterMovement;
                PlayerCombat pc = thirdPersonCharacterMovement.GetComponent<PlayerCombat>();
                if (GameManager.Instance.CurrentSaveData.ContainsKey("maxHealth")) {
                    pc.maxHealth = GameManager.Instance.CurrentSaveData.GetFloat("maxHealth");
                    pc.currentHealth = pc.maxHealth;
                }
            }
        }
    }

    public void FashionUpdate()
    {
        for (int x = 0; x < clothingUnlocked.GetLength(0); x++)
        {
            for (int y = 0; y < clothingUnlocked.GetLength(1); y++)
            {
                if(GameManager.Instance.CurrentSaveData.GetBool( SaveFileUtils.GetFashionItemUnlockedSaveKey(x, y) ))
                {
                    clothingUnlocked[x, y] = true;
                } else
                {
                    clothingUnlocked[x, y] = false;
                }

                UnityEngine.Debug.Log($"Unlock status of clothing at {x},{y} = {clothingUnlocked[x,y]}");
            }
        }

        for (int x = 0; x < wornClothing.GetLength(0); x++)
        {
            for(int y = 0; y < wornClothing.GetLength(1); y++)
            {
                if (GameManager.Instance.CurrentSaveData.GetBool( SaveFileUtils.GetFashionItemWornSaveKey(x, y) ))
                {
                    wornClothing[x, y] = true;
                } else
                {
                    wornClothing[x, y] = false;
                }

                UnityEngine.Debug.Log($"Currently worn status of clothing at {x},{y} = {wornClothing[x, y]}");
            }
        }
    }

    private void ClearScene()
    {
        GameManager.Instance.thirdPersonCharacterMovement = null;
        GameManager.Instance.beetleFlight = null;
        GameManager.Instance.enemySpawnManager = null;
        allCollectiblesInCurrentScene.Clear();
        otherGameManager = new GameManager[0];
        bulletPool.Clear();

        if (currentMusicPlayer != null)
        {
            currentMusicPlayer.volumeDestination = 0f;
        }

        if (Camera.main)
        {
            Camera.main.transform.parent = null;
        }

        if (fakeFog)
            Destroy(fakeFog.gameObject);

        GameObject[] root = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < root.Length; i++)
        {
            GameObject go = root[i];
            if (go.GetComponent<Camera>())
            {
                Transform[] children = go.GetComponentsInChildren<Transform>();
                foreach(Transform t in children)
                {
                    if (!t.GetComponent<AudioSource>() && !t.GetComponent<Camera>())
                    {
                        t.parent = null;
                        Destroy(t.gameObject);
                    }
                }
            }

            if (!go.GetComponent<Camera>() && !go.GetComponent<GameManager>() && !go.GetComponent<MusicPlayer>())
            {
                Destroy(go);
            }
        }
    }

    public void SetDestinationScene(string sceneName)
    {
        GameManager.Instance.destinationScene = sceneName;
    }

    public void LoadSceneImmediate(Scene sceneToLoad)
    {
        LoadSceneImmediate(sceneToLoad.name);
    }

    public void LoadSceneImmediate(string sceneName)
    {
        SetDestinationScene(sceneName);
        ClearScene();
        Loading();
    }

    public void LoadAfterDelay(float delayAmmount, Scene sceneToLoad)
    {
        LoadAfterDelay(delayAmmount, sceneToLoad);
    }

    public void LoadAfterDelay(float delayAmmount, string sceneName)
    {
        SetDestinationScene(sceneName);
        ClearScene();
        GameManager.Instance.SetIsPaused(false);
        Invoke("Loading", delayAmmount);
    }

    /// <summary>
    /// Exit the current scene and enter the loading screen between scenes. The destinationScene string of this GameManager must be assigned properly before calling.
    /// </summary>
    private void Loading()
    {
        GameManager.Instance.SetIsPaused(false);

        if (currentMusicPlayer)
        {
            currentMusicPlayer.GetComponent<AudioSource>().clip = null;
            Destroy(currentMusicPlayer.gameObject);
            currentMusicPlayer = null;
        }

        Resources.UnloadUnusedAssets();

        foreach(Camera c in FindObjectsOfType<Camera>())
        {
            c.backgroundColor = Color.black;
        }

        StartCoroutine(GameManager.Instance.ChangeScene("LoadingScene"));
    }

    /// <summary>
    /// Assign the number of remaining collectibles in the scene to a save file variable unique to the current scene.
    /// This is only used / useful for the elevator floor scenes since Towns will have collectibles strewn over multiple scenes.
    /// </summary>
    public void CheckIfSceneCompleted(){

        string sceneName = SceneManager.GetActiveScene().name;
        // string sceneName = "Floor 0";

        //Above all else do not let this sceneName have both Town and Floor.
        if(sceneName.Contains("Floor") && sceneName.Contains("Town")){
            UnityEngine.Debug.LogWarning("Something is seriously wrong with this scene name! You should not name your scene with 'Town' and 'Floor' in the same scene name.");
            return;
        }

        //Towns are comprised of the outside areas the player may fly to during the course of their journey.
        //Elevator floors are accessed in the central tower in the middle of the world and each floor can be accessed from the elevator of that tower.

        //Elevator floors
        if(!sceneName.Contains("Floor")){
            return; //Don't continue the rest of this code if we're not on an elevator floor
        }

        //If this current scene is an elevator floor we need to check how many collectibles are remaining
        int remainingCollectibleCount = 0;
        if(allCollectiblesInCurrentScene != null && allCollectiblesInCurrentScene.Count > 0)
        {
            for(int i = 0; i < allCollectiblesInCurrentScene.Count; i++)
            {
                if(allCollectiblesInCurrentScene[i] && !allCollectiblesInCurrentScene[i].hasBeenCollected)
                {
                    remainingCollectibleCount++;
                }
            }
        }

        //Assign the number of remaining collectibles to the appropriate variable
        GameManager.Instance.CurrentSaveData.SetValue(sceneName + "remainingItems", remainingCollectibleCount);
    }

    public IEnumerator ChangeScene(string sceneName)
    {
        CheckSteamAchievements();
        AsyncOperation ao = Resources.UnloadUnusedAssets();

        while (!ao.isDone)
        {
            yield return null;
        }
        System.GC.Collect();
        GC.WaitForPendingFinalizers();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        Debug.Log("Load Level " + sceneName);

        do
        {
            // 0.9f is a hardcoded magic number inside the SceneManager API
            // ... this is OFFICIAL! Not a hack!
            if (asyncOperation.progress >= 0.9f - 0.0001f)
            {
                yield return new WaitForEndOfFrame();

                asyncOperation.allowSceneActivation = true;
            }

            yield return null;

            Debug.Log("asyncOp: " + asyncOperation.isDone + " progress: " + asyncOperation.progress);
        } while (!asyncOperation.isDone); // note: this will crash at 0.9 if you're not careful


        Debug.Log("Loading loop finished");

        yield return new WaitForEndOfFrame();
    }

    public int GetDestinationPlayerStart()
    {
        return destinationPlayerStart;
    }

    public void SetDestinationPlayerStart(int val)
    {
        destinationPlayerStart = val;

        if(GameManager.Instance.CurrentSaveData != null)
        GameManager.Instance.CurrentSaveData.SetValue("destinationPlayerStart", destinationPlayerStart);
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public bool TogglePaused()
    {
        if (GameManager.Instance.GetIsPaused())
        {
            FreeCameraMovement freeCameraMovement = FindObjectOfType<FreeCameraMovement>(true);
            if(freeCameraMovement != null)
            {
                freeCameraMovement.DisableFreecam();
            }

            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

        GameManager.Instance.isPaused = !GameManager.Instance.isPaused;


        return GameManager.Instance.isPaused;
    }

    public void SetIsPaused(bool newPausedValue)
    {
        if (newPausedValue) //If is paused
        {
            if (GameManager.Instance.GetIsPaused())
            {
                //do nothing
            } else
            {
                TogglePaused();
            }
        } else //If is not paused
        {
            if (GameManager.Instance.GetIsPaused())
            {
                TogglePaused();
            } else
            {
                //do nothing
            }
        }
    }

    public void Save()
    {
        SaveFileUtils.WriteSaveToFile(this.currentSaveDataFilePath, this.CurrentSaveData);
    }

    public bool IsOnSteamDeck()
    {
        return SystemInfo.operatingSystem.Contains("SteamOS");
    }

    /// <summary>
    /// Sets language from options
    /// </summary>
    public void SetLanguage()
    {
        SaveData optionsSaveData = OptionsFileHandler.LoadOptions();

        if (optionsSaveData == null) //Instead uses the options values as they are by default
        {
            UnityEngine.Debug.Log("Options data for language does not exist yet");
            return;
        }

        if (optionsSaveData.ContainsKey("language"))
        {
            string language = optionsSaveData.GetString("language");
            foreach(Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if(locale.name.Contains(language))
                {
                    LocalizationSettings.SelectedLocale = locale;
                }
            }
        } else 
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
        }
    }
}