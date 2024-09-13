using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyModel_CodeFirst.Models;

namespace MyModel_CodeFirst.ViewComponents
{
    public class VCReBooks:ViewComponent
    {
        private readonly GuestBookContext _context;

        public VCReBooks(GuestBookContext context)
        {
            _context = context;
        }

        //2.4.4 撰寫InvokeAsync()方法取得回覆留言資料
        public async Task<IViewComponentResult> InvokeAsync(long gid)
        {
            var rebook=await _context.ReBook.Where(m=>m.GId==gid).OrderByDescending(m=>m.TimeStamp).ThenByDescending(m=>m.RId).ToListAsync();

            return View(rebook);
        }
    }
}
