# Android_JUnit 本地測試

Android 本地測試與 java 相同，使用 org.junit.Test 相關引用包
> AndroidText 不同在允許使用方法測試

## 引用

`junit` 所需的引用庫
```js
dependencies {
    
}
```

## 建立

撰寫前先建置相關資料夾與檔案
1. 切換成 `projcet`
2. app/src 中建立資料夾 `test/java` ，編輯器自動偵測成 JUnit
3. 建立 `Package` (com.example.test)， 建立測試用檔案 new > `Kotlin Class/File`

## 測試步驟

完成代碼後
1. JUint class/fun 左側會出現可執行的圖示
2. 點擊後開始執行測試用程式碼
3. 結果會顯示在下方控制台

## 測試代碼

目標程式碼要如下修飾
```js
@VisibleForTesting
internal fun calc(){...}
```

測試用程式碼如下修飾
```js
@Test
fun calc_test() {...}
```

範例
```js
//內碼中使用 assert 相關程式碼比較產生的結果值即可
@Test
fun calculateTip_20PercentNoRoundup() {
    val amount = 10.00
    val tipPercent = 20.00
    val expectedTip = NumberFormat.getCurrencyInstance().format(2)
    val actualTip = calculateTip(amount = amount, tipPercent = tipPercent, false)
    assertEquals(expectedTip, actualTip)
}
```

## 覆蓋率

`Run 'Test' with Coverage` 可以驗證 App 所有功能是否驗證
使用後右側出現 `Coverage` 視窗 可以看到所有資料夾內的程式檔案
右側會出現覆蓋的比例
|    Element    |  class%   | Method%  |   Line%    |
| :-----------: | :-------: | :------: | :--------: |
| GmaeViewModel | 100%(1/1) | 87%(7/8) | 95%(39/41) |
| MainActivity  |  0%(0/1)  | 0%(0/2)  |  0%(0/3)   |

## 測試內容

一般會測試以下項目
1. 成功 輸入正確值驗證是否符合
2. 失敗 輸入錯誤值驗證是否符合
3. 狀態 如果目標存在狀態機 測試所有路線是否符合狀態

## 常用

- `assertEquals()` 等同 Equals
- `assertNotEquals()` 等同 !Equals
- `assertTrue()` 比較 == true
- `assertFalse()` 比較 == false
- `assertNull()` 比較 == null
- `assertNotNull()` 比較 != null

測試錯誤內容
```js
@Test(expected = IllegalArgumentException::class)
```

<https://developer.android.com/reference/junit/framework/Assert>