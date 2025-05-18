using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shoppng_Tutorial.Repository.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly DataContext _datacontext;
        public BrandsViewComponent(DataContext datacontext)
        {
            _datacontext = datacontext;
        }
        public async Task<IViewComponentResult> InvokeAsync()=>View(await _datacontext.Brands.ToListAsync());
    }
}
