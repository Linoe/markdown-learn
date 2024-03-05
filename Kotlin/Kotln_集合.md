# Kotln_集合

kotln 變數集合型態

## Array 表達式

使用數字表達式集合，型態等同 Array
```js
//建立
1..6  //[1,2,3,4,5,6]
0 until 3  //[0,1,2] 不包含3
1..<5  //[1,2,3,4] 不包含5
1..10 step 2  //[1,3,5,7,9] 間隔2 
9 downTo 0 step 3  //[9,6,3,0] 倒序 間隔3
'a'..'d'    // ['a','b','c','d'] 使用母集合，型態 char
//方法
(1..6).random()  // 隨機 1~6
```

## Array

同一型態的集合，不能增減長度
<https://kotlinlang.org/docs/arrays.html#primitive-type-arrays>
```js
//建立
val simpleArray = arrayOf(1, 2, 3)
val exampleArray = IntArray(5)       //只建立索引，不指向內容
```

## List

同一型態的集合，可以增減長度
- `MutableList` 可變
- `List` 不可變
```js
//建立 list ，可追加集合
val mlist = mutableListOf(1, 2, 3)        // MutableList<Int>
val list = listOf("a", "b", "c")          // List<String>
val its = List(5) {                       // ArrayList
    'a' + it  // it 等於 0..5 索引
}// [a, b, c, d, e] char 字元位移
//加入
list.add("d")
//過濾
val positives = list.filter { x -> x > 0 }
val positives = list.filter { it > 0 }
//判定包含
if ("john@example.com" in emailsList) { ... }
//轉換
val softBakedMenu = cookies.filter {it.softBaked} //轉換成 map
val totalPrice = cookies.fold(0.0) {total, cookie -> total + cookie.price} //歷遍計算總和
```

## Map

鍵值組合的集合
- `MutableMap` 可變
- `Map` 不可變
```js
//建立 map ，K-V集合
val mmap = mutableMapOf(1 to 100, 2 to 100, 3 to 100) //MutableMap<Int, Int>
val map = mapOf("a" to 1, "b" to 2, "c" to 3) //Map<String, Int>
//呼叫
val value = map["key"]  //取值
map.getValue("a")    //方法取值
map["key"] = value   //賦值
//判定
val mainEmail = emails.firstOrNull() ?: ""
ZPassAccounts.containsKey(accountId)       //如果存在 回傳 True
//歷遍
for ((k, v) in map) {...}   //歷遍所有的建值
//方法
// withDefault 建立當找不到時回傳 預設值 it.length 鍵
val mapWithDefault = map.withDefault { k -> k.length }
// 不存在鍵 key2 回傳 預設值  "key2".length
val value4 = mapWithDefault.getValue("key2")
```


## Set

無序不重複的集合，不能使用序列索引
- `MutableSet` 可變
- `Set` 不可變
```js
//建立 Set ，無序集合
val solarSystem = mutableSetOf("Mercury", "Venus", "Earth", "Mars")  //MutableSet<String>
//加入
solarSystem.add("now")  //尚未重複且加入成功，回傳 true
//判定
solarSystem.contains("Pluto")  //如果存在，回傳 true
```

## 方法

轉換
- `filter` 篩選，判定成功元素重新輸出為新集合
- `map` map轉換，當前元素作為 key ，輸出內容作為值

判定
- `any` bool轉換，依序判定，判定輸出為新集合
- `all` 歷遍判定，所有判定成功，回傳 ture
- `none` 歷遍判定，所有判定不成功，回傳 ture ， all() 的相反

搜尋
- `find` 開頭搜尋，判定成功回傳第一個元素
- `findLast` 尾端搜尋，判定成功回傳倒數的元素
- `first` `firstOrNull` 符合條件 第一個元素
- `last` `lastOrNull` 符合條件 倒數的元素
- `maxOrNull` 最大 數字集合搜尋
- `minOrNull` 最小 數字集合搜尋

搜尋集合
- `associateBy` 搜尋元素為鍵，符合最後一個元素作為值，輸出 `Map` 
- `groupBy` 搜尋元素為鍵，符合所有元素作為集合，輸出 `Map` 
- `partition` 判定分類，判定結果的元素分成兩個集合

歷遍
- `flatMap` 元素歷遍，元素是集合則會內容也歷遍而非視為集合物件

排序
- `sorted` 小至大 排序
- `sortedBy` 小至大 自訂排序 輸出元素可處理
- `sortedDescending` 大至小 排序
-  `sortedByDescending` 大至小 自訂排序 輸出元素可處理

其他
- `zip` 集合合併，依序取元素合併成 list ，以最小的長度為標準
- `getOrElse` ifelse 集合訪問，不存在則執行 {}

```js
val numbers = listOf(1, -2, 3, -4, 5, -6)
// filter 篩選 判定成功元素重新輸出為新集合
val negatives = numbers.filter{it < 0}
// `map` 轉換 當前元素作為 key ，輸出內容作為值
val tripled = numbers.map { it * 3 }

//`any`任意 歷遍內容 it < 0 回傳 bool 集合
val anyNegative = numbers.any { it < 0 }
//`all`所有 歷遍內容  it % 2 == 0 所有內容符合 回傳 true
val allEven = numbers.all { it % 2 == 0 }
//`none`不符 歷遍內容  it > 6 所有內容不符合 回傳 true ， all 反向
val allLess6 = numbers.none { it > 6 }
```

```js
val words = listOf("Lets", "find", "something", "in", "collection", "somehow")
//`find`開頭  回傳開頭 "some" 第一個字串
val first = words.find { it.startsWith("some") }
//`findLast`尾端  回傳包含 "nothing" 第一個字串
val last = words.findLast { it.contains("some") }

val numbers = listOf(1, -2, 3, -4, 5, -6) 
// `first` `firstOrNull`集合首個
val first = numbers.first()                          // 1 第一個元素
val firstEven = numbers.first { it % 2 == 0 }        // -2 等同 find
val firstOrNull = numbers.firstOrNull { it % 2 == 0 }// -2 等同 find ，可能回傳 null
// `last` `lastOrNull`尾段元素
val last = numbers.last()                            // -6 最後一個元素
val lastOdd = numbers.last { it % 2 != 0 }           // 5 等同 findLast
val lastOrNull = numbers.lastOrNull { it % 2 != 0 }  // 5 等同 findLast ，可能回傳 null
```

```js
val numbers = listOf(1, 2, 3)
val empty = emptyList<Int>()

// `minOrNull`最小 數字
min = numbers.minOrNull()  // 1 最小元素
min = empty.minOrNull()  // null 空集合 不存在元素
// `maxOrNull`最大 數字
max = numbers.maxOrNull()  // 3 最大元素
max = empty.maxOrNull()  // null 空集合 不存在元素
```

```js
data class Person(val name: String, val city: String, val phone: String)
val people = listOf(
    Person("John", "Boston", "+1-888-123456"),
    Person("Sarah", "Munich", "+49-777-789123"),
    Person("Svyatoslav", "Saint-Petersburg", "+7-999-456789"),
    Person("Vasilisa", "Saint-Petersburg", "+7-999-123456"))

// associateBy 輸出為鍵 ， 元素為值
val phoneBook = people.associateBy { it.phone }  //(Person::phone, Person)
// associateBy 第一項為鍵 ， 第二項如果存在作為值 
val lastPersonCity = people.associateBy(Person::city, Person::name)  //(Person::city, Person)
// groupBy 第一項為鍵 ， 第二項如果存在所有元素作為值
val peopleCities = people.groupBy(Person::city, Person::name)  //(Person::city, [Person])
```

```js
// partition() 判定結果的元素分成兩類
val evenOdd = numbers.partition { it % 2 == 0 }
evenOdd.first   // true
evenOdd.second  // false
val (positives, negatives) = numbers.partition { it > 0 } // (true, flase)
```

```js
//`flatMap` 元素歷遍，元素是集合則會內容也歷遍而非視為集合物件
val fruitsBag = listOf("apple","orange","banana","grapes")
val clothesBag = listOf("shirts","pants","jeans")
val cart = listOf(fruitsBag, clothesBag)    // 建立 二維 list?
val mapBag = cart.map { it }     // 重新歷遍成 一維
val flatMapBag = cart.flatMap { it }    // 歷遍內容 ， list 則會像內容也全部歷遍而非物件
```


```js
val shuffled = listOf(5, 4, 2, 1, 3, -10)
val natural = shuffled.sorted()
//`sorted` // 小至大 排序
val inverted = shuffled.sortedBy { -it }
//`sortedBy` // 小至大 -it 負元素進行排序
//`sortedDescending` // 大至小 排序
val descending = shuffled.sortedDescending()
//`sortedByDescending` // 大至小 abs(it) 絕對值元素進行排序
val descendingBy = shuffled.sortedByDescending { abs(it)  } 
```


```js
val A = listOf("a", "b", "c")
val B = listOf(1, 2, 3, 4)

// `zip` 集合合併，會以最小的長度為標準
val resultPairs = A zip B                      // [(a, 1), (b, 2), (c, 3)] 合併成一個列表
val resultReduce = A.zip(B) { a, b -> "$a$b" } // [a1, b2, c3] 合併轉換成 "$a$b" 的列表
```

```js
// `getOrElse` 集合訪問，不存在則
val list = listOf(0, 10, 20)
println(list.getOrElse(1) { 42 })    // 10 索引1
println(list.getOrElse(10) { 42 })   // 42 不存在 索引10

val map = mutableMapOf<String, Int?>("x" to 3)
println(map.getOrElse("x") { 1 })       // 3 鍵值 "x" 得值
map["x"] = null
println(map.getOrElse("x") { 1 })       // 1 鍵值 "x" 被移除了
```

## 應用

`Sequence` 序列 預先設置元素處理方式 等待執行
- Sequence 依序處理元素 該元素所有方法執行完畢後 執行下一個元素
- list 依序處理元素 所有元素執行完畢 進行下一個方法

```js
val words = "The quick brown fox jumps over the lazy dog".split(" ")
val lengthsList = words
    .filter { println("filter: $it"); it.length > 3 }
    .map { println("length: ${it.length}"); it.length }
    .take(4)

println("Lengths of first 4 words longer than 3 chars:")
println(lengthsList)
/*
filter: The
filter: quick
filter: brown
filter: fox
filter: jumps
filter: over
filter: the
filter: lazy
filter: dog
length: 5
length: 5
length: 5
length: 4
length: 4
Lengths of first 4 words longer than 3 chars:
[5, 5, 5, 4]
*/

val words = "The quick brown fox jumps over the lazy dog".split(" ")
val wordsSequence = words.asSequence() //轉換 Sequence

val lengthsSequence = wordsSequence
    .filter { println("filter: $it"); it.length > 3 }
    .map { println("length: ${it.length}"); it.length }
    .take(4)

println("Lengths of first 4 words longer than 3 chars")
println(lengthsSequence.toList())
/*
Lengths of first 4 words longer than 3 chars
filter: The
filter: quick
length: 5
filter: brown
length: 5
filter: fox
filter: jumps
length: 5
filter: over
length: 4
[5, 5, 5, 4]
*/
```