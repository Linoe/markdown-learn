# Android_External 外部資源

紀錄 `bulid.gradle.dependencies` 常用引用包

## dependencies 引用庫

字段解釋
- `compose` 適用於 Jetpack Compose 開發 kotlin 語言

引用庫 搜尋
<https://developer.android.com/jetpack/androidx/explorer>
```js
// material-icons-extended 額外圖示
implementation("androidx.compose.material:material-icons-extended")
// ViewModel 畫面背景控制資料
implementation("androidx.lifecycle:lifecycle-viewmodel-compose:2.6.1")
// navigation 導覽控制元件
implementation("androidx.navigation:navigation-compose:2.7.4")

// junit 單元測試用
testImplementation("junit:junit:4.13.2")
```