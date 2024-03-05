# Kotlin_Coroutine 基礎

Kotlin Coroutine 處理非同步線程類別

coroutine 協同作業 = cooperation(合作) + routine(例行)

結構化並發(Structured concurrency)
協程遵循結構化並發原則 代表新的協程只能在特定的CoroutineScope中啟動
啟動多協程 在所有子協程完成前 外部域詞序等待 確保程式碼中 錯誤都報告不丟失 範圍限定了協程的生命週期

## 引用

Coroutine 不包含在標準庫內，需要額外引用
```js
//引用庫
implementation("org.jetbrains.kotlinx:kotlinx-coroutines-core:1.4.2")
//import package
import kotlinx.coroutines.*
```

## 類別關係

- `CoroutineScope` 範圍物件，控制 範圍內 執行緒
  - `CoroutineContext` 執行物件，資訊儲存

`launch` 啟動一個子協程 預設立即執行
  - `Job` 協程物件，控制 開始等待暫停
`async` 啟動一個子協程 結束回傳值 使用函數 await 等待回傳結果
  - `Deferred` 等同 Job async 實例

## Thread 與 Coroutine

`Thread` 線程 直接佔用執行緒
`Coroutine` 協程 程式區塊 依據設定分配給執行序執行

`Thread` 一個程式區塊對一個執行緒
`Coroutine` 多個程式區塊對一個執行緒
```js
val thread = Thread {
    for (i in 10 downTo 1) {
        runOnUiThread {
            binding.tvShow.text = "count down $i ..."
        }
    }
    runOnUiThread {
        binding.tvShow.text = "Done!"
    }
}
thread.start()
binding.btnTest.setOnClickListener {
    thread.interrupt()
}
```

## 常用
```js
suspend fun pForecast() {
    delay(1000)
}

launch {pForecast()}
```

```js
suspend fun pTemprature(): String {
    delay(1000)
    "Sunny"
}

val job = async{pTemprature()}
val solu = job.await()
```

```js
suspend fun getWeatherReport() = coroutineScope {
    val a1 = async{pForecast()}
    val a2 = async{pTemprature()}
    "${a1.await()} ${a2.await()}"
}

val solu = getWeatherReport()
```
```js
suspend fun getWeatherReport() = coroutineScope {
    val a1 = async{pForecast()}
    val a2 = async{
        try {
            pTemprature()
        } catch(e: AssertionError) {
            "{ No temperature found }"
        }
    }
    "${a1.await()} ${a2.await()}"
}
```
Kotlin Playground 中的 CoroutineScope
runBlocking() 提供 CoroutineScope.
coroutineScope { } 建立新的作用域 CoroutineScope


Android 應用程式中的 CoroutineScope
提供協程作用域
1. Activity(lifecycleScope)
2. ViewModel(viewModelScope)
如果 Activity 被銷毀 ，ActivitylifecycleScope將被取消，所有子協程也將自動被取消

Dispatchers.Main：Main Android 執行緒上執行協程。用於 UI 更新和交互
Dispatchers.IO：此調度程序經過最佳化，用於磁碟讀寫檔案或網路 I/O 操作
Dispatchers.Default：launch()/async()使用的預設調度程序。用於主執行緒之外執行計算密集型工作。例如，處理點陣圖影像檔。


啟動：將協程啟動到一個範圍內，該範圍對其生存時間有明確的邊界。
完成：只有在其子作業完成後，該作業才算完成。
取消：該操作需要向下傳播。當取消協程時，子協程也需要取消。
失敗：此操作應向上傳播。當協程拋出異常時，父級將取消其所有子級，取消自身，並將異常傳播到其父級。這將持續到故障被捕獲並處理為止。它確保程式碼中的任何錯誤都正確報告並且永遠不會丟失。



## 基礎

避免協程還未執行完畢就結束 使用以下測試
```js
fun main() = runBlocking (){
  ...
}
```

`Coroutine` 啟動一個協程 必須在一個鞋程範圍內
- 特定協程範圍(runBlocking)
  ```js
  runBlocking {
    launch() {...}
  }
  ```
- 自訂協程範圍(CoroutineScope)
  ```js
  // 必須給予 CoroutineContext
  val scope = CoroutineScope(context)
  scope.launch() {...}
  ```

### launch / async
`Coroutine` 啟動方式有以下
- `launch` 啟動 建立一個新的協程 一旦呼叫自動執行 使用 join 等待完成
  ```js
  val job = launch() {...} // 啟動後自執行 StandaloneCoroutine 型態
  job.join()  // 等待完成
  ```
- `async` 回傳啟動 建立一個新的協程 一旦呼叫自動執行 使用 await 等待並接收回傳值
  ```js
  val job = async() { "Complete" } // 啟動後自執行  DeferredCoroutine 型態
  val rsu = job.await() // 等待完成並接收回傳值
  ```

### suspend fun
`suspend fun` 如果要在方法執行協程相關 必須修飾
```js
suspend fun doTask() = coroutineScope {
  launch { println("doTask launch") }
}
fun main() = runBlocking (){
  doTask() // 由於 launch 執行非同步 可能順序不一
  println("Done")
}
```

### 協程範圍關係
協程存在父子範圍關係
```js
//協程範圍 關閉後直接停止內部所有 協程
val mainScope = CoroutineScope(Dispatchers.Default){
  repeat(10) { i ->
      launch {
          delay((i + 1) * 200L)
      }
  }
}
mainScope.cancel() //直接關閉所有 協程
```

`Job` 協程本體 指定的話脫離範圍關係
```js
//覆蓋父級 協程
val request = launch {
    launch(Job()) {}  //不會受到父級協程 cancel() 影響
}
request.cancel()
```

### 協程範圍種類

`CoroutineScope` 本身是個工廠類別 同時也是所有協程傳遞追隨 lambda
必須先建立才能使用
```js
// CoroutineScope 必須給予 context
val scope = CoroutineScope(Dispatchers.Default)
scope.launch {...}
```

`coroutineScope` 小寫 當前範圍建立子協程範圍 結束時回傳值
```js
val solu = coroutineScope{
  launch() {...}
  "return"
}
```

`runBlocking` 當前範圍建立子協程範圍 並阻塞線程
```js
runBlocking{
  launch() {...}
}
```

`GlobalScope` 指定不屬於任何協程範圍 啟動協程
```js
GlobalScope.launch {...}
```

`MainScope` 指定主線程 當前範圍建立子協程範圍
```js
val scope = MainScope()
scope.launch {...}
```

`withContext` 短暫修改 Context 內容 結束時回傳
```js
val result = withContext(Dispatchers.Default) {
    "Complete" // result 接收 "Complete" 回傳值
}
```

`withTimeout` 限制時間執行內容 結束時回傳 超時則錯誤
`withTimeoutOrNull` 同 withTimeout 但超時回傳 `null`
```js
val result = withTimeout(1000){...} //超過時間錯誤
val result = withTimeoutOrNull(1000){...}  //超過執行時間 回傳 null
```

## 進階
### 協程內容

`coroutineContext` 包含所有協程資訊
- `CoroutineName` 協程名稱
- `Job` 協程控制
- `CoroutineDispatcher` 協程線程 使用 Dispatchers
- `CoroutineExceptionHandler` 協程例外捕抓

`Dispatchers` 決​​定的線程使用的 thread
- `Main` main thread 上執行 coroutine ， 例如 suspend 函數、畫面更新
- `Default`：使用後台 threads ，通常不是 main thread ， 例如清單進行排序和解析 JSON
- `Unconfined`：啟動時使用當前的 thread ，如果經過停止回復執行，改由其他 thread 繼續執行
- `IO`：設計用來執行 IO 工作，最多 64 thread ，例如讀寫檔案或網路操作
> `Unconfined` 變更線程條件，執行過程出現停止行為(`delay()`)，回復後會從 後台 threads 重新挑選執行

```js
// 所有協程範圍都能訪問
runBlocking{
  println(coroutineContext[Job])
  println(coroutineContext.job)
}
```

`coroutineContext` 大多使用在啟動協程指定功能
```js
CoroutineScope(Dispatchers.Default) // 指定線程
launch(CoroutineName("child")) // 修改協程名
async(coroutineContext + Dispatchers.Default) // + 連接內容
```

`coroutineContext.cancelChildren()` 當前協程 關閉所有子協程
  `NonCancellable` 指定為不可取消的協程
  `CanCellable` 可以取消 但預設可取消沒用

### 協程控制

`await` 等待 `async` 協程回傳值
```js
val job = async() { "Complete" } //回傳 DeferredCoroutine 型態
val rsu = job.await() // 等待並回傳 async 回傳值
```
`join` 等待協程完成
```js
val job = launch {
    delay(1000)
    println("task over")
}
job.join() // 等待所有子協程完成
println("Done")
```
`start` 開始執行 必須先指定 CoroutineStart.LAZY
```js
val job = launch(start = CoroutineStart.LAZY) // 不會立即執行
job.start() // 呼叫後執行
``` 
`cancel` 關閉協程
```js
val job = launch { ... }
job.cancel()  // 尚未執行完畢 仍強制關閉
```
`awaitAll` 同時等待 透過集合管理複數協程
```js
val deferreds = listOf(
    async { fetchDoc(1) },
    async { fetchDoc(2) }
)
deferreds.awaitAll() // `async` 協程集合時才能使用
```

### 協程單例

繼承 `CoroutineScope` 重新指定內部 `coroutineContext`
```js
class MyScope : CoroutineScope {
    // Job 新建獨立協程 並重新定義指向自己
    private val job = Job()
    override val coroutineContext: CoroutineContext
        get() = job

    //執行特定工作
    private fun runTimer() {
        launch {...}
    }

    //關閉協程範圍
    private fun onDestroy() {
        job.cancel()
    }
}

```

繼承 `CoroutineScope` by `MainScope`
```js
//CoroutineScope 委託MainScope
//launch 沒有指定 coroutineContext 使用同範圍 也就是 MainScope
class MyMainScope : CoroutineScope by MainScope() {

    private fun runTimer() {
        val job = launch {...}
        button.setOnClickListener {
            //job cancel意味著只取消這個工作
            job.cancel()
        }
    }

    private fun onDestroy() {
        //Scope cancel意味著全部取消
        cancel()
    }
}
```

### 協程掛載

`suspend` 讓 fun 執行協程相關方法 主要以下用法
- `coroutineScope` 建立一個子協程範圍
  ```js
  suspend fun doCoroutineScope() = coroutineScope {}
  ```
- `withContext` 指定 Context 內容
  ```js
  suspend fun readSomeFile() = withContext(Dispatchers.Default) {}
  ```
- `suspend()->` 委派協程修飾
  ```js
  suspend fun massiveRun(action: suspend () -> Unit) = launch { 
    action()
  }
  massiveRun {  counter++ }
  ```

### 協程指令

`delay()` 等待指定時間 讓出當前線程

`yield()` 讓出當前線程 如果指定線程池不構 可能會被暫停


### 協程偵錯

`ThreadLocal()` 本地線程 讀取當前現成資訊
`currentThread()` 當前的 thread 物件 例如 `[main @coroutine#1]` [線程 @協程]
```js
println("${Thread.currentThread()}")
launch {
  Thread.currentThread().name   //線程名稱
}
```

`CoroutineExceptionHandler` 捕抓子協程錯誤 但仍會照常中斷協成 通常搭配 supervisorScope 
```js
val handler = CoroutineExceptionHandler { _, exception -> 
    println("Error[$exception]") 
}
val rus = supervisorScope {
    launch(handler) {
        delay(1000)
        throw AssertionError()
    }
    "Done"
}
// 如果發生錯誤 等待 CoroutineExceptionHandler 完成才會繼續
println(rus)
```

`try-catch` 必須在協程外部捕抓 否則人仍會傳遞錯誤
```js
job = launch { 
  throw Exception("From job2")  //不能在此捕抓
}
try {
  jo2.join()  //必須在外部
} catch (e: Exception) { 
  println(e)
}
```

`measureTimeMillis{}` 計時 結束時回傳執行時間
```js
//計時用線程，結束回傳時間
runBlocking {
  val timeInMillis = measureTimeMillis {
    delay(500)
  }
}
```

## 線程
### 線程相關

`newSingleThreadContext()` 啟動一個不能更改單一 thread 新的協程
```js
//ThreadLocal 本地線程
val threadLocal = ThreadLocal<String?>()
threadLocal.set("main")
launch(Dispatchers.Default + threadLocal.asContextElement(value = "launch")){}
```

### 線程安全

`volatile` 易變 保證所有線程看到該變數 但不保證原子性
```js
@Volatile
var count = 0

launch{ count += 10 } // 無法確定 讀取時是否已完成 上一個協程 += 10 
launch{ count += 10 } // 結果可能會是 10 或 20
```

`Atomic` 原子 保證數值原子性
```js
val counter = AtomicInteger(0) //只有基本型態類別

launch{ counter.addAndGet(10) } // 必須使用方法處理
launch{ counter.addAndGet(10) } // 上一個尚未處理完畢 會阻塞等待 保證 20 物件呼叫使用上不方便
```

`Mutex` 互斥 資源鎖定範圍 阻塞協程
```js
val mutex = Mutex()
launch{ mutex.withLock {...} }
launch{ mutex.withLock {...} } // 如果 mutex 正在被使用 結束前會持續等待 且效率不好
```

`newSingleThreadContext` 單一線程協程 保證協程不使用其他線程
```js
//newSingleThreadContext 單一線程協程
val counterContext = newSingleThreadContext("CounterContext")
launch(counterContext){ ... } 
launch(counterContext){ ... } // 指定線程池 當都被使用中個時 持續等待直到有被釋放

launch(newSingleThreadContext("MyThread")){} //建立獨立線程，且命名
```

示範 執行效率 差異
`volatile`(不安全) >= `Atomic` > `newSingleThreadContext` >>> `Mutex`

```js
// 100 000 次協程呼叫
suspend fun massiveRun(action: suspend () -> Unit) {
    val n = 100  // 協程數量
    val k = 1000 // 重複執行數量
    val time = measureTimeMillis {
        coroutineScope { // 使用當前協程資訊
            repeat(n) {
                launch {
                    repeat(k) { action() }
                }
            }
        }
    }
    println("Completed ${n * k} actions in $time ms")
}
// Volatile 
@Volatile 
var counter = 0

fun main() = runBlocking {
    withContext(Dispatchers.Default) {
        massiveRun {
            counter++
        }
    }
    println("Counter = $counter")
}
// 花費 44 ms 但無法保證 10000 次

// Atomic 原子變數 保證數據原子性 相對使用較不自由
val counter = AtomicInteger()

fun main() = runBlocking {
    withContext(Dispatchers.Default) {
        massiveRun {
            counter.incrementAndGet()
        }
    }
    println("Counter = $counter")
}
// 花費 41 ms 保證 10000 次

// Mutex 互斥 共用可變物件的鎖定操作 協程版 synchronized 阻塞協程 不阻塞線程
val mutex = Mutex()
var counter = 0

fun main() = runBlocking {
    withContext(Dispatchers.Default) {
        massiveRun {
            // protect each increment with lock
            mutex.withLock {
                counter++
            }
        }
    }
    println("Counter = $counter")
}
// 花費 793 ms 保證 10000 次

// newSingleThreadContext 指定協程固定線程
val counterContext = newSingleThreadContext("CounterContext") //指定協程只能在固定線程
var counter = 0

fun main() = runBlocking {
    withContext(counterContext) {//指定線程
        massiveRun {
          counter++
        }
    }
    println("Counter = $counter")
}
// 花費 53 ms 保證 10000 次

```