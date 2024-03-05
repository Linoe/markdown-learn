# Android_Studio 平台

紀錄關於 android studio 設定

## Plugins

插件視窗
> File> Settings> Plugins

Kotlin Fill Class - 新增所有類別參數
<https://plugins.jetbrains.com/plugin/10942-kotlin-fill-class>

## Codeing
### 快捷鍵

+ `Alt_Shift_→` 切換程式/分割/設計視窗

+ `Ctrl-Alt-O` 優化 import
+ `Alt-Enter` 智能建議

+ `Ctrl-B` 轉至宣告
+ `Ctrl-Alt-B` 轉至實現(只限繼承)

+ `Ctrl-Alt-F7` 搜尋使用類別 (Ctrl-左鍵)

+ `Ctrl-Space` 程式碼建議
+ `Ctrl-Alt-L` 程式碼格式化
+ `Ctrl-P` 顯示參數

+ `Ctrl-Shift-L/U` 切換大小寫
+ `Ctrl-D` 複製行
+ `Ctrl-Y` 刪除行
+ `Alt_Shift_↑` 行往上
+ `Ctrl-/` 註解
+ `Shift-F6` 修改指定名稱
+ `Ctrl-Shift-R` 搜尋覆蓋

自定
+ `Shift-C` 選擇行

### 智能完成(Alt-Enter)

+ `Surround with widget` 將光標的元件外層再增加一個元件
+ `Extract string resource` 將光標得字串新增成字串資源檔
+ `sp` `dp` 所有單位都必須額外引用


## 專案
### 專案檔案總管

左側 `Project` 視窗，可以選擇以下顯示方式

+ `Android` 以撰寫頁面功能為主
  + `manifests` APP 設定
  + `java` APP 程式包，撰寫主要程式
  + `res` APP 頁面資源，包含圖片與字碼等
  + `Gradle Scripts` 編輯器設定
+ `Project` 專案檔案為主，舊式顯示方式
  + `app` 程式，資源，編譯設定

### 偵錯器

兩種開啟方式，不管哪個效果一樣
+ `Attach the debugger to an app process` 啟動中的 app 附加偵錯
+ `Debug 'app'` 以偵錯方式啟動 app

開啟下方 `Debug` 視窗，主要分為兩個
+ `Console` 後台
+ `Debugger` 偵錯
  + `Frames` 當前堆疊，顯示程式實行處
  + `Variables` 當前記憶體，顯示使用的變數

### Logcat 日誌

下方分頁 `Logcat` ，模擬器自帶 adb 連線
實機開啟開發者模式，允許 adb 連線

### 編譯錯誤訊息

File > Setting > Build > Complie
`Command-line Options` 選項中添加
+ `--stacktrace` 程式碼跟蹤
+ `--info` `--debug` 更多訊息
+ `--scan` 取得見解?

### SDK 編譯版本

+ `compiledSdkVersion` 默認設置為最新 Android 可用版本，編譯器實際編譯的版本
+ `minSdkVersion` 表示 APP 可支持的 Android 最早版本，僅用於安裝 APP 時檢查，編譯過程不會檢查是否可用
+ `targetSdkVersion` 表示 APP 可執行目標的 Android 版本，沒有功用

+ `compiledSdkVersion` 編譯版本
File > Project Structure > Modules
Properties 分頁 `Complie Sdk Version`
> 等同於修改 Gradle Scripts/build.gradle.kts
> android { compileSdk = 21 }

+ `minSdkVersion` 最小版本 `targetSdkVersion` 目標版本
File > Project Structure > Modules
Default Config 分頁 `Target SDK Version` 與 `Min SDK Version`


### 查看市場版本

1. 建立新專案時，選擇 Minimum SDK 會有 Help me choose
2. 點擊後會顯示目前市面上活耀中的 API 版本百分率

### 檔案修改

Android studio package 名稱自動縮排方便點擊 相對造成不能直接移動檔案位置道中間資料夾
使用以下方法
> 檔案右鍵> Refactor> Move(F6)> 視窗內修改 To package 指定資料夾

### 匯入圖片

View > Resource Manager
左側 `Resource Manager` 分頁 > 左上icon `+`

+ `QUALIFIER TYPE` 儲存型態，圖像為 `Density`
+ `VALUE` 使用解析度，依據選擇放置在 `drawable-nodpi`

### 預覽無法顯示

+ 編譯專案(右上角重整)
+ 清除快取 
> Flie > Invalidate caches

### 依賴程式庫缺少

建立專案時，依據選擇方案預先載入依賴程式庫不同
可以在以下確認
> Gradle Scripts> bulid.gradle.kts> 節點 dependencies{...}
如果要修改依賴程式庫可以在下面修改
> File> Project Structure> Dependencies 中增減項目

### 加入新檔案刷新

如果直接從資料夾增加檔案，可能不會及時同步，此時使用以下
> Flie> Relorad All Form Disk

## 平台設定

自動儲存
> File> Settings> Appearance & Behavior> System Settings> Synchronization> 
> 取消 Save files on frame deactivation 和 Save files automatically if application is idle

未儲存檔案顯示
> File> Settings> Editor> General> Editor Tabs> Mark modified tabs with asterisk

自動 import
> Editor> General> Auto Import
> 開啟 Add unambiguous imports on the fly 與 Optimlze imports on the fly

### 字形設定

字形相關可以在下面找到
> File> Settings> 視窗> Editor> Font
有兩個設定可以調整
- Font 字形
- Fallback font 如果有缺失字體 則使用此

### 更改 AVD 資料夾

環境變數中增加，會在目的地生成 `.android` 資料夾
> 變數名稱：`ANDROID_SDK_HOME`
> 變數值：`D:\Android_SDK_HOME`

### 右側滾動軸高光

Warning 警告高光 永久取消方法
> File> Settings> 視窗 Editor> Color Scheme> General
> Warning/Weak Warning 選項 Error stripe mark 取消掉

### AGP 相容性

AGP (Android Gradle Plugin) 平台與 Gradle 編譯檔溝通差件
<https://developer.android.com/build/releases/gradle-plugin?buildsystem=ndk-build>

Gradle 對應版本
| AGP version | 最低要求 Gradle version |
| :---------: | :---------------------: |
|     8.3     |           8.3           |
|     8.2     |           8.2           |
|     8.1     |           8.0           |
|     8.0     |           8.0           |
|     7.4     |           7.5           |

Android Studio 對應版本
| Android Studio version  | 必須 AGP version |
| :---------------------: | :--------------: |
|    Iguana - 2023.2.1    |     3.2-8.3      |
|   Hedgehog - 2023.1.1   |     3.2-8.2      |
|   Giraffe - 2022.3.1    |     3.2-8.1      |
|   Flamingo - 2022.2.1   |     3.2-8.0      |
| Electric Eel - 2022.1.1 |     3.2-7.4      |

Android API 對應版本
| API level | 最低 Android Studio version | 最低 AGP version |
| :-------: | :-----------------------------: | :-----------------: |
|    34     |       Hedgehog - 2023.1.1       |        8.1.1        |
|    33     |       Flamingo - 2022.2.1       |         7.2         |

```js

```