# ex_Android_保留

紀錄可能會使用到的範例程式

## 取得時間

package 來源
```js
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale
```

範例
```js
private fun getFormattedDate(): String {
    val calendar = Calendar.getInstance()
    calendar.add(java.util.Calendar.DATE, 1)
    val formatter = SimpleDateFormat("E MMM d", Locale.getDefault())
    return formatter.format(calendar.time)
}
```

## 網路API溝通

`Retrofit` 
`Ktor` 

## Kotlin_Coroutine Activity 安卓協程

紀錄 Activity 如何使用 Coroutine

## 範例

1. 繼承CoroutineScope使用job
```js
class MainActivity : AppCompatActivity(), CoroutineScope {
    //job沒有特別定義的話
    //launch，會跑在非MainThread，不可刷新UI
    private val job = Job()

    override val coroutineContext: CoroutineContext
        get() = job

    private fun runTimer() {
        launch {
            for (i in 10 downTo 1) {
                //不能刷新UI會閃退
                //binding.tvShow.text = "count down $i ..." // update text
                delay(1000)
            }
            //不能刷新UI會閃退
            //binding.tvShow.text = "Done!"
        }
        binding.btnTest.setOnClickListener {
            job.cancel()
        }
    }

    override fun onDestroy() {
        super.onDestroy()
        //只有取消這個job
        job.cancel()
    }
}
```

1. CoroutineScope委託MainScope
```js
//CoroutineScope委託MainScope
//此時launch會跑在MainThread
class MainActivity : AppCompatActivity(), CoroutineScope by MainScope() {
    override fun onDestroy() {
        super.onDestroy()
        //Scope cancel意味著全部取消
        cancel()
    }

    private fun runTimer() {
        val job = launch {
            for (i in 10 downTo 1) {
                binding.tvShow.text = "count down $i ..." // update text
                delay(1000)
            }
            binding.tvShow.text = "Done!"
        }
        binding.btnTest.setOnClickListener {
            //job cancel意味著只取消這個工作
            job.cancel()
        }
    }
}
```

1. CoroutineScope搭配suspend
```js
//CoroutineScope委託MainScope
//此時launch會跑在MainThread
class MainActivity : AppCompatActivity(), CoroutineScope by MainScope() {
    override fun onDestroy() {
        super.onDestroy()
        //Scope cancel意味著全部取消
        cancel()
    }

    //suspend讓自己原先的執行緒暫停
    //切換到Dispatchers.IO
    //處理完後，再回來原先的執行緒繼續往下作
    suspend fun readSomeFile() = withContext(Dispatchers.IO) {
        //模擬處理一些資料
        var j = 0
        for(i in 0 until 1000000000) {
            j += 1
        }
    }

    private fun runTimer() {
        val job = launch(Dispatchers.Main) {
            binding.tvShow.text = "123"
            binding.tvShow.visibility = View.VISIBLE
            readSomeFile()
            binding.tvShow.visibility = View.GONE
        }
        binding.btnTest.setOnClickListener {
            //job cancel意味著只取消這個工作
            job.cancel()
        }
    }
}
```

1. CoroutineScope+async+await
```js
//以下會打印result2

class MainActivity : AppCompatActivity(), CoroutineScope {
    private val job = Job()
    override val coroutineContext: CoroutineContext
        get() = job
    override fun onCreate(savedInstanceState: Bundle?) {
        val rawText: Deferred<String> = async(Dispatchers.IO) {
            "result2"
        }
        launch {
            //需放在此使用
            val text = rawText.await()
            println(text)
        }
        //在此用會error
        //rawText.await()
    }

    override fun onDestroy() {
        super.onDestroy()
        //Scope cancel意味著全部取消
        job.cancel()
    }
}
```

1. runBlocking+async+suspend
```js
// 以下會打印

The answer is 42
Completed in 1049 ms

private fun testFunction() = runBlocking {
    val time = measureTimeMillis {
        val one = async { doSomethingUsefulOne() }
        val two = async { doSomethingUsefulTwo() }
        println("The answer is ${one.await() + two.await()}")
    }
    println("Completed in $time ms")
}

suspend fun doSomethingUsefulOne(): Int {
    delay(1000L)
    return 13
}

suspend fun doSomethingUsefulTwo(): Int {
    delay(1000L)
    return 29
}
```

1. CoroutineScope+runBlocking
```js
val mScope = object : CoroutineScope {
    override val coroutineContext: CoroutineContext
        get() = Job()
}

private fun testFunction() = runBlocking {
    //先打印runBlocking 2
    //再打印runBlocking 1
    mScope.launch (Dispatchers.Main){
        delay(300)
        println("runBlocking 1")
    }
    mScope.launch (Dispatchers.Main){
        delay(100)
        println("runBlocking 2")
    }
    Thread.sleep(1000) //Keep process alive
}

fun testFunction2() {
    //先打印runBlocking 1
    //再打印runBlocking 2
    runBlocking (Dispatchers.IO){
        delay(300)
        println("runBlocking 1")
    }
    runBlocking  (Dispatchers.IO){
        delay(100)
        println("runBlocking 2")
    }
    Thread.sleep(1000) //Keep process alive
}

override fun onDestroy() {
    super.onDestroy()
    mScope.cancel()
}
```