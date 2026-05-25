# Single responsibility principle 單一職責原則

一個類應該只有一個改變的理由,也就是它所承擔的單一責任

## 判斷方式
單一職責：
- 描述類別具有哪些實作行為，如果超過兩個以上行為，可能具備複數職責
- 這些行為歸納後判斷職責邊界，如果不能用統合名稱來描述，可能不符合單一職責

修改理由：
- 因為兩種不同領域的需求而修改，代表違反 SRP

```C#
// 壞例子，這個 Play 如果要修改聲音、移動、特效都必須直接修改這個腳本
public class UnrefactoredPlayer : MonoBehaviour
{
    [SerializeField] private string inputAxisName;
    [SerializeField] private float positionMultiplier;
    private float yPosition;
    private AudioSource bounceSfx;
    private void Start()
    {
        bounceSfx = GetComponent<AudioSource>();

    }
    private void Update()
    {
        float delta = Input.GetAxis(inputAxisName) * Time.deltaTime;
        yPosition = Mathf.Clamp(yPosition + delta, -1, 1);
        transform.position = new Vector3(transform.position.x, yPosition * positionMultiplier, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        bounceSfx.Play();
    }
}
```

```C#
// 好例子，這個 Play 僅負責容器職責，不具有實作子系統
[RequireComponent(typeof(PlayerAudio), typeof(PlayerInput), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerAudio playerAudio;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }
}
// 若需要修改聲音、移動、特效則會去修改子系統
public class PlayerAudio : MonoBehaviour { }
public class PlayerInput : MonoBehaviour { }
public class PlayerMovement : MonoBehaviour { }
```

## 其他判斷

- 可讀性:較短的課程內容更易閱讀。雖然沒有固定規則,但大多數情況下都是如此。 開發人員將代碼行數限制在200到300行之間。你可以自己或與團隊一起決定具體的數值。 當超過此臨界值時,即構成“過短”的情況。此時需判斷是否可以將其拆分成更小的部分。

- 可擴展性:您可以更輕鬆地從小型類別繼承功能。修改或替換這些類別時,也不必擔心會破壞原有的功能。

- 可重用性:將類別設計得簡潔且模組化,如此才能將其重用於遊戲的其他部分。