# ex_Kotlin_簡易例子

官方提供用來學習 Kotlin 編碼例子

## 方法

Hello, World
```js
package org.kotlinlang.play         // 命名空間包，如果其他檔案引用則 import 此

fun main() {                        // main()，應用程序的入口
    println("Hello, World!")        // "Hello, World!" 輸出到控制台
}
```

方法 `fun` 默認參數值和命名參數
```js
fun printMessage(message: String): Unit {                               // 參數 message 類行為 String ， Unit 無回傳值
    println(message)
}
//printMessage("Hello")
fun printMessageWithPrefix(message: String, prefix: String = "Info") {  // 參數 prefix 默認值 "Info"，省略回傳值等同 Unit
    println("[$prefix] $message")
}
//printMessageWithPrefix("Hello", "Log")                                //完整參數呼叫
//printMessageWithPrefix("Hello")                                       //省略參數呼叫，僅限於有默認值
//printMessageWithPrefix(prefix = "Log", message = "Hello")             //帶有參數名稱呼叫，依據名稱而不是依序
fun sum(x: Int, y: Int): Int {                                          // Int 回傳值， reutrn 回傳 x + y
    return x + y
}

fun multiply(x: Int, y: Int) = x * y                                    // = 單行回傳值，等同上面
```


方法命名參數
```js
fun format(userName: String, domain: String) = "$userName@$domain"

fun main() {
    println(format("mario", "example.com"))                         // 依據參數順序輸入
    println(format(userName = "foo", domain = "bar.com"))           // 依據參數名稱輸入
    println(format(domain = "frog.com", userName = "pepe"))         // 同上
```

中綴 `infix` 方法
```js
fun main() {

  infix fun Int.times(str: String) = str.repeat(this)        // 擴充 Int.times() ，定義為 infix 中縐方法為 str.repeat()
  println(2 times "Bye ")                                    // 透過 Int.times() 方法 呼叫 str.repeat()

  val pair = "Ferrari" to "Katrina"                          // pair 定義為 String.to() 中縐方法變數?
  println(pair)

  infix fun String.onto(other: String) = Pair(this, other)   // 擴充 String.onto() ，定義為 infix 中縐方法為 Pair(other)
  val myPair = "McLaren" onto "Lucas"                        // 進行了兩次中縐方法
  println(myPair)                                            // "McLaren" onto "Lucas" 到 "Ferrari" to "Katrina"
}

class Person(val name: String) {
  val likedPeople = mutableListOf<Person>()
  infix fun likes(other: Person) { likedPeople.add(other) }  // 建立 Person.likes() ，定義為 infix 中縐方法為 likedPeople.add(other)
}
  val sophia = Person("Sophia")
  val claudia = Person("Claudia")
  sophia likes claudia   
```

操作 `operator` 方法
```js
operator fun Int.times(str: String) = str.repeat(this)       // 定義 Int * String 時的 中輟方法
println(2 * "Bye ")

operator fun String.get(range: IntRange) = substring(range)  // 定義 String[ .. ] IntRange範圍時的 中輟方法
val str = "Always forgive your enemies; nothing annoys them so much."
println(str[0..14])
```

修飾 `vararg` 可變參數的方法
```js
fun printAll(vararg messages: String) {                            // 變數 messages 定義為可變參數 vararg
    for (m in messages) println(m)
}
printAll("Hello", "Hallo", "Salut", "Hola", "你好")                // 定義為 vararg ，可輸入多參數

fun printAllWithPrefix(vararg messages: String, prefix: String) {  // 通常 vararg 要在最後一位參數
    for (m in messages) println(prefix + m)
}
printAllWithPrefix(
    "Hello", "Hallo", "Salut", "Hola", "你好",
    prefix = "Greeting: "                                          // 改使用名稱參數對應
)

fun log(vararg entries: String) {                                  // entries 型態為 Array<String>
    printAll(*entries)                                             // *entries 型態為 vararg of String
}
```

高階方法(委託 `func` )
```js
fun calculate(x: Int, y: Int, operation: (Int, Int) -> Int): Int {  // 聲明一個高階函數 operation 
    return operation(x, y)
}

fun sum(x: Int, y: Int) = x + y                                     // 聲明一個與高階函數參數相等的方法

fun main() {
    val sumResult = calculate(4, 5, ::sum)                          // 呼叫傳入兩個整數值和函數聲明
    val mulResult = calculate(4, 5) { a, b -> a * b }               // 呼叫傳入兩個整數值和 lambda 
    println("sumResult $sumResult, mulResult $mulResult")
}


fun operation(): (Int) -> Int {                                     // 回傳一個高階函數 (Int) -> Int
    return ::square
}

fun square(x: Int) = x * x                                          // 聲明一個與高階函數參數相等的方法

fun main() {
    val func = operation()                                          // 建立 operation() 方法變數
    println(func(2))                                                // 等同 square(2)
}
```

`Lambda` 表達式(->)
```js
val upperCase1: (String) -> String = { str: String -> str.uppercase() } // lambda 表達式 的變數 類型 (String) -> String
val upperCase2: (String) -> String = { str -> str.uppercase() }         // 省略參數類型 String
val upperCase3 = { str: String -> str.uppercase() }                     // 省略變數類型 (String) -> String
//val upperCase4 = { str -> str.uppercase() }                           // 錯誤 無法確定來源型態
val upperCase5: (String) -> String = { it.uppercase() }                 // 使用 it 隱式變量?
val upperCase6: (String) -> String = String::uppercase                  // 使用 函數指針
```

`class.fun()`擴充方法
```js
data class Item(val name: String, val price: Float)                                         // 定義 data class Item

data class Order(val items: Collection<Item>)                                               // 定義 data class Order， Collection 為 listOf 基類

fun Order.maxPricedItemValue(): Float = this.items.maxByOrNull { it.price }?.price ?: 0F    // 定義 Order.maxPricedItemValue(): Float = 單行方法
fun Order.maxPricedItemName() = this.items.maxByOrNull { it.price }?.name ?: "NO_PRODUCTS"  // this 擴充的對象

val Order.commaDelimitedItemNames: String                                                   // 定義 Order.commaDelimitedItemNames 屬性成員
    get() = items.map { it.name }.joinToString()

fun main() {
    val order = Order(listOf(Item("Bread", 25.0F), Item("Wine", 29.0F), Item("Water", 12.0F)))
    
    println("Max priced item name: ${order.maxPricedItemName()}")                           // 4
    println("Max priced item value: ${order.maxPricedItemValue()}")
    println("Items: ${order.commaDelimitedItemNames}")                                      // 5
}
```

`external` 允許使用 JavaScript API ?
```js
external fun alert(msg: String)   // 現有 JavaScript 函數String

fun main() {
  alert("Hi!")
}
```

## 變數

`var` 可變數 `val` 不可變變數的宣告初始化與使用
```js
//var 可變變數， val 不可變變數
var a: String = "initial"  // 宣告一個 String 可變數並初始值 "initial"
println(a)
val b: Int = 1             // 宣告一個 Int 不可變數並初始值 1
val c = 3                  // 宣告一個不可變數並初始值 3 ，自動推測為 Int 型態

var e: Int  // 宣告一個可變數而不進行初始化
println(e)  // 錯誤，e尚未初始化


val d: Int  // 宣告一個不可變數而不進行初始化

if (someCondition()) {//根據某些條件以不同的值初始化
    d = 1
} else {
    d = 2
}
```

判定 `null` 變數與建立
```js
var neverNull: String = "This can't be null"            // 宣告一個非null字串變數。
neverNull = null                                        // 嘗試指派null，會產生編譯錯誤

var nullable: String? = "You can keep a null here"      // 宣告一個非null字串變數。可設定為null
nullable = null                                         // 將值設定null

var inferredNonNull = "The compiler assumes non-null"   // 宣告一個非null字串變數。自動推斷 String 型態
inferredNonNull = null                                  // 嘗試指派null，會產生編譯錯誤

fun strLength(notNull: String): Int {                   // 宣告不可為 null 得參數
    return notNull.length
}
strLength(nullable)                                     // nullable 可能為 null 錯誤，

fun describeString(maybeString: String?): String {              // 宣告可為 null 得參數
    if (maybeString != null && maybeString.length > 0) {        // 判定是否非 null ，同時不能字串長度超過0
        return "String of length ${maybeString.length}"
    } else {
        return "Empty or null string"                           
    }
}
```

`$string` `${string}` 字串模板
```js
val greeting = "Kotliner"

println("Hello $greeting")                  // $greeting 等同外部 greeting
println("Hello ${greeting.uppercase()}")    // ${greeting.uppercase()} 等同執行 greeting.uppercase()
```

`() =`變數解構
```js
val (x, y, z) = arrayOf(5, 10, 15)                              // 解構一個 Array

val map = mapOf("Alice" to 21, "Bob" to 25)
for ((name, age) in map) {                                      // 遍歷 解構 map 為 k-v
    println("$name is $age years old")          
}

val (min, max) = findMinMax(listOf(100, 90, 50, 98, 76, 83))    // findMinMax() 回傳 Array? 等同解構 Array?


data class User(val username: String, val email: String)

fun getUser() = User("Mary", "mary@somewhere.com")

fun main() {
    val user = getUser()
    val (username, email) = user                            // 解構 User("Mary", "mary@somewhere.com") ，依據名稱放置
    println(username == user.component1())                  // data class 本身存在 .component1() 依序對應參數

    val (_, emailAddress) = getUser()                       // _ 捨棄參數，名稱沒有對應時 依序放入
    
}
```

`class` 自訂解構式 ， 等同 `data class` 解構
```js
class Pair<K, V>(val first: K, val second: V) {    // class 不存在 component1() 不能直接解構
    operator fun component1(): K {                 // 定義 component1() 
        return first
    }

    operator fun component2(): V {                 // 定義 component2()  
        return second
    }
}

fun main() {
    val (num, name) = Pair(1, "one")             // 定義了 component1() 可以解構

    println("num = $num, name = $name")
}
```

省略型態自動推測
```js
val date: java.time.chrono.ChronoLocalDate? = java.time.LocalDate.now()    // ChronoLocalDate? 可為 null 宣告 取得現在時間

if (date != null) {
    println(date.isLeapYear)                    // if{date != null} 允許呼叫不可空的方法 isLeapYear
}

if (date != null && date.isLeapYear) {          // 判定前方後 後面允許不可空的方法
    println("It's a leap year!")
}

if (date == null || !date.isLeapYear) {         // 條件內的智慧轉換（也可以透過短路啟用） ?
    println("There's no Feb 29 this year...")
}

if (date is LocalDate) {
    val month = date.monthValue                 // if (date is LocalDate) date自動推測以 LocalDate 類型呼叫方法
    println(month)
}
```


`dynamic` 動態類型，(關閉 Kotlin 的類型檢查器) 呼叫方法後回傳後型態始終保持 `dynamic`
```js
val a: dynamic = "abc"                                               // 任何值都可以指派給dynamic變數類型
val b: String = a                                                    // 動態值可以指派給任何東西

fun firstChar(s: String) = s[0]

println("${firstChar(a)} == ${firstChar(b)}")                        // 動態變數可以作為參數傳遞給任何函數

println("${a.charCodeAt(0, "dummy argument")} == ${b[0].toInt()}")   // 可以對變數呼叫任何帶有任何參數的屬性或函數dynamic

println(a.charAt(1).repeat(3))                                // 對變數的函數呼叫dynamic始終會傳回動態值，因此可以連結呼叫。

fun plus(v: dynamic) = v + 2

println("2 + 2 = ${plus(2)}")                                 // 運算子、賦值和索引存取 ( [..]) 會以「原樣」翻譯。謹防！
println("'2' + 2 = ${plus("2")}")
```

`js()` 字串 程式碼執行
```js
js("alert(\"alert from Kotlin!\")") // 字串內容以程式碼執行

val json = js("{}")               // 建立 Object 型態是 dynamic
json.name = "Jane"                // 定義屬性
json.hobby = "movies"

println(JSON.stringify(json))
```

## 條件判定

`when` 條件選擇
```js
fun cases(obj: Any) {                                
    when (obj) {                                     // when 條件式 (bool 判定) ，會執行單行
        1 -> println("One")                          // obj == 1
        "Hello" -> println("Greeting")               // obj == "Hello"
        is Long -> println("Long")                   // obj is Long
        !is String -> println("Not a string")        // obj !is String
        else -> println("Unknown")                   // 其他
    }   
}

fun whenAssign(obj: Any): Any {
    val result = when (obj) {                   // when 條件式 (bool 判定) ，會將值賦予 result
        1 -> "one"                              // obj == 1
        "Hello" -> 1                            // obj == "Hello"
        is Long -> false                        // obj is Long
        else -> 42                              // 其他
    }
    return result
}
```

`if-else` 條件式
```js
if (a > b) {
  ...             // if(true) 執行此
} else {
  ...             // if(false) 執行此
}
var c = if (a > b) a else b            // 初始值 執行 if-else
fun max(a: Int, b: Int) = if (a > b) a else b         // 方法 執行 if-else

val x = 2
if (x in 1..5) {            // 檢查 1 <= x && x <= 5
    print("x is in range from 1 to 5")
}
if (x !in 6..10) {          // 檢查 !(6 <= x && x <= 10)
    print("x is not in range from 6 to 10")
}
```

`==` `===` 相等檢查
```js
val authors = setOf("Shakespeare", "Hemingway", "Twain")
val writers = setOf("Twain", "Shakespeare", "Hemingway")

println(authors == writers)   // true 等同 if (a == null) b == null else a.equals(b)
println(authors === writers)  // false 檢查 是否相同引用
```

## 迴圈

`For` 迴圈次數
```js
val cakes = listOf("carrot", "cheese", "chocolate")  // 建立 List

for (cake in cakes) {                               // 循環 cakes 所有內容
    println("Yummy, it's a $cake cake!")
}
```

`while` `do` 條件迴圈
```js
fun main(args: Array<String>) {
    var cakesEaten = 0
    var cakesBaked = 0
    
    while (cakesEaten < 5) {                    // while (true) 則會迴圈， while (false) 則不執行
        cakesEaten ++
    }
    
    do {                                        // 會先執行一遍，之後等同 while
        cakesBaked++
    } while (cakesBaked < cakesEaten)
}
```

自訂迭代器(本例子實際沒有完成，僅中輟 List.iterator )
```js
class Animal(val name: String)

class Zoo(val animals: List<Animal>) {
    operator fun iterator(): Iterator<Animal> {             // 重載 iterator() 方法
        return animals.iterator()                           // 回傳 List.iterator()
    }
    // 完整需要完成以下方法
    // next():Animal
    // hasNext():Boolean
}

fun main() {
    val zoo = Zoo(listOf(Animal("zebra"), Animal("lion")))
    for (animal in zoo) {                                   // zoo 歷遍 listOf(Animal("zebra"), Animal("lion"))
        println("Watch out, it's a ${animal.name}")
    }
}
```
## 方法區塊

`let` 執行給定的程式碼區塊並傳回其最後一個表達式的結果
```js
val empty = "test".let {               // .let{} 將內容傳入，作為區塊回傳
    customPrint(it)                    // it 為 "test"
    it.isEmpty()                       // it 是否為空 ， 最後因此結果回傳
}
println(" is empty: $empty")


fun printNonNull(str: String?) {
    println("Printing \"$str\":")

    str?.let {                         // let 程式碼區塊僅在非 null 執行。
        print("\t")
        customPrint(it)
        println()
    }
}

fun printIfBothNonNull(strOne: String?, strTwo: String?) {
    strOne?.let { firstString ->       // 便巢狀let，必須使用明確定義餐數名稱
        strTwo?.let { secondString ->
            customPrint("$firstString : $secondString")
            println()
        }
    }
}

printNonNull(null)
printNonNull("my string") 
printIfBothNonNull("First","Second") 
```

`run` 等同 `let` 但內部是透過 存取自身類別 this ，而非 it 參數
```js
fun getNullableLength(ns: String?) {
    println("for \"$ns\":")
    ns?.run {
        println("\tis empty? " + isEmpty())                    // 直接呼叫到 ns: String 方法
        println("\tlength = $length")                           
        length                                                 // 直接呼叫到 ns: String 屬性
    }
}
getNullableLength(null)
getNullableLength("")
getNullableLength("some string with Kotlin")
```

`with` 執行給定的程式碼區塊，內部是透過 存取自身類別 this ，但需要將對象作為參數導入
```js
class Configuration(var host: String, var port: Int) 

fun main() {
    val configuration = Configuration(host = "127.0.0.1", port = 9000) 

    with(configuration) {                                      // with(對象)
        println("$host:$port")                                 // 直接呼叫 Configuration 得屬性
    }
}
```

`apply` 等同 `with` 用來初始化對象? ，結果回傳對象
```js
data class Person(var name: String, var age: Int, var about: String) {
    constructor() : this("", 0, "")
}

val jake = Person()
val stringDescription = jake.apply {                    // 執行 apply{} 回傳物件本身?
    name = "Jake"                                       // 直接呼叫 Configuration 得屬性
    age = 30
    about = "Android developer"
}.toString()                                            // 傳回值是實例本身，因此您可以連結其他操作
```

`also` 等同 `apply` 但用本身會以 it 參數傳入
```js
data class Person(var name: String, var age: Int, var about: String) {
             constructor() : this("", 0, "")
}
         
fun writeCreationLog(p: Person) {
    println("A new person ${p.name} was created.")              
}
val jake = Person("Jake", 30, "Android developer")
    .also {                                          // 傳回值是物件本身。
        writeCreationLog(it)                         // 自身 物件作為參數傳遞
    }
```