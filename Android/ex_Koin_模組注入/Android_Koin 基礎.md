# Android_Koin 基礎

`Koin` 讓程式在執行期間可以訪問模組化類別 主要使用在 `Kotlin` 與 `Android`

<https://insert-koin.io/>
<https://juejin.cn/post/7189917106580750395>

## 依賴庫

```js
dependencies {
    implementation ("io.insert-koin:koin-android:3.5.3") // 使用於 AppCompat 包含 ViewModel
    implementation ("io.insert-koin:koin-androidx-compose:3.5.3") // 使用於 Jetpack Compose
}
```

## 基礎

`Koin` 使用順序如下
1. 模組撰寫 `module`
    ```js
    val appModule = module {
        singleOf(::UserRepositoryImpl) { bind<UserRepository>() }
        factoryOf(::UserPresenter) // 另一種寫法 factory { MyPresenter(get()) }
    }
    ```
2. 啟動 `startKoin`
    ```js
    startKoin {
        modules(appModule) // 預先寫好的 Module
    }
    ```
3. 使用處注入 `by inject`
    ```js
    val presenter: UserPresenter by inject()
    ```

## Android Single

`AndroidManifest.xml` 對 `application` 設定對應的類別名稱
以便開啟程式時執行 `Koin` 設定
```xml
<application
    android:name=".MainApplication"
    ...
    >
    <activity
        android:name=".MainActivity"
        ...
        >
    </activity>
</application>
```

`module` 事先寫入預計使用的模組
  `singleOf` 單例模式 啟動後會保存到結束為止
  `factoryOf` 工廠模式 只有在呼叫才會生成 完成後自動移除
```js
// 模組撰寫
class Echo() {
    private var _t : String = ""

    fun setHello(text : String){
        _t = text
    }

    fun sayHello() : String{
        return _t
    }
}

// Koin 模組設定
val echoModule = module {
    singleOf(::Echo)
}
```

`startKoin` 啟動 `Koin` 程式的最早執行點呼叫
  `androidContext` 用來保存 Android `Context`
  `androidLogger` 啟動 Log 自動傳遞 Koin 相關訊息
  `modules` 注入預先寫好的模組
```js
class MainApplication : Application() {
    private val echo : Echo by inject() // by inject() 運行時取得 Koin 實例

    override fun onCreate() {
        super.onCreate()

        startKoin {
            androidContext(this@MainApplication)
            androidLogger() // logger(LEVEL.INFO) 啟用 log ?
            modules(echoModule) // 函數startKoin載入給定的模組列表
        }

        echo.setHello("Hello World!") // 取得的模組設定文字
    }
}
```

`by inject()` 從 Koin 取的對應的類別 直接當作一般變數使用
```js
class MainActivity : AppCompatActivity() {
    private val echo : Echo by inject() // by inject() 運行時取得 Koin 實例

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_simple)

        // 畫面文字元件更換文字
        findViewById<TextView>(R.id.text).setText("${echo.sayHello()}")
    }
}
```

## Android Factory

`Factory` 與 `Single` 差異在不會保留 主要作為包裝資料後的工具使用
依據上面範例修改 可以發現修改後字串沒被保留
```js
val echoModule = module {
  //singleOf(::Echo)
    factoryOf(::Echo)
}
```

以下一般使用方法
建立 `Factory` 用類別
```js
class EchoFactory (private val echo: Echo){

    fun setHello(text: String){
        echo.setHello(text)
    }

    fun sayHello():String {
        return echo.sayHello()
    }
}
```

`module` 事先寫入預計使用的模組
  `singleOf` 單例模式 啟動後會保存到結束為止
  `factoryOf` 工廠模式 只有在呼叫才會生成 完成後自動移除
```js
val echoModule = module {
    singleOf(::Echo)
    factoryOf(::EchoFactory)
}
```

`startKoin` 不變
`by inject` 改為只對 `Factory` 進行
```js
class MainApplication : Application() {

    private val echo : EchoFactory by inject()

    override fun onCreate() {
        super.onCreate()

        startKoin {
            androidContext(this@MainApplication)
            androidLogger()
            modules(echoModule)
        }

        echo.setHello("HelloWorld!")
    }
}
```

```js
class MainActivity : AppCompatActivity() {

    private val echo : EchoFactory by inject()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        findViewById<TextView>(R.id.text).setText(echo.sayHello())
    }
}
```

## Android ViewModel

依據上面範例修改
`ViewModel` 依照原本方式撰寫
```js
/**
 * Echo 自動從 singleOf(::Echo) 撈取
 */
class EchoViewModel(private val echo: Echo): ViewModel() {

    fun sayHello(): String{
        return echo.sayHello()
    }
}
```

`module` 增加 `viewModelOf` 對應的
```js
val echoModule = module {
    singleOf(::Echo)
    viewModelOf(::EchoViewModel)
}
```

`startKoin` 不變
取的 `Echo` 會直接與 `EchoViewModel` 連動
```js
class MainApplication : Application() {

    private val echo : Echo by inject()

    override fun onCreate() {
        super.onCreate()

        startKoin {
            androidContext(this@MainApplication)
            androidLogger()
            modules(echoModule)
        }

        echo.setHello("HelloWorld!")
    }
}
```

`by viewModel` 取的 `Koin` 的 `viewModel`
`viewModel` 來自 `import org.koin` 非 `Android` 原生
```js
//import org.koin.androidx.viewmodel.ext.android.viewModel
class MainActivity : AppCompatActivity() {

    private val echo : EchoViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        findViewById<TextView>(R.id.text).setText(echo.sayHello())
    }
}
```

## Android Compose

依據上面的範例 其他保持不變 僅修改以下

依賴庫
```js
    implementation("io.insert-koin:koin-androidx-compose:3.5.3")
```

`MainActivity` `Composable` 過程中直接注入
- `get()` org.koin 底下的 `get()` 目前不推薦使用 因為名稱重複性過高
- `koinInject()` 可以取的保存模組的物件
- `koinViewModel()` 可以取的保存模組的 `ViewModel`
```js
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            KoinComposeTheme {
                Surface(
                    modifier = Modifier.fillMaxSize()
                ) {
                    Column(
                      verticalArrangement = Arrangement.Center
                    ){
                        FactoryInject() // 示範 Factory
                        ViewModelInject() // 示範 ViewModel
                    }
                }
            }
        }
    }


//該get函數允許我們檢索 ViewModel 實例，為您建立關聯的 ViewModel Factory 並將其綁定到生命週期
    @Composable
    fun FactoryInject(
        presenter: EchoFactory = get() // koin 的 get() 自動注入對應資料
    ){
        Text(
          text = presenter.sayHello(),
          modifier = Modifier.padding(8.dp)
        )
    }

//該koinViewModel函數允許我們檢索 ViewModel 實例，為您建立關聯的 ViewModel Factory 並將其綁定到生命週期
    @Composable
    fun ViewModelInject(
        viewModel: EchoViewModel = koinViewModel()
    ){
        Text(
          text = viewModel.sayHello(),
          modifier = Modifier.padding(8.dp)
        )
    }
}
```


## 測試

依賴庫
```js
dependencies {
    testImplementation("io.insert-koin:koin-test-junit4:3.5.3")
}
```

`KoinTest` 繼承測試模組運行
```js
class CheckModulesTest : KoinTest {
    @Test
    fun checkAllModules() {
        appModule.verify()
    }
}
```