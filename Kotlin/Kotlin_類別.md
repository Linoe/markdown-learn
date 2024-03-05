# Kotlin_類別

`kotlin` 建立類別使用方式

## class

一般類別

```js
//宣告
class MyClass(){} //可省略 (){}

//建構參數，參數會直接轉類別變數
class Rect(var height: Double, var width: Double) {
  //類別變數
  var area = height * height
  
  //類別方法
  fun Size(var length: Double): Double = area * length
}

//實例化
val rect = Rect(10.0, 5.0)

//成員呼叫
rect.Size(12.0)
rect.area

//繼承類別 open ，可以實例化
open class MyClass(){
  //繼承方法，可以使用
  open fun MyFun(){}
}
class MySub(): MyClass(){
  override fun MyFun(){}
}



class Customer                                  // 建立一個 不帶任何屬性與方法的類別，不能幹嘛

class Contact(val id: Int, var email: String)   // 建立一個 帶有 id email 屬性，且兩個參數的建構式

fun main() {
    val customer = Customer()                   // 宣告 Customer 變數
    val contact = Contact(1, "mary@gmail.com")  // 宣告 Contact 變數，並建構式給予初始值
    println(contact.id)                         // 呼叫 Contact.id 的屬性
    contact.email = "jane@gmail.com"            // 重新 contact.email 附值
}
```

## data class

資料形式類別，自帶以下方法
- `equals()` 比較方法
- `hashCode()` 哈希碼，比較用的數值
- `toString()` 轉換字串
- `copy()` 複製，生成一個相同參數的物件
- `component1()` `component2()` 參數，對應建構參數順序
- `getter` `setter` 建構參數 自動生成

```js
//宣告
data class MyDataClass(){}

//建構參數，參數會直接轉類別變數
data class Dog(val name: String, val age: Int){}


data class User(val name: String, val id: Int) {           // 定義 data 資料類別
    override fun equals(other: Any?) =
        other is User && other.id == this.id               // 重載 equals() 如果用戶具有相同的id.
}
fun main() {
    val user = User("Alex", 1)
    println(user)                                          // User(name=Alex, id=1)

    val secondUser = User("Alex", 1)
    val thirdUser = User("Max", 2)

    println("user == secondUser: ${user == secondUser}")   // true ， 有重載 equals 只會比較 id
    println("user == thirdUser: ${user == thirdUser}")

    // hashCode() function
    println(user.hashCode())                               // 具有完全匹配屬性的資料類實例具有相同的hashCode.
    println(secondUser.hashCode())
    println(thirdUser.hashCode())

    // copy() function
    println(user.copy())                                   // copy() 產生相同內容
    println(user === user.copy())                          // false ， copy() 與原先的來源不同
    println(user.copy("Max"))                              // copy() 參數等同建構值
    println(user.copy(id = 3))                             // 使用命名參數

    println("name = ${user.component1()}")                 // 自動產生的 componentN 等同建構值屬性順序
    println("id = ${user.component2()}")
}
```

## abstract class

抽象類別，不能實例化，只能用於繼承

```js
//宣告
abstract class MyAbstract(){
  //抽象方法，不修飾不能重載
  abstract fun Say()
}

//繼承類別
class MyClass(): MyAbstract(){
  //繼承方法
  override fun Say()
}

//object 繼承
val myObject = object : MyAbstract() {
  override fun Say()
}
```


`sealed` 密封類別，本身等同 `abstract` 僅能使用在同一檔案程式碼?
```js
sealed class Mammal(val name: String)                                                   // 定義一個密封類別

class Cat(val catName: String) : Mammal(catName)                                        // 繼承密封類別，必須同一 package
class Human(val humanName: String, val job: String) : Mammal(humanName)

fun greetMammal(mammal: Mammal): String {
    when (mammal) {                                                                     // 建立依據類型條件
        is Human -> return "Hello ${mammal.name}; You're working as a ${mammal.job}"    // mammal is Human
        is Cat -> return "Hello ${mammal.name}"                                         // mammal is Cat
    }
}
println(greetMammal(Cat("Snowy")))
```

## enum class
```js
enum class MyEnum {
    A, B, C
}

enum class State {
    IDLE, RUNNING, FINISHED                           // 定義常數
}

fun main() {
    val state = State.RUNNING                         // 枚舉建立，直接赴值
    val message = when (state) {                      // when 搭配 enum
        State.IDLE -> "It's idle"
        State.RUNNING -> "It's running"
        State.FINISHED -> "It's finished"
    }
    println(message)
}

enum class Color(val rgb: Int) {                      // 定義 枚舉的屬性
    RED(0xFF0000),                                    // 依據建構值輸入
    GREEN(0x00FF00),
    BLUE(0x0000FF),
    YELLOW(0xFFFF00);                                 // 枚舉類別成員與常數定義之間以分號分隔。

    fun containsRed() = (this.rgb and 0xFF0000 != 0)  // 建立 枚舉方法
}

fun main() {
    val red = Color.RED
    println(red)                                      // "RED" 回傳 Color.RED.toString()
    println(red.containsRed())                        // 呼叫枚舉常數方法。
    println(Color.BLUE.containsRed())                 // 直接呼叫枚舉常數方法。
}
```

## sealed class

等同於 `enum` 但內部各自成員
```js
// sealed class MyMarsUiState
sealed interface MyMarsUiState {
    // data class
    data class Success(val photos: String) :MyMarsUiState
    // object
    object Error : MyMarsUiState
    object Loading : MyMarsUiState
}

// 搭配 when 直接轉型使用
val marsUiState: MyMarsUiState = MyMarsUiState.Success("Success")
nowState(marsUiState)

fun nowState(marsUiState: MyMarsUiState) {
  when (marsUiState) {
    is MyMarsUiState.Loading -> LoadingScreen()
    is MyMarsUiState.Success -> ResultScreen(marsUiState.photos) // 轉型後可以讀取到內部數值
    is MyMarsUiState.Error -> ErrorScreen()
  }
}


```
性能實際上是 `abstract` 透過語法包裝達到 `enum` 效果

## annotation class

`annotation` 建立針對 類別 方法 等... 的附加屬性
```js
// 建立
annotation class Fancy
// 使用
@Fancy class Foo {
    @Fancy fun baz(@Fancy foo: Int): Int {
        return (@Fancy 1)
    }
}
// 建構式
class Foo @Inject constructor(dependency: MyDependency) { ... }
// 成員
class Foo {
    var x: MyDependency? = null
        @Inject set
}
```

`annotation` 參數代表 建構子
```js
// 建立
annotation class Special(val why: String)
// 使用
@Special("example") class Foo {}
```

`annotation` 串接 按照一般 class 原理 做類別宣告
```js
annotation class ReplaceWith(val expression: String)

annotation class Deprecated(
        val message: String,
        val replaceWith: ReplaceWith = ReplaceWith(""))
// 內部可以直接寫入
@Deprecated("This function is deprecated, use === instead", ReplaceWith("this === other"))
```

`annotation` lambda 建立委派
```js
annotation class Suspendable

val f = @Suspendable { Fiber.sleep(10) }
```

`annotation` 內建註釋
```js
@file
@property//（具有此目標的註解對 Java 不可見）
@field
@get//（屬性取得者）
@set//（屬性設定者）
@receiver//（擴展函數或屬性的接收者參數）
@param//（建構函數參數）
@setparam//（屬性設定器參數）
@delegate//（儲存委託屬性的委託實例的欄位）
```

## object

object 與其他語言設計不同，主要用來建立
- 臨時 class 物件
- 伴隨物件

```js

//object 建立
fun rentPrice(standardDays: Int, festivityDays: Int, specialDays: Int): Unit { 
    val dayRates = object {                                                     //建立自訂的 object 屬性
        var standard: Int = 30 * standardDays
        var festivity: Int = 50 * festivityDays
        var special: Int = 100 * specialDays
    }
    val total = dayRates.standard + dayRates.festivity + dayRates.special       //訪問對象的屬性。
}

//object 成員
object DoAuth {                                                 //建立外部物件
    fun takeParams(username: String, password: String) {        //定義物件方法
        println("input Auth parameters = $username:$password")
    }
}
fun main(){
    DoAuth.takeParams("foo", "qwerty")                          //呼叫該物件方法，等同呼叫 自己的 class 成員
}
//object 伴隨(靜態)
class BigBen {                                  //定義一個類別
    companion object Bonger {                   //定義一個 companion object ， Bonger 名稱可以省略
        fun getBongs(nTimes: Int) {             //定義 companion object 方法
            for (i in 1 .. nTimes) {
                print("BONG ")
            }
        }
    }
}

fun main() {
    BigBen.getBongs(12)                         //透過類別名稱呼叫伴隨物件方法， Bonger 省略
}
```


## Companion Object

 `object` 成員都會是靜態
`object` 特定指定方法
```js
fun suma(a: Int, b: Int): Int {
    return a + b
}

fun multiplica(a: Int, b: Int): Int {
    return a * b
}

fun main() {
    
    val operations = object {
        val _suma = ::suma
        val _multiplica = ::multiplica
    }

    println("sua suma:: ${operations._suma(2, 3)}")
    println("sua multiplica:: ${operations._multiplica(2, 3)}")
}
```

`Companion Object` 類別的靜態物件
```js
fun main() {
    Robot.Color.color= "Blue"
    Robot.Color.color= "Yellow"
}
class  Robot{
    companion object Color{
        var color= "White"
    }
}
```

`companion object` 建立物件
```js
class MyClass {
    companion object Factory {
        fun createInstance(): MyClass = MyClass()
    }
}
val instance = MyClass.Factory.createInstance()
```

`companion object` 可以繼承
```js
interface Theme {
    fun someFunction(): String
}

abstract class FactoryCreator {
    abstract fun produce(): Theme
}


class FirstRelatedClass : Theme {
    companion object Factory : FactoryCreator() {
        override fun produce() = FirstRelatedClass()
    }
    override fun someFunction(): String {
        return "I am from the first factory."
    }
}

class SecondRelatedClass : Theme {
    companion object Factory : FactoryCreator() {
        override fun produce() = SecondRelatedClass()
    }
    override fun someFunction(): String {
        return "I am from the second factory."
    }
}

fun main() {
    val factoryOne: FactoryCreator = FirstRelatedClass.Factory
    println(factoryOne.produce().someFunction())

    val factoryTwo: FactoryCreator = SecondRelatedClass.Factory
    println(factoryTwo.produce().someFunction())
}
```

`companion object` 可以保存在任何類別上
```js
interface MyInterface {
    companion object {
        const val PROPERTY = "value"
    }
}
```

`companion object` 搭配 `abstract` 抽象工廠建立
```js
interface Weapon {

    fun use():String
}
abstract class WeaponFactory {

    abstract fun buildWeapon(): Weapon
}

// 繼承工廠
class Crossbow : Weapon {

    companion object Factory : WeaponFactory() {
        override fun buildWeapon() = Crossbow()
    }

    override fun use(): String {
        return "Using crossbow weapon"
    }
}

class Katana : Weapon {

    companion object Factory : WeaponFactory() {
        override fun buildWeapon() = Katana()
    }

    override fun use(): String {
        return "Using katana weapon"
    }
}

// 範例
val factory : WeaponFactory = Crossbow.Factory
val crossbow = factory.buildWeapon()

assertNotNull(crossbow)
assertEquals("Using crossbow weapon", crossbow.use())


```


## `open` 類別繼承， `override` 重載方法

```js

open class Dog {                // 宣告 Dog 類別，允許繼承
    open fun sayHello() {       // 宣告 sayHello() 方法，允許重載
        println("wow wow!")
    }
}
class Yorkshire : Dog() {       // 繼承 Dog 類別
    override fun sayHello() {   // 重載 sayHello() 方法
        println("wif wif!")
    }
}
fun main() {
    val dog: Dog = Yorkshire()  // 建立 Dog 承載 Yorkshire
    dog.sayHello()
}


open class Tiger(val origin: String) {
    fun sayHello() {
        println("A tiger from $origin says: grrhhh!")
    }
}
class SiberianTiger : Tiger("Siberia")                  // 繼承 Dog 類別，建構式給初始值
fun main() {
    val tiger: Tiger = SiberianTiger()
    tiger.sayHello()
}


open class Lion(val name: String, val origin: String) {
    fun sayHello() {
        println("$name, the lion from $origin says: graoh!")
    }
}

class Asiatic(name: String) : Lion(name = name, origin = "India") // 繼承 Lion 類別，Asiatic建構式(name) 會轉給 Lion(name)

fun main() {
    val lion: Lion = Asiatic("Rufo")                              // 因為 Lion.origin 給初始值，只需要給 Asiatic.name
    lion.sayHello()
}
```

## <> 泛型
```js
class MutableStack<E>(vararg items: E) {              // 宣告 MutableStack<E> 類別 ， E 外部輸入的類型
  private val elements = items.toMutableList()
  fun push(element: E) = elements.add(element)        // 方法 變數類型 為 E
  fun peek(): E = elements.last()                     // 方法 回傳值 為 E
  fun pop(): E = elements.removeAt(elements.size - 1)
  fun isEmpty() = elements.isEmpty()
  fun size() = elements.size
  override fun toString() = "MutableStack(${elements.joinToString()})"
}

fun <E> mutableStackOf(vararg elements: E) = MutableStack(*elements)  //可以用 泛型方法 回傳 建立
                                                               // 不能使用 elements ， *elements 才會指向 vararg elements: E
fun main() {
  val stack = mutableStackOf(0.62, 3.14, 2.7)  // 等同 MutableStack(0.62, 3.14, 2.7)
  println(stack)
}
```

## 成員

成員可以透過建構式或內部建立
```js
class Rect(var height: Double, var width: Double){
  val area: Int = height * width
}
// 自動建立以下
Rect.height
Rect.width
Rect.area
```

成員個別建立 `get` `set` 內部自動產生以下
- `field` 成員自身
- `value` 外部給予值 (只在 `set` )
```js
var speakerVolume = 2
    get() = field  
    set(value) { field = value}
```

## 常用
```js
//建構式，通常用來建立多載
constructor(name: String, age: Int) : this(name) {
}

//指定父類別方法，如果方法重複時，指定用
super.turnOn()

//方法索引子
val myVal = ::MyFun
//myVal() 錯誤 不能直接呼叫
val myVal = MyFun
myVal()       // 等同 MyFun() 相當於委託

//委託類別
class MyDelegation(){}
class MyClass(){
  var func by MyDelegation()
}

```
## 委派

`by` 委派創造目標是為了減少直接繼承的耦合 盡可能的組合關聯
`by` 定義為 該成員的屬性由的委派對象的 `getValue` `setValue` 決定
`by` 相當於 kotlin 委派 但使用方法上不一樣 主要有以下用法
1. 繼承委派
  ```js
    class Person(cloth: Cloth) : Cloth by cloth
  ```
2. 變數委派
  ```js
  var p: Type by Delegate()
  ```
3. 集合委派
  ```js
  class Rect(map: Map<String, Any?>) {
      val width: Int by map
      val height: Int by map
  }
  ```
4. 自訂委派
  ```js
  class ValueVar<T>(){
      private var value: T? = null
      override fun getValue(thisRef: Any?, property: KProperty<*>): T {
          return value ?: throw IllegalStateException()
      }
      override fun setValue(thisRef: Any?, property: KProperty<*>, value: T) {
          this.value = if (this.value == null) value
            else throw IllegalStateException("")
      }
  }
  ```
5. 對象委派
  ```js
  object NamedObject {
      operator fun getValue(thisRef: Any?, property: KProperty<*>): String = ...
  }
  ```

`by` 繼承委派
1. 透過子成員確立實作的對象
  ```js
  // 介面
  interface Cloth {
      fun wear()
  }
  // 實作
  class Shirt : Cloth {
      override fun wear() = println("wear shirt")
  }
  class Dress : Cloth {
      override fun wear() = println("wear dress")
  }
  // 透過委派實例化 (與繼承相差無異)
  class ShirtPerson : Cloth by Shirt
  class DressPerson : Cloth by Dress
  // 範例
  ShirtPerson().wear() // wear shirt
  DressPerson().wear() // wear dress

  // 藉由成員判別實例化對象
  class Person(cloth: Cloth) : Cloth by cloth

  // 範例
  Person(Shirt()).wear() // wear shirt
  Person(Dress()).wear() // wear dress
  ```
2. 允許多重繼承委派
  ```js
  // 介面
  interface Tops {
      fun wearTop()
  }
  // 實作
  class Shirt : Tops {
      override fun wearTop() = println("wear shirt")
  }
  class Dress : Tops {
      override fun wearTop() = println("wear dress")
  }
  // 委派
  class Person(top:Tops, cloth: Cloth) : Tops by top, Cloth by cloth
  ```

`by` 繼承委派 編譯時重新撰寫委派的繼承項 並將內部的方法重新轉向委派目標
```js
// 介面
interface Cloth {
    fun wear()
}
// 實作
class Shirt : Cloth {
    override fun wear() = println("wear shirt")
}
// 藉由成員判別實例化對象
class Person(cloth: Cloth) : Cloth by cloth

//編譯過後 (java)
public class Person implements Cloth {
  private Cloth cloth
  public void wear(){
    this.cloth.wear()
  }
}
```

`by` 成員委派 自動擷取委派的 `setValue` `getValue` 對應成員的 `get` `set`
`thisRef` 類別對象
`prop` 類別成員
```js
class Foo {
    var p: Type by Delegate()
}
class Delegate{
    var text: String = ""
    override fun setValue(thisRef: Any, prop: KProperty<*>, value: String) {
      text = value
    }
    override fun getValue(thisRef: Any, prop: KProperty<*>): String {
      return text
    }
}
// 等同
class Foo {
    private val p$delegate = Delegate()
    var prop: Type
    	get() = delegate.getValue(this, this::prop)
    	set(value: Type) = delegate.setValue(this, this::prop, value)
}
```

`by` 成員委派 可以委派給自己
```js
class MyClass {
   var newName: Int = 0
   var oldName: Int by this::newName
}
```

`by` 成員委派 提供兩個類別繼承使用
- `ReadWriteProperty` 對應 `val` 可以 `get` `set`
- `ReadOnlyProperty` 對應 `var` 只能 `get`
```js
class Foo {
    val argVal: String by DelegateVal()
    var argVar: String by DelegateVar()
}
class DelegateVal : ReadOnlyProperty<Foo, String> {
    override fun getValue(thisRef: Foo, property: KProperty<*>): String {
        return "ReadOnlyProperty getValue()"
    }
}
class DelegateVar : ReadWriteProperty<Foo, String> {
    override fun getValue(thisRef: Foo, property: KProperty<*>): String {
        return "ReadWriteProperty getValue()"
    }
    override fun setValue(thisRef: Foo, property: KProperty<*>, value: String) {
        println("ReadWriteProperty setValue()")
    }

}
// 範例
Example().argVal
Example().argVar = "hello"
Example().argVar

// 透過方法 搭配 object 建立繼承 ReadWriteProperty 物件
fun resourceDelegate(resource: Resource = Resource()): ReadWriteProperty<Any?, Resource> =
    object : ReadWriteProperty<Any?, Resource> {
        var curValue = resource
        override fun getValue(thisRef: Any?, property: KProperty<*>): Resource = curValue
        override fun setValue(thisRef: Any?, property: KProperty<*>, value: Resource) {
            curValue = value
        }
    }

val readOnlyResource: Resource by resourceDelegate()  // ReadWriteProperty as val
var readWriteResource: Resource by resourceDelegate()
```

`by` 成員委派 可以透過擴充方式臨時增加
```js
class Foo {
    var p: String by Delegate()                                               // p 等同 Delegate() 變數
}
class Delegate() {
  var text = ""
  fun get(): String = text
  fun set(value: String) {text = value}
}
// 擴充成員委派方法
operator fun Delegate.setValue(thisRef: Any, property: KProperty<*>, value: String) = set(value)
operator fun Delegate.getValue(thisRef: Any, property: KProperty<*>) = get()
```


`by lazy` 成員延遲初始化 `lazy` Kotlin 內建提供成員呼叫時才會建立類別
```js
class LazySample {
    val lazyStr: String by lazy {"my lazy"} // 最後參數會被當作成員值
}
```
`by lazy` 常用來作為泛用類的擴充 利用未被使用不會建立特性 達到效能節省
```js
// 對 基類 進行擴充 因為 lazy 關係 只要不被呼叫就不會被建立
fun <R: Any> R.logger(): Lazy<Logger> = lazy {
    Logger.getLogger(unwrapCompainionClass(this.javaClass).name)
}

class Something {
    val LOG by logger()
}
```


 `by Delegates.observable` 成員觀察 每次存取時都會呼叫一次
 ```js
 class User {
    var name: String by Delegates.observable("name") { // 初始值
        prop, old, new ->  // 分配給的屬性 舊值 新值
        println("$old -> $new")
    }
}
 ```

 `by Delegates.vetoable` 成員攔截 每次存取判定是否允許修改
 ```js
 
 class User {
    var max: Int by Delegates.vetoable(0) { // 初始值
      property, oldValue, newValue ->  // 分配給的屬性 舊值 新值
    newValue > oldValue  // true 允許修改 false 忽視
}
}
 ```

`by Delegates.notNull` 成員不能 null 成員在存取時 null 會發生錯誤
 ```js
 class App : Application() {
    companion object {
        var instance: App by Delegates.notNull()
    }

    override fun onCreate() {
        super.onCreate()
        instance = this
    }
}
 ```

 
`by Map` 委派集合 可以透過集合鍵值取的對應的成員名稱的鍵值
```js
class User(val map: Map<String, Any?>) {
    val name: String by map // 來自於 map[name]
    val age: Int by map  // 來自於 map[age]
}
// 範例
val user = User(mapOf(
  "name" to "John Doe",
  "age"  to 25
))
user.name // John Doe
user.age // 25

// 可變成員
class MutableUser(val map: MutableMap<String, Any?>) {
    var name: String by map
    var age: Int     by map
}

```

`by` 可用範圍
```js
// 成員(var fun)
class C<Type> {
    private var impl: Type = ...
    var prop: Type by ::impl
}

// 對象(object class)
object NamedObject {
    operator fun getValue(thisRef: Any?, property: KProperty<*>): String = ...
}
val s: String by NamedObject

// 具有 getter 模組
val impl: ReadOnlyProperty<Any?, String> = ...

class A {
    val s: String by impl
}

// 常數(this null...)
class A {
    operator fun getValue(thisRef: Any?, property: KProperty<*>) ...
    val s by this
}
```

`PropertyDelegateProvider` 提供當 `by` 作為委派時的類型 如果需要依據目標類型判斷時使用
這也是實現繼承委派的實作
```js
class DatabaseDelegateProvider<in R, T>(readQuery: String, writeQuery: String, id: Any) 
    : PropertyDelegateProvider<R, ReadWriteDelegate<R, T>> {
    override operator fun provideDelegate(thisRef: T, prop: KProperty<*>): ReadWriteDelegate<R, T> {
        if (prop.returnType.isMarkedNullable) {
            return NullableDatabaseDelegate(readQuery, writeQuery, id)
        } else {
            return NonNullDatabaseDelegate(readQuery, writeQuery, id)
        }
    }
}
```

`by` 也可以用作成員更新兼容
```js
class Example {
    var newName: String = ""
    
    @Deprecated("Use 'newName' instead", ReplaceWith("newName"))
    var oldName: String by this::newName
}
```

`by` `=` 變數屬性上 效率上差異不大 差別在存取時使用方法
- `by` 操作對象由委派對象的 `getValue` `setValue` 決定
- `=` 系統的 `get` `set`
基於以上歸類出以下規則
- `=` 需要對委派對象類別進行操作
- `by` 只需要對委派對象提供的數值進行操作
```js
var a = IntDelegate(0)
var b by IntDelegate(0)

a += 0 // 直接對 IntDelegate 類別進行操作
b += 0 // by 進行包裝 實際上是 b.setValue(b.getValue + 1)
```

## 可見範圍

可見關係，不同 class 之間
| 修飾符    | 同級可訪問  |  在子類別中可訪問  | 可在同一模組中存取  |  可存取的外部模組 |
| :-------- | :--------: | :--------------: | :----------------: | :--------------: |
| private   |     ✔      |        𝗫         |         𝗫          |        𝗫         |
| protected |     ✔      |        ✔         |         𝗫          |        𝗫         |
| internal  |     ✔      |        ✔         |         ✔          |        𝗫         |
| public    |     ✔      |        ✔         |         ✔          |        ✔         |


## actual/expect 實際/預期

expect/actual 是 Kotlin Multiplatform 中，用于声明和实现跨平台代码
expect 用于声明跨平台接口，即定义一组期望实现的公共 API
actual 则用于实现跨平台接口，即提供针对具体平台的实现代码。

跨平台接口 MyLogger
```js
//共享模块（Kotlin 代码的共同部分）中定义 MyLogger 的 expect 部分：
expect class MyLogger(tag: String) {
    fun d(message: String)
    fun e(message: String, throwable: Throwable? = null)
}

//  Android 或 iOS）中，我们可以实现 MyLogger 的 actual
actual class MyLogger actual constructor(tag: String) {
    private val logger = platformLogger(tag)
    
    actual fun d(message: String) {
        logger.debug(message)
    }
    
    actual fun e(message: String, throwable: Throwable?) {
        logger.error(message, throwable)
    }
}
```


expect 和 actual ， expect 看起來像是 interface 而 actual 則看起來像是 expect 的實作。

```js
//in commonMain
expect class Platform() {
  val platform: String
}

//in iOSmain
actual class Platorm actual constructor(){
  actual val platform: String = 
    UIDevice.currenDevice.systemName() + " " + UIDevice.currenDevice.systemVersion
}

//in androidMain
actual class Platform actual constructor(){
  actual val platform: String = "Andorid ${android.os.Build.VERSION.SDK_INT}"
}
```

## MyClass.func 擴充方法

Kotlin 直接在外部替 Class 擴充方法
```js
class MyClass
// 擴充方法
fun MyClass.say()
// 擴充變數
val MyClass.factor: Float

// 使用
val mc = MyClass()
mc.say()
mc.factor
```

主要作為如果要在其他檔案擴充互動方法時使用
```js
// test1.kt 檔案
package com.example.test1
class MyClass

// test2.kt 檔案
package com.example.test2
val text = "hello"
fun MyClass.say(): String = text

// 使用 需要同時 import 否則不能使用
import com.example.test1
import com.example.test2
val mc = MyClass()
mc.say()
```

## init{} 初始化

Class 中 `init` 處理當第一次被初始化時呼叫
```js
class MyClass{
  init{}
}
```

與建構式不同 分開執行
```js
class MyClass{
  init{} // 優先執行
  constructor(){} // 後執行
}
```

可以在 `init` 中使用以下來確定變數內容 基本效果一樣 僅差異在拋出的錯誤訊息
`check` - 檢查狀態 false，抛出 IllegalStateException
`require` - 檢查數值 false时，抛出 IllegalArgumentException
`assert` - 檢查結果 false时，抛出 AssertionError
```js
class MyClass(
  val a:Int
){
  init{
    check(a > 100) {"錯誤"}
    require(a > 100){"錯誤"}
    assert(a > 100) {"錯誤"}
  }
}
```