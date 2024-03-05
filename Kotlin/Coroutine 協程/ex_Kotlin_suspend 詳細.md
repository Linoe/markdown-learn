# ex_Kotlin_suspend 詳細

`suspend` 用在方法(fun) 委派(()->{}) 標記此為協程
此用紀錄原理

## suspend 作用

`suspend` 被稱為掛起函數 原因用來修飾方法關鍵詞
```js
suspend fun myFunc(){...}
```

`suspend` 本身代表 協成內執行 可以使用線程相關方法
```js
suspend fun myFunc(){
  delay(1000)
  Thread.sleep(1000)
}
```

`suspend` 修飾協程 會直接繼承呼叫 CoroutineContext
```js
fun main() = runBlocking<Unit> {
    launch { scopeCheck(this) }
}
suspend fun scopeCheck(scope: CoroutineScope) {
    println(scope.coroutineContext === coroutineContext)
}
```

## suspend 原理
<https://medium.com/jastzeonic/kotlin-coroutine-%E5%B9%95%E5%BE%8C%E9%82%A3%E4%B8%80%E5%85%A9%E4%BB%B6%E4%BA%8B-4e07b08cb31a>
`suspend` 用來修飾 fun 可以使用線程相關語法
`suspend` 代表掛載一個協程

以下反編譯 java 
```js
// kotlin
suspend fun getUserDescription(name:String,id:Int):String{
    return ""
}
// java
instance.getUserDescription("name", 0, new Continuation<String>() {
    @NotNull
    @Override
    public CoroutineContext getContext() {
        return null;
    }

    @Override
    public void resumeWith(@NotNull Object o) {

    }
});
return 0;
```

`Continuation` 每個協程都會有一個 處理與其他協程 return 與 Context
用處是編譯器使用 非程式開發者直接使用
實現協程中 執行結束時 回報銷毀協程

resumeWith() 回傳的值
```js
public interface Continuation<in T> {
    public val context: CoroutineContext
    public fun resumeWith(result: Result<T>)
}
```

`Continuation` 編譯器會生成 label (state) invokeSuspend(callback)
suspendFunction("text",this) 可以看到會把自己傳進去
suspendFunction 反射後可以得到與 `Continuation` 相似的代碼

可以確定 `suspend` 建立一個協程接口 同時被視作協程
如果使用到該方法 編譯器會轉編譯成 `Continuation` 接口範圍 當作 callback 互相通知與回傳
```js
//kotlin
fun main() {
    GlobalScope.launch {
        val text = suspendFunction("text")
        println(text) // print after delay
    }

}

suspend fun suspendFunction(text:String) = withContext(Dispatchers.IO){
    val result = doSomethingLongTimeProcess(text)
    result
}
// java
Continuation { // GlobalScope.Lanuch()
    var label = 0
    fun invokeSuspend(result:Any):Any{
        when(label){
            0->{
                val functionResult = suspendFunction("text",this)
                lable = 1
                if(functionResult == COROUTINE_SUSPENDED){
                    return functionResult
                }
            }
            1->{
                throwOnFailure(result)
                break
            }
        }
        val text = result as String
        print(text)
    }
}

public static final Object suspendFunction(@NotNull final String text, @NotNull Continuation $completion) {
   return BuildersKt.withContext(
(CoroutineContext)Dispatchers.getIO(), (Function2)(new Function2((Continuation)null) {
//...ignore
   }), $completion);
}
// Java 反射
Object text = instance.suspendFunction("", new Continuation<String>() {
    @NotNull
    @Override
    public CoroutineContext getContext() {
        return Dispatchers.getMain();
    }

    @Override
    public void resumeWith(@NotNull Object o) {

    }
});
System.out.println(text);
```