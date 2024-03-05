# Android_Activity

Android 用來執行頁面設定的程式

## Jetpack Compose

Android 2020 年後主力推行 View UI 撰寫
<https://developer.android.com/jetpack/compose/documentation?hl=zh-tw>

## Compose 方法顏色

Compose 方法會被標記 <font color="#6BB38A">綠色</font>
帶有此必須要在 `@Composable` 修飾 fun 執行

## 資料與畫面
`ViewModel` view 與 data 分離處理模組
`LiveData` 資料的容器
`LifecycleScope` Activity fragment 生命週期監聽模組

## 資料儲存方法

<https://developer.android.com/topic/libraries/architecture/lifecycle>
使用此
> `ViewModel` objects.

儲存方式
> Jetpack Compose: `rememberSaveable`
> Views: `onSaveInstanceState()`
> ViewModels: `SavedStateHandle`

對應引用庫名稱
`ViewModelScope` androidx.lifecycle:lifecycle-viewmodel-ktx:2.4.0
`LifecycleScope` androidx.lifecycle:lifecycle-runtime-ktx:2.4.0
`liveData` androidx.lifecycle:lifecycle-livedata-ktx:2.4.0

`ComponentActivity` 使用 Jetpack Compose 方式的布局
不使用 xml layer，僅使用程式碼進行布局

## Theme 風格

自動生成的 `Activity` 可以看到如下格式
除了 `MyAppTheme` 是由 `Theme.kt` 變數提供
`Theme` 的設定 [Android_Resoures](Android_Resoures%20%E8%B3%87%E6%BA%90.md) 查看

```js
import com.example.myapp.ui.theme.MyAppTheme
...
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MyAppTheme(darkTheme = true) {
              ...
            }
        }
    }
```
## 預覽

`@Preview` 修飾的方法會被預覽頁面 `Design` 執行
可以用此方式檢查程式問題

## 週期循環

生命週期
|   狀態   |           啟動            |     關閉      |              範圍              |
| :------: | :-----------------------: | :-----------: | :----------------------------: |
| Resumed  |       `onResume()`        |  `onPause()`  | 回到/脫離 APP 焦點(如分享視窗) |
| Started  | `onStart()` `onRestart()` |  `onStop()`   |     回到/脫離 APP(如桌面)      |
| Createed |       `onCreate()`        | `onDestroy()` |         開啟/關閉 App          |
|   狀態   |        initlaized         |   Destroyed   |                                |

啟動
+ `onCreate()` 系統創建 APP ， UI 布局入口點
+ `onStart()` APP 在螢幕上可見，用戶無法互動
+ `onRestart()` 僅當onStop()被呼叫且活動隨後重新啟動時才會呼叫它
+ `onResume()` APP 帶到前台，用戶可以互動

關閉
+ `onPause()` APP 放置後台，用戶無法互動
+ `onStop()` APP 在螢幕上不可見，用戶無法互動。
+ `onDestroy()` 系統銷毀 APP
  
呼叫用
+ `setContent()` 用來設定頁面參數，與結束後執行

## 標籤

+ `@Composable` 指定為 ui 建置方法，被修飾後只能在同為 `@Composable` 方法呼叫
  使用有參數 `content: @Composable () -> Unit` ，目的使方法合併
+ `@Preview` 如果執行在分割預覽頁面，在方法前修飾此
+ `@StringRes` 如果方法中，標記傳入參數為字串資源時，在參數前修飾此
+ `@DrawableRes` 如果方法中，標記傳入參數為圖片資源時，在參數前修飾此


## 視窗大小

|    大小類別     | 中斷點  |                          裝置佔比                          |
| :-------------: | :-----: | :--------------------------------------------------------: |
|  Compact width  | < 600dp |                   99.96% 直向模式的手機                    |
|  Medium width   | 600dp+  | 93.73% 直向模式的平板電腦、portrait模式的展開大型內部螢幕  |
| Expanded width  | 840dp+  | 97.22% 橫向模式的平板電腦、landscape模式的展開大型內部螢幕 |
| Compact height  | < 480dp |                   99.78% 橫向模式的手機                    |
|  Medium height  | 480dp+  |      96.56% 橫向模式的平板電腦、97.59% 直向模式的手機      |
| Expanded height | 900dp+  |                 94.25% 直向模式的平板電腦                  |

320dp：一般手機螢幕 (240x320 ldpi、320x480 mdpi、480x800 hdpi 等)
480dp：大型手機螢幕，約 5 吋 (480x800 mdpi)
600dp：7 吋平板電腦 (600x1024 mdpi)
720dp：10 吋平板電腦 (720x1280 mdpi、800x1280 mdpi 等)

```js
// 透過 material3-window-size-class 引用庫
val windowSize = calculateWindowSizeClass(this).widthSizeClass
  when (windowSize) {
      WindowWidthSizeClass.Compact -> 
      WindowWidthSizeClass.Medium -> 
      WindowWidthSizeClass.Expanded -> 
      else ->
  }
// 一般取得
private fun computeWindowSizeClasses() {
    val metrics = WindowMetricsCalculator.getOrCreate().computeCurrentWindowMetrics(this)

    val width = metrics.bounds.width()
    val height = metrics.bounds.height()
    val density = resources.displayMetrics.density
    val windowSizeClass = WindowSizeClass.compute(width/density, height/density)
    // COMPACT, MEDIUM, or EXPANDED
    val widthWindowSizeClass = windowSizeClass.windowWidthSizeClass
    // COMPACT, MEDIUM, or EXPANDED
    val heightWindowSizeClass = windowSizeClass.windowHeightSizeClass

    // Use widthWindowSizeClass and heightWindowSizeClass.
}
    private fun computeWindowSizeClasses() {
        val metrics = WindowMetricsCalculator.getOrCreate()
            .computeCurrentWindowMetrics(this)

        val widthDp = metrics.bounds.width() /
            resources.displayMetrics.density
        val widthWindowSizeClass = when {
            widthDp < 600f -> WindowSizeClass.COMPACT
            widthDp < 840f -> WindowSizeClass.MEDIUM
            else -> WindowSizeClass.EXPANDED
        }

        val heightDp = metrics.bounds.height() /
            resources.displayMetrics.density
        val heightWindowSizeClass = when {
            heightDp < 480f -> WindowSizeClass.COMPACT
            heightDp < 900f -> WindowSizeClass.MEDIUM
            else -> WindowSizeClass.EXPANDED
        }

        // Use widthWindowSizeClass and heightWindowSizeClass.
    }
```

## 常用

+ `NumberFormat` 數字轉換成特定貨幣格式

+ `painterResource()` 讀取圖片資源檔，使用參數 id = R.drawable.android_image
+ `stringResource()` 讀取字串資源檔，使用參數 id = R.string.android_text
+ `dimensionResource()` 讀取尺寸資源檔，使用參數 id = R.dimen.android_dimen
+ `LocalContext.current.getString()` 讀取字串資源檔，可以使用在非 @Composable 方法內?

`BackHandler` 設置系統返回監聽
```js
BackHandler {
    onBackPressed()
}
```

### remember 保存數值

`remember()` 儲存對象用，畫將初始值保存， ui 重組時返回值
`mutableStateOf()` 偵測並數值發勝變化時，刷新 ui ，通常搭配 remember

`remember` 可用來儲存可變動與不可變動的物件 且在重新組成時傳回所儲存的值 達到數值變更的效果
以下的寫法 都能達到相同效果
```js
// 存取數值本身 利用 by 的特性
var value by remember { mutableStateOf(default) }
// 存取 mutableStateOf 類別
val mutableState = remember { mutableStateOf(default) } // mutableState.value 取得數值
val (value, setValue) = remember { mutableStateOf(default) }
```

使用 `by` 需要引用
```js
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
```

其他保存數值方法
`remember` 可協助您在各次重組間保留狀態，但只要設定有所變更，狀態就無法保留
`rememberSaveable` 會自動儲存可儲存在 Bundle 中的任何值。其他值可在自訂儲存器物件中傳送。

`ArrayList<T>` 或 `mutableListOf()` 作為儲存對象可能無法觸發重組
`State<List<T>>` 和 `不可變動的 listOf()` 使用可觀察的資料容器

錯誤示範
```js
//錯誤 每次 UI 刷新時會被生成新物件
@SuppressLint("UnrememberedMutableState") //未使用 remember 強制要求修飾
  var amountInput:MutableState<String> = mutableStateOf("0")
  TextField(
      value = amountInput.value,
      onValueChange = {amountInput.value = it})

// remember 將數值保存在靜態變數
  var amountInput by remember { mutableStateOf("") }
  TextField(
      value = amountInput,
      onValueChange = {amountInput = it})
```

### Log 訊息

`Log.d()` 後台輸出字串
```js
Log.d("Tag", "msg")
```

### LocalContext 當前內容

`LocalContext.current` 取得當前使用 `Activity`
```js
@Composable
private fun FinalScoreDialog() {
    val activity = (LocalContext.current as Activity)
    
    activity.finish()
}

```

### WindowCompat 視窗兼容

`WindowCompat` 用於與 `Window` API 進行溝通工具

```js
// 設定 App 視窗範圍系統視窗大小(整體畫面)
WindowCompat.setDecorFitsSystemWindows(window, false)
```


### Intent 呼叫應用程式

`Intent` 建立呼叫用包
`Intent(Intent.ACTION_SEND)` 建立指定外傳訊息動作包
`Intent.type` 指定內容屬性
`Intent.putExtra()` 放入指定屬性

```js
private fun shareOrder(context: Context, subject: String, summary: String) {
    // Create an ACTION_SEND implicit intent with order details in the intent extras
    val intent = Intent(Intent.ACTION_SEND).apply {
        type = "text/plain"
        putExtra(Intent.EXTRA_SUBJECT, subject)  // 副標題
        putExtra(Intent.EXTRA_TEXT, summary) // 內容文字
    }
    context.startActivity(
        Intent.createChooser(
            intent,
            context.getString(R.string.new_cupcake_order)
        )
    )
}
```

`context.startActivity()` 啟動一個 Activity() 此用來啟動傳遞其他應用程式
`Intent.createChooser()` 依據 Intent 參數決定啟動的動作?
```js
    context.startActivity(
        Intent.createChooser(
            intent,
            context.getString(R.string.new_cupcake_order)
        )
    )
```