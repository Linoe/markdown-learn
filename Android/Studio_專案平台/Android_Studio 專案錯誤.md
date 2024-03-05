# Android_Studio 專案錯誤

紀錄與平台相關的設定錯誤

## 實驗元件錯誤

部分元件可能出現如下
> This material API is experimental and is likely to change or to be removed in the future.
這是編輯器部分 SDK 版本會出現，可以以下解決

+ `@OptIn(ExperimentalMaterial3Api::class)` 方法修飾
+ `build.gradle` 檔案添加以下內容
```js
kotlinOptions {
    allWarningsAsErrors = false
    freeCompilerArgs += [
        '-opt-in=androidx.compose.material3.ExperimentalMaterial3Api'
    ]
}
```

## 底版本機台圖片載入錯誤

如果出現以下且確定 `painterResource()` 錯誤，可能是運行在低版本中發生
> Only VectorDrawables and rasterized asset types are supported ex. PNG, JPG

改使用 AndroidView 仍然錯誤， `Modifier.size()` 錯誤可能出現記憶體堆疊溢出
> Failed to allocate a 345461772 byte allocation with 4194304 free bytes and 211MB until OOM
<https://blog.csdn.net/weixin_40431223/article/details/122722005>
<https://stackoverflow.com/questions/72582702/painterresource-throws-illegalargumentexception-only-vectordrawables-and-raster>

## Gradle 版本錯誤

可能原因很多 以下去檢查
1. 檢查 AGP 與 Gradle 對應
2. 檢查 build.gradle 中 SDK 版本 是否符合 AGP 與 Gradle
  ```js
  android {
    compileSdk = 33

    defaultConfig {
        minSdk = 24
        targetSdk = 33
    }
  }
  ```
3. 檢查 build.gradle 中 dependencies 引用版本是否超出最低 SDK 版本
  ```js
  dependencies {
      implementation("androidx.activity:activity-compose:1.7.2")
  }
  ```
關於版本號目前有一說法
- 函式庫的主版本號碼是最小編譯sdk版本
> support libraries v21.x.x -> requires API 21 
> support libraries v22.x.x -> requires API 22 
> support libraries v23.x.x -> requires API 23 

## AAR 數據錯誤

Android Gradle 低於目標版本
> Execution failed for task ':app:checkDebugAarMetadata'.
> A failure occurred while executing com.android.build.gradle.internal.tasks.CheckAarMetadataWorkAction
>   3 issues were found when checking AAR metadata:

修改方法與 Gradle 版本錯誤 相同
1. 檢查 AGP 與 Gradle 對應
2. 檢查 build.gradle 中 SDK 版本 是否符合 AGP 與 Gradle
3. 檢查 build.gradle 中 dependencies 引用版本是否超出最低 SDK 版本

## 新舊引用合併錯誤

Android Gradle 新版本 使用舊引用庫導致不允許
> Your project has set `android.useAndroidX=true`, but configuration `debugRuntimeClasspath` still contains legacy support libraries, which may cause runtime issues.
> This behavior will not be allowed in Android Gradle plugin 8.0.
> Please use only AndroidX dependencies or set `android.enableJetifier=true` in the `gradle.properties` file to migrate your project to AndroidX (see https://developer.android.com/jetpack/androidx/migrate for more info).
> The following legacy support libraries are detected:
>  debugRuntimeClasspath -> com.alibaba:arouter-api:1.5.2 -> com.android.support:support-v4:28.0.0

`gradle.properties`添加以下
```js
android.useAndroidX=true
android.enableJetifier=true
```
- `android.useAndroidX` 該標誌設置為 true 時，Android 插件會使用對應的 AndroidX 庫，而非支持庫。如果未指定，那麼該標誌默認為 false。
- `android.enableJetifier` 該標誌設置為 true 時，Android 插件會通過重寫其二進製文件來自動遷移現有的第三方庫，以使用 AndroidX 依賴項。如果未指定，那麼該標誌默認為 false。

## MainManifest 合併錯誤

新舊檔案合併錯誤訊息
> Execution failed for task ':app:processDebugMainManifest'.
> Manifest merger failed : android:exported needs to be explicitly specified for element <activity#com.ben.retrofit2_demo.MainActivity>. 
> Apps targeting Android 12 and higher are required to specify an explicit value for `android:exported` when the corresponding component has an intent filter defined. 
> See https://developer.android.com/guide/topics/manifest/activity-element#exported for details.

`AndroidManifest.xml` 添加以下
```xml
<activity
    android:exported="true">
</activity>
```

## layout 未生成 databinding

`layout` 未生成對應的檔案

`build.gradle` 添加以下
```js
android {
    buildFeatures {
        dataBinding true
    }
}
```