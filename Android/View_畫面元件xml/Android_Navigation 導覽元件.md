Navigation UI 支援類別

`Toolbar`
`CollapsingToolbarLayout`
`ActionBar`

## Toolbar

Layout 配置
```xml
<LinearLayout>
    <androidx.appcompat.widget.Toolbar
        android:id="@+id/toolbar" />
    <androidx.fragment.app.FragmentContainerView
        android:id="@+id/nav_host_fragment"
        ... />
    ...
</LinearLayout>
```

程式取得
```js
override fun onCreate(savedInstanceState: Bundle?) {
    setContentView(R.layout.activity_main)

    ...

    val navController = findNavController(R.id.nav_host_fragment)
    val appBarConfiguration = AppBarConfiguration(navController.graph)
    findViewById<Toolbar>(R.id.toolbar)
        .setupWithNavController(navController, appBarConfiguration)
}
```

搭配 navHost
```js
override fun onCreate(savedInstanceState: Bundle?) {
    ...

    val navHostFragment =
        supportFragmentManager.findFragmentById(R.id.nav_host_fragment) as NavHostFragment
    val navController = navHostFragment.navController
    val appBarConfiguration = AppBarConfiguration(
        topLevelDestinationIds = setOf(),
        fallbackOnNavigateUpListener = ::onSupportNavigateUp
    )
    findViewById<Toolbar>(R.id.toolbar)
        .setupWithNavController(navController, appBarConfiguration)
}
```

## CollapsingToolbarLayout

layout 配置
```xml
<LinearLayout>
    <com.google.android.material.appbar.AppBarLayout
        android:layout_width="match_parent"
        android:layout_height="@dimen/tall_toolbar_height">

        <com.google.android.material.appbar.CollapsingToolbarLayout
            android:id="@+id/collapsing_toolbar_layout"
            android:layout_width="match_parent"
            android:layout_height="match_parent"
            app:contentScrim="?attr/colorPrimary"
            app:expandedTitleGravity="top"
            app:layout_scrollFlags="scroll|exitUntilCollapsed|snap">

            <androidx.appcompat.widget.Toolbar
                android:id="@+id/toolbar"
                android:layout_width="match_parent"
                android:layout_height="?attr/actionBarSize"
                app:layout_collapseMode="pin"/>
        </com.google.android.material.appbar.CollapsingToolbarLayout>
    </com.google.android.material.appbar.AppBarLayout>

    <androidx.fragment.app.FragmentContainerView
        android:id="@+id/nav_host_fragment"
        ... />
    ...
</LinearLayout>
```

程式取得
```js
override fun onCreate(savedInstanceState: Bundle?) {
    setContentView(R.layout.activity_main)

    ...

    val layout = findViewById<CollapsingToolbarLayout>(R.id.collapsing_toolbar_layout)
    val toolbar = findViewById<Toolbar>(R.id.toolbar)
    val navHostFragment =
        supportFragmentManager.findFragmentById(R.id.nav_host_fragment) as NavHostFragment
    val navController = navHostFragment.navController
    val appBarConfiguration = AppBarConfiguration(navController.graph)
    layout.setupWithNavController(toolbar, navController, appBarConfiguration)
}
// 處理返回
override fun onSupportNavigateUp(): Boolean {
    val navController = findNavController(R.id.nav_host_fragment)
    return navController.navigateUp(appBarConfiguration)
            || super.onSupportNavigateUp()
}
```

## AppBarLayout


```xml
<LinearLayout>
    <com.google.android.material.appbar.AppBarLayout
        ... />

        <androidx.appcompat.widget.Toolbar
            android:id="@+id/toolbar"
            ... />

        <com.google.android.material.tabs.TabLayout
            ... />

    </com.google.android.material.appbar.AppBarLayout>
    ...
</LinearLayout>
```

```js
override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
    val navController = findNavController()
    val appBarConfiguration = AppBarConfiguration(navController.graph)

    view.findViewById<Toolbar>(R.id.toolbar)
            .setupWithNavController(navController, appBarConfiguration)
}
```

## DrawerLayout

```xml
<?xml version="1.0" encoding="utf-8"?>
<!-- Use DrawerLayout as root container for activity -->
<androidx.drawerlayout.widget.DrawerLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/drawer_layout"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:fitsSystemWindows="true">

    <!-- Layout to contain contents of main body of screen (drawer will slide over this) -->
    <androidx.fragment.app.FragmentContainerView
        android:name="androidx.navigation.fragment.NavHostFragment"
        android:id="@+id/nav_host_fragment"
        android:layout_width="match_parent"
        android:layout_height="match_parent"
        app:defaultNavHost="true"
        app:navGraph="@navigation/nav_graph" />

    <!-- Container for contents of drawer - use NavigationView to make configuration easier -->
    <com.google.android.material.navigation.NavigationView
        android:id="@+id/nav_view"
        android:layout_width="wrap_content"
        android:layout_height="match_parent"
        android:layout_gravity="start"
        android:fitsSystemWindows="true" />

</androidx.drawerlayout.widget.DrawerLayout>
```

```js
val appBarConfiguration = AppBarConfiguration(navController.graph, drawerLayout)

override fun onCreate(savedInstanceState: Bundle?) {
    setContentView(R.layout.activity_main)

    ...

    val navHostFragment = supportFragmentManager.findFragmentById(R.id.nav_host_fragment) as NavHostFragment
    val navController = navHostFragment.navController
    findViewById<NavigationView>(R.id.nav_view)
        .setupWithNavController(navController)
}
```

## ViewPager

```xml
<androidx.viewpager.widget.ViewPager
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:id="@+id/pager"
    android:layout_width="match_parent"
    android:layout_height="match_parent">

    <com.google.android.material.tabs.TabLayout
        android:id="@+id/tab_layout"
        android:layout_width="match_parent"
        android:layout_height="wrap_content" />

</androidx.viewpager.widget.ViewPager>
```

```js
class CollectionDemoFragment : Fragment() {
    ...
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        val tabLayout = view.findViewById(R.id.tab_layout)
        tabLayout.setupWithViewPager(viewPager)
    }
    ...
}

class DemoCollectionPagerAdapter(fm: FragmentManager) : FragmentStatePagerAdapter(fm) {

    override fun getCount(): Int  = 4

    override fun getPageTitle(position: Int): CharSequence {
        return "OBJECT ${(position + 1)}"
    }
    ...
}
```

## BottomNavigationView

```xml
<LinearLayout>
    ...
    <androidx.fragment.app.FragmentContainerView
        android:id="@+id/nav_host_fragment"
        ... />
    <com.google.android.material.bottomnavigation.BottomNavigationView
        android:id="@+id/bottom_nav"
        app:menu="@menu/menu_bottom_nav" />
</LinearLayout>
```

```js
override fun onCreate(savedInstanceState: Bundle?) {
    setContentView(R.layout.activity_main)

    ...

    val navHostFragment =
        supportFragmentManager.findFragmentById(R.id.nav_host_fragment) as NavHostFragment
    val navController = navHostFragment.navController
    findViewById<BottomNavigationView>(R.id.bottom_nav)
        .setupWithNavController(navController)
}
```

## 其他

NavController 監聽
```js
navController.addOnDestinationChangedListener { _, destination, _ ->
   if(destination.id == R.id.full_screen_destination) {
       toolbar.visibility = View.GONE
       bottomNavigationView.visibility = View.GONE
   } else {
       toolbar.visibility = View.VISIBLE
       bottomNavigationView.visibility = View.VISIBLE
   }
}
```

## argument 參數
```xml
<?xml version="1.0" encoding="utf-8"?>
<navigation xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/navigation\_graph"
    app:startDestination="@id/fragmentOne">
    <fragment
        android:id="@+id/fragmentOne"
        android:name="com.example.android.navigation.FragmentOne"
        android:label="FragmentOne">
        <action
            android:id="@+id/action\_fragmentOne\_to\_fragmentTwo"
            app:destination="@id/fragmentTwo" />
    </fragment>
    <fragment
        android:id="@+id/fragmentTwo"
        android:name="com.example.android.navigation.FragmentTwo"
        android:label="FragmentTwo">
        <argument
            android:name="ShowAppBar"
            android:defaultValue="true" />
    </fragment>
</navigation>
```

```js
navController.addOnDestinationChangedListener { _, _, arguments ->
    appBar.isVisible = arguments?.getBoolean("ShowAppBar", false) == true
}
```

## 不同尺寸配置

res/layout-w960dp
```xml
<!-- res/layout-w960dp/navigation_activity.xml -->
<RelativeLayout
   xmlns:android="http://schemas.android.com/apk/res/android"
   xmlns:app="http://schemas.android.com/apk/res-auto"
   xmlns:tools="http://schemas.android.com/tools"
   android:layout_width="match_parent"
   android:layout_height="match_parent"
   tools:context="com.example.android.codelabs.navigation.MainActivity">

   <com.google.android.material.navigation.NavigationView
       android:id="@+id/nav_view"
       android:layout_width="wrap_content"
       android:layout_height="match_parent"
       android:layout_alignParentStart="true"
       app:elevation="0dp"
       app:headerLayout="@layout/nav_view_header"
       app:menu="@menu/nav_drawer_menu" />

   <View
       android:layout_width="1dp"
       android:layout_height="match_parent"
       android:layout_toEndOf="@id/nav_view"
       android:background="?android:attr/listDivider" />

   <androidx.appcompat.widget.Toolbar
       android:id="@+id/toolbar"
       android:layout_width="match_parent"
       android:layout_height="wrap_content"
       android:layout_alignParentTop="true"
       android:layout_toEndOf="@id/nav_view"
       android:background="@color/colorPrimary"
       android:theme="@style/ThemeOverlay.MaterialComponents.Dark.ActionBar" />

   <androidx.fragment.app.FragmentContainerView
       android:id="@+id/my_nav_host_fragment"
       android:name="androidx.navigation.fragment.NavHostFragment"
       android:layout_width="match_parent"
       android:layout_height="match_parent"
       android:layout_below="@id/toolbar"
       android:layout_toEndOf="@id/nav_view"
       app:defaultNavHost="true"
       app:navGraph="@navigation/mobile_navigation" />
</RelativeLayout>
```

res/layout-h470dp 
```xml
<!-- res/layout-h470dp/navigation_activity.xml -->
<LinearLayout
   xmlns:android="http://schemas.android.com/apk/res/android"
   xmlns:app="http://schemas.android.com/apk/res-auto"
   xmlns:tools="http://schemas.android.com/tools"
   android:layout_width="match_parent"
   android:layout_height="match_parent"
   android:orientation="vertical"
   tools:context="com.example.android.codelabs.navigation.MainActivity">

   <androidx.appcompat.widget.Toolbar
       android:id="@+id/toolbar"
       android:layout_width="match_parent"
       android:layout_height="wrap_content"
       android:background="@color/colorPrimary"
       android:theme="@style/ThemeOverlay.MaterialComponents.Dark.ActionBar" />

   <androidx.fragment.app.FragmentContainerView
       android:id="@+id/my_nav_host_fragment"
       android:name="androidx.navigation.fragment.NavHostFragment"
       android:layout_width="match_parent"
       android:layout_height="0dp"
       android:layout_weight="1"
       app:defaultNavHost="true"
       app:navGraph="@navigation/mobile_navigation" />

   <com.google.android.material.bottomnavigation.BottomNavigationView
       android:id="@+id/bottom_nav_view"
       android:layout_width="match_parent"
       android:layout_height="wrap_content"
       app:menu="@menu/bottom_nav_menu" />
</LinearLayout>
```
res/layout
```xml
<!-- res/layout/navigation_activity.xml -->
<androidx.drawerlayout.widget.DrawerLayout
   xmlns:android="http://schemas.android.com/apk/res/android"
   xmlns:app="http://schemas.android.com/apk/res-auto"
   xmlns:tools="http://schemas.android.com/tools"
   android:id="@+id/drawer_layout"
   android:layout_width="match_parent"
   android:layout_height="match_parent"
   tools:context="com.example.android.codelabs.navigation.MainActivity">

   <LinearLayout
       android:layout_width="match_parent"
       android:layout_height="match_parent"
       android:orientation="vertical">

       <androidx.appcompat.widget.Toolbar
           android:id="@+id/toolbar"
           android:layout_width="match_parent"
           android:layout_height="wrap_content"
           android:background="@color/colorPrimary"
           android:theme="@style/ThemeOverlay.MaterialComponents.Dark.ActionBar" />

       <androidx.fragment.app.FragmentContainerView
           android:id="@+id/my_nav_host_fragment"
           android:name="androidx.navigation.fragment.NavHostFragment"
           android:layout_width="match_parent"
           android:layout_height="match_parent"
           app:defaultNavHost="true"
           app:navGraph="@navigation/mobile_navigation" />
   </LinearLayout>

   <com.google.android.material.navigation.NavigationView
       android:id="@+id/nav_view"
       android:layout_width="wrap_content"
       android:layout_height="match_parent"
       android:layout_gravity="start"
       app:menu="@menu/nav_drawer_menu" />
</androidx.drawerlayout.widget.DrawerLayout>
```

依據尺寸存取?
```js
class MainActivity : AppCompatActivity() {

   private lateinit var appBarConfiguration : AppBarConfiguration

   override fun onCreate(savedInstanceState: Bundle?) {
      super.onCreate(savedInstanceState)
      setContentView(R.layout.navigation_activity)
      val drawerLayout : DrawerLayout? = findViewById(R.id.drawer_layout)
      appBarConfiguration = AppBarConfiguration(
                  setOf(R.id.home_dest, R.id.deeplink_dest),
                  drawerLayout)

      ...

      // Initialize the app bar with the navigation drawer if present.
      // If the drawerLayout is not null here, a Navigation button will be added
      // to the app bar whenever the user is on a top-level destination.
      setupActionBarWithNavController(navController, appBarConfig)

      // Initialize the NavigationView if it is present,
      // so that clicking an item takes
      // the user to the appropriate destination.
      val sideNavView = findViewById<NavigationView>(R.id.nav_view)
      sideNavView?.setupWithNavController(navController)

      // Initialize the BottomNavigationView if it is present,
      // so that clicking an item takes
      // the user to the appropriate destination.
      val bottomNav = findViewById<BottomNavigationView>(R.id.bottom_nav_view)
      bottomNav?.setupWithNavController(navController)

      ...
    }

    ...
}
```
