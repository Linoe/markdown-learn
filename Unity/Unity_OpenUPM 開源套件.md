# Unity_OpenUPM 開源套件

Unity 可以透過 OpenUPM 下載開源套件 效果等同使用 Packages Manager 安裝

## 安裝 OpenUPM

透過安裝 `openupm-cli` 命令列使用
1. 安裝 `Node.js` (已安裝可跳過)
   > 官網 <https://nodejs.org/en>
2. 開啟命令列 (`CMD`, `PowerShell`)
   > 可以輸入 `npm -v` 確認版本
3. 安裝 `openupm-cli`
   > `npm install -g openupm-cli`</br>
   > 可以輸入 `openupm-cn --version` 確認成功

可能出現以下問題
- 因為這個系統上已停用指令碼執行
  - 受限於 `PowerShell` 權限問題 可以輸入以下解決
    > 確認當前版本 `Get-ExecutionPolicy -List`</br>
    > 設定權限 `Set-ExecutionPolicy RemoteSigned`
- .js 檔案代碼 thow 錯誤
  - 可能是版本上過舊 以下解決
    > 更新 `Node.j
    s`

## Unity 專案下載 UPM 套件

<https://openupm.cn/zh/docs/getting-started.html#%E5%AE%89%E8%A3%85openupm-cli>
Unity 建立專案可以直接透過 `openupm-cli` 下載套件

1. 移動至專案資料夾
   > `cd ~/projects/hello-openupm`
2. 搜索目標軟件
   > 可以進行模糊搜索 `openupm-cn search addressable`
   > 或是完整名稱搜索 `openupm-cn search com.littlebigfun.addressable-importer`
3. 下載套件
   > `openupm-cn add com.littlebigfun.addressable-importer`
4. 完成後可以在 Unity 的 Packages Manager 查看安裝

## Unity 專案解除 UPM 套件

Unity 專案可以直接透過 `openupm-cli` 解除套件

1. 移動至專案資料夾
   > `cd ~/projects/hello-openupm`
2. 移除安裝目標軟件
   > `openupm-cn remove com.littlebigfun.addressable-importer`
3. 完成後可以在 Unity 的 Packages Manager 查看安裝

## 參考

Unity 使用 OpenUPM
<https://www.youtube.com/watch?v=lZrL0YZ7vAo>
OpenUPM 官網
<https://openupm.cn/>
OpenUPM 快速入門
https://openupm.cn/zh/docs/getting-started.html#%E5%AE%89%E8%A3%85openupm-cli
OpenUPM Github
<https://github.com/openupm/openupm>