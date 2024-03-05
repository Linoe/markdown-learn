## 基礎

`NavigationDrawer` 導覽盒佈局

```js
ModalNavigationDrawer() {
    // Screen content
}
```

`ModalNavigationDrawer.drawerContent{}` 導覽盒項目布局內容
`ModalDrawerSheet` 導覽盒項目布局
`NavigationDrawerItem` 導覽盒項目
```js
ModalNavigationDrawer(
    drawerContent = {
        ModalDrawerSheet {
            NavigationDrawerItem()
            // ...other drawer items
            NavigationDrawerItem()
        }
    }
) {
    // Screen content
}
```

完整範例
```js
ModalNavigationDrawer(
    drawerContent = {
        ModalDrawerSheet {
            Text("Drawer title", modifier = Modifier.padding(16.dp))
            Divider()
            NavigationDrawerItem(
                label = { Text(text = "Drawer Item") },
                selected = false,
                onClick = { /*TODO*/ }
            )
            // ...other drawer items
        }
    }
) {
    // Screen content
}
```

### DrawerState

DrawerState 控制向?
```js

val drawerState = rememberDrawerState(initialValue = DrawerValue.Closed)
val scope = rememberCoroutineScope()

ModalNavigationDrawer(
    drawerState = drawerState,
    drawerContent = {
        ModalDrawerSheet { /* Drawer content */ }
    },
){
  Button(
    onClick = {
        scope.launch {
            drawerState.apply {
                if (isClosed) open() else close()
            }
        }
    }
  ){Text("drawerState")}
}
```


## Navigation 導覽類別

```js
dependencies {
    implementation "androidx.compose.material:material:1.3.1"
}
```

```js
ModalNavigationDrawer() {
    ModalDrawerSheet(
        drawerShape = MaterialTheme.shapes.small,
        drawerContainerColor = MaterialTheme.colorScheme.primaryContainer,
        drawerContentColor = MaterialTheme.colorScheme.onPrimaryContainer,
        drawerTonalElevation = 4.dp,
    ) {
        DESTINATIONS.forEach { destination ->
            NavigationDrawerItem(
                selected = selectedDestination == destination.route,
                onClick = { ... },
                icon = { ... },
                label = { ... }
            )
        }
    }
}
```
```js
ModalNavigationDrawer(
    drawerContent = {
        ModalDrawerSheet {
            // Drawer contents
        }
    },
    gesturesEnabled = false
) {
    // Screen content
}
```
控制項行為
```js
val drawerState = rememberDrawerState(initialValue = DrawerValue.Closed)
val scope = rememberCoroutineScope()
ModalNavigationDrawer(
    drawerState = drawerState,
    drawerContent = {
        ModalDrawerSheet { /* Drawer content */ }
    },
) {
    Scaffold(
        floatingActionButton = {
            ExtendedFloatingActionButton(
                text = { Text("Show drawer") },
                icon = { Icon(Icons.Filled.Add, contentDescription = "") },
                onClick = {
                    scope.launch {
                        drawerState.apply {
                            if (isClosed) open() else close()
                        }
                    }
                }
            )
        }
    ) { contentPadding ->
        // Screen content
    }
}
```


```js
val drawerState = rememberDrawerState(DrawerValue.Closed)
val scope = rememberCoroutineScope()
// icons to mimic drawer destinations
val items = listOf(Icons.Default.Favorite, Icons.Default.Face, Icons.Default.Email)
val selectedItem = remember { mutableStateOf(items[0]) }
ModalNavigationDrawer(
    drawerState = drawerState,
    drawerContent = {
        ModalDrawerSheet {
            Spacer(Modifier.height(12.dp))
            items.forEach { item ->
                NavigationDrawerItem(
                    icon = { Icon(item, contentDescription = null) },
                    label = { Text(item.name) },
                    selected = item == selectedItem.value,
                    onClick = {
                        scope.launch { drawerState.close() }
                        selectedItem.value = item
                    },
                    modifier = Modifier.padding(NavigationDrawerItemDefaults.ItemPadding)
                )
            }
        }
    },
    content = {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text(text = if (drawerState.isClosed) ">>> Swipe >>>" else "<<< Swipe <<<")
            Spacer(Modifier.height(20.dp))
            Button(onClick = { scope.launch { drawerState.open() } }) {
                Text("Click to open")
            }
        }
    }
)
```

```js
// icons to mimic drawer destinations
val items = listOf(Icons.Default.Favorite, Icons.Default.Face, Icons.Default.Email)
val selectedItem = remember { mutableStateOf(items[0]) }
PermanentNavigationDrawer(
    drawerContent = {
        PermanentDrawerSheet(Modifier.width(240.dp)) {
            Spacer(Modifier.height(12.dp))
            items.forEach { item ->
                NavigationDrawerItem(
                    icon = { Icon(item, contentDescription = null) },
                    label = { Text(item.name) },
                    selected = item == selectedItem.value,
                    onClick = {
                        selectedItem.value = item
                    },
                    modifier = Modifier.padding(horizontal = 12.dp)
                )
            }
        }
    },
    content = {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text(text = "Application content")
        }
    }
)
```

```js
val drawerState = rememberDrawerState(DrawerValue.Closed)
val scope = rememberCoroutineScope()
// icons to mimic drawer destinations
val items = listOf(Icons.Default.Favorite, Icons.Default.Face, Icons.Default.Email)
val selectedItem = remember { mutableStateOf(items[0]) }
BackHandler(enabled = drawerState.isOpen) {
    scope.launch {
        drawerState.close()
    }
}

DismissibleNavigationDrawer(
    drawerState = drawerState,
    drawerContent = {
        DismissibleDrawerSheet {
            Spacer(Modifier.height(12.dp))
            items.forEach { item ->
                NavigationDrawerItem(
                    icon = { Icon(item, contentDescription = null) },
                    label = { Text(item.name) },
                    selected = item == selectedItem.value,
                    onClick = {
                        scope.launch { drawerState.close() }
                        selectedItem.value = item
                    },
                    modifier = Modifier.padding(horizontal = 12.dp)
                )
            }
        }
    },
    content = {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text(text = if (drawerState.isClosed) ">>> Swipe >>>" else "<<< Swipe <<<")
            Spacer(Modifier.height(20.dp))
            Button(onClick = { scope.launch { drawerState.open() } }) {
                Text("Click to open")
            }
        }
    }
)
```

```js
var selectedItem by remember { mutableIntStateOf(0) }
val items = listOf("Songs", "Artists", "Playlists")

NavigationBar {
    items.forEachIndexed { index, item ->
        NavigationBarItem(
            icon = { Icon(Icons.Filled.Favorite, contentDescription = item) },
            label = { Text(item) },
            selected = selectedItem == index,
            onClick = { selectedItem = index }
        )
    }
}
```

```js
var selectedItem by remember { mutableIntStateOf(0) }
val items = listOf("Home", "Search", "Settings")
val icons = listOf(Icons.Filled.Home, Icons.Filled.Search, Icons.Filled.Settings)
NavigationRail {
    items.forEachIndexed { index, item ->
        NavigationRailItem(
            icon = { Icon(icons[index], contentDescription = item) },
            label = { Text(item) },
            selected = selectedItem == index,
            onClick = { selectedItem = index }
        )
    }
}
```

`NavigationRail` 側邊導覽條
  `NavigationRailItem` 側邊導覽內部元件
`NavigationBar` 底邊導覽霸
  `NavigationBarItem` 底邊導覽內部元件
  
`PermanentNavigationDrawer` 平板導覽盒
  `PermanentDrawerSheet` 平板導覽盒內的內容
`ModalNavigationDrawer`
  `ModalDrawerSheet`
`DismissableNavigationDrawer`
  `DismissableDrawerSheet`
`NavigationDrawerItem` 平板導覽盒內部元件

`BottomNavigation` 底部的導覽


```js
val navController = rememberNavController()
Scaffold(
  bottomBar = {
    BottomNavigation {
      val navBackStackEntry by navController.currentBackStackEntryAsState()
      val currentDestination = navBackStackEntry?.destination
      items.forEach { screen ->
        BottomNavigationItem(
          icon = { Icon(Icons.Filled.Favorite, contentDescription = null) },
          label = { Text(stringResource(screen.resourceId)) },
          selected = currentDestination?.hierarchy?.any { it.route == screen.route } == true,
          onClick = {
            navController.navigate(screen.route) {
              // Pop up to the start destination of the graph to
              // avoid building up a large stack of destinations
              // on the back stack as users select items
              popUpTo(navController.graph.findStartDestination().id) {
                saveState = true
              }
              // Avoid multiple copies of the same destination when
              // reselecting the same item
              launchSingleTop = true
              // Restore state when reselecting a previously selected item
              restoreState = true
            }
          }
        )
      }
    }
  }
) { innerPadding ->
  NavHost(navController, startDestination = Screen.Profile.route, Modifier.padding(innerPadding)) {
    composable(Screen.Profile.route) { Profile(navController) }
    composable(Screen.FriendsList.route) { FriendsList(navController) }
  }
}

```