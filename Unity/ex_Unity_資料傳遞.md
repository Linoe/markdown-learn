# Unity_資料傳遞

紀錄資料傳遞方法

主要以下三種:
1. 使用 `static` 變數
2. 保存在不受場景轉換引響 `DontDestroyOnLoad`
3. 儲存在本地端
  + `PlayerPrefs`
  + 序列化資料(XML/JSON/Binary)保存 `FileIO`
4. `ScriptableObject` 資料容器

https://stackoverflow.com/questions/32306704/how-to-pass-data-and-references-between-scenes-in-unity

# static

class 直接使用 static 
```C#
//初始場景中掛載
public class MyStaticData
{
  static public int counter = 0;
  static float timer = 100;
  //包裝
  public int getTimer()
  {
    return MyStaticData.timer;
  }
}

//其他場景直接讀取
int i = MyStaticData.counter;

//包裝後使用
MyStaticData data = new MyStaticData();
data.getTimer();
```

# DontDestroyOnLoad

執行 `DontDestroyOnLoad` 使物件保存在不受場景轉換區域<br>
為了讓其他場景能保持讀取，需要 `static` 存放來源<br>

```C#
//建立靜態物件，用於所有場景單一取得
static public DontDestroyScript instance;
void Awake() 
{
    if(instance == null)
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);
    instance = this;
}

//另一種方法透過場景搜尋(不推薦)
GameObject.Find("Cube") // 物件 name
GameObject.FindObjectsOfType(typeof(DontDestroyScript)); //搜尋 class
GameObject.FindGameObjectsWithTag("Untagged");  //搜尋 tag
```

# PlayerPrefs

保存在本機端，以便讀取

```C#
int playerScore = 80;
void OnDisable()
{
    PlayerPrefs.SetInt("score", playerScore);
}
void OnEnable()
{
    playerScore = PlayerPrefs.GetInt("score");
}
```

# Serializable

資料轉換成序列，保存在本機端，以便讀取

```C#
[Serializable]
public class PlayerInfo
{
    public List<int> ID = new List<int>();
    public List<int> Amounts = new List<int>();
    public int life = 0;
    public float highScore = 0;
}

//持存使用 File.WriteAllBytes File.ReadAllBytes 
//相關製作參考下方
  PlayerInfo saveData = new PlayerInfo();
  saveData.life = 99;
  saveData.highScore = 40;

  DataSaver.saveData(saveData, "players");

//讀取
  PlayerInfo loadedData = DataSaver.loadData<PlayerInfo>("players");

//刪除
  DataSaver.deleteData("players");

```


```C#
public class DataSaver
{
    //Save Data
    public static void saveData<T>(T dataToSave, string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Convert To Json then to bytes
        string jsonData = JsonUtility.ToJson(dataToSave, true);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        }
        //Debug.Log(path);

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To PlayerInfo Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    //Load Data
    public static T loadData<T>(string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return default(T);
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return default(T);
        }

        //Load saved Json
        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
            Debug.Log("Loaded Data from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        string jsonData = Encoding.ASCII.GetString(jsonByte);

        //Convert to Object
        object resultValue = JsonUtility.FromJson<T>(jsonData);
        return (T)Convert.ChangeType(resultValue, typeof(T));
    }

    public static bool deleteData(string dataFileName)
    {
        bool success = false;

        //Load Data
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return false;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return false;
        }

        try
        {
            File.Delete(tempPath);
            Debug.Log("Data deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Delete Data: " + e.Message);
        }

        return success;
    }
}
```

# ScriptableObject

原本功能是製作保存在 `Assets` 資料
<br>在遊戲中可以暫時性生成儲存在檔案

如果要保存需要 `FileIO` 搭配保存在本機d

```C#

//建立資源檔案
//ProjectWindowで右クリック→[ Create ]→[ Click Count Scriptable Object ]

[CreateAssetMenu(fileName = "Data", menuName = "Examples/ClickCountScriptableObject")]
public class ClickCountScriptableObject : ScriptableObject
{
    [System.NonSerialized] public int ClickCount;
}

//渡す側
// Inspector 拖動 Assets 檔案連結
[SerializeField] private ClickCountScriptableObject clickCountScriptableObject;
clickCountScriptableObject.ClickCount++;
//受け取る
//任何場景中接收到的都會指向 Assets 檔案
[SerializeField] private ClickCountScriptableObject clickCountScriptableObject;
text.text = clickCountScriptableObject.ClickCount.ToString();


```