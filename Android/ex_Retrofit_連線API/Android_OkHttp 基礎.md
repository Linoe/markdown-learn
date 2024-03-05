# Android_OkHttp 基礎

`OkHttp`  是現代應用程式網路的方式。這就是我們交換資料和媒體的方式。有效地執行 HTTP 可以使您的內容載入速度更快並節省頻寬。
- HTTP/2 支援允許對同一主機的所有請求共享套接字。
- 連線池可減少請求延遲（如果 HTTP/2 不可用）。
- 透明 GZIP 縮小了下載大小。
- 回應快取完全避免了網路重複請求。

<https://square.github.io/okhttp/></br>
<https://github.com/square/okhttp>

## 依賴庫

`build.gradle`
```js
dependencies {
    implementation("com.squareup.okhttp3:okhttp:4.0.1") // 本體
    implementation("com.squareup.okhttp3:logging-interceptor:4.0.1") // logging 攔截器
}
```

`AndroidManifest` 增加網路的權限
```xml
<uses-permission android:name=”android.permission.INTERNET” />
```

## 基礎

## URL 解析

https://api.openweathermap.org/data/2.5/weather?lat=44.34&lon=10.99&units=metric&appid=6a0fd44251ed66157191cc45bd275a01

- `https://api.openweathermap.org`：這是 OpenWeatherMap API 的基本 URL。它代表服務器的地址，可在此地址上找到所有可用的 API 端點。
- `/data/2.5/weather`：這是 API 端點的路徑，用於獲取指定位置的當前天氣數據。2.5 是 API 版本號，weather 表示要訪問的特定資源。
- `?`：這個字符表示查詢參數部分的開始。在這之後，指定各種查詢參數(參上表)，以便 API 知道如何返回您需要的數據。
- `lat={lat}`：這是一個查詢參數，表示要查詢的位置的緯度。
- `&lon={lon}`：這是另一個查詢參數，表示要查詢的位置的經度。
- `&appid={API key}`：表示 OpenWeatherMap API 密鑰。需將 {API key} 替換為實際 API 密鑰。
- 在此範例中，我們添加 unit 參數，設為 metric 表示以攝氏表示溫度(可在Learn more中看到各參數值的意思)。

## 攔截器


```js
private var logging = HttpLoggingInterceptor(object : HttpLoggingInterceptor.Logger {  
    override fun log(message: String) {  
        Log.i("interceptor msg", message)  
    }  
})  
  
private var okHttpClient : OkHttpClient  
  
init {  
    logging.level = HttpLoggingInterceptor.Level.BODY  
    okHttpClient = OkHttpClient().newBuilder().addInterceptor(logging).build()  
    retrofit = Retrofit.Builder()  
            .baseUrl(Config.URL)  
            .addConverterFactory(GsonConverterFactory.create())  
            .client(okHttpClient)  
            .build()  
}
```

## gson 轉換器

<https://medium.com/@alice.margatroid.love/%E5%B0%88%E6%A1%88-%E5%BE%9E%E7%B6%B2%E8%B7%AF%E7%8D%B2%E5%8F%96json%E8%B3%87%E6%96%99%E4%B8%A6%E4%BB%A5recyclerview%E5%91%88%E7%8F%BE-81c060dd79e7>