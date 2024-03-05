# ex_kotlin_Job 協程控制

`Job` 說是協程本體 控管生命週期 呼叫 錯誤返回 父子關係等
`launch` `async` 會傳一個 `Job` 實作類別
可以用來操作生命週期

## 生命週期

`isActive()` 是否正在執行中
`isCancelled()` 是否已取消 只有使用 cancel() 中止 才會回傳 true
`isCompleted()` 是否已完成 不論是否被 cancel() 中止 結束狀態都回傳 true
```js
/** 官方狀態圖示
 * ```
 *                                       wait children
 * +-----+ start  +--------+ complete   +-------------+  finish  +-----------+
 * | New | -----> | Active | ---------> | Completing  | -------> | Completed |
 * +-----+        +--------+            +-------------+          +-----------+
 *                  |  cancel / fail       |
 *                  |     +----------------+
 *                  |     |
 *                  V     V
 *              +------------+                           finish  +-----------+
 *              | Cancelling | --------------------------------> | Cancelled |
 *              +------------+                                   +-----------+
 * ```
 * | **State**                        | [isActive] | [isCompleted] | [isCancelled] |
 * | -------------------------------- | ---------- | ------------- | ------------- |
 * | _New_ (optional initial state)   | `false`    | `false`       | `false`       |
 * | _Active_ (default initial state) | `true`     | `false`       | `false`       |
 * | _Completing_ (transient state)   | `true`     | `false`       | `false`       |
 * | _Cancelling_ (transient state)   | `false`    | `false`       | `true`        |
 * | _Cancelled_ (final state)        | `false`    | `true`        | `true`        |
 * | _Completed_ (final state)        | `false`    | `true`        | `false`       |
**/


val job = launch{...} //任意 launch
job.start()  //啟動
...
if(job.isActive()){...} //執行中
if(job.isCompleted()){ //已完成
  if(job.isCancelled()){...} //已取消
}
```

## Job 協程

<https://github.com/Kotlin/kotlinx.coroutines/blob/master/kotlinx-coroutines-core/common/src/Job.kt>

launch 反射型態會得到 StandaloneCoroutine 與 Job 關係如下
`Job` 是個 Interface
`AbstractCoroutine` 實作 Job 抽象類別
`StandaloneCoroutine` 繼承 AbstractCoroutine 類別
也就是 StandaloneCoroutine 是 Job 實作

```js
// StandaloneCoroutine = launch 型態 
val job = launch{}
```

`Job` 打印會顯示列別與狀態
```js
val job = Job()
println(job)
// class{state}@address
// JobImpl{Active}@4c98385c
```

`Job` 等同 CoroutineContext 關係如下
```js
//source
public interface CoroutineContext{}
public interface Element : CoroutineContext{}
public interface Job : CoroutineContext.Element{}
```

## Job 協程範圍

關閉 `Job` 等同範圍關閉
```js
// source
public fun CoroutineScope.cancel(cause: CancellationException? = null) {
    val job = coroutineContext[Job] ?: error("Scope cannot be cancelled because it does not have a job: $this")
    job.cancel(cause)
}
```

## Job 父子協程

`Job` 可以透過底下成員確認父子關係
  `parent` 回傳父 Job 可能是範圍或協程
  `children` 回傳子集合 Job 其底下所有子範圍或協程
```js
//source
public val parent: Job?
public val children: Sequence<Job>

//範例
val scope = CoroutineScope(Job())
val job = scope.launch(){...}
if(scope.coroutineContext.job.children.contains(job)){...}
if(job.parent == scope.coroutineContext.job){...}

```

`Job` 如果沒有獨立 不存在父子關係 被視為其他協程 期使在其範圍內生成
```js
runBlocking {
  launch(Job()) {...}
  //該 launch 不被視為 runBlocking 子協程 
  // runBlocking 不會等待
}
```

`job` launch 後產生的子協程 完成後也不會結束 父級 job
```js
suspend fun main(): Unit = coroutineScope {
    val job = Job()
    launch(job) { // the new job replaces one from parent
        delay(1000)
        println("Text 1")
    }
    launch(job) { // the new job replaces one from parent
        delay(2000)
        println("Text 2")
    }
    job.join() // 會一直持續等待 子協程結束並不會觸發父級的 complete()
    println("Will not be printed")
}
//修改
suspend fun main(): Unit = coroutineScope {
    val job = Job()
    launch(job) { // the new job replaces one from parent
        delay(1000)
        println("Text 1")
    }
    launch(job) { // the new job replaces one from parent
        delay(2000)
        println("Text 2")
    }
    job.children.forEach { it.join() }
}
```

## Job 協程阻塞

`Job` 阻塞等待完成 使用 join()
```js
runBlocking {
    val job1 = launch {
        delay(1000)
        println("Test1")
    }
    job1.join()
    println("All tests are done")
}
// 複數 Job 等待方式
runBlocking {
    launch {
        delay(1000)
        println("Test1")
    }
    launch {
        delay(2000)
        println("Test2")
    }

    val children = coroutineContext[Job]?.children

    val childrenNum = children?.count()
    println("Number of children: $childrenNum")
    children?.forEach { it.join() } // 歷遍所有子協程
    println("All tests are done")
}
```

## Job 完成結束

`complete()` 代表完成 一但呼叫不能再產生子協程
如果外部有 join() 其所有子協成完成後也不會卡死
```js
//手動 complete()
runBlocking {
    val job = Job()

    launch(job) {
        repeat(5) { num ->
            delay(200)
            println("Rep$num")
        }
    }

    launch {
        delay(2000)
        job.complete()
    }

    job.join() // 有呼叫 complete() 不會卡死
    println(job)
    println("Done")
}
//complete 呼叫不會中斷子協程
suspend fun main(): Unit = coroutineScope {
    val job = Job()
    launch(job) { // the new job replaces one from parent
        delay(1000)
        println("Text 1")
    }
    launch(job) { // the new job replaces one from parent
        delay(2000)
        println("Text 2")
    }
    job.complete()
    job.join()
}
```

`Job` complete 後無法作為協程使用
```js
runBlocking {
    val job = Job()

    launch {
        delay(1000)
        job.complete()
    }

    job.join() // 有呼叫 complete() 不會卡死
    println(job)
    launch(job) {  // 不會被啟動
        println("Will not be printed")
    }

    println("Done")
}
```

`cancel()` 中斷協成 包誇所有子協程
```js
suspend fun main(): Unit = coroutineScope {
  val parentJob = Job()
  val job = Job(parentJob)
  launch(job) {
      delay(1000)
      println("Text 1")
  }
  launch(job) {
      delay(2000)
      println("Text 2")
  }
  delay(1100)
  parentJob.cancel()
  job.children.forEach { it.join() }
}
```

## Job 異常捕抓

`SupervisorJob` 異常錯誤不影響父子層
`completeExceptionally()` 呼叫異常中斷 停止底下所有子協程
```js
fun main() = runBlocking {
    val job = Job()

    launch(job) { //中途會被停止
        repeat(5) { num ->
            delay(200)
            println("Rep$num")
        }
    }

    launch {
        delay(500)
        job.completeExceptionally(Error("Some error"))
    }

    job.join()
    println("Done")
}
```