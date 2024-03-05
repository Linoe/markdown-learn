#

紀錄舊式 Activity 畫面配置方法

## 引用庫

`implementation` 舊版本元件
`buildFeatures.dataBinding` 開啟自動生成對應 `layout.xml` 對應檔案
```js
android {
    buildFeatures {
       dataBinding true
    }
}

dependencies {
  implementation("androidx.appcompat:appcompat:1.6.1") // AppCompatActivity
  implementation("androidx.constraintlayout:constraintlayout:2.1.4") // ConstraintLayout
  implementation("com.google.android.material:material:1.8.0") // 基礎畫面元件
  implementation("androidx.navigation:navigation-fragment-ktx:2.5.3") // fragment 支援 navigation
  implementation("androidx.navigation:navigation-ui-ktx:2.5.3") // navigation
}
```

## Navigation
### AppCompatActivity

`activity_main.xml` 會自動透過 databinding 產生一個對應的資料

```xml
<androidx.constraintlayout.widget.ConstraintLayout 
    ...
    android:id="@+id/container">

</androidx.constraintlayout.widget.ConstraintLayout>
```
```js
// activity_main.xml
import com.example.myapplication.databinding.ActivityMainBinding
```

`AppCompatActivity` 透過 `databinding` 取得畫面元件
```js
class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        // 取得 databinding 產生檔案
        binding = ActivityMainBinding.inflate(layoutInflater)
        // root 設置為主畫面
        setContentView(binding.root)
    }
}
```

### Fragment

`layout.xml` 會自動透過 `databinding` 產生一個對應的資料
透過 `context` 找尋對應啟動的 `Fragment`
```xml
<androidx.constraintlayout.widget.ConstraintLayout xmlns:android="http://schemas.android.com/apk/res/android"
    ...
    tools:context=".ui.home.HomeFragment">

    <TextView
        android:id="@+id/text_home"
        ... />
</androidx.constraintlayout.widget.ConstraintLayout>
```
```js
//fragment_home.xml
import com.example.myapplication.databinding.FragmentHomeBinding
```

`Fragment` 透過 `databinding` 取得畫面元件
`ViewModel` 搭配數值變更
```js
class HomeFragment : Fragment() {

    // databinding 產生的檔案
    private var _binding: FragmentHomeBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
            inflater: LayoutInflater,
            container: ViewGroup?,
            savedInstanceState: Bundle?
    ): View {
      // 取得後台模組資料
        val homeViewModel =
                ViewModelProvider(this).get(HomeViewModel::class.java)
      // databinding 抓取當前畫面物件
        _binding = FragmentHomeBinding.inflate(inflater, container, false)
      // 取得  root 畫面與 text 元件 名稱會與 id 相同
        val root: View = binding.root
        val textView: TextView = binding.textHome
      // 透過 ViewModel 給予數值
        homeViewModel.text.observe(viewLifecycleOwner) {
            textView.text = it
        }
      // 提交 root 畫面
        return root
    }
    // 結束釋放 databinding
    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}

class HomeViewModel : ViewModel() {

    private val _text = MutableLiveData<String>().apply {
        value = "This is home Fragment"
    }
    val text: LiveData<String> = _text
}
```

### Navigation

示範如何配置 `Navigation`

`BottomNavigationView` 上方配置導覽 bar 元件
`fragment` 啟動 navGraph 會在下方配置分頁導覽 bar
`fragment` name 自動導入對應的 xml 
`activity_main.xml`
```xml
<androidx.constraintlayout.widget.ConstraintLayout 
    ...
    android:id="@+id/container">

    <com.google.android.material.bottomnavigation.BottomNavigationView
        android:id="@+id/nav_view"
        ...
        app:menu="@menu/bottom_nav_menu" />

    <fragment
        android:id="@+id/nav_host_fragment_activity_main"
        android:name="androidx.navigation.fragment.NavHostFragment"
        ...
        app:defaultNavHost="true"
        app:navGraph="@navigation/mobile_navigation" />

</androidx.constraintlayout.widget.ConstraintLayout>
```

`BottomNavigationView` 中 menu 設置 (上方 bar 的右側選單)
`bottom_nav_menu.xml`
```xml
<menu xmlns:android="http://schemas.android.com/apk/res/android">

    <item
        android:id="@+id/navigation_home"
        android:icon="@drawable/ic_home_black_24dp"
        android:title="@string/title_home" />
    ...

</menu>
```

`fragment` 中 navGraph 設置(下方分頁 bar)
`mobile_navigation.xml`
```xml
<navigation 
    ...
    android:id="@+id/mobile_navigation"
    app:startDestination="@+id/navigation_home">

    <fragment
        android:id="@+id/navigation_home"
        android:name="com.example.myapplication.ui.home.HomeFragment"
        android:label="@string/title_home"
        tools:layout="@layout/fragment_home" />
    ...

</navigation>
```

`MainActivity` 設置 `fragment` NavHost 相關設置
```js
// 取得 root 畫面
val navView: BottomNavigationView = binding.navView
// 取得 navGraph 設置的 fragment
val navController = findNavController(R.id.nav_host_fragment_activity_main)
// 設置對應 fragment id 與 navigation 元件 id 一致
val appBarConfiguration = AppBarConfiguration(setOf(
        R.id.navigation_home, R.id.navigation_dashboard, R.id.navigation_notifications))
// 設置上方 bar 下方分頁 bar 控制器
setupActionBarWithNavController(navController, appBarConfiguration)
// root 畫面設置 上方bar 分頁
navView.setupWithNavController(navController)
```

## FloatingActionButton

`FloatingActionButton` 浮動 Button 只要設置在 xml 即可使用
```xml
<androidx.coordinatorlayout.widget.CoordinatorLayout 
    ...
    tools:context=".MainActivity">

    <com.google.android.material.floatingactionbutton.FloatingActionButton
        android:id="@+id/fab"
        ... />

</androidx.coordinatorlayout.widget.CoordinatorLayout>
```

`MainActivity` 透過自動生成 binding 抓取元件設置
```js
binding = ActivityMainBinding.inflate(layoutInflater)
...
binding.fab.setOnClickListener { view ->
    Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
        .setAction("Action", null).show()
}
```

## AppBarLayout

`AppBarLayout`更早期版本的 導覽 bar 配置方式如下
`fitsSystemWindows` 第一次啟動會保留 bar 空間?
`activity_main.xml`
```xml
<androidx.coordinatorlayout.widget.CoordinatorLayout 
    ...
    android:fitsSystemWindows="true"
    tools:context=".MainActivity">

    <com.google.android.material.appbar.AppBarLayout
        ...
        android:fitsSystemWindows="true">

        <com.google.android.material.appbar.MaterialToolbar
            android:id="@+id/toolbar"
            ... />
    </com.google.android.material.appbar.AppBarLayout>

    <include layout="@layout/content_main" />

</androidx.coordinatorlayout.widget.CoordinatorLayout>

```
`content_main.xml` 用於配置 `fragment`
```xml
<androidx.constraintlayout.widget.ConstraintLayout 
    ...
    app:layout_behavior="@string/appbar_scrolling_view_behavior">

    <fragment
        android:id="@+id/nav_host_fragment_content_main"
        android:name="androidx.navigation.fragment.NavHostFragment"
        ...
        app:defaultNavHost="true"
        app:navGraph="@navigation/nav_graph" />
</androidx.constraintlayout.widget.ConstraintLayout>
```
`nav_graph.xml` 設置兩個用於交替分頁用的 action 
```xml
<navigation 
    ...
    android:id="@+id/nav_graph"
    app:startDestination="@id/FirstFragment">

    <fragment
        android:id="@+id/FirstFragment"
        android:name="com.example.myapplication6.FirstFragment"
        android:label="@string/first_fragment_label"
        tools:layout="@layout/fragment_first">

        <action
            android:id="@+id/action_FirstFragment_to_SecondFragment"
            app:destination="@id/SecondFragment" />
    </fragment>
    <fragment
        android:id="@+id/SecondFragment"
        android:name="com.example.myapplication6.SecondFragment"
        android:label="@string/second_fragment_label"
        tools:layout="@layout/fragment_second">

        <action
            android:id="@+id/action_SecondFragment_to_FirstFragment"
            app:destination="@id/FirstFragment" />
    </fragment>
</navigation>
```

`MainActivity` 設置對應監聽與 分頁導覽
`onCreateOptionsMenu` 配置 menu
`onOptionsItemSelected` menu 點擊選項
`onSupportNavigateUp` 控制分頁順配置畫面元件
```js
class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)
        // 配置導覽分頁
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        appBarConfiguration = AppBarConfiguration(navController.graph)
        setupActionBarWithNavController(navController, appBarConfiguration)
    }
    //配置 menu
    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.menu_main, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            R.id.action_settings -> true
            else -> super.onOptionsItemSelected(item)
        }
    }
    // 設置分頁
    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()
    }
}
```

`Fragment` 取得對應 binding 畫面設置為 root
`onViewCreated` 當畫面被重新建置時會呼叫 此處處理監聽
```js
class FirstFragment : Fragment() {

    private var _binding: FragmentFirstBinding? = null

    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        _binding = FragmentFirstBinding.inflate(inflater, container, false)
        return binding.root

    }
    // 設置監聽
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        // 點及按鈕時 控制分頁改顯示另一個分頁
        binding.buttonFirst.setOnClickListener {
            findNavController().navigate(R.id.action_FirstFragment_to_SecondFragment)
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
```