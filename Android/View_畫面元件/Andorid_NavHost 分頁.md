## Andorid_NavHost 分頁

Android Compose 負責分頁導覽控制元件

<https://developer.android.com/jetpack/compose/navigation?hl=zh-tw>

## 引用庫

```js

dependencies {
  implementation("androidx.navigation:navigation-runtime-ktx:2.7.4")
  implementation("androidx.navigation:navigation-compose:2.7.4")
}
```

## 基礎

`NavHost` 分頁控制器 建立方式如下
1. `NavHostController` 取得控制器
    ```js
    val navController: NavHostController = rememberNavController()
    ```
2. 建立分頁名稱
    ```js
    enum class PageHost{
        PAGE1, PAGE2, PAGE3
    }
    ```
3. `NavHost` `composable` 組合畫面元件
    ```js
    NavHost(
        navController = navController,
        startDestination = PageHost.PAGE1.name,
        modifier = Modifier
    ) {
        composable(route = PageHost.PAGE1.name){...}
        composable(route = PageHost.PAGE2.name){...}
        composable(route = PageHost.PAGE3.name){...}
    }
    ```

完整內容如下
```js
//分頁名稱
enum class PageHost{
    PAGE1, PAGE2, PAGE3
}
//分頁組件
@Composable
fun TestNavApp(
    nav: NavHostController = rememberNavController(),
    mod: Modifier = Modifier
) {
    NavHost(
        navController = nav,
        startDestination = PageHost.PAGE1.name,
        modifier = mod
    ) {
        composable(route = PageHost.PAGE1.name){
            TestNavPage(
                PageHost.PAGE1,
                PageHost.PAGE2,
                nav
            )
        }
        composable(route = PageHost.PAGE2.name){
            TestNavPage(
                PageHost.PAGE2,
                PageHost.PAGE3,
                nav
            )
        }
        composable(route = PageHost.PAGE3.name){
            TestNavPage(
                PageHost.PAGE3,
                PageHost.PAGE1,
                nav
            )
        }
    }
}
//分頁內容
@Composable
fun TestNavPage(
    page: PageHost,
    next: PageHost,
    navController: NavHostController,
    mod: Modifier = Modifier
){
    Button(onClick = {navController.navigate(next.name)}) {
        Text(text = page.name)
    }
}
```

## 分頁建立

`NavHostController` 是 NavController 類別的子類別，提供額外功能以搭配 NavHost 可組合元件使用。
  `navController`：負責在到達網頁之間切換，也就是應用程式中的螢幕。
  `NavGraph`：將可組合元件目的地對應到導覽目的地。

`NavHost`：作為容器的可組合元件，用於顯示 NavGraph 的目前目的地。
  `startDestination` : 開始的分頁

`composable` 函式是 NavGraphBuilder 的擴充功能函式。
  `route`：分頁名稱
  `content`：您可以在這裡按需要呼叫可組合元件，用於為特定路徑顯示。
  `arguments`: 參考引數
  `deepLinks`: 參考深層
  `backStackEntry ->` 返回堆疊中的條目


```js

val navController: NavHostController = rememberNavController() // 建立分頁控制器
NavHost(
   navController = navController,
   startDestination = CupcakeScreen.Start.name
) {
    composable(route = CupcakeScreen.Start.name) { backStackEntry ->
        MyStartView(...)
    }
}
```

## 取的當前分頁

```js
// 取的當前分頁
val backStackEntry by navController.currentBackStackEntryAsState()
// 獲取當前屏幕的名稱
val currentScreen = CupcakeScreen.valueOf(
    backStackEntry?.destination?.route ?: CupcakeScreen.Start.name
)
```

## 跳轉分頁

`navController.navigate()` 跳轉分頁
```js
composable(route = CupcakeScreen.Start.name) {
   MyStartView(
       onNextButtonClicked = {
           navController.navigate(CupcakeScreen.Flavor.name)
       }
   )
}
composable(route = CupcakeScreen.Flavor.name) {
   MyFlavorView(
       onNextButtonClicked = { 
          navController.navigate(CupcakeScreen.Start.name) 
       }
   )
}
```

## 返回分頁

`navController.popBackStack` 返回分頁
```js
  navController.popBackStack(CupcakeScreen.Start.name, inclusive = false)
```

## 嘗試返回分頁

`navController.navigateUp()` 等同 `popBackStack` 但他更接近按下返回鍵?
`navController.previousBackStackEntry != null` 判定是否存在跳轉過分頁暫存

```js
if (navController.previousBackStackEntry != null) {
  navController.navigateUp() 
}
```

## navigate 行為
```js
//將所有內容都彈出到後堆棧之前的“home”目的地
//導航到“ Friendslist”目的地
navController.navigate("friendslist") {
    popUpTo("home")
}

//將所有內容彈出並包括“home”目的地關閉
//在導航到“ Friendselist”目的地之前的後堆
navController.navigate("friendslist") {
    popUpTo("home") { inclusive = true }
}

//僅在我們尚未打開時才導航到“search”目標
//“search”目的地，避免在頂部的多個副本返回堆疊
navController.navigate("search") {
    launchSingleTop = true
}

```

## navArgument 參數

`navArgument` 可以透過 `startDestination` `route` 取得內部參數
`backStackEntry.arguments?.getString("userId")` 方式取得參數
```js
NavHost(startDestination = "profile/{userId}") {
    ...
    composable(
        "profile/{userId}",
        arguments = listOf(navArgument("userId") { type = NavType.StringType })
    ) {...}
}

// 賦予參數
navController.navigate("profile/user1234" )
// 取得參數
composable("profile/{userId}") { backStackEntry ->
    Profile(navController, backStackEntry.arguments?.getString("userId"))
}
```

## NavDeepLink 深層連結

`NavDeepLink` 給予當外部其他應用程式呼叫時給予通知
當其他應用程式觸發深層連結時，Navigation 會自動深層連結到該可組合項

`manifest.xml` 設定對應的呼叫
```xml
<activity …>
  <intent-filter>
    ...
    <data android:scheme="https" android:host="www.example.com" />
  </intent-filter>
</activity>
```

用於取的對應的參數?
```js
val uri = "https://www.example.com"

composable(
    "profile?id={id}",
    deepLinks = listOf(navDeepLink { uriPattern = "$uri/{id}" })
) { backStackEntry ->
    Profile(navController, backStackEntry.arguments?.getString("id"))
}
```

用於呼叫其他應用程式 `NavDeepLink` 的封裝
```js
val id = "exampleId"
val context = LocalContext.current
val deepLinkIntent = Intent(
    Intent.ACTION_VIEW,
    "https://www.example.com/$id".toUri(),
    context,
    MyActivity::class.java
)

val deepLinkPendingIntent: PendingIntent? = TaskStackBuilder.create(context).run {
    addNextIntentWithParentStack(deepLinkIntent)
    getPendingIntent(0, PendingIntent.FLAG_UPDATE_CURRENT)
}

```

## 巢狀 Navigation

`navigation` 視作內部的 `NavHost` 不需要再另外製作 navController
```js
NavHost(navController, startDestination = "home") {
    // route 分頁名稱
    // startDestination 開啟分頁
    navigation(startDestination = "username", route = "login") {
        composable("username") { ... }
        composable("password") { ... }
        composable("registration") { ... }
    }
}

// 擴充方法 包裝
fun NavGraphBuilder.loginGraph(navController: NavController) {
    navigation(startDestination = "username", route = "login") {
        composable("username") { ... }
        composable("password") { ... }
        composable("registration") { ... }
    }
}
NavHost(navController, startDestination = "home") {
    loginGraph(navController)
}

```
