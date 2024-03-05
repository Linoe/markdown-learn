# Android_Coroutine 協程

紀錄 Android 使用 協程 相關方法
<https://developer.android.com/jetpack/compose/side-effects?hl=zh-tw>

## LaunchedEffect 組合中協程

`Compose` 中啟動協程 使用 `LaunchedEffect` 帶有以下特性
- 當進入組合時，將啟動一個協程，將程式碼區塊作為 協程執行
- 當退出時，自動取消協程
- 重新組合時，取消現有協程，重新再組合新協程 ?
- 作為參數 key 當發生變化時 重新組合 代表協程被取消

```js
@Composable
fun LaunchedEffectExample() {
  var state by remember { mutableStateOf(false) }

  LaunchedEffect(state) {
    state = true // 當 key 變化時 發生重組
  }
  /* UI */
}
```

## rememberCoroutineScope 組合中協程範圍
`rememberCoroutineScope` 建立一個在組合被使用的協程範圍
```js
val scaffoldState: ScaffoldState = rememberScaffoldState()

val scope = rememberCoroutineScope()

Scaffold(scaffoldState = scaffoldState) {
    Column {
        Button(
            onClick = {
                // Create a new coroutine in the event handler to show a snackbar
                scope.launch {
                    scaffoldState.snackbarHostState.showSnackbar("Something happened!")
                }
            }
        )
    }
}
```

## 關於重組時

如果協程中使用外部數值 期間發生重組可能讀取到重組前數值
```js
// Timer() 可能會被反覆重組 buttonColour 會產生變化
@Composable
fun Timer(
    buttonColour: String
) {
    val timerDuration = 5000L
    LaunchedEffect(key1 = Unit, block = {
      delay(timerDuration)

      println("Timer ended")
      println("$buttonColour")  // 即使發生重組 仍只會讀取到第一次 buttonColour
    })
}
```

`remember` 當發生重組時 會保留上次更新數值
`rememberUpdatedState` 當發生重組時 會重新更新先前的引用 最常用在協程中
```js
// Timer() 可能會被反覆重組 buttonColour 會產生變化
@Composable
fun Timer(
    buttonColour: String
) {
    val timerDuration = 5000L
    val buttonColorUpdated by rememberUpdatedState(newValue = buttonColour)
    LaunchedEffect(key1 = Unit, block = {
      delay(timerDuration)

      println("Timer ended")
      println("$buttonColour")  // 即使發生重組 仍只會讀取到第一次 buttonColour
      println("$buttonColorUpdated") // 每次重組時都會被 rememberUpdatedState 跟新數值
    })
}

```