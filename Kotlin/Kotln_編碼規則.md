# Kotln_編碼規則

官方推薦編碼規則
<https://kotlinlang.org/docs/coding-conventions.html>

## 命名規則

```js
//套件的名稱始終為小寫且不使用底線 ( org.example.project)。
//通常不鼓勵使用多單字名稱，但如果確實需要使用多個單詞，
//則可以將它們連接在一起或使用駝峰式大小寫 ( org.example.myProject)。
open class DeclarationProcessor { /*...*/ }

//類別和物件的名稱以大寫字母開頭並使用駝峰式大小寫：
object EmptyDeclarationProcessor : DeclarationProcessor() { /*...*/ }

//函數、屬性和局部變數的名稱以小寫字母開頭，使用駝峰式大小寫且不帶底線：
fun processDeclarations() { /*...*/ }
var declarationCount = 1

//測試方法的名稱
//在測試中（並且僅在測試中），您可以使用帶有反引號括起來的空格的方法名稱。
//請注意，Android 運行時目前不支援此類方法名稱。測試程式碼中也允許方法名稱中使用底線。
class MyTestCase {
     @Test fun `ensure everything works`() { /*...*/ }

     @Test fun ensureEverythingWorks_onAndroid() { /*...*/ }
}

//屬性名稱
//常數的名稱（以 、 標記的屬性，或沒有保存深度不可變資料的自訂函數的const頂級屬性或物件屬性）
//應使用大寫下劃線分隔（尖叫蛇大小寫）名稱：valget
const val MAX_COUNT = 8
val USER_NAME_FIELD = "UserName"

//保存具有行為或可變資料的物件的頂級或物件屬性的名稱應使用駝峰命名法：
val mutableCollection: MutableSet<String> = HashSet()

//儲存對單例物件的參考的屬性名稱可以使用與object聲明相同的命名樣式：
val PersonComparator: Comparator<Person> = /*...*/
//對於枚舉常數，可以使用大寫下劃線分隔的名稱（尖叫蛇大小寫） 或大寫駝峰大小寫名稱，具體取決於用法。
enum class Color { RED, GREEN }

//支援屬性的名稱
//如果一個類別有兩個概念上相同的屬性，但一個是公共 API 的一部分，另一個是實作細節，請使用底線作為私有屬性名稱的前綴：

class C {
    private val _elementList = mutableListOf<Element>()

    val elementList: List<Element>
         get() = _elementList
}
```
