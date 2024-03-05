## package 建議配置

`com.公司名稱.APP名稱` 標準配置 放置 Activity 檔案
  `.data` List 等資料資源
  `.modle` data class 等模板資源
  `.ui` Fragment 等子頁面
    `.theme` Color Theme Type 畫面元件內部資源
      `Color.kt` Color() 自訂資源(res/Colors.xml 不同)
      `Type.kt` Typography() 文字風格資源
      `Theme.kt` Composable 第一層 Theme 程式