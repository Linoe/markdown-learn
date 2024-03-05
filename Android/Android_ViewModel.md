# Android_ViewModel 模板

`ViewModel` Android 區隔畫面與資料 讓資料持續存活一個背景模組
數據會以 `Flow` 相關類別與畫面溝通傳遞

<https://developer.android.com/topic/libraries/architecture/viewmodel?hl=zh-tw>

## 引用庫

`viewmodel` 根據使用範圍有不同引用庫
  `lifecycle-runtime-ktx` 繼承與使用 (androidx.lifecycle.ViewModel)
  `lifecycle-viewmodel-ktx` 繼承與使用 (androidx.lifecycle.ViewModel)
  `lifecycle-viewmodel-compose` @Composable 中使用 (androidx.lifecycle.viewmodel.compose.viewModel)

```js
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.6.2")
    implementation("androidx.lifecycle:lifecycle-viewmodel-ktx:2.6.2")
    implementation("androidx.lifecycle:lifecycle-viewmodel-compose:2.6.2")
```

## 生命週期
`ViewModel` 生命週期比 `Activity` 還要長 從程式啟動後 到結束才會被關閉

`onCreate()` `ViewModel` 存在是從首次存取時起
`onDestroy()` 直到 `Activity` 刪除為止。

## 基礎

`ViewModel` 外部撰寫繼承此的類別
```js
class MyViewModel: ViewModel()
```
將原本觀察用資料全部移至 `ViewModel` 類別
1. `StateFlow` 狀態流 觀察資料更新流的 
    ```js
    // 資料
    data class UiState(
        val score: Int = 0,
        val isOver: Boolean = false
    )
    // 模組
    class MyViewModel: ViewModel(){
        // 狀態流
        private val _state = MutableStateFlow(UiState())
        // 狀態流 提供外部存取
        val state:StateFlow<UiState> = _state.asStateFlow()
        // 內部數值使用
        var _score: Int = 0
        const val SCORE_MAX: Int = 10

        // 來自某處觸發 更新狀態流
        fun updateState(updateScore: Int) {
          _score += updateScore
          if(_score >=  SCORE_MAX)
            _state.update { currentState -> // 指向 UiState 資料結構
                currentState.copy(
                    score = _score
                    isOver = true
                )
          else
            _state.update { currentState -> // 指向 UiState 資料結構
                currentState.copy(
                    score = _score
                    isOver = false
                )
        }

    }
    // 外部存取
    @Composable
    fun MyScreen(myViewModel: MyViewModel = viewModel()) {
      val myState by myViewModel.state.collectAsState()
      // 資料變動就會更新畫面
      if (myState.isGameOver) {
        FinalScoreDialog(
            score = gameUiState.score
        )
      }else{
        Button(
            onClick = { myViewModel.updateState() } // 觸發更新資料
        ){
          Text(
              text = "${gameUiState.score}"
          )
        }
      }
    }
    ```

2. `State` 觀察資料更新畫面 
    ```js
    // 模組
    class MyViewModel: ViewModel(){
      // 提供外部更新畫面資料用
      var text by mutableStateOf("")
    }
    // 外部存取
    @Composable
    fun MyScreen(myViewModel: MyViewModel = viewModel()) {
      OutlinedTextField(
          value = myViewModel.text,
          onValueChange = { myViewModel.text = it } // 資料變動就會更新畫面
      )
    }
    ```


## 範例

建立
```js
data class DiceUiState(
    val firstDieValue: Int? = null,
    val secondDieValue: Int? = null,
    val numberOfRolls: Int = 0,
)

class DiceRollViewModel : ViewModel() {

    // Expose screen UI state
    private val _uiState = MutableStateFlow(DiceUiState())
    val uiState: StateFlow<DiceUiState> = _uiState.asStateFlow()

    // Handle business logic
    fun rollDice() {
        _uiState.update { currentState ->
            currentState.copy(
                firstDieValue = Random.nextInt(from = 1, until = 7),
                secondDieValue = Random.nextInt(from = 1, until = 7),
                numberOfRolls = currentState.numberOfRolls + 1,
            )
        }
    }
}
```

存取 分兩種
```js
// activity (androidx.activity:activity-ktx)
class DiceRollActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        // Create a ViewModel the first time the system calls an activity's onCreate() method.
        // Re-created activities receive the same DiceRollViewModel instance created by the first activity.

        // Use the 'by viewModels()' Kotlin property delegate
        // from the androidx.activity: activity-ktx artifact
        val viewModel: DiceRollViewModel by viewModels()
        lifecycleScope.launch {
            repeatOnLifecycle(Lifecycle.State.STARTED) {
                viewModel.uiState.collect {
                    // Update UI elements
                }
            }
        }
    }
}
// Composable (androidx.lifecycle:lifecycle-viewmodel-compose)
@Composable
fun ReplyApp(
    windowSize: WindowWidthSizeClass,
    modifier: Modifier = Modifier
){
    val navigationType: ReplyNavigationType
    val viewModel: ReplyViewModel = viewModel()

}
```

`ViewModel` 搭配協程
```js
//ViewModel 2.5.0 以上
class MyViewModel(
    private val coroutineScope: CoroutineScope =
        CoroutineScope(SupervisorJob() + Dispatchers.Main.immediate)
) : ViewModel() {

    // Other ViewModel logic ...

    override fun onCleared() {
        coroutineScope.cancel()
    }
}
// Lifecycle 2.5
class CloseableCoroutineScope(
    context: CoroutineContext = SupervisorJob() + Dispatchers.Main.immediate
) : Closeable, CoroutineScope {
    override val coroutineContext: CoroutineContext = context
    override fun close() {
        coroutineContext.cancel()
   }
}

class MyViewModel(
    private val coroutineScope: CoroutineScope = CloseableCoroutineScope()
) : ViewModel(coroutineScope) {
    // Other ViewModel logic ...
}
```

`ViewModel` object 工廠建立
```js
    class MyViewModel(
        private val myRepository: MyRepository,
        private val savedStateHandle: SavedStateHandle
    ) : ViewModel() {

        // ViewModel logic
        // ...

        // Define ViewModel factory in a companion object
        companion object {

            val Factory: ViewModelProvider.Factory = object : ViewModelProvider.Factory {
                @Suppress("UNCHECKED_CAST")
                override fun <T : ViewModel> create(
                    modelClass: Class<T>,
                    extras: CreationExtras
                ): T {
                    // Get the Application object from extras
                    val application = checkNotNull(extras[APPLICATION_KEY])
                    // Create a SavedStateHandle for this ViewModel from extras
                    val savedStateHandle = extras.createSavedStateHandle()

                    return MyViewModel(
                        (application as MyApplication).myRepository,
                        savedStateHandle
                    ) as T
                }
            }
        }
    }


class MyActivity : AppCompatActivity() {

    private val viewModel: MyViewModel by viewModels { MyViewModel.Factory }

    // Rest of Activity code
}

//吝一種
class MyViewModel(
    private val myRepository: MyRepository,
    private val savedStateHandle: SavedStateHandle
) : ViewModel() {
    // ViewModel logic

    // Define ViewModel factory in a companion object
    companion object {
        val Factory: ViewModelProvider.Factory = viewModelFactory {
            initializer {
                val savedStateHandle = createSavedStateHandle()
                val myRepository = (this[APPLICATION_KEY] as MyApplication).myRepository
                MyViewModel(
                    myRepository = myRepository,
                    savedStateHandle = savedStateHandle
                )
            }
        }
    }
}

//ViewModel 2.5.0 以下
class MyViewModel(
private val myRepository: MyRepository,
private val savedStateHandle: SavedStateHandle
) : ViewModel() {

// ViewModel logic ...

// Define ViewModel factory in a companion object
companion object {
    fun provideFactory(
        myRepository: MyRepository,
        owner: SavedStateRegistryOwner,
        defaultArgs: Bundle? = null,
    ): AbstractSavedStateViewModelFactory =
        object : AbstractSavedStateViewModelFactory(owner, defaultArgs) {
            @Suppress("UNCHECKED_CAST")
            override fun <T : ViewModel> create(
                key: String,
                modelClass: Class<T>,
                handle: SavedStateHandle
            ): T {
                return MyViewModel(myRepository, handle) as T
            }
        }
    }
}
class MyActivity : AppCompatActivity() {

    private val viewModel: MyViewModel by viewModels {
        MyViewModel.provideFactory((application as MyApplication).myRepository, this)
    }

    // Rest of Activity code
}
```

## 保存資料

`SavedStateHandle` 用來保存離線資料

```js
class SavedStateViewModel(private val state: SavedStateHandle) : ViewModel() { ... }

class MainFragment : Fragment() {
    val vm: SavedStateViewModel by viewModels()
    
    // 不確定
    fun try(){
      val name = vm.state.get("NAME", name)
      vm.state.set("NAME", name)
    }
}
// Test 單元
class MyViewModelTest {

    private lateinit var viewModel: MyViewModel

    @Before
    fun setup() {
        val savedState = SavedStateHandle(mapOf("someIdArg" to testId))
        viewModel = MyViewModel(savedState = savedState)
    }
}

```

`LiveData` 可觀察的資料持有者類別
<https://developer.android.com/topic/libraries/architecture/livedata>

```js
class SavedStateViewModel(private val savedStateHandle: SavedStateHandle) : ViewModel() {
    val filteredData: LiveData<List<String>> =
        savedStateHandle.getLiveData<String>("query").switchMap { query ->
        repository.getFilteredData(query)
    }

    fun setQuery(query: String) {
        savedStateHandle["query"] = query
    }
}
```

`StateFlow`

```js
class SavedStateViewModel(private val savedStateHandle: SavedStateHandle) : ViewModel() {
    val filteredData: StateFlow<List<String>> =
        savedStateHandle.getStateFlow<String>("query")
            .flatMapLatest { query ->
                repository.getFilteredData(query)
            }

    fun setQuery(query: String) {
        savedStateHandle["query"] = query
    }
}
```

`Compose`
```js
class SavedStateViewModel(private val savedStateHandle: SavedStateHandle) : ViewModel() {

    var filteredData: List<String> by savedStateHandle.saveable {
        mutableStateOf(emptyList())
    }

    fun setQuery(query: String) {
        withMutableSnapshot {
            filteredData += query
        }
    }
}
```

本機
```js
class TempFileViewModel : ViewModel() {
    private var tempFile: File? = null

    fun createOrGetTempFile(): File {
        return tempFile ?: File.createTempFile("temp", null).also {
            tempFile = it
        }
    }
}
//吝一個
private fun File.saveTempFile() = bundleOf("path", absolutePath)

class TempFileViewModel(savedStateHandle: SavedStateHandle) : ViewModel() {
    private var tempFile: File? = null
    init {
        savedStateHandle.setSavedStateProvider("temp_file") { // saveState()
            if (tempFile != null) {
                tempFile.saveTempFile()
            } else {
                Bundle()
            }
        }
    }

    fun createOrGetTempFile(): File {
        return tempFile ?: File.createTempFile("temp", null).also {
            tempFile = it
        }
    }
}
//還原
private fun File.saveTempFile() = bundleOf("path", absolutePath)

private fun Bundle.restoreTempFile() = if (containsKey("path")) {
    File(getString("path"))
} else {
    null
}

class TempFileViewModel(savedStateHandle: SavedStateHandle) : ViewModel() {
    private var tempFile: File? = null
    init {
        val tempFileBundle = savedStateHandle.get<Bundle>("temp_file")
        if (tempFileBundle != null) {
            tempFile = tempFileBundle.restoreTempFile()
        }
        savedStateHandle.setSavedStateProvider("temp_file") { // saveState()
            if (tempFile != null) {
                tempFile.saveTempFile()
            } else {
                Bundle()
            }
        }
    }

    fun createOrGetTempFile(): File {
      return tempFile ?: File.createTempFile("temp", null).also {
          tempFile = it
      }
    }
}

```