# Git_基礎

Git 分散式版本控制 用於追蹤檔案的變化與多人協同作業

## Git 存儲庫

Git 存儲庫可以在本地端直接建立 基本具有所有 Git 功能

以下是建立與加入檔案:
1. 在欲建立儲存庫的資料夾 git 初始化(`init`)
   > git init
2. 對要存儲的檔案 `add`
   > git add README.md
3. 提交(`commit`)進行保存並紀錄本次提交訊息(`Initial commit`)
   > git commit -m "Initial commit"

## Git 裸存儲庫

Git 裸存儲庫是只保存版本紀錄 不會保留其工作目錄內的檔案</br>
一般是作為共享的遠端存儲庫

以下是建立與上下傳:
1. 在欲建立裸儲存庫的資料夾 git 初始化(`init --bare`)
   > git init --bare
2. 在欲下載的資料夾內設置(`remote add`) git 遠程儲存庫地址(`origin`)
   > git remote add origin /path/to/my_bare_repo
3. 從遠程儲存庫(`origin`) 下載到 本地分支(`master`)
   > git push origin master
4. commit 完成後 將本地分支(`master`) 上傳到 遠程儲存庫(`origin`)
   > git pull origin master