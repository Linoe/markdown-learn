# Kotlin_ExceptionHandler 偵錯

紀錄針對錯誤處理方式

## 錯誤範圍

`join` `await` 子協程 依據以下規則
1. `join` 獨立協程錯誤不會影響任何
   ```js
    val job = GlobalScope.launch { // 不屬於任何協程範圍
        throw IndexOutOfBoundsException() 
    }
    job.join() //錯誤 仍會繼續執行
   ```
2. `await` 獨立協程錯誤導致持續等待中斷
   ```js
   val deferred = GlobalScope.async {
        throw ArithmeticException() 
    }
    deferred.await() //錯誤 整個中斷
   ```
3. `try-catch` 捕抓到獨立處理 不會中斷 必須在協程外部
   ```js
    launch{
      throw ArithmeticException() //不能在此處捕抓
    }
    try {
        launch.join() //外部才會被 try-catch 捕抓
    } catch (e: ArithmeticException) {
        println("Caught ArithmeticException")
    }
   ```
4. `CoroutineExceptionHandler` 捕抓到處理錯誤訊息 仍會中斷
   ```js
    val handler = CoroutineExceptionHandler { _, exception ->  //當前協程的錯誤捕抓
        println("CoroutineExceptionHandler got $exception") 
    }
    val job = GlobalScope.launch(handler) {
        throw AssertionError()
    }
    val deferred = GlobalScope.async(handler) {
        throw ArithmeticException() // 無法補抓 必須呼叫 await()
    }
    joinAll(job, deferred)   
   ```
5. 通常子協程錯誤會往上中斷 導致父協程中斷
   ```js
    launch {
        val child = launch {
            throw AssertionError()
        }
        child.join()
        println("Done") //執行不到 子協程錯誤導致父協程中斷 
    }
   ```
6. `Supervisor` 子協程錯誤不會往上中斷 但父協成錯誤會往下中斷
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
   ```

## CoroutineExceptionHandler

`CoroutineExceptionHandler` 捕抓協程錯誤 但仍會傳遞錯誤

等待中被中斷 捕抓不到 改用 `try-finally` 捕抓
```js
val handler = CoroutineExceptionHandler { _, exception -> 
    println("$exception") 
}
val job = GlobalScope.launch(handler) {
    launch { // 等待被中斷協程
        try {
            delay(Long.MAX_VALUE) //等待被其他協程中斷
        } finally {
            println("1 Children finally") 
        }
    }
    launch { // 引發錯誤協程
        delay(10)
        throw ArithmeticException()
    }
}
job.join() 
```

`CoroutineExceptionHandler` 只會捕抓 第一個
第二個會在 `exception.suppressed.contentToString()`
```js

@OptIn(DelicateCoroutinesApi::class)
fun main() = runBlocking {
    val handler = CoroutineExceptionHandler { _, exception ->
        println("$exception")    //捕抓 throw IOException()
        println("${exception.suppressed.contentToString()}")  //捕抓 throw ArithmeticException()
    }
    val job = GlobalScope.launch(handler) {
        launch {
            try {
                delay(Long.MAX_VALUE) // it gets cancelled when another sibling fails with IOException
            } finally {
                throw ArithmeticException() // the second exception
            }
        }
        launch {
            delay(100)
            throw IOException() // the first exception
        }
    }
    job.join()  
}
```

`try-catch` 引發錯誤會被 `CoroutineExceptionHandler` 捕抓

```js
@OptIn(DelicateCoroutinesApi::class)
fun main() = runBlocking {
    val handler = CoroutineExceptionHandler { _, exception ->
        println("CoroutineExceptionHandler got $exception")
    }
    val job = GlobalScope.launch(handler) {
        val inner = launch { // 沒有使用 handler 錯誤不會被捕抓?
            launch {
                launch {
                    throw IOException()
                }
            }
        }
        try {
            inner.join()
        } catch (e: CancellationException) {
            println("Rethrowing CancellationException with original cause")
            throw e // 重新引發錯誤 會被 CoroutineExceptionHandler 捕抓
        }
    }
    job.join()    
}
```

### 原理

`CoroutineExceptionHandler` 協程例外炳 發生錯誤後會收到兩個參數
  `context` 發生錯誤 CoroutineContext
  `exception` 錯誤訊息 Throwable
`CoroutineExceptionHandler` 在錯誤發生時 會從任意執行緒被呼叫
```js
val handler = CoroutineExceptionHandler { context, exception -> 
    println("got $exception") 
}
supervisorScope {
    val child = launch(handler) {
        throw AssertionError() // 錯誤會被 CoroutineExceptionHandler 捕抓
    }
    child.join() // supervisorScope 不會被子協程錯誤中斷
    println("Done")
}
```

協成例外傳播有以處理方式
1. 子協程自行處理 (try-catch)
2. 子協程的錯誤不會傳播到父級(supervisorScope)
3. CoroutineExceptionHandler 捕抓

沒有處理(try-catch)且未捕獲(CoroutineExceptionHandler)錯誤 以下方式處理：

- 錯誤是CancellationException 則忽略 因為這些錯誤用於取消協程
- 如果 context 存在 Job 則呼叫 Job.cancel 
- 最後以特定於平台的方式處理異常

## SupervisorJob

`SupervisorJob` 令同級子協程 錯誤也不會中斷 與其他捕抓錯誤比較
1. `CoroutineExceptionHandler` 子協程不影響
   ```js
   with(CoroutineScope(SupervisorJob())) {
      val firstChild = launch(CoroutineExceptionHandler { _, _ ->  }) {
          throw AssertionError()
      }
      println("Done") //不會中斷
   }
   ```
2. `try-catch` 捕抓不到 子協程不影響
  ```js
  with(CoroutineScope(SupervisorJob())) {
      val firstChild = launch(CoroutineExceptionHandler { _, _ ->  }) {
          throw AssertionError() //引發後 firstChild 會中斷
      }
      try{
          firstChild.join()
      }catch(e: Exception){
          println("Error") // 不會觸發
      }
      println("Done") //不會中斷
  }
  ```
3. 等待中 `try-finally` 去捕抓
   ```js
    val supervisor = SupervisorJob()
    with(CoroutineScope(supervisor)) {
        val secondChild = launch {
            try {
                delay(Long.MAX_VALUE)
            } finally {
                println("finally")
            }
        }
        delay(100)
        supervisor.cancel()
    }
   ```
4. `try-catch` 最外圈可以捕抓
   ```js
    try {
        supervisorScope {
            val child = launch {
                try {
                    delay(Long.MAX_VALUE) 
                } finally {
                    println("The child is cancelled") // 先觸發
                }
            }
            yield()
            throw AssertionError()
        }
    } catch(e: AssertionError) {
        println("Caught an assertion error") // 後觸發
    }
   ```

`supervisorScope` 等同使用 CoroutineScope(coroutineContext + SupervisorJob())