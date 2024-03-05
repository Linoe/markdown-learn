# Kotlin_Flow 資料流

flow 方便協程處理資料，帶有 `sequence` `suspend` 特定
flow 一整串耗時的流程，還有接收耗時流程的結果

## 引用

flow 不包含在標準庫內，需要額外引用
```js
//引用庫
implementation("org.jetbrains.kotlinx:kotlinx-coroutines-core:1.4.2")
//import package
import kotlinx.coroutines.flow.*
```

## 類別差異
`Channel` 資料溝通的管道 通常是用來傳遞結果 只負責傳接球
  是主動傳遞(自動開始執行) 被稱為 hot channel(熱流)
`Flow` 一整串耗時的流程 還有接收耗時流程的結果
  是被動傳遞(啟動開始執行) 被稱為 cold flow(冷流)
`SharedFlow` 等同 Flow 但主動傳遞 屬於 hot SharedFlow(熱流)
`StateFlow` 等同 SharedFlow 但只會接收到最新值

## 資料處理

`flow` 可以令資料流類似迭代器功能
以下相似例子

```js
// List 集合資料
fun simple(): List<Int> = listOf(1, 2, 3)
// sequence 單一元素處理
fun simple(): Sequence<Int> = sequence {
    for (i in 1..3) { yield(i) } // yield 傳遞元素 
}
// suspend 不阻塞主線程
suspend fun simple(): List<Int> {
    delay(100)
    return listOf(1, 2, 3)
}
// flow 包含上方所有功能?
fun simple(): Flow<Int> = flow { // 單一元素處理
    for (i in 1..3) {
        delay(100) // 不會阻塞
        emit(i) // 傳遞元素 
    }
}
```

`flow` 資料處理與集合 `sequence` 等同

## 基礎

`Flow` 專門處理資料流的 以下特性
- Flow 執行在當前線程
- 受協程範圍引響 withTimeoutOrNull

`Flow<T>` 資料迭代器 處理資料過程 T 傳遞資料
```js
fun simple(): Flow<Int> = flow { // 單一元素處理
    for (i in 1..3) {
        emit(i) // 傳遞元素 
    }
}
fun main() = runBlocking<Unit> { //必須要在協程中
    simple().collect { value -> println(value) } 
}
```

`flow.emit(T)` 傳遞資料 T 要與 `Flow<T>` 相等
```js
fun simple(): Flow<Int> = flow { // 單一元素處理
    emit(i) // 傳遞元素 
}
```

`flow.collect{it}` 收集資料 當開始 collect 才會開始處理資料
`Flow` 動作必須要在協程中操作
```js
fun main() = runBlocking<Unit> { // 紀錄方便下面省略此
    simple().collect { value -> println(value) } 
}
```

`List.asFlow()` 將集合轉換成 `Flow` 進行操作
```js
(1..3).asFlow() // 等同底下
val f: Flow<Int> = flow {
    for (i in 1..3) { emit(i) }
}
```

## 進階
### 轉換

`map` 元素轉換處理其他型態
```js
// 方法中處理
fun simple(): Flow<String> = flow {
      for (i in 1..3) { emit(i) }
  }.map { value ->"fun $value" }

//外部中處理
simple()
  .map { value ->"fun $value"}
  .collect { value -> println("main $value") }

//集合轉換處理
(1..3).asFlow()
    .map { request -> "response $request" }
    .collect { response -> println(response) }
```

`transform` 元素轉換處理其他型態且多輸出 使用 emit 重新輸出
```js
//transform 任意轉換
(1..3).asFlow()
    .transform { request ->
        emit("Making request $request") 
        emit("response $request") 
    }
    .collect { response -> println(response) }
```

### 累加器

`reduce` 累加元素 傳遞上一個輸出值 結束時回傳結果
```js
val rus = (1..3).asFlow() //rus = 1+2+3
    .reduce { sum, v -> // sum 上一個結果 v 當前元素
      sum + v // 結果往下傳遞
    }
```

`fold` 累加元素 等同 reduce 差異需要傳遞一個初始值
```js
//fold 累加元素
val rus = (1..3).asFlow() //rus = 10 + 1 + 2 + 3
    .fold(10) { sum, v -> // 最初 sum 等於 10 
      sum + v  // sum 上一個結果 v 當前元素
    }
```

### 資料節點

`onEach` 回應 讀取每個元素 不會對資料進行處理
```js
('a'..'c').asFlow()
    .onEach { println("Check: $it") } //不會對資料進行處理
    .map { it.toInt() }
    .collect { println("Value is $it") }
```

`onCompletion` 完成 全部元素完畢後執行
```js
(1..3).asFlow()
    .onCompletion { println("Done") } // 會在 collect 結束後
    .collect { value -> println(value) } // 1 2 3 Done
```

### 資料塞選

`take` 指定數量 讀取指定元素後 多餘捨棄 不夠時結束
```js
(1..3).asFlow()
    .take(2)  //讀取兩個元素後 多餘捨棄 結果不會有 3
    .collect { value -> println(value) } // 1 2
```

`filter` 過濾 判定元素 false 則捨棄
```js
(1..5).asFlow()
    .filter { it % 2 == 0 } // it % 2 == 0 true 通過 false 捨棄
    .collect { println("$it") } // 2 4
```

### 效能

`flowOn` 更換線程 不建議 耗損效能 使用 Dispatchers
```js
// 方法
fun simple(): Flow<Int> = flow {
  ...
}.flowOn(Dispatchers.Default)
// 過程
val singleValue = intFlow
    .map { ... } //  IO
    .flowOn(Dispatchers.IO)
    .filter { ... } //  Default
    .flowOn(Dispatchers.Default)
    .single() //  Main

//不能中途更改線程 Flow invariant is violated
fun simple(): Flow<Int> = flow {
    withContext(Dispatchers.Default) {...} // 錯誤
}
```

`buffer` 緩衝 不會造成阻塞
```js
simple() // 假如每個元素 delay(100)
    .buffer() // 產生另一個協程收集 不會造成阻塞
    .collect { println(it) }
```

### 收集

`conflate` 合併 不會等待 可能會丟失元素
```js
simple() // 假如每個元素 delay(100)
    .conflate() // 產生另一個協程收集 不會等待
    .collect { value -> 
        delay(300) //時間差 會丟失這段期間的元素
        println(value)
    }
```

`collectLatest` 收集最新 新元素出現 中斷當前收集動作 改執行新元素
```js
simple() // 假如每個元素 delay(100)
    .collectLatest { value -> 
        println("Collecting $value") 
        delay(300) // 可能因為延遲過久被強制取消
        println("Done $value") // 元素產出過快 可能執行不到
    } 
```
`single` 輸出第一元素

## 複數流處理
### 元素合併

`zip(){}` 兩個 Flow 合併輸出一個 Flow 長度只會取最少元素
```js
val nums = (1..4).asFlow() // 1 2 3 4
val strs = flowOf("one", "two", "three")  // "one", "two", "three"

nums.zip(strs) { a, b -> "$a -> $b" } // a = nums, b = strs
    .collect { println(it) } // 僅輸出 1 2 3 元素
```

`combine(){}` 兩個 Flow 合併輸出一個 Flow
兩邊 Flow 有新元素 便會執行一次 元素可能是舊的
```js
val nums = (1..3).asFlow().onEach { delay(300) } // 1 2 3
val strs = flowOf("one", "two", "three").onEach { delay(400) }
val startTime = System.currentTimeMillis() //計時用

nums.combine(strs) { a, b -> "$a -> $b" } // a = nums, b = strs
    .collect {
        println("$value at ${System.currentTimeMillis() - startTime} ms") 
    } 
/*
1 -> one at 452 ms
2 -> one at 651 ms
2 -> two at 854 ms
3 -> two at 952 ms
3 -> three at 1256 ms
*/
```

### 元素轉流

`flatMapConcat{}` 扁平轉換 元素重新輸出 Flow
並把 Flow 元素拆成一個個元素收集
```js
fun requestFlow(i: Int): Flow<String> = flow {
    emit("$i: First") 
    delay(500)
    emit("$i: Second")    
}
(1..3).asFlow().onEach { delay(100) }
    .flatMapConcat { requestFlow(it) } //重新將流轉為輸出
    .collect { value ->
        println("$value at ${System.currentTimeMillis() - startTime} ms") 
    } 
/*
1: First at 121 ms
1: Second at 622 ms
2: First at 727 ms
2: Second at 1227 ms
3: First at 1328 ms
3: Second at 1829 ms
*/
```

`flatMapMerge{}` 扁平轉換合併 元素重新輸出 Flow
新 Flow 使用協程收集元素 會有搶快現象
```js
(1..3).asFlow().onEach { delay(100) }
    .flatMapMerge { requestFlow(it) } //多協成執行 輸出快元素優先執行
    .collect { value ->
        println("$value at ${System.currentTimeMillis() - startTime} ms") 
    }
/*
1: First at 167 ms
2: First at 264 ms
3: First at 365 ms
1: Second at 668 ms
2: Second at 764 ms
3: Second at 867 ms
*/
```

`flatMapLatest{}` 扁平轉換最新 元素重新輸出 Flow
一旦新 Flow 會停止舊的 Flow 協程 可能會遺失
```js
val startTime = System.currentTimeMillis() // remember the start time 
(1..3).asFlow().onEach { delay(100) } // a number every 100 ms 
    .flatMapLatest { requestFlow(it) }                                                                           
    .collect { value -> // collect and print 
        println("$value at ${System.currentTimeMillis() - startTime} ms from start") 
    } 
/*
1: First at 142 ms
2: First at 322 ms
3: First at 425 ms
3: Second at 931 ms
*/
```

### 啟動協程

`launchIn` 以啟動協程 處理資料
```js
(1..3).asFlow()
    .onEach { event -> println("Event: $event") }
    .launchIn(this) // 開啟後 所有過程都會以協程進行
```

`cancel` 關閉協成 如果 Flow 是在協程中處理
```js
(1..3).asFlow()
    .launchIn(this)
    .collect { value -> 
        if (value == 3) cancel()  
        println(value)
    }
```

`cancellable` 開啟協程取消 但預設可取消沒用?
```js
(1..5).asFlow()
  .cancellable()
  .launchIn(this)
  .collect { value -> 
      if (value == 3) cancel()  
      println(value)
  } 
```
`NonCancellable` 指定為不可取消的協程
`isCancelled` 判斷是否取消
`currentCoroutineContext()` 回傳當前 CoroutineContext?

## SharedFlow

`Flow` 被動形式啟動才會動作 主動發送資料 `SharedFlow` 出現
`SharedFlow` 無論是否有接收者 持續發送資料
  自帶緩衝 可以接收一定時間內的資料

`SharedFlow` 通常以 類別方式建立使用
```js
// 傳遞者
class TickHandler(
    private val externalScope: CoroutineScope,
    private val tickIntervalMs: Long = 5000
) {
    private val _tickFlow = MutableSharedFlow<Unit>(replay = 0)
    val tickFlow: SharedFlow<Event<String>> = _tickFlow

    init {
        externalScope.launch {
            while(true) {
                _tickFlow.emit(Unit)
                delay(tickIntervalMs)
            }
        }
    }
}
// 接受者
val tickHandler: TickHandler
val externalScope: CoroutineScope

externalScope.launch {
    tickHandler.tickFlow.collect {
        ...
    }
}
// 更短的傳遞者
class EventBus {
    private val _events = MutableSharedFlow<Event>() // private mutable shared flow
    val events = _events.asSharedFlow() // publicly exposed as read-only shared flow

    suspend fun produceEvent(event: Event) {
        _events.emit(event) // suspends until all subscribers receive it
    }
}
val eventBus: EventBus
eventBus.events.collect{...} // 會放在某個協程持續接收
```


## StateFlow

`StateFlow` 繼承於 `SharedFlow` 不同於 只會接受到最新值
因此所有 collect 線程都會收到 相對可能漏接
```js
//StateFlow 只會發接收最新值 舊值會漏接
class CounterModel {
    private val _counter = MutableStateFlow(0) // private mutable state flow
    val counter = _counter.asStateFlow() // publicly exposed as read-only state flow

    fun inc() {
        _counter.update { count -> count + 1 } // atomic, safe for concurrent use
    }
}

// 取得數值
val value: Int by counter.collectAsState()
Text("Value is $value")
```

`StateFlow.update` 通知數值更新
```js
_uiState.update { current -> current.build {} }
// 另一種方式?
_uiState.value.build {
      isLoading = true
  }
_uiState.value = _uiState.value.build {...}
```
`StateFlow.collectAsState` 轉換取得數值流 當數值更新時會自動變更
```js
val value: Int by counter.collectAsState()
Text("Value is $value")
```

## 偵錯

`Flow` 偵錯直接在過程插入
`check()` 檢查 如果錯誤則執行

```js
fun simple(): Flow<String> = flow {
      for (i in 1..3) {
          println("Emitting $i")
          emit(i) // emit next value
      }
  }
  .map { value ->
      check(value <= 1) { "Crashed on $value" }                 
      "string $value"
  }
try {
    simple().collect { value -> println(value) }
} catch (e: Throwable) {
    println("Caught $e") // 元素執行 2 發生錯誤中斷
} 
/*
Emitting 1
main simple 1
Emitting 2
Caught java.lang.IllegalStateException: check on 2
*/
```

`catch{}` 錯誤 等同 try-catch 但不會中斷等個 flow 只會停止當前元素
```js
// catch 等同 try-catch 
simple()
    .catch { e -> emit("Caught $e") } // 重新輸出 emit() 不會中斷
    .collect { value -> println(value) }
// 不輸出 emit()
simple()
    .catch { e -> println("Caught $e") }
    .collect()  // collect 不允許掛載

// 發生錯誤時 依序傳遞可接受的 例:onCompletion 
simple() // 假如 emit(1) throw RuntimeException()
    .onCompletion { cause -> //錯誤仍會往下傳遞
        if (cause != null) println("Flow completed exceptionally") }
    .catch { cause -> println("Caught exception") }
    .collect { value -> println(value) }
/*
1
Flow completed exceptionally
Caught exception
*/
simple()
    .catch { cause -> println("Caught exception") } //中斷後不執行下方
    .onCompletion { cause -> // 接收不到
        if (cause != null) println("Flow completed exceptionally") }
    .collect { value -> println(value) }
/*
1
Caught exception
*/
```