using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

public class SurplusModel : PageModel
{
    private const decimal SurplusDiscount = 0.30m;
    private const int BoxKg = 5;
    private const int ShelfLifeDays = 7;
    private const int LowStockKg = 50;


    public SurplusModel Surplus { get; } = new()
    {
        
    }
}