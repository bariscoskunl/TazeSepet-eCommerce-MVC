using App.Data;
using eTicaretUygulamasi.Mvc.App.Data;
using eTicaretUygulamasi.Mvc.App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eTicaretUygulamasi.Mvc.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly IDataRepository _repo;

        public CategoryListViewComponent(IDataRepository repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _repo.GetAll<CategoryEntity>();
            return View(categories);
        }
    }
}
