# Android_JUnit 協程測試

Android Test 與協程相關測試方法
<https://developer.android.com/kotlin/coroutines/test>

## 引用

`junit` 所需的引用庫
```js
dependencies {
    
}
```

## runTest 協程

`runTest` 可以掛起協程 允許呼叫協程
```js
suspend fun fetchData(): String {
    delay(1000L)
    return "Hello world"
}

@Test
fun dataShouldBeHelloWorld() = runTest {
    val data = fetchData()
    assertEquals("Hello world", data)
}
```

### 其他

`advanceTimeBy` 指定執行時間 時間到後停止的所有協程
`runCurrent` 啟動呼叫的所有 `suspend fun`

```js
@Test
fun ProgressUpdated() = runTest {
    launch { run() } //執行測試目標

    val delayMillis:Int = 1000
    advanceTimeBy(delayMillis) // 運後時間停止
    runCurrent() // 開始運行
    
}
```


`advanceUntilIdle` 運行所有協程 直到結束
