using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyModel_CodeFirst.Models;
using System.Diagnostics;

namespace MyModel_CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GuestBookContext _context;

        public HomeController(ILogger<HomeController> logger, GuestBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //select top 10 * from Book where photo is null order by TimeStamp desc, GId desc
            var result = _context.Book.Where(b => b.Photo != null).OrderByDescending(b => b.TimeStamp).ThenByDescending(b => b.GId).Take(10);


            return View(await result.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}




//MyModel_CodeFirst專案進行步驟

//1. 使用Code First建立Model及資料庫

//1.1   在Models資料夾裡建立Book及ReBook兩個類別做為模型
//1.1.1 在Models資料夾上按右鍵→加入→類別，檔名取名為Book.cs，按下「新增」鈕
//1.1.2 設計Book類別的各屬性，包括名稱、資料類型及其相關的驗證規則及顯示名稱(Display)
//1.1.3 在Models資料夾上按右鍵→加入→類別，檔名取名為ReBook.cs，按下「新增」鈕
//1.1.4 設計ReBook類別的各屬性，包括名稱、資料類型及其相關的驗證規則及顯示名稱(Display)
//1.1.5 撰寫兩個類別間的關聯屬性做為未來資料表之間的關聯


//1.2   建立DbContext類別
//      ※安裝下列兩個套件※
//      (1)Microsoft.EntityFrameworkCore.SqlServer
//      (2)Microsoft.EntityFrameworkCore.Tools
//      ※與DB First安裝的套件一樣※
//1.2.1 在Models資料夾上按右鍵→加入→類別，檔名取名為GuestBookContext.cs，按下「新增」鈕
//1.2.2 撰寫GuestBookContext類別的內容
//      (1)須繼承DbContext類別
//      (2)撰寫建構子
//      (3)描述資料庫裡面的資料表
//1.2.3 在appsettings.json中撰寫資料庫連線字串
//1.2.4 在Program.cs內以依賴注入的寫法撰寫讀取連線字串的物件
//      ※注意程式的位置必須要在var builder = WebApplication.CreateBuilder(args);這句之後
//1.2.5 在套件管理器主控台(檢視 > 其他視窗 > 套件管理器主控台)下指令
//      (1)Add-Migration InitialCreate
//      (2)Update-database
//      ※第(1)項的「InitialCreate﹞是自訂的名稱，若執行成功會看到「Build succeeded.」※
//      ※另外會看到一個Migrations的資料夾及其檔案被建立在專案中，裡面紀錄著Migration的歷程※
//      ※若第(1)項指令執行成功才能執行第(2)項指令※
//      (3)至SSMS中查看是否有成功建立資料庫及資料表(目前資料表內沒有資料)



//1.3   創建Initializer物件建立初始(種子)資料(Seed Data)
//      ※※※我們可以在創建資料庫時就創建幾筆初始的資料在裡面以供開發時測試之用※※※
//      ※※※請先將資料庫刪除，並將專案中Migrations資料夾及內含檔案整個刪除※※※

//1.3.1 在Models資料夾上按右鍵→加入→類別，檔名取名為SeedData.cs，按下「新增」鈕
//1.3.2 撰寫SeedData類別的內容
//      (1)撰寫靜態方法 Initialize(IServiceProvider serviceProvider)
//      (2)撰寫Book及ReBook資料表內的初始資料程式
//      (3)撰寫getFileBytes，功能為將照片轉成二進位資料
//1.3.3 在Program.cs撰寫啟用Initializer的程式(要寫在var app = builder.Build();之後)
//      ※這個Initializer的作用是建立一些初始資料在資料庫中以利測試，所以不一定要有Initializer※
//      ※注意:初始資料的照片放在SeedSourcePhoto資料夾中※
//1.3.4 建置專案，確定專案完全建置成功
//1.3.5 再次於套件管理器主控台(檢視 > 其他視窗 > 套件管理器主控台)下指令
//      (1)Add-Migration InitialCreate
//      (2)Update-database
//1.3.4 至SSMS中查看是否有成功建立資料庫及資料表(目前資料表內沒有資料)
//1.3.5 在瀏覽器上執行網站首頁以建立初始資料(若沒有執行過網站，初始資料不會被建立)
//1.3.6 再次至SSMS中查看資料表內是否有資料


//2. 建立留言板後台管理功能

//2.1   製作自動生成的Book資料表CRUD
//2.1.1 在Controllers資料夾上按右鍵→加入→控制器
//2.1.2 選擇「使用EntityFramework執行檢視的MVC控制器」→按下「加入」鈕
//2.1.3 在對話方塊中設定如下
//      模型類別: Book(MyModel_CodeFirst.Models)
//      資料內容類別: GuestBookContext(MyModel_CodeFirst.Models)
//      勾選 產生檢視
//      勾選 參考指令碼程式庫
//      勾選 使用版面配置頁
//      控制器名稱使用預設即可(BooksController)
//      按下「新增」鈕
//2.1.4 執行/Books/Index 進行測試
//2.1.5 修改Index View將Photo及ImageType欄位、Create、Edit及Details超鏈結移除
//2.1.6 依喜好自行修改介面


//2.2   調整BooksController內容 
//2.2.1 改寫Index Action的內容，將留言依新到舊排序
//2.2.2 移除Details Action (亦可一併刪除 Details.cshtml)
//2.2.3 移除Create Action (亦可一併刪除 Create.cshtml)
//2.2.4 移除Edit Action (亦可一併刪除 Edit.cshtml)


//2.3   修改Delete View的排版方式
//2.3.1 照自己喜好修改排版方式(這裡我們使用Bootstrap Card元件)
//2.3.2 在BooksController內增加讀取照片的方法
//2.3.3 在Delete View加入取得照片的HTML
//2.3.4 測試


//2.4   使用「ViewComponent」技巧實作「將回覆留言內容顯示於Delete View」
//   ※此單元將要介紹ViewComponent的使用方式※
//2.4.1 在專案中新增ViewComponents資料夾(專案上按右鍵→加入→新增資料夾)以放置所有的ViewComponent元件檔
//2.4.2 在ViewComponents資料夾中建立VCReBooks ViewComponent(右鍵→加入→類別→輸入檔名→新增)
//2.4.3 VCReBooks class繼承ViewComponent(注意using Microsoft.AspNetCore.Mvc;)
//2.4.4 撰寫InvokeAsync()方法取得回覆留言資料
//2.4.5 在/Views/Shared裡建立Components資料夾，並在Components資料夾中建立VCReBooks資料夾
//2.4.6 在/Views/Shared/Components/VCReBooks裡建立檢視(右鍵→加入→檢視→選擇「Razor檢視」→按下「加入」鈕)
//2.4.7 在對話方塊中設定如下
//      檢視名稱: Default
//      範本:Empty(沒有模型)
//      不勾選 建立成局部檢視
//      不勾選 使用版面配置頁
//   ※注意：資料夾及View的名稱不是自訂的，而是有預設的名稱，規定如下：※
//   /Views/Shared/Components/{ComponentName}/Default.cshtml
//   /Views/{ControllerName}/Components/{ComponentName}/Default.cshtml
//2.4.8 在Default View上方加入@model IEnumerable<MyModel_CodeFirst.Models.ReBook>
//2.4.9 依喜好編輯Default View排版方式
//2.4.10 編寫Delete View，加入VCReBooks ViewComponent
//2.4.11 測試



//2.5   製作留言刪除功能
//2.5.1 在Delete View中的刪除鈕上加入確認對話方塊
//   ※注意！兩個資料表的關聯是連動的，主留言被刪除後，會一併刪除所有回覆它的留言，以符合參考完整性※
//2.5.2 在BooksController內增加刪除回覆留言Action
//2.5.3 在在VCReBook ViewComponent的View中(Default.cshtml)建立每則回覆留言的刪除鈕
//2.5.4 測試


//2.7   Layout的處理
//2.7.1 選單的改變
//2.7.2 主畫面(首頁)的配置
//2.7.3 在Shared資料夾中建立一個名為_Layout2.cshtml的 View做為前台使用的Layout，Layout風格自訂
//2.7.4 將_ViewStart.cshtml裡預設的Layout改為Layout2
//      ※這樣一來所有的View將會套用_Layout2.cshtml※
//      ※補充：_ViewStart.cshtml為指定預設Layout用的檔案※



//3.  製作留言板前台

//3.1   製作自動生成的Book資料表CRUD
//3.1.1 在Controllers資料夾上按右鍵→加入→控制器
//3.1.2 選擇「使用EntityFramework執行檢視的MVC控制器」→按下「加入」鈕
//3.1.3 在對話方塊中設定如下
//      模型類別: Book(MyModel_CodeFirst.Models)
//      資料內容類別: GuestBookContext(MyModel_CodeFirst.Models)
//      勾選 產生檢視
//      勾選 參考指令碼程式庫
//      勾選 使用版面配置頁
//      控制器名稱改為PostBooksController
//      按下「新增」鈕
//3.1.4 修改PostBooksController，移除Edit、Delete Action
//3.1.5 刪除Edit、Delete View檔案
//3.1.6 修改Index Action的寫法


//3.2   顯示功能
//3.2.1 修改適合前台呈現的Index View
//3.2.2 將PostBooksController中Details Action改名為Display(View也要改名字)
//3.2.3 修改Index View中Details的超鏈結為Display
//3.2.4 修改Display View 排版樣式，排版可以個人喜好呈現
//      ※這裡每一筆回覆留言上都會有刪除的功能鈕，這個情況並不合理，在後面會處理※
//      ※排版可以個人喜好呈現※


//3.3   留言功能
//3.3.1 修改Create View，修改上傳檔案的元件
//3.3.2 修改Create View，將<form>增加 enctype="multipart/form-data" 屬性
//3.3.3 加入前端效果，使照片可先預覽
//3.3.4 刪除ImageType欄位
//3.3.5 刪除TimeStamp欄位
//3.3.6 修改Post Create Action，加上處理上傳照片的功能
//3.3.7 測試留言功能
//3.3.8 在Index View中加入未上傳照片的留言之顯示方式
//3.3.9 在Display View中加入未上傳照片的留言之顯示方式
//3.3.10 在Index View中加入處理「有換行的留言」顯示方式
//3.3.11 在Display View中加入處理「有換行的留言」顯示方式
//3.3.12 將後台介面Index與Delete View 也補上有換行的留言顯示方式及未上傳照片的留言之顯示方式



//3.4   基本的錯誤處理(Error Handle)
//3.4.1 測試:故意讓程式發生例外看看結果
//3.4.2 在PostBooksController裡寫一個會發生例外的Action如下
//public IActionResult ExceptionTest()
//{
//    int a = 0;
//    int s = 100 / a;
//    return View();
//}
//3.4.3 利用原本預設的Home Controller Error Handler來處理例外
//3.4.4 修改 Program.cs中的程式碼，將是否在開發模式下的錯誤處理判斷註解掉
//3.4.5 測試:再次故意讓程式發生例外看看結果
//3.4.6 修改Error.cshtml內容
//3.4.7 測試:故意讓程式發生HttpNotFound(404)錯誤
//3.4.8 在Program.cs中的程式碼註冊處理HttpNotFound(404)錯誤的Error Handler
//3.4.7 測試:再次故意讓程式發生HttpNotFound(404)錯誤
//      ※最基本的錯誤處理，就是不讓使用者看到系統錯誤訊息※



//3.5   回覆留言功能
//3.5.1 在Controllers資料夾上按右鍵→加入→控制器
//3.5.2 選擇「使用EntityFramework執行檢視的MVC控制器」→按下「加入」鈕
//3.5.3 在對話方塊中設定如下
//      模型類別: ReBook(MyModel_CodeFirst.Models)
//      資料內容類別: GuestBookContext(MyModel_CodeFirst.Models)
//      勾選 產生檢視
//      勾選 參考指令碼程式庫
//      不勾選 使用版面配置頁
//      控制器名稱改為RePostBooksController
//      按下「新增」鈕
//3.5.4 修改RePostBooksController，僅保留Create Action，其它全部刪除
//3.5.5 僅保留Create View檔案，其它全部刪除
//3.5.6 修改 Create View
//      ※製作前後端分離的回覆留言功能※
//3.5.7 在PostBooks\Display View中將RePostBooks\Create View以Ajax方式讀入
//3.5.8 配合Boostrap Modal Component顯示出畫面
//3.5.9 修改RePostBooksController中的Create Action，使其Return JSON資料
//3.5.10 將RePostBooks\Create View中<form>加上id
//3.5.11 在PostBooks\Display View中撰寫相關的JavaScript程式，以Ajax方式執行新增回覆留言
//3.5.12 測試效果


//3.6    新增回覆留言後前端的處理
//3.6.1  將 Modal內的表單清空並結束Modal顯示
//3.6.2  重新讀取回覆留言的ViewComComponent,使其能顯示最新的回覆留言
//3.6.3  在RePostBooksController撰寫GetRebookByViewComponent Action
//3.6.4  以Ajax呼叫GetRebookByViewComComponent Action
//3.6.5  測試效果


//3.7    Bootstrap應用
//       利用Bootstrap裡的功能作首頁


//4.  簡易登入功能製作

//4.1   在資料庫新增一個Login資料表存放管理者帳號密碼
//4.1.1 在Models資料夾裡建立Login類別做為模型
//4.1.2 Models資料夾上按右鍵→加入→類別，檔名取名為Login.cs，按下「新增」鈕
//4.1.3 設計Login類別的各屬性，包括名稱、資料類型及其相關的驗證規則及顯示名稱(DisplayName)
//4.1.4 修改GuestBookContext類別的內容，加入描述資料庫裡Login的資料表
//4.1.5 在套件管理器主控台(檢視 > 其他視窗 > 套件管理器主控台)下指令
//      (1)Add-Migration AddLoginTable
//      (2)Update-database
//4.1.6 至SSMS中查看是否有成功建立Login資料表(目前資料表內沒有資料)
//4.1.7 在Login資料表中建立一筆帳號密碼的資料(admin, 12345678)



//4.2   製作Login功能與畫面
//4.2.1 在Controllers資料夾上按右鍵→加入→控制器
//4.2.2 選擇「MVC控制器-空白」→按下「加入」鈕
//4.2.3 檔名取名為「LoginController」→按下「新增」鈕
//4.2.4 建立Get與Post的Login Action
//4.2.5 建立Login View(Login Action中按右鍵→新增檢視→Razor檢視→按下「加入」鈕)
//      在對話方塊中設定如下
//      檢視稱: Login (使用預設名稱)
//      範本: Create
//      模型類別: Login(MyModel_CodeFirst.Models)
//      資料內容類別: GuestBookContext(MyModel_CodeFirst.Models)
//      不勾選 建立成局部檢視
//      勾選 參考指令碼程式庫
//      勾選 使用版面配置頁
//4.2.6 將ViewData["Error"]加入Login View
//4.2.7 在Program.cs中註冊及啟用Session
//4.2.8 在VCReBooks View Component中加入未登入看不到刪除鈕的判斷式
//4.2.9 測試


//4.3   建立進入後台必須是登入狀態實作
//4.3.1 在_Layout2.cshtml中加入登入後台的按鈕
//4.3.2 將/Views/Books裡所有View的Layout指定為_Layout.cshtml(後台用的Layout)
//4.3.3 在_Layout.cshtml加入未登入則將網頁自動導往/Home/Index的判斷式



//4.4   登出功能與Layout編輯
//4.4.1 在Login Controller加入Logout Action
//4.4.2 在_Layout.cshtml加入登出後台的按鈕
//      ※前後台的Layout可依照自己的喜好做呈現※