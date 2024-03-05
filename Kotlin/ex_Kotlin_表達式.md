# ex_Kotlin_表達式

Kotlin 許多與常規寫法 目的是為了特定寫法組合容易(障礙)閱讀
以下紀錄發現組合

## fun

`fun()=` 單行縮寫
```js
/**
 * fun 方法名稱(
 *   val 參數
 * ): 回傳類型 = 單行式
 */
fun <A : ComponentActivity> AndroidComposeTestRule<ActivityScenarioRule<A>, A>.onNodeWithStringId(
    @StringRes id: Int
): SemanticsNodeInteraction = onNodeWithText(activity.getString(id))
```

`fun(()->)` 委派參數
```js
fun ReplyHomeScreen(
    onTabPressed: (MailboxType) -> Unit,
    onDetailScreenBackPressed: () -> Unit,
) {...} 
```

## class

`init{fun()}` 初始化方法
```js
class ReplyViewModel {
    init {
        initializeUIState()
    }
    private fun initializeUIState(){...}
}
```

## data class

結構類別 內部成員使用 lazy 節省效能
```js
data class ReplyUiState(
    val mailboxes: Map<MailboxType, List<Email>> = emptyMap(), // 成員 ReplyUiState.mailboxes
    val currentMailbox: MailboxType = MailboxType.Inbox // 成員 ReplyUiState.currentMailbox
) {
    val currentMailboxEmails: List<Email> by lazy { mailboxes[currentMailbox]!! } //延遲成員 ReplyUiState.currentMailboxEmails
}

val uiState = ReplyUiState()
```

## enum class

`.values().forEach {it->}` 循序遍歷
```js
enum class MyColors(val color: Color) {
    Red(Color.Red), Green(Color.Green), Blue(Color.Blue)
}
MyColors.values().forEach { myColors ->}
```

## with interface 擴充

如果 `interface` 內部撰寫特定對象的擴充 搭配 `with` 可以使用到該方法

```js
@Immutable
@JvmDefaultWithCompatibility
interface Density {
    @Stable
    fun Dp.toPx(): Float = value * density
}

val density = LocalDensity.current
with(density) { -40.dp.toPx() } // 原本的 DP 並不存在 toPx() 方法
```


## 符號

`?.` null 判定 非null則執行
`(A)?:(B)` null 判定 非null回傳 A 否則 B
```js
val currentSelectedEmail = mailboxes[MailboxType.Inbox]?.get(0)
                    ?: LocalEmailsDataProvider.defaultEmail
```

## 範例

`?.apply/also/let/run` 經常用來作為搭配
```js
key?.let { it }.{ it } // it = return 最後一行
key?.run { this }.{ this } // this = return 最後一行
key?.apply { it }.{ it } // it = this 自身
key?.also { this }.{ this } // this = this 自身
```

`var = (A)?:(B)` 經常作為初始化
```js
var v = with(key) { this } // 回傳最後一行 apply/also/let/run 也有同樣效果
          ?: key2
```

集合的搜尋
```js
val accounts = listOf(Account())

fun accountById(id: Long): Account {
    return accounts.firstOrNull { it.id == id } // 比較元素 id
        ?: accounts.first() // null 則回傳第一個
}
```

擴充方法關鍵字
```js
@ExperimentalAnimationApi
infix fun EnterTransition.with(exit: ExitTransition) = ContentTransform(this, exit)
// 這是一個在 EnterTransition 空間下的 with 方法
{ fadeIn() with slideOutVertically }
```

`if else` 串接方法
```js
@ExperimentalAnimationApi
infix fun ContentTransform.using(sizeTransform: SizeTransform?) = this.apply {
    this.sizeTransform = sizeTransform
}
// 這是一個 if else 回傳值後 串接 ContentTransform 變數的 using
if (targetState > initialState) { fadeOut()
} else { fadeOut()
}.using(
    SizeTransform(clip = false)
)
```

`(0..3).forEach{it->}` 指定數量迴圈
```js
(0..3).forEach{ it -> 
  println(it)
}
```