# ex_Kotlin_CoroutineScope 詳細

`CoroutineScope` 作為協成操作工廠類別 還有其他類似協成範圍物件

相似用途範圍
- `GlobalScope` 取得一個不屬於任何協程的範圍 目前僅用於 launch
- `MainScope` 取得一個主線程的範圍

等待範圍內子協程
- `runBlocking` 阻塞當前線程 執行一段協成 等待並回傳
- `coroutineScope` 差異大小寫 當前範圍執行一段協成 等待並回傳
- `supervisorScope` 等同 coroutineScope 不會因子協程錯誤整個中斷
- `withContext` 暫時修改 Context 範圍執行 結束後回傳
- `withTimeout` 限制時間協程 主要用在需要等待/阻塞協程

繼承類別 不會使用會回傳型態看到
- `ContextScope` CoroutineScope(context) 回傳型態 CoroutineScope 實作
- `ScopeCoroutine` coroutineScope 傳遞 lambda 內部 this 型態
- `Job` `Deferred` launch/async 協程基類
- `Supervisor` 相關字段 代表不因子協程中斷 導致所有協程中斷
- `EmptyCoroutineContext` GlobalScope 使用 Context 不屬於任何協程的範圍

## CoroutineScope

`CoroutineScope` 作為協成操作工廠類別 主要用來以下
- 管理特定範圍的協成
- 啟動子協程(launch async)
  ```js
  val scope = CoroutineScope(coroutineContext) // 建立需要 Context
  val job1 = scope.launch{}
  val job2 = scope.asyne{}
  ```

`CoroutineScope` 程式上用途有兩個
1. 協程生成的工廠類別
2. 協程的傳遞尾隨(Passing trailing) lambda 型態(除了 produce)
原始碼如下
- interface 類別 僅一個成員 CoroutineContext
  ```js
  // source
  public interface CoroutineScope {
      public val coroutineContext: CoroutineContext
  }
  ```
- 所有功能透過擴充方法實現
  ```js
  // source
  public val CoroutineScope.isActive: Boolean
  get() = coroutineContext[Job]?.isActive ?: true
  ```
- 協成使用參數 context 如果不存在 Job 新建
  ```js
  // source
  public fun CoroutineScope(context: CoroutineContext): CoroutineScope = ContextScope(
      if (context[Job] != null) context else context + Job()
    )
  ```

### launch/async

`launch` `async` 用來啟動一段協程
- 必須在協程中
- 未指定 context 會與當前協程建立父子關係
```js
// runBlocking（單線程啟動）
fun main() = runBlocking<Unit> {  // this: CoroutineScope
    launch {}  // implicit this.launch {}
}

// coroutineScope 工廠函數（在主執行緒啟動，但在需要時切換到預設排程器）。
suspend fun main() = coroutineScope<Unit> {  // this: CoroutineScope
    launch {}  // implicit this.launch {}
}

// CoroutineScope 需要提供 coroutineContext
val scope = CoroutineScope(coroutineContext)
val job = scope.launch{}

// CoroutineScope 擴充方法 可以在所有繼承於 CoroutineScope 類別呼叫
fun CoroutineScope.doThis() { launch { ... }}
```

`launch` `async` 有以下特性
- 啟動方法必須 Job 和 CoroutineDispatcher 被包含在 Context
- 回傳型態 Job/Deferred
  ```js
  // source
  public fun CoroutineScope.launch(
      context: CoroutineContext = EmptyCoroutineContext,
      start: CoroutineStart = CoroutineStart.DEFAULT,
      block: suspend CoroutineScope.() -> Unit
  ): Job
  public fun <T> CoroutineScope.async(
      context: CoroutineContext = EmptyCoroutineContext,
      start: CoroutineStart = CoroutineStart.DEFAULT,
      block: suspend CoroutineScope.() -> T
  ): Deferred<T> 
  ```
- 透過 `CoroutineStart` 控制啟動方式
  `DEFAULT` 立即執行 依據 context 內容執行 默認值
  `LAZY` 僅在 start() join() await() 才會執行
  `ATOMIC` 直到斷點 無法被取消(基於 Atomically 特性)
  `UNDISPATCHED` 直到斷點 改依據 context 內容執行?
  ```js
  val job = launch(CoroutineStart.LAZY){...} //不會立即啟動
  job.start() //啟動子協程
  ```


## 指定範圍
### MainScope

`MainScope` 回傳一個 SupervisorJob (不因錯誤中斷) 與 main 線程的 ContextScope
```js
// source
public fun MainScope(): CoroutineScope = ContextScope(SupervisorJob() + Dispatchers.Main)

// example
private val scope = MainScope()
override fun onDestroy() {
    super.onDestroy()
    scope.cancel()
}
```

### GlobalScope

`GlobalScope` object 物件 等同於 CoroutineScope
`GlobalScope.launch` 為主要使用方式
- 啟動協程一個不屬於任何範圍 無論在哪個協程中
  ```js
  // suspend 可以等同在外部協程中 此範例不使用也不會錯誤
  suspend fun globalTask() = GlobalScope.launch {
    println(coroutineContext.job)
    println(coroutineContext.job.parent) // null 不存在父範圍
  }
  ```
- 多次啟動 建議不要再在協程中使用?
  ```js
  // 錯誤
  suspend fun globalTask() = GlobalScope.launch {...}
  runBlocking {
    globalTask()
    globalTask()
  }
  // 正確 1
  runBlocking {
    GlobalScope.launch{...}
    GlobalScope.launch{...}
  }
  // 正確 2
  fun globalTask() = GlobalScope.launch{...}
  globalTask()
  globalTask()
  ```
 - 被中斷會發出錯誤

`CoroutineScope` 差異於 coroutineContext 固定為 EmptyCoroutineContext
```js
// source
public object GlobalScope : CoroutineScope {
    override val coroutineContext: CoroutineContext
        get() = EmptyCoroutineContext
}

```

## 範圍等待
### runBlocking

`runBlocking` 使用在一般程式區塊 非協程裡 通常是用來測試用的函數

`runBlocking` 運行一個阻塞線程協程 並阻塞當前線程直到完成 回傳最後一行回傳值
```js
val rus = runBlocking {
    delay(1000)
    "Done"
}
println(rus)
```

`runBlocking` 如果內部有子協程 無法透過 cancel 關閉的
```js
val job = runBlocking {
    launch {
        println("runBlocking start")
        delay(1000)
        println("runBlocking end")
    }
    this
}
println("try cancel ${job}")
job.cancel()
```

`runBlocking` 不會因暫停釋放線程 
```js
runBlocking() {  // 測試用
    val dbDispatcher = Dispatchers.IO.limitedParallelism(2) //限制 2 個線程池
    repeat(10) {
        launch(dbDispatcher3) {//個別獨立
            runBlocking { //完成後才會釋放線程
                println("Start No.$it in runBlocking on ${Thread.currentThread().name}")
                delay(50)
                println("End No.$it in runBlocking on ${Thread.currentThread().name}")
            }
            // coroutineScope { //比較用 
            //     println("Start No.$it in coroutineScope on ${Thread.currentThread().name}")
            //     delay(50)
            //     println("End No.$it in coroutineScope on ${Thread.currentThread().name}")
            // }
        }
    }
}
```

### coroutineScope

`coroutineScope` 當前範圍執行一段協成 等待並回傳 
`CoroutineScope` 與名字相似但功能不同 差異如下 
| `coroutineScope` | `CoroutineScope` |
| :--------------: | :--------------: |
|   `interface`    |      `fun`       |
|     工廠類別     |   啟動範圍協程   |
|     不是協程     |   是協程會等待   |

有以下特性
- 在當前協程下運行子協程
  ```js
  println(coroutineContext.job) // 需要在其他協程中
  coroutineScope {
    println(coroutineContext.job)
    println(coroutineContext.job.parent)
  }
  ```
- 回傳最後一行回傳值
  ```js
  val rus = coroutineScope {
      "Done"
  }
  println(rus) // "Done"
  ```
- 等待其所有子協程完成
  ```js
  coroutineScope { // 等待所有協程完成
      launch {
        delay(1000)
        println("Finished task 1")
      }
  }
  println("Done")
  ```
- 當父協程被取消時 中斷所有協程
  ```js
  val job = launch(Job()) { //避免同協程範圍內 必須要在其他協程中 才能取消 否則會等待
      coroutineScope {
          launch {
              delay(1000)
              println("Finished task 1")
          }
          launch {
              delay(2000)
              println("Finished task 2")
          }
      }
  }
  delay(1500)
  job.cancel()
  println("Done")
  ```

### supervisorScope

`supervisorScope` 具有 coroutineScope 特性 差異如下
- 發生錯誤也不會中斷所有協程
  ```js
  val rus = supervisorScope {
      launch {
          delay(500)
          throw Error()  // 錯誤 後面不會打印
          println("Task1 Done")
      }
      launch {
          delay(1000)
          println("Task2 Done")  // 不會被中斷
      }
      "Done"  // 不會被中斷
  }
  println(rus)  // 即使子協程錯誤 依然回傳
  ```
- 使用時機 啟動多協程範圍時 避免錯誤導致整個中斷
  ```js
  supervisorScope {
      actions.forEach { action ->
          launch {
              notifyAnalytics(action)
          }
      }
  }
  ```
- 子協程錯誤 不會傳遞父級 必須額外處理 使用 `CoroutineExceptionHandler` 捕抓錯誤
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
- Context 使用 SupervisorCoroutine

關於 `withContext(SupervisorJob())` 不能取代 `supervisorScope` 原因如下
- 使用 `UndispatchedCoroutine` 非 `SupervisorCoroutine` 
  ```js
  withContext(SupervisorJob()) {
      println("${this.javaClass.typeName}") // UndispatchedCoroutine
      launch {
          delay(1000)
          throw Error()
      }
  }
  println("After") //不會執行
  ```
- `supervisorScope` 範圍底下所有協程使用 SupervisorJob
處理 SupervisorJob 直接建立一個失敗的子作業，該子作業不會使父親 SupervisorJob 失敗。

withContext(SupervisorJob())

使用 withContext 建立了兩個 一個 Job 其父級 SupervisorJob


關於 `supervisorScope` 使用 try-catch 捕抓錯誤
只能捕抓到父級別 子協程不會傳播 無法捕抓
```js
try {
    supervisorScope {
        val child = launch {
            try {
                delay(Long.MAX_VALUE) //持續等待 直到父級別錯誤
            } finally {
                println("The child is cancelled")
            }
        }
        // 不使用 join() 避免永久等待完成
        yield()
        throw AssertionError()
    }
} catch(e: AssertionError) {
    println("Caught an assertion error")
}
```

### withContext

`withContext` 可以短暫修改 Context 主要用在其他協程中
```js
launch(Dispatchers.Main) {
    view.showProgressBar()
    withContext(Dispatchers.IO) {
        fileRepository.saveData(data)
    }
    view.hideProgressBar()
}
```

### withTimeout

`withTimeout` 限制時間協程 主要用在需要等待/阻塞協程 有以下特性
- 限制執行時間
- 等待範圍內執行並回傳
- 超過時間引發錯誤
```js
val rus = withTimeout(1000){
    delay(500)
    "Done"
}
println(rus)
```

`withTimeout` 原始碼如下
- 不能修改 Context
- 產生子協程 TimeoutCoroutine
```js
// source
public suspend fun <T> withTimeout(timeMillis: Long, block: suspend CoroutineScope.() -> T): T {
    contract {
        callsInPlace(block, InvocationKind.EXACTLY_ONCE)
    }
    if (timeMillis <= 0L) throw TimeoutCancellationException("Timed out immediately")
    return suspendCoroutineUninterceptedOrReturn { uCont ->
        setupTimeout(TimeoutCoroutine(timeMillis, uCont), block)
    }
}
```

`withTimeoutOrNull` 等同 withTimeout 差異如下
- 超過時間改為回傳 null
- 不會引發錯誤 只會取消並返為 null
```js
try{
    val rus = withTimeoutOrNull(1000){
        delay(1500)
        "Done"
    }
    println(rus)   
}catch (e: TimeoutCancellationException) {
    println("Cancelled") //沒有錯誤 不會執行
}
```



## 內部類別
### ContextScope

`ContextScope` CoroutineScope 實作 僅增加 toString 方便除錯
`CoroutineScope` 是個 interface 透過方法去指向 ContextScope 
> ContextScope : CoroutineScope
```js
// source
public fun CoroutineScope(context: CoroutineContext): CoroutineScope =
    ContextScope(if (context[Job] != null) context else context + Job())

internal class ContextScope(context: CoroutineContext) : CoroutineScope {
    override val coroutineContext: CoroutineContext = context
    // CoroutineScope is used intentionally for user-friendly representation
    override fun toString(): String = "CoroutineScope(coroutineContext=$coroutineContext)"
}

// example
val scope = CoroutineScope(coroutineContext) //需要一個 Context
println("${scope}")
```

### ScopeCoroutine

`ScopeCoroutine` 是 coroutineScope 協程型態 傳遞 lambda 中使用 this 確認
```js
// source
public suspend fun <R> coroutineScope(block: suspend CoroutineScope.() -> R): R {
    contract {
        callsInPlace(block, InvocationKind.EXACTLY_ONCE)
    }
    return suspendCoroutineUninterceptedOrReturn { uCont ->
        val coroutine = ScopeCoroutine(uCont.context, uCont)
        coroutine.startUndispatchedOrReturn(coroutine, block)
    }
}
// example
coroutineScope {
  println(this) // "coroutine#2":ScopeCoroutine{Active}@2a18f23c
}
```

`ScopeCoroutine` 則是繼承於 AbstractCoroutine 可以視為協程類別
> ScopeCoroutine : AbstractCoroutine<T>(context, true, true), CoroutineStackFrame
> AbstractCoroutine : JobSupport(active), Job, Continuation<T>, CoroutineScope
> Job : CoroutineContext.Element
```js
internal open class ScopeCoroutine<in T>(
    context: CoroutineContext,
    @JvmField val uCont: Continuation<T> // unintercepted continuation
) : AbstractCoroutine<T>(context, true, true), CoroutineStackFrame {...}

public abstract class AbstractCoroutine<in T>(
    parentContext: CoroutineContext,
    initParentJob: Boolean,
    active: Boolean
) : JobSupport(active), Job, Continuation<T>, CoroutineScope {...}
```

### SupervisorCoroutine

`Supervisor` 相關字段 代表不因子協程中斷 導致所有協程中斷
```js
private class SupervisorCoroutine<in T>(
    context: CoroutineContext,
    uCont: Continuation<T>
) : ScopeCoroutine<T>(context, uCont) {
    override fun childCancelled(cause: Throwable): Boolean = false
}
private class SupervisorJobImpl(parent: Job?) : JobImpl(parent) {
    override fun childCancelled(cause: Throwable): Boolean = false
}
```

`SupervisorCoroutine` 是 SupervisorJob 協程型態 傳遞 lambda 中使用 this 確認

```js
// source
@Suppress("FunctionName")
public fun SupervisorJob(parent: Job? = null) : CompletableJob = SupervisorJobImpl(parent)

public suspend fun <R> supervisorScope(block: suspend CoroutineScope.() -> R): R {
    contract {
        callsInPlace(block, InvocationKind.EXACTLY_ONCE)
    }
    return suspendCoroutineUninterceptedOrReturn { uCont ->
        val coroutine = SupervisorCoroutine(uCont.context, uCont)
        coroutine.startUndispatchedOrReturn(coroutine, block)
    }
}
// example
supervisorScope {
  println(this) // "coroutine#1":SupervisorCoroutine{Active}@1bc6a36e
}
```

`SupervisorJob` 特性如下
1. 子協程錯誤不會網上傳播 同時也不會被捕抓
2. 父協程錯誤依然會中斷子協程
```js
fun main() = runBlocking {
  val supervisor = SupervisorJob() //父級別協程

  //使用 with(CoroutineScope()) 不使用 coroutineScope 避免 父級別 中斷後不能繼續操作
  with(CoroutineScope(coroutineContext + supervisor)) {
    // 驗證子協程錯誤 往上傳播
    val firstChild = launch(CoroutineExceptionHandler { _, _ ->  }) {
        delay(100)
        throw AssertionError("Error") //即使錯誤也不會中斷父級別
    }
    // 驗證父協程錯誤 往下傳播
    val secondChild = launch {
        firstChild.join()
        try {
            delay(Long.MAX_VALUE) //持續存活 等待父協成錯誤中斷
        } finally {
            println("try-finally")
        }
    }
    firstChild.join() //等待 firstChild 發生錯誤
    
    supervisor.cancel() //強制中斷父協成 因為當前區塊不是在父協程中 後面仍會運行
    secondChild.join() //等待 secondChild 接收到父協成錯誤
  }
}
```

### EmptyCoroutineContext

`EmptyCoroutineContext` object 物件 作為不屬於任何協成範圍
使用在 GlobalScope 與 suspend fun main() 作為 Context
```js
// source
public object EmptyCoroutineContext : CoroutineContext, Serializable {
    private const val serialVersionUID: Long = 0
    private fun readResolve(): Any = EmptyCoroutineContext

    public override fun <E : Element> get(key: Key<E>): E? = null
    public override fun <R> fold(initial: R, operation: (R, Element) -> R): R = initial
    public override fun plus(context: CoroutineContext): CoroutineContext = context
    public override fun minusKey(key: Key<*>): CoroutineContext = this
    public override fun hashCode(): Int = 0
    public override fun toString(): String = "EmptyCoroutineContext"
}
// example
suspend fun main(){
  println(coroutineContext.job)
}

GlobalScope.launch {
  println(coroutineContext.job)
}
```
