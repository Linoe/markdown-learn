using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace notes
{
    #region ..ctor/..cctor 建構式/初始化
    // ..ctor 當存在建構式時會被編譯器產生
    // ..cctor 當存在靜態數值時會被編譯器產生，用來初始化靜態變數
    //https://www.cnblogs.com/mouhong-lin/archive/2008/05/18/1201747.html
    public class cctor
    {
        public static string sa = "A";
        public cctor() {; }
    }
    #endregion
}