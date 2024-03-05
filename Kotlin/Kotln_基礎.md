# Kotln 基礎

紀錄基礎用法

官方教學
<https://kotlinlang.org/docs/home.html>
<https://kotlinlang.org/docs/basic-syntax.html#type-checks-and-automatic-casts>
官方 api (可以連結原碼)
<https://kotlinlang.org/api/latest/jvm/stdlib/>


## import 規則

```js
//import 可以單獨引用 類別 或 變數，條件必須是同級別
import com.example.trainingwoof.data.Dog
import com.example.trainingwoof.data.dogs
//Dog.kt
data class Dog(...)
val dogs = listOf<Dog>(...)
```

## 基本語法

Hello world
```js
// 單行註解

/*  多行
    註解 */
/* 一層註解
/* 二層註解 *⁠/
   一層註解 */

//命名空間
package my.demo
//引用
import kotlin.text.*

//主程式進入口
fun main(){
  //控制台輸出
  print("Hello ")
  print("world!")
  //控制台輸出換行
  println(42)
}

```


## 方法
```js
//方法結構 fun funName(param1 : type, param2 : type): returnType{...}
fun sum(a: Int, b: Int): Int {
    //回傳，同時結束方法
    return a + b
}
//方法單行回傳，可省略輸出型態
fun sum(a: Int, b: Int) = a + b

//Unit 無回傳值
fun printMessage(message: String): Unit {...}

//允許回傳 null
fun parseInt(str: String): Int? {...}

//預設值
fun foo(a: Int = 0, b: String = "") {...}

//反覆呼叫?
val fruits = listOf("banana", "avocado", "apple", "kiwifruit")
fruits
    .filter { it.startsWith("a") }
    .sortedBy { it }
    .map { it.uppercase() }
    .forEach { println(it) }

//類別方法擴充
fun String.spaceToCamelCase() { ... }
println("Quiz".spaceToCamelCase)

// 擴充屬性，無法儲存數據，必須是唯讀
val Quiz.StudentProgress.progressText: String
    get() = "${answered} of ${total} answered"
println(Quiz.progressText)

//搭配 when
fun transform(color: String): Int = when (color) {
        "Red" -> 0
        "Green" -> 1
        "Blue" -> 2
        else -> throw IllegalArgumentException("Invalid color param value")
    }

//Unit 的方法構建器風格?
fun arrayOfMinusOnes(size: Int): IntArray {
    return IntArray(size).apply { fill(-1) }
}

//with 省略變數呼叫方法
val myTurtle = Turtle()
with(myTurtle) {
    penDown() //myTurtle.penDown()
    penUp() //myTurtle.penUp()
}

//別名呼叫，將自身傳入 let 方法，可以使用 it 呼叫
question2.let {
        println(it.questionText) // println(question2.questionText)
        println(it.answer)       // println(question2.answer)
        println(it.difficulty)   // println(question2.difficulty)
        }

//{}內等同自身類別，省略自身呼叫
val myRectangle = Rectangle().apply {
    length = 4       // Rectangle.length = 4
    breadth = 5      // Rectangle.breadth = 5
    color = 0xFAFAFA // Rectangle.color = 0xFAFAFA
}

// measureTimeMillis 計算執行時間
//import kotlin.system.*
val time = measureTimeMillis { ... }   
```

## 區域 Scope
### run
可以將 run 想像成一個獨立出來的 Scope ， run 會把最後一行的東西回傳或是帶到下一個 chain。
```js
val whatsMyName = "Francis"
run {
    val whatsMyName = "Ajax"
    println("Call my name! $whatsMyName")
}
println("What's my name? $whatsMyName")

//run 還能將最後一行的東西回傳，或傳給下一個 chain
run {
    val telephone = Telephone()
    telephone.whoCallMe = "English"
    telephone    // <--  telephone 被帶到下一個 Chain
}.callMe("Softest part of heart")    // <-- 這裡可以執行 `Telephone` Class 的方法

// callMe function in Telephone class
fun callMe(myName: String) {
    println("$whoCallMe ! Call me $myName !!");
}

//吝一個
val wowCall = run {
        val telephone = Telephone()
        telephone.fromWhere = "Sagittarius"
        telephone.whoCallMe = "Still Unknown"
        telephone  // <-- telephone 回傳，wowCall 型態成為 Telephone
    }
println("WOW, This signal is from ${wowCall.fromWhere}")
```

### with
with 一般常常作為初始化時使用， with(T) 之中的傳入值可以以 this (稱作 identifier) 在 scope 中取用，
不用打出 this也沒關係。雖然， with 也會將最後一行回傳，但目前看起來大部分還是只用它來做初始化。
透過 with()很明確知道是為了括弧中的變數進行設定。
```js
//{} 設定為 this 可以省略變數名稱 直接呼叫方法
val greatSmartphone = GoodSmartPhone()
with(greatSmartphone) {
    this.setCleanSystemInterface(true)
    
    // `this` is not necessary
    setGreatBatteryLife(true)
    setGreatBuildQuality(true)
    setNouch(ture)
}

//可為空的變數，如此一來 with的 scope 中就必須要宣告 「?」或「!!」
private fun buildGreatSmartphone(goodSmartPhone: GoodSmartPhone?) {
    with(goodSmartPhone) {
        this?.setCleanSystemInterface(true)
        this?.setGreatBatteryLife(true)
        this?.setGreatBuildQuality(true)
        this?.setNouch(ture)
    }
}
```

T.run 需要接在一個變數後面才行。像是 someVariable.run { /* do something */ }
T.run 也能像 with 一樣來做初始化，而且 extension function 有個好處是可以在使用時就進行 「?」 或 「!!」 的宣告。
另外，T 能夠以 this 的形式在 scope 內取用。像是上面的範例，如果用 T.run來 做初始化
```js
private fun buildGreatSmartphone(goodSmartPhone: GoodSmartPhone?) {
    goodSmartPhone?.run {
        this.setCleanSystemInterface(true)
        // `this` is not necessary
        setGreatBatteryLife(true)
        setGreatBuildQuality(true)
        setNouch(ture)
    }
}

//變數是空值， T.run{} 內的程式碼就根本不會執行
//T.run 和 run 的特性完全一樣。可以將最後一行的東西回傳，或是傳給下一個 chain。
//以下範例，要根據筆電系統版本印出 Windows 的開發代號：
// data class Laptop(maker, model, system)
val laptopA = Laptop("Dell", "XPS 13 9343c", "Windows 8.1")
val laptopB = Laptop("Lenovo", "T420s", "Windows 7")
val laptopC = Laptop("MSI", "GS65 Stealth", "Windows 10")
printWindowsCodeName(laptopA)
printWindowsCodeName(laptopB)
printWindowsCodeName(laptopC)

fun printWindowsCodeName(laptop: Laptop?) {
    val codename = laptop?.run {
            // `this` is Laptop.
            // `this` can ignore when use fields and methods
            system.split(" ")    // <-- pass to next chain
        }?.run {
            // `this` is the split strings. a List<String>
            val result = when (this.last()) {
                "7" -> "Blackcomb"
                "8" -> "Milestone"
                "8.1" -> "Blue"
                "10" -> "Threshold"
                else -> "Windows 9"
            }
            result    //  <-- pass value back
        }
    
    println("${laptop?.system} codename is $codename")
}

//取用外層變數或方法，但 this
presenter?.run {
    attachView(this@MainActivity)
    addLifecycleOwner(this@MainActivity)
}
```
### let
let/T.let
T 在 scope 內則是用 it 來存取而不是 this。也可以依照需求改成其他的名字，增加可讀性。
與 run 相同，會將最後一行帶到下一個 chain 或是回傳。
let 的操作方式基本上與上述的 run或 T.run大至相同
```js
class TreasureBox {
    private val password = "password"
    private val treasure = "You've got a Windows install USB"
    fun open(key: String?): String {
        val result = key?.let {
            // `it` is the key String.
            // `this` is TreasureBox.
            
            var treasure = "error"
            if (it == password) {
                treasure = this.treasure
            }
            treasure     // <-- pass value back
        } ?: "error"
      
        return result
    }
}

// 希望 this 可以存取到上層內容時，建議使用 let 
val treasureBox = TreasureBox()
println("Open the box , and ${treasureBox.open(null)}")
println("Open the box , and ${treasureBox.open("admin")}")
println("Open the box , and ${treasureBox.open("password")}")

key?.let { topSecretPassword ->
    var treasure = "error"
    if (topSecretPassword == password) {
        treasure = this.treasure
    }
    treasure
}
```
### also
also/T.also
使用於初始化物件  run 與 let的不同之處在於： 
run與 let 會將最後一行傳給下個 Chain 或是回傳，物件類型依最後一行而定； 
also和 apply 則是將「自己 (this)」回傳或傳入下個 chain。
 also在 scope 內可以透過 it 來存取 T本身
```js
// 半糖少冰
val drink = FiftyLan().also {
    it.setSugarLevel(FiftyLan.SugarLevel.Half)
}.also {
    it.setIceLevel(FiftyLan.IceLevel.Few)
}.also {
    it.要多帶我們一杯紅茶拿鐵嗎好喝喔 = false
}.also {
    it.plasticBag = true
}

drink.printResult()
```

### apply
apply/T.apply 
apply與 also 有像，不同的地方是 apply 在 scope 內 T的存取方式是 this ，其他都與 also 一樣。

```js

companion object {
    private const val COFFEE_SHOP_LIST_KEY = "coffee-list-key"
    fun newInstance(coffeeShops: List<CoffeeShop>): ListFragment {
        return ListFragment().apply {
            // `this` is `ListFragment` in apply scope  
            arguments = Bundle().also {
                // `it` is `Bundle` in also scope
                // `this` is `ListFragment`        
                it.putParcelableArrayList(COFFEE_SHOP_LIST_KEY, coffeeShops as ArrayList<out Parcelable>)
            }
        }
    }
}
```
需要傳遞自己，即有 apply 或 also
需要傳遞自己回傳最後一行， run、 T.run、 with 和 let

## 變數 val
```js
//宣告 名稱 : 類型 = 初始值
//val name : type = value
val a: Int = 1
//省略型態，自動推測
val b = 2
//不初始化，可能出錯
val c: Int
// 延遲作業?
c = 3
// 基礎型態允許 null
val b: Boolean? = ...
if (b == true) {
    ...
} else {
    // `b` is false or null
}

//方法外呼叫
var x = 0
fun incrementX() { 
    x += 1 
}

//無效 null
if (x != null){
  return
}

//任意型態 any
val obj: Any
//型態判定 is
if (obj is String) {
    // `obj` is automatically cast to `String` in this branch
    return obj.length
}
// 左側優先判定，避免 obj.length 錯誤
if (obj is String && obj.length > 0) {
    return obj.length
}

//懶惰屬性?
val p: String by lazy { 
  // 該值僅在首次訪問時計算
  //計算字符串
}

// var?. 判定是否 null 不執行
val files = File("Test").listFiles()
println(files?.size)

//變數不為 null 執行
val value = ...
value?.let { ... }

//呼叫後再透過 ?: 判定 ， null 則後段
val mapped = value?.let { transformValue(it) } ?: defaultValue

//搭配 if-else
val y = if (x == 1) {
    "one"
} else if (x == 2) {
    "two"
} else {
    "other"
}

//交換變數內容?
var a = 1
var b = 2
a = b.also { b = a }

//可能為 null 強制使用方法(可能錯誤)
var favoriteActor: String? = "Sandra Oh"
println(favoriteActor!!.length)

//反射 java
System.out.println(obj.javaClass.name)                 // double
System.out.println(obj.javaClass.typeName)             // type
System.out.println(obj.javaClass.kotlin)               // class kotlin.Double
System.out.println(obj.javaClass.kotlin.qualifiedName) // kotlin.Double
```

數字
```js
// 無條件進位整數
kotlin.math.ceil(x)
```

字串 string
```js
var a = 1
// $變數
val s1 = "a is $a" 

a = 2
// ${}單行程式
val s2 = "${s1.replace("is", "was")}, but now is $a"
```

object 物件
```js
// 宣告了 object，都代表寫了一個 class, 並且立即產生一個實體 特性如下
// class 宣告 不能包含 constructor, 所以也不能被其他人建立
object Singleton{
    fun singleFuc() {}
}

// companion object 相當於 class 對應的 static 區塊
class Com{
    companion object {
        const val SOME_VALUE = 3
    }
}
println(Com.SOME_VALUE)   //直接呼叫

// 變數任意物件宣告，生成指定的型態，匿名物件
btn.setOnClickListener(object : View.OnClickListener{
    override fun onClick(v: View?) {...}
})

// companion object 搭配變數任意物件宣告
companion object CREATOR : Parcelable.Creator<User>{
    // ...
}

// interface companion object 結合成默認物件
interface Modifier {
  ...
  companion object : Modifier {...}
}
fun call(mod :Modifier = Modifier){...}   //預設默認

```

## 條件式
### if-else
```js
//if(條件) {...} else {...}
if (a > b) {
    return a
} else {
    return b
}

//單行條件
if (a > b) a else b

//方法搭配單行條件式
fun maxOf(a: Int, b: Int) = if (a > b) a else b

//條件範圍 c in a..b
val x = 10
val y = 9
if (x in 1..y+1) {
    println("fits in range")
}

//判定索引範圍
val list = listOf("a", "b", "c")
if (-1 !in 0..list.lastIndex) {
    println("-1 is out of range")
}
//?
if (list.size !in list.indices) {
    println("list size is out of valid list indices range, too")
}

// ?: If-not-null-else 簡寫
// (not-null)?:(...)
val files = File("Test").listFiles()
println(files?.size ?: "empty") // Null 不執行，否則 "empty" ?
// 如果執行複雜怎使用 run{...}
val filesSize = files?.size ?: run {
    val someSize = getSomeSize()
    someSize * 2
}
```

### try-catch 
`try-catch` 除錯測試
```js
val result = try {
      count()
  } catch (e: ArithmeticException) {
      throw IllegalStateException(e)
  } finally {
      println("Done")
  }

//Java 7 的 try-with-resources
val stream = Files.newInputStream(Paths.get("/some/file.txt"))
stream.buffered().reader().use { reader ->
    println(reader.readText())
}

//check 判定 false 拋出錯誤 
check(value <= 1) { "Collected $value" }
```
### when

`when` 條件選擇
```js
// val 變數 = when(內容){ 等於 -> 回傳 }
val s = when (obj) {
    1          -> "One" // 條件判斷
    "orange", "Hello"    -> "Greeting" // 多條件判斷
    is Long    -> "Long" // is 類型 判定型態
    !is String -> "Not a string"
    else       -> { // else 其他
      println("Whatever");
      "Unknown"
    } // { 多行 }
}
```

`when` 無參數 等同 `if-else`
```js
// 不使用參數 僅判斷 true false
val randomNumber = (1..99).random()

when {
    randomNumber.isEven() -> println("$randomNumber , This is an even number.")
    randomNumber.isOdd() -> println("$randomNumber, This is an odd number. ")
    else -> println("$randomNumber, This is what??")
}
```

`when` 集合包含
```js
// 字串
val items = ["orange", "bad"]
when {
    "orange" in items -> println("juicy")
    "apple" in items -> println("apple is fine too")
}
// 數字
val randomNumber = (1..99).random()
when (randomNumber) {
    in 1..25 -> println("$randomNumber is Not big enough!")
    in 26..40 -> println("$randomNumber is umm... not big or small")
    !in 1..40 -> println("$randomNumber is quite LARGE!")
}
```

`when` 自動轉型
```js
//自動轉型 在內部會被自動視為該型態 不需轉型
private fun whatItIs(any: Any) = when (any) {
    is String -> println("${any.first()}")
    is Int -> println("${any * 2}")
    else -> println("Whatever")
}
```

`when` 搭配 `for` 迴圈 使用 `break` `continue`
```js
val colors = setOf("Red", "Green", "Blue")
for (color in colors) {
    when(color) {
        "Red" -> break // 直接脫離迴圈
        "Green" -> continue // 進入下一個環圈
        "Blue" -> println("This is blue") // 執行完畢後繼續迴圈
    }
}
```

### for while
`for` `while` 迴圈式
```js
// 迴圈次數 1~5
for (x in 1..5) {...}
// 宣告範圍呼叫方法 forEach 迭代
(1..10).forEach { ... }

// for(內容 in 陣列) {...}
for (item in items) {
    println(item)
}

// 抓取索引陣列(0, 1, 2)
for (index in items.indices) {
    println("item at $index is ${items[index]}")
}


// while(條件) {...}
var index = 0
while (index < items.size) {
    println("item at $index is ${items[index]}")
    index++
}
```



## `lambda` 表達式
```js
// lambda 型態寫法  ， 視作方法型態
(Int) -> Unit 

// 方法型態， 隱式參數 it 
{ it } //最後一行為回傳 it

// lambda 形式的變數 沒有參數
val treat: () -> Unit = {
    println("Have a treat!")
}

// lambda 形式的變數 有參數
val coins: (Int) -> String = { quantity ->
    "$quantity quarters"
}

// lambda 形式的變數 有參數 使用 隱式參數 it
val coins: (Int) -> String = {
    "$it quarters"
}

//傳遞尾隨，如果函數的最後一個參數是函數，則可以將作為對應參數傳遞的 lambda 表達式放在括號之外：
val product = items.fold(1) { acc, e -> acc * e }
run { println("...") }  //唯一參數，則可以省略括號

// 可為 null 方法型態，等同可為 null 型態
// ((Int) -> String)?
fun trickOrTreat(extraTreat: ((Int) -> String)?){...}

val trickFunction = trickOrTreat(null) //允許
trickFunction()

// 尾隨 lambda 參數
// lambda 參數位在最後，可以將 {} 提出到外面，效果不會變
fun trickOrTreat(trick: bool, extraTreat: ((Int) -> String)){...}
val treatFunction = trickOrTreat(false) { "$it quarters" }

// 尾隨 lambda 搭配 repeat()
// repeat(times: Int, action: (Int) -> Unit) 反覆執行
repeat(3){println(it)} // it 傳入循環次數
```

`inline` `noinline` 內聯
`inline` 方法被編譯後會將程式碼直接貼到呼叫處，節省資源消耗
`noinline` 則是不允許後續 `inline` 修飾的方法
```js
// 方法
inline fun foo( // 修飾 inline 方法
             inlined: () -> Unit,
    noinline notInlined: () -> Unit // 修飾 noinline 變數
) { ... }

// 變數
var bar: Bar
    get() = ...
    inline set(v) { ... } // 修飾特定
inline var bar: Bar //修飾變數
    get() = ...
    set(v) { ... }

// inline 搭配 尾隨 lambda 配合 return
fun inline(block: () -> Unit) {...}
inline fun inlined(block: () -> Unit) {...}

inlined { return } // 允許 return
inline {} // 不允許 return
```

`reified` 具體型別 依據傳入型態進行判別
泛型`T` 不能被 `is` 使用 `inline fun <reified T>` 語法糖包裝 達到執行期間取的型別
```js
// 擴充方法 依據回傳型態
inline fun <reified T> Any.asType(): T? {
    if (this is T) {
        this
    } else {
        null
    }
}
var v:Int = key.asType()?: 0 //必須要標示型別

// 判別型態 依據參數型態
inline fun <reified T> Bundle.plus(key: String, value: T) {
    when(value) {
        is String -> putString(key, value)
        is Int-> putInt(key, value)
    }
}
var a:Int = 0
var b:String = "hello"
Bundle.plus("TAG", a)
Bundle.plus("TAG", b)
```

