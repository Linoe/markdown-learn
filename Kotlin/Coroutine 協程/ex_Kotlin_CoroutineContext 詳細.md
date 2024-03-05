# ex_Kotlin_CoroutineContext 詳細

`CoroutineContext` 協程中保存資料的結合類別
所有的協成資訊都會被包含在此

## 引用

<https://github.com/JetBrains/kotlin/blob/master/libraries/stdlib/src/kotlin/coroutines/CoroutineContext.kt>
`CoroutineContext` `coroutineContext` import 位置不同
`coroutineContext` 存在兩個地方
各自關係如下
```js
// CoroutineContext 擴充類別
import kotlinx.coroutines.CoroutineContext
// CoroutineScope 內部的 coroutineContext 成員
import kotlinx.coroutines.CoroutineScope.coroutineContext
// CoroutineScope 內部的 coroutineContext 實作介面 CoroutineContext 類別
import kotlin.coroutines.CoroutineContext
// CoroutineScope 內部的 coroutineContext 實作介面 CoroutineContext 內部 inline 變數
import kotlin.coroutines.coroutineContext
```

## CoroutineContext 
### 成員

`coroutineContext` 訪問當前 `CoroutineContext`
透過 [] 訪問內部元素
```js
coroutineContext[CoroutineName]
coroutineContext[Job]
coroutineContext[CoroutineDispatcher]
coroutineContext[CoroutineExceptionHandler]
```

### 取得

`kotlin.coroutines.coroutineContext` 用來取得當前範圍 CoroutineContext 
```js
// source
@SinceKotlin("1.3")
@Suppress("WRONG_MODIFIER_TARGET")
@InlineOnly
public suspend inline val coroutineContext: CoroutineContext
    get() {
        throw NotImplementedError("Implemented as intrinsic")
    }
```

`CoroutineScope` 提供方法 `currentCoroutineContext()` 抓取
實際上等同直接去操作 `coroutineContext`
```js
// source
public suspend inline fun currentCoroutineContext(): CoroutineContext = coroutineContext
```

### 結合

`CoroutineContext` 主要作為協程元素集合類別 以下特性
  1. 用 `+` 連接內容
  2. 使用 [] 搜尋元素 (coroutineContext[Job])
  3. 父子協程 Context 基本不共用

關於 `CoroutineContext` 使用方式
```js
// 取得 CoroutineContext
fun main() = runBlocking<Unit> {
    println("My context is: $coroutineContext")
}
// 使用 + 連結
fun main() = runBlocking<Unit> {
    println("A context with name: ${coroutineContext + CoroutineName("test")}")
}
// CoroutineContext 內部成員
fun main() = runBlocking<Unit> {
    println("My job is: ${coroutineContext[Job]}")
}
```
<https://elizarov.medium.com/coroutine-context-and-scope-c8b255d59055>


### 新建

`CoroutineScope.newCoroutineContext()` 擴充方法 建立 `CoroutineContext`
實際上是去抓取 `coroutineContext` 仍舊需要先有 `CoroutineContext`
```js
@ExperimentalCoroutinesApi
public actual fun CoroutineScope.newCoroutineContext(context: CoroutineContext): CoroutineContext {
    val combined = foldCopies(coroutineContext, context, true)
    val debug = if (DEBUG) combined + CoroutineId(COROUTINE_ID.incrementAndGet()) else combined
    return if (combined !== Dispatchers.Default && combined[ContinuationInterceptor] == null)
        debug + Dispatchers.Default else debug
}
```

## CoroutineDispatcher 線程

`CoroutineDispatcher` 設置謝程
Kotlin 提供利用線程池 轉換成 Dispatcher 方法
假如想自己控管線程時可以使用此方法
```js
//建立 ExecutorCoroutineDispatcherImpl
val myDispatcher = Executors.newFixedThreadPool(2).asCoroutineDispatcher()

kotlinx.coroutines/newFixedThreadPoolContext

// 建立獨立線程
Executors.newSingleThreadExecutor().asCoroutineDispatcher()


val context1 = Executors.newFixedThreadPool(2).asCoroutineDispatcher() //有問題?
val context2 = newFixedThreadPoolContext(2, "Fixed")  //正常
val dbDispatcher3 = Dispatchers.IO.limitedParallelism(2)  //正常
```

`newFixedThreadPoolContext` 同等功能的方法
```js
// source
@DelicateCoroutinesApi
public actual fun newFixedThreadPoolContext(nThreads: Int, name: String): ExecutorCoroutineDispatcher {
    require(nThreads >= 1) { "Expected at least one thread, but $nThreads specified" }
    val threadNo = AtomicInteger()
    val executor = Executors.newScheduledThreadPool(nThreads) { runnable ->
        val t = Thread(runnable, if (nThreads == 1) name else name + "-" + threadNo.incrementAndGet())
        t.isDaemon = true
        t
    }
    return executor.asCoroutineDispatcher()
}
```