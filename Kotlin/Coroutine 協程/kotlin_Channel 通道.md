# kotlin_Channels 通道

Channel 一個溝通的管道，通常是用來傳遞耗時過程後最後的結果，只負責傳接資料

## 引用
```js
import kotlinx.coroutines.*
import kotlinx.coroutines.channels.*
```

## 類別關係

`Channel` 通道 可以在不同協程傳遞與等待回傳值
`Produce` 協程通道 建立一個協程處理通道
`select` 表達式 {} 內可以同時處理多個通道
`ticker` 通知用計時型通道工具? 固定時間延遲

## 基礎
`Channel<T>` 泛型建立 T 傳遞資料 可以在不同協程傳遞資料
```js
val channel = Channel<String>() //泛型 確定傳遞資料
launch {    
    channel.send("Done") // send 傳遞資料
}
{ println(channel.receive()) } // receive 接受資料
```

`channel.send` 傳遞資料 持續等待
`channel.receive` 接收資料 持續等待
```js
channel = Channel<Int>()
launch {
    for (x in 1..5) channel.send(x) // 傳遞 5 次
}
repeat(5) { println(channel.receive()) } // 接收 5 次
```

`channel.close()` 結束通道 結束後不能再任何動作 通常通知迴圈中結束
```js
launch {
    for (x in 1..5) channel.send(x)
    channel.close() // 通知 迴圈結束
}
for (i in channel){ // 尚未 close 持續接收
   println(i)
}
```

`Produce<T>` T傳遞資料 建立一個可以用 Channel 協程 通常使用在 fun
```js
//  ReceiveChannel<Int> 可以省略
fun CoroutineScope.produceFrom(): ReceiveChannel<Int> = produce<Int> {
    send(100)
}

var pd = produceFrom() // 取得 channel
val rus = pd.receive() // 視作 channel 使用
```

## 進階
### Channel 通道

`SendChannel<T>` Channel 型態 使用在通道串接傳遞
```js
suspend fun sendString(channel: SendChannel<String>) {
    channel.send("Done")
}

val channel = Channel<String>()
launch { sendString(channel)} //協程 不會造成 傳遞等待
println(channel.receive())
```

`Channel<T>(n)` 建立帶有 n 元素緩衝 滿時會等待
```js
val channel = Channel<Int>(4)
val sender = launch {
    repeat(10) { // 迴圈 4 次
        println("$it") // it 為 index
        channel.send(it) // 第 5 次時會進入等待
    }
}
delay(1000) // 等待測試打印
sender.cancel() // 避免錯誤 關閉
```

`consumeEach` 持續接收所有元素直到通道關閉
```js
val channel = Channel<Int>
launch {
  for (x in 1..5) channel.send(x * x)
  channel.close() // 不關閉 無法通知等待接收的通道
}
channel.consumeEach { println(it) } //持續到 close
```

`channel.seed` 不同協程 持續阻塞直到被接收
`channel.receive` 也是會持續阻塞
```js
val channel = Channel<String>()
launch { 
  delay(200)
  channel.send("Boo")
  delay(300)
  channel.send("Boo")
}
launch { 
  delay(300)
  channel.send("Foo")
  delay(200)
  channel.send("Foo")
}
repeat(4) { println(channel.receive()) }
}
```

`for channel` 迴圈使用 通道 持續等待
```js
launch {
    for (x in 1..5) channel.send(x)
    channel.close() // 通知 迴圈結束
}
for (i in channel){ // 尚未 close 持續接收
   println(i)
}
```

`for channel` 搭配多協程 依序等待
```js
data class Ball(var hits: Int)
suspend fun player(name: String, table: Channel<Ball>) {
    for (ball in table) { // 持續等待
        ball.hits++
        println("$name $ball")
        delay(300) // 延遲避免重疊
        table.send(ball) // 自動循環兩個協程互相傳遞
    }
}

val table = Channel<Ball>()
launch { player("ping", table) }
launch { player("pong", table) }
table.send(Ball(0)) // 作為啟動
// 等待 結束協成
delay(1000) // 等待 自動協程自動傳遞資料
coroutineContext.cancelChildren()

```

### Produce 協程通道

`Produce` 參數使用 ReceiveChannel 可以互相串接
`ReceiveChannel<T>` 用於 produce 回傳值型態 製作串接通道時會使用
```js
fun CoroutineScope.produceNumbers() = produce<Int> {
    var x = 1
    while (true) send(x++) // 持續 +1 傳送
}

fun CoroutineScope.square(numbers: ReceiveChannel<Int>): ReceiveChannel<Int> = produce {
    for (x in numbers) send(x * x) // 持續接收 外部通道
}

val numbers = produceNumbers() // 作為內部通道
val squares = square(numbers) // 作為操作偷動
repeat(5) {
    println(squares.receive()) //
}
coroutineContext.cancelChildren() // produce 是子協程 會被關閉
```
### 多通道處理 Select

`select<T>` 同時處理多個通道 T 回傳值
`onReceive` 等同 receive 接收值
```js
fun CoroutineScope.call(s: String) = produce<String> {
    while (true) { // 保持持續傳遞
        delay(500)
        send(s)
    }
}
val fizz = call("fizz")
val buzz = call("buzz")
repeat(7) {
    select<Unit> { // <Unit> 不會產生回傳值
        fizz.onReceive { value ->  // 接收後進入等待
            println(value)
        }
        buzz.onReceive { value ->  // 因為有延遲 交替接受
            println(value)
        }
    }
}
coroutineContext.cancelChildren() // 關閉所有子協程 
```

`onReceiveCatching` 等同 onReceive 但可以接收異常 異常時接收到 NULL
```js
val pd = produce<String> {
    repeat(4) { send("Hello $it") }
}
repeat(8) { //資料筆數最多 4 次 超過 4 次後開始傳送 null
  val rus = select<String> {
      pd.onReceiveCatching { it ->
          val value = it.getOrNull() //使用 getOrNull() 取得數值
          "$value"
      }
  }
  println(rus)
}
coroutineContext.cancelChildren() 
```

`onSend` 等同 send 傳遞資訊
```js
fun CoroutineScope.produceNumbers(side: SendChannel<Int>) = produce<Int> {
    for (num in 1..10) { // 打印 10 筆資料
        delay(100) 
        select<Unit> {
            onSend(num) {} // 傳遞自己
            side.onSend(num) {} // 傳遞外部通道
        }
    }
}

val side = Channel<Int>() // 建立
launch { // 開啟持續打印 協程
    side.consumeEach { println("$it") }
}
produceNumbers(side).consumeEach { //傳遞 side 通道 並持續處理美個元素
    println("Consuming $it")
    delay(250) // 延遲打印
}
coroutineContext.cancelChildren() 
```

`onAwait` 等同 await 等待接收 async 協程回傳 只接收最快回傳的
```js
// onAwait 接收 async 回傳值 
fun CoroutineScope.asyncString(time: Int) = async {
    delay(time.toLong())
    "$time ms"  // async 回傳值 onAwait 接收
}

val result = select<String> { //result 會優先回傳為取得
  asyncString(10).onAwait { answer ->
      "delay '$answer'"  //得到
  }
  asyncString(20).onAwait { answer ->
      "delay '$answer'"  //遺失
  }
}
println(result)
coroutineContext.cancelChildren()
```


### Ticker 不清楚用法

`ticker` 作為計算通道傳遞延遲工具?

`ticker` 傳遞一個 ReceiveChannel<Unit> 通道 每次呼叫會延遲
  `delayMillis` 傳遞元素每次延遲
  `initialDelayMillis` 第一個元素初始延遲
  `context` 使用的協成
  `mode` 模式 使用 TickerMode
    `FIXED_PERIOD` 如果 receive 緩慢 則調整延遲以維持固定時間 預設
    `FIXED_DELAY` 如果 receive 緩慢 則在生成的元素之間保持固定的延遲
```js
val tickerChannel = ticker(delayMillis = 100, initialDelayMillis = 0) // 创建ticker通道
var nextElement = withTimeoutOrNull(1) { tickerChannel.receive() }
println("Initial element is available immediately: $nextElement") // 初始化延时还没传递

nextElement = withTimeoutOrNull(50) { tickerChannel.receive() } // 所有后续元素有100毫秒延时
println("Next element is not ready in 50 ms: $nextElement")

nextElement = withTimeoutOrNull(60) { tickerChannel.receive() }
println("Next element is ready in 100 ms: $nextElement")

// 模拟消费大延时
println("Consumer pauses for 150ms")
delay(150)
// 下一个元素立即可取
nextElement = withTimeoutOrNull(1) { tickerChannel.receive() }
println("Next element is available immediately after large consumer delay: $nextElement")
// 请注意，“receive”调用之间的暂停会被考虑在内，下一个元素会更快地到达
nextElement = withTimeoutOrNull(60) { tickerChannel.receive() } 
println("Next element is ready in 50ms after consumer pause in 150ms: $nextElement")

tickerChannel.cancel() // 表明不再需要元素了
```
