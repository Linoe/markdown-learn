## Android_AndroidTest 實機測試

Android 直接實機上進行程式碼測試
效果等同直接運行 App 
差異在於可以用程式碼的代替輸入
> JUnit 不同在會必須撰寫在 Class 中

## 建立

撰寫前先建置相關資料夾與檔案
1. 切換成 `projcet`
2. app/src 中建立資料夾 `androidTest/java` ，編輯器自動偵測成 androidTest
3. 建立 `Package` (com.example.test)， 建立測試用檔案 new > `Kotlin Class/File`

## 測試步驟

完成代碼後
1. class/fun 左側會出現可執行的圖示
2. 點擊後開始執行測試用程式碼(沒有啟用先開始模擬器)
3. 依據程式碼顯示畫面與輸入數值
4. 結果會顯示在下方控制台

## 測試代碼

模擬畫面取得測試用 Compose
```js
class TipUITests {
  @get:Rule
  val composeTestRule = createComposeRule()
  // val composeTestRule = createAndroidComposeRule<ComponentActivity>() //指定 ComponentActivity?
}
```


`@Before` 會在所有 `@Text` 前執行 通常建立 View
```js
@Before
fun setupMyActivity() {
  composeTestRule.setContent {
      MyTheme {
          Surface (modifier = Modifier.fillMaxSize()){
              MyLayout()
          }
      }
  }
}
```

測試用程式碼如下修飾
```js
@Test
fun calc_test() {...}
```

測試用輸入
```js
//搜尋相同的節點，對其進行輸入
composeTestRule.onNodeWithText("Tip Percentage").performTextInput("20")
//測試用資料
val expectedTip = NumberFormat.getCurrencyInstance().format(2)
//assertExists 內容為不相等時錯誤訊息
composeTestRule.onNodeWithText("Tip Amount: $expectedTip").assertExists(
  "No node with this text was found."
)
```

範例
```js

class TipUITests {

  @get:Rule
  val composeTestRule = createComposeRule()

  @Before
  fun setupMyActivity() {
    composeTestRule.setContent {
        MyTheme {
            Surface (modifier = Modifier.fillMaxSize()){
                MyLayout()
            }
        }
    }
  }

  @Test
  fun calculate_20_percent_tip() {
    composeTestRule.onNodeWithText("Bill Amount")
        .performTextInput("10")
    composeTestRule.onNodeWithText("Tip Percentage").performTextInput("20")
    val expectedTip = NumberFormat.getCurrencyInstance().format(2)
    composeTestRule.onNodeWithText("Tip Amount: $expectedTip").assertExists(
        "No node with this text was found."
    )
  }
}
```

## 測試用 Activity 擴充方法

擴充方法
```js
fun <A : ComponentActivity> AndroidComposeTestRule<ActivityScenarioRule<A>, A>.onNodeWithStringId(
    @StringRes id: Int
): SemanticsNodeInteraction = onNodeWithText(activity.getString(id))
```

擴充後使用
```js
val composeTestRule = createAndroidComposeRule<ComponentActivity>()
composeTestRule.onNodeWithStringId(R.string.next)
```

## 常用

`composeTestRule.onNodeWithText(activity.getString(id))` 取得指定字串畫面元件
`composeTestRule.onNodeWithText().performClick()` 對取得畫面元件點擊
`composeTestRule.onNodeWithText().performTextInput("Hello")` 對取得畫面元件輸入字串
`composeTestRule.activity.getString(R.string.back_button)` 取得字串資源
`composeTestRule.onNodeWithContentDescription(backText)` 取得指定描述畫面元件?
`composeTestRule.onNodeWithContentDescription().assertDoesNotExist()` 判定元件是否存在