# Android_Resoures 資源

Android 資源會被放置在 res 資料夾，字串 圖片 排版等
圖片資源會依據畫面尺寸讀取不同資料夾
資源檔名命名規則 小寫_小寫_數字

## 取得資源

```js
// Composable
@Composable
fun ReplyHomeScreen(
  val str = getString(R.string.hello)
}
//Non Composable
val description: String = MyApplication.context.getString(R.string.no_description_yet)
val description: String = Resources.getSystem().getString(R.string.no_description_yet)
```

## MaterialTheme 材質風格

使用預設材質進行風格的修改
- `Theme.kt` 撰寫風格檔案
```js
// darkTheme 設定深色模式
// dynamicColor 動態顏色， Android 12+ 自動變更主題色
@Composable
fun MyAppTheme(
   darkTheme: Boolean = isSystemInDarkTheme(),
   dynamicColor: Boolean = true,
   content: @Composable () -> Unit
){
  ...
  MaterialTheme(  //使用預設的風格，可以在此添加其他風格影響整體
      colorScheme = colorScheme,  //顏色
      typography = Typography,  //字體 Type.kt
      shapes = Shapes,  //形狀，如果有切邊的圖形
      content = content
  )
}
```

## Typography 字型

字體提供風格使用
- `Type.kt` 撰寫字型變數，提供給風格使用
- `res/font` 放置字型檔案
```js
//撰寫需要的字型變數
val Montserrat = FontFamily(
    Font(R.font.montserrat_regular),
    Font(R.font.montserrat_bold, FontWeight.Bold)
)
//修改風格的字型
val Typography = Typography(
    bodyLarge = TextStyle(
        fontFamily = Montserrat,
        fontWeight = FontWeight.Normal,
        fontSize = 14.sp
    )
)

//字型可以針對需要的部分進行調整，每個變數都對應一個字型
public constructor Typography(
    val displayLarge: TextStyle,
    val displayMedium: TextStyle,
    val displaySmall: TextStyle,
    val headlineLarge: TextStyle,
    val headlineMedium: TextStyle,
    val headlineSmall: TextStyle,
    val titleLarge: TextStyle,
    val titleMedium: TextStyle,
    val titleSmall: TextStyle,
    val bodyLarge: TextStyle,
    val bodyMedium: TextStyle,
    val bodySmall: TextStyle,
    val labelLarge: TextStyle,
    val labelMedium: TextStyle,
    val labelSmall: TextStyle
)
```

## Image Asset 圖片資源匯入

這個匯入用在使用 Android .xml 撰寫的向量圖檔
> Tools> Resoures Manager> 左上 `+` 圖示> Image Asset
可以選擇要匯入的圖片用途

## drawable mipmap 資料夾

使用上並無差別， mipmap 使用 mip貼圖(近的時候用高解析度，而遠的時候用低解析度)
安裝後 drawable 只保留所需要的解析度圖檔，使用上如下

- `mipmap` 系統圖標(啟動 icon)
- `drawable` 圖片資源(背景)

## mipmap 資料夾(api v26

Android 8.0 以後移除關於 mipmap dip 相關資料夾處理
如果最低 API v26 以上只會生成 `mipmap-anydpi-v26`
否則適應 API v26 以下版本生成 `mipmap-anydpi-v26` 與 `mipmap-XXXdpi` ，以 .webp 檔案型式


## dpi 資料夾密度

Android 圖形會依據資料夾的後缀進行圖片縮放
當前螢幕dpi / 圖片資料夾dpi 以此放大
px = dp * (dpi / 160)
手機 dpi = 一英吋px面積 / 160
<https://developer.android.com/training/multiscreen/screendensities#TaskProvideAltBmp>
| 資料夾    |                  名稱                  |   dpi   |
| :-------- | :------------------------------------: | :-----: |
| `ldpi`    |             低密度 (ldpi)              | 120 dpi |
| `mdpi`    |             中密度 (mdpi)              | 160 dpi |
| `hdpi`    |             高密度 (hdpi)              | 240 dpi |
| `xhdpi`   |            超高密度 (xhdpi)            | 320 dpi |
| `xxhdpi`  |          超超高密度 (xxhdpi)           | 480 dpi |
| `xxxhdpi` |         超超超高密度 (xxxhdpi)         | 640 dpi |
| `nodpi`   | 獨立密度(無論密度為何，系統都不會縮放)    |         |
| `tvdpi`   |          mdpi ~ hdpi 密度資源          | ~213dpi |
> 關於 tvdpi :
> 這並不是「主要的」像素密度分組。主要用於電視，且大多數應用程式不需要使用此限定詞。
> 如果您認為有必要提供 tvdpi 資源，請將資源大小設為 1.33 * mdpi。
> 例如，mdpi 螢幕的 100x100 像素圖片在 tvdpi 螢幕上為 133x133 像素。

## res 資料夾

`res` 放置所有資源相關(圖片 畫面 文字...)
`drawable` 無 dpi 圖片資源
`layout` 畫面資源
`menu` 放置 menu.xml 元件資源
`mipmap` 啟動 Icon 資源
`navigation` 放置 navigation.xml 元件資源
`values` 放置 color.xml dimen.xml string.xml style.xml 常數資源
`xml` 放置 xml 建構資源(網頁)

`values` `drawable` 可以透過後綴設置對應尺寸
`*-land` 橫向
`*-night` 夜晚
`*-v23` api v23 以上版本
`*-w600dp` 寬 600 dp 以上
`*-h1240dp` 高 1240 dp 以上
