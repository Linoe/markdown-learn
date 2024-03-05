# Android_Retrofit 基礎

`Retrofit` 是底層基於 `OkHttp` 的連線套件 即遵循 `RESTful` 的 Http

<https://square.github.io/retrofit/>
<https://square.github.io/retrofit/2.x/retrofit/>
<https://square.github.io/retrofit/2.x/retrofit/>

## 依賴庫

`build.gradle`
```js
dependencies {
    implementation("com.squareup.retrofit2:retrofit:2.9.0") // 本體
    implementation("com.squareup.retrofit2:converter-gson:2.6.0") // 用於轉換 json 格式
    implementation("com.squareup.okhttp3:okhttp:4.0.1") // okhttp 不使用可以省略 大部分功能以被包含在 retrofit
}
```

`AndroidManifest` 增加網路的權限
```xml
<uses-permission android:name=”android.permission.INTERNET” />
```

## 基礎

`Retrofit` 建置時必須要有以下
1. `URL` 網址 不包含API指令
2. `interface API` API 介面 用於建立物件時 使用何種 API
3. `class Data` Response 回傳資料 用於當資料回傳經過轉換後 產出可讀的資料物件 
4. `Retrofit.Builder()` 建構器 設置實例
5. `Retrofit.create()` 建立物件 將設置設定建立成實體物件 同時需要 API
6. `Call<T>` 呼叫 API 後回傳 Call 物件後 回傳連線取得資料方式有兩種
  - `execute()` 同步執行 運行在當前執行緒 Android 不允許在主線程執行此
  - `enqueue(CallBack)` 異步執行 額外建立執行緒 資料回傳到 `CallBack`

## 資料
### URL

`URL` 可以放在任何靜態存取變數
```js

object Constants{
  const val URL = "https://jsonplaceholder.typicode.com"
}
```

### API

`API` 客戶端 介面返回一個 `Call<T>` 泛型則是自訂的 Data

`API` 透過網址與伺服器溝通 規則如下
- `Http` GET 方法
- `Host` api.github.com 伺服器網址 通常是 URL
- `Path` /users/{username}/repos 伺服器內路徑


```java
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;
```
```js
/*
  https://jsonplaceholder.typicode.com/posts
  URL : https://jsonplaceholder.typicode.com/
  Path : posts
  Http 方法透過注釋

  Call<List<PostResponse>> 固定型態
  List<T> 對應的 Json 列表 []
  PostsResp 回傳內容的 Json 物件 {}
*/
interface ApiService {
    @GET("posts")
    fun posts(): Call<List<PostResponse>>
}
```

方法注釋有以下
- `@GET` 發送一個 HTTP GET 請求 
- `@PUT`
- `@POST`
- `@PATCH`
- `@HEAD`
- `@DELETE`
- `@OPTIONS`
- `@HTTP` 發送一個自訂 HTTP 協定?
- `@Url` 用於動態 URL 網址
<https://square.github.io/retrofit/2.x/retrofit/retrofit2/http/package-summary.html>
```js

interface WeatherService {
    @GET("2.5/weather")
    fun getWeather(
        @Query("lat") lat:Double,
        @Query("lon") lon:Double,
        @Query("units") units:String?,
        @Query("appid") appid:String,
    ): Call<WeatherResponse>
}
```

參數注釋有以下
- `@Query` 作為有 `?` 網址參數
- `@Path` 網址中 `{}` 指向對應參數
```js
interface GitHubService {

    @GET("users/{username}/repos")
    fun loadRepos(
        @Path("username") username: String, // 變數
        @Query("sort") sort: String // 引数
    ): Call<ResponseBody>
}
```

### DATA

資料依據回傳 `Json` 內容而定 通常最外層的 List 會放置在 Call 

```json
// Call 整個資料
// [] Json 的列表 List 
// {} Json 回傳內容的物件 Class
[
  {
    "userId": 1,
    "id": 1,
    "title": "String",
    "body": "String"
  },
  {
    "userId": 2,
    "id": 2,
    "title": "String",
    "body": "String"
  }
]
```


`data class` 一般類別物件即可
```js
data class PostResponse (
    val userId: Int,
    val id: Int,
    val title: String,
    val body: String
)
```

`@SerializedName` 需要 Json 名稱與變數名稱不同時
```js
class Posts {  
    @SerializedName("userId")  
    var userId: Int = 0  
    @SerializedName("id")  
    var id: Int = 0  
    @SerializedName("title")  
    var title: String? = null  
    @SerializedName("body")  
    var body: String? = null  
}
```

## Retrofit Builder/Create

`Retrofit.Builder()` 建立連線開始 建立需要以下
- `.Builder()` 取的建立物件
- `.baseUrl(URL)` 伺服器網址
- `.addConverterFactory(Converter)` 資料變換 Json xml ...等 常見資料型態
- `.client(okHttpClient)` 如果需要自行控制 okHttp 使用此
- `.build()` 產生準備連線物件

`.create(T)` 建立依據 API 呼叫的物件
```js
// 建立工廠用 interface 避免擴充到其他 Bulid
interface Bulid {
    companion object {
        const val URL = "https://jsonplaceholder.typicode.com"

        fun Retrofit(): Retrofit = Retrofit.Builder()
            .baseUrl(Bulid.URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        fun Server():ApiService = Retrofit().create(ApiService::class.java)
    }
}

// 用於公眾
//fun buildRetrofit(): Retrofit = Retrofit.Builder()
//    .baseUrl(Bulid.URL)
//    .addConverterFactory(GsonConverterFactory.create())
//    .build()

//inline fun <reified T> createServer(): T = Retrofit().create(T::class.java)
```

利用 `class` 與 `object` 建立單一公共的連線
```js
class AppClient private constructor() {  
    private val retrofit: Retrofit  
    private val okHttpClient = OkHttpClient()  
  
    init {  
        retrofit = Retrofit.Builder()  
                .baseUrl(Config.URL)  
                .addConverterFactory(GsonConverterFactory.create())  
                .client(okHttpClient)  
                .build()  
    }  
  
    companion object {  
        private val OBJ = AppClient()  
        val client: Retrofit  
            get() = OBJ.retrofit  
    }  
}
```

## Call enqueue/execute

`Call` `Retrofit.Builder().create()` 產生後回傳物件
連線執行則是透過此物件執行 以下方法
- `execute()` 同步執行 運行在當前執行緒 Android 不允許在主線程執行此
- `enqueue(CallBack)` 異步執行 額外建立執行緒 資料回傳到 `CallBack`

```js
var call = Bulid.Server().posts() // Call<List<PostResponse>> 產生連線物件
// execute
var resp = call.execute() // Response<List<PostResponse>> 回傳資料
// enqueue
call.enqueue(object : Callback<T> {
     override fun onResponse(call: Call<T>, response: Response<T>) {
         // in response
     }
    
     override fun onFailure(call: Call<T>, t: Throwable) {
         // in error
     }
})
```

以下 `enqueue` 範例
```js
var call = Bulid.Server().posts() // Call<List<PostResponse>> 產生連線物件
call.enqueue(object : Callback<List<PostResponse>> {
    //請求成功時
    override fun onResponse(
        call: Call<List<PostResponse>>,
        response: Response<List<PostResponse>> // 所有的資料變數?
    ) {
        // 資料處理
        val sb = StringBuffer()

        response.body()?.forEach { it ->
            sb.append(it.body)
            sb.append("\n")
            sb.append("---------------------\n")
        }

        // 輸出資料
        Log.e(sb.toString())

    }
    //請求失敗時 這裡不處理
    override fun onFailure(call: Call<List<PostResponse>>, t: Throwable) {}
})
```

## 進階
### Call Response 差異

`Call<T>` 向網頁伺服器發送請求並回傳回應 例如處理錯誤或回應 可以追蹤連線狀態
`Response` 當伺服器回傳時 取的的資料容器

通常取的順序

1. `Retrofit.build().create(T)` 產生 `API` 物件
2. `API` 執行方法取得 `Call` 物件
3. `Call` 執行 `.enqueue()` 或 `.execute()` 回傳 `Response`
4. `Response` 內部 `.body()` 取得自訂 `Data` 物件

### Retrofit.Builder 方法

`Retrofit.Builder()` 設置連線資訊 主要功能以下
- `addConverterFactory` 添加轉換器 資料轉換自訂物件
- `baseUrl` 設置 URL 可以使用 okhttp3.HttpUrl
- `build` 產生配置後 Raturofit 物件

其他功能
- `client` 設置 okhttp3.OkHttpClient 需要自行控制連線時
- `addCallAdapterFactory` 添加 Call 適配器 讓回傳不用 Call 型態?
- `callFactory` 設置 okhttp3.Call.Factory 用於自訂義 Call 適配器?
- `callbackExecutor` 當連線回傳時呼叫 callback 時 所執行線程 默認主線程

檢查用
- `validateEagerly` 驗證所有的 API 註解是否正確?
- `callAdapterFactories` 取得 Call 適配器
- `converterFactories` 取得轉換器

<https://square.github.io/retrofit/2.x/retrofit/retrofit2/Retrofit.Builder.html>

### Call<T> 方法

`Call<T>` 呼叫 API 後回傳的型態 主要功能以下
- `execute` 同步發送請求 等待回傳
- `enqueue` 非同步發送請求 需要一個 Callback<T> 接收回傳
- `cancel` 取消發送請求 並結束連線?

其他功能
- `clone` 建立一個相同的 Call
- `request` 取得原始 HTTP 請求?

檢查用
- `isCanceled` 如果結束則 true
- `isExecuted` 如果已執行則 true

<https://square.github.io/retrofit/2.x/retrofit/retrofit2/Call.html>

### Response<T> 方法

`Response<T>` 在 Call 請求後回傳型態 主要功能以下
- `body` 回傳成功主體 等同 T 泛型
- `errorBody` 回傳失敗主體 一個 okhttp3.ResponseBody 型態

其他功能
- `code` HTTP 狀態代碼
- `headers` HTTP headers
- `message` HTTP 狀態消息
- `raw` HTTP 原始串
- `error` 建立一個錯誤訊息的 Response<T> 物件 用於溝通用?
- `success` 建立一個成功訊息的 Response<T> 物件 用於溝通用?

檢查用
- `isSuccessful` 如果 `code` 在 [200..300] 則 true

<https://square.github.io/retrofit/2.x/retrofit/retrofit2/Response.html#error-okhttp3.ResponseBody-okhttp3.Response->

## 範例

```js
interface GitHubService {

    @GET("users/{username}/repos")
    fun loadRepos(
        @Path("username") username: String, @Query("sort") sort: String
    ): Call<ResponseBody>
}


class MainActivity : AppCompatActivity(R.layout.main_activity) {

    companion object {
        private const val TAG = "MainActivity"
        private const val BASE_URL = "https://api.github.com" //網址
    }

    private val retrofit = Retrofit.Builder() // 建構器
        .baseUrl(BASE_URL)
        .build()

    private val service = retrofit.create(GitHubService::class.java) // API 實體化

    private val loadRepos = service.loadRepos("octocat", "created") // 呼叫 loadRepos 方法回傳內容

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        // 設置按鈕監聽
        findViewById<Button>(R.id.button).setOnClickListener { executeReposLoad() }
    }

  // execute 同步
    private fun executeReposLoad() {
        thread { // 建立執行續 execute() 不能再主執行續執行
            runCatching { loadRepos.clone().execute() }
                .onSuccess { response -> // 成功取的內容時
                    if (response.isSuccessful) { // isSuccessful 判定成功
                        response.body()?.string()?.let { json -> 
                            Log.d(TAG, json)
                        }
                    } else {
                        val msg = "HTTP error. HTTP status code: ${response.code()}"
                        Log.e(TAG, msg)
                    }
                }
                .onFailure { t -> Log.e(TAG, t.toString()) } // 如果失敗則執行
        }
    }

  // enqueue 異步
    private fun executeReposLoad() {
        loadRepos.clone().enqueue(object : Callback<ResponseBody> {
            override fun onResponse( // 成功時
                call: Call<ResponseBody>,
                response: Response<ResponseBody>
            ) {
                if (response.isSuccessful) {
                    response.body()?.string()?.let { json ->
                        Log.d(TAG, json)
                    }
                } else {
                    val msg = "HTTP error. HTTP status code: ${response.code()}"
                    Log.e(TAG, msg)
                }
            }

            override fun onFailure( // 失敗時
                call: Call<ResponseBody>,
                t: Throwable
            ) {
                Log.e(TAG, t.toString())
            }
        })
    }
}
```


```js
/*
import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import com.example.retrofitsimple.models.WeatherResponse;
import com.example.retrofitsimple.network.WeatherService;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
*/

class MainActivity : AppCompatActivity() {
    private lateinit var tvLatitude:TextView
    private lateinit var tvLongitude:TextView
    private lateinit var tvWeather:TextView
    private lateinit var tvWindSpeed:TextView
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        tvLatitude = findViewById(R.id.tvLatitude)
        tvLongitude = findViewById(R.id.tvLongitude)
        tvWeather = findViewById(R.id.tvWeather)
        tvWindSpeed = findViewById(R.id.tvWindSpeed)

        val retrofit: Retrofit = Retrofit.Builder()
            .baseUrl(Constants.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        val service: WeatherService = retrofit
            .create(WeatherService::class.java)

        val listCall: Call<WeatherResponse> = service.getWeather(
            Constants.LATITUDE,Constants.LONGITUDE,Constants.METRIC_UNIT,Constants.APP_ID
        )
        listCall.enqueue(object : Callback<WeatherResponse> {
            override fun onResponse(
                call: Call<WeatherResponse>,
                response: Response<WeatherResponse>
            ) {
                if(response.isSuccessful){
                    val weatherList : WeatherResponse? = response.body()
                    Log.i("Response Result", "$weatherList ")

                    tvLatitude.text = "經度： ${weatherList!!.coord.lat}"
                    tvLongitude.text = "緯度： ${weatherList.coord.lon}"
                    tvWeather.text = "天氣： ${weatherList.weather[0].main}"
                    tvWindSpeed.text = "風速： ${weatherList.wind.speed}"
                }else{
                    when(response.code()){
                        400->{
                            Log.e("Error 400", "Bad Connection" )
                        }
                        404->{
                            Log.e("Error 400", "Not Found" )
                        }
                        else->{
                            Log.e("Error", "Generic Error" )
                        }
                    }
                }
            }

            override fun onFailure(call: Call<WeatherResponse>, t: Throwable) {
                Log.e("Errorrrrrr", t.message.toString())
            }

        })
    }
}
```