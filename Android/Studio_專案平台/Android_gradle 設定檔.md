# Android_gradle 設定檔

gradle 提供專案編譯

`dependencies` 依附元件 依賴庫 引用庫

## dependencies 引用庫

開啟方法
> File > Project Structure > Dependencies
Modules 右側分頁 `+` 圖示，下載加入引用庫

`app/bulid.gradle.kts` 最下方有引用庫 開頭對應個別使用的
`dependencies` 引用庫最外層
  `implementation` APP 所有範圍適用
  `testImplementation` junit 範圍適用
  `androidTestImplementation` androidTest 範圍適用
  `debugImplementation` debug 模式適用
```js
dependencies {
    implementation()
    testImplementation()
    androidTestImplementation()
    debugImplementation()
}
```
## 自訂變數

```js
dependencies {
    val compose_version = "1.5.0"
    
    implementation("androidx.compose.ui:ui:$compose_version")
}
```

## 自訂常數

`porjcet/bulid.gradle` 最外層檔案 可以添加設定中會使用到的環境常數

```js
// porjcet/bulid.gradle
buildscript {
    extra.apply {
        set("lifecycle_version", "2.6.2")
    }
}
// app/bulid.gradle
dependencies {
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:${rootProject.extra["lifecycle_version"]}")
}
```

## 宣告 Compose 的依附元件

```js
android {
    buildFeatures {
        compose = true
    }

    composeOptions {
        kotlinCompilerExtensionVersion = "1.4.2"
    }

    kotlinOptions {
        jvmTarget = "1.8"
    }
}
```

## plugins 差件描述位置

`build.gradle` app資料夾底下導入差件
```js
plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
}
```

實際上是指向此差件位置
`build.gradle` 專案資料夾底下
```js
plugins {
    id("com.android.application") version "8.1.1" apply false
    id("org.jetbrains.kotlin.android") version "1.9.0" apply false
}
```

## 依賴庫對應版本

以下紀錄對應的版本號

| 依賴庫                               | 版本  |
| ------------------------------------ | ----- |
| AGP                                  | 8.1.1 |
| Gradle                               | 8.0   |
| androidx.core:core-ktx               | 1.7.2 |
| com.google.android.material:material | 1.9.0 |

