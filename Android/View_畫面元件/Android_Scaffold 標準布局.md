


最外層使用分布元件 放置 topBar
```js
    Scaffold(
        topBar = {
            CupcakeAppBar(
                currentScreen = currentScreen,
                canNavigateBack = navController.previousBackStackEntry != null,
                navigateUp = { navController.navigateUp() }
            )
        }
    ){ innerPadding -> 
      NavHost(
              navController = navController,
              startDestination = CupcakeScreen.Start.name,
              modifier = Modifier.padding(innerPadding)
          ) {...}
    }
```

`appbar` 內 `navigationIcon` 修改依據分頁顯示內容
`title` 標題
`navigationIcon` 左測圖示 分頁中使用 `IconButton` 按鈕圖示設置 `onClick` 監聽
```js
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun CupcakeAppBar(
    currentScreen: CupcakeScreen,
    canNavigateBack: Boolean,
    navigateUp: () -> Unit,
    modifier: Modifier = Modifier
) {
    TopAppBar(
        title = { Text(stringResource(currentScreen.title)) },
        colors = TopAppBarDefaults.mediumTopAppBarColors(
            containerColor = MaterialTheme.colorScheme.primaryContainer
        ),
        modifier = modifier,
        navigationIcon = {
            if (canNavigateBack) {
                IconButton(onClick = navigateUp) {
                    Icon(
                        imageVector = Icons.Filled.ArrowBack,
                        contentDescription = stringResource(R.string.back_button)
                    )
                }
            }
        }
    )
}
```