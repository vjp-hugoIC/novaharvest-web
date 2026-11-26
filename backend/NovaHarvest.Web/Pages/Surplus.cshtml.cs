using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

public class SurplusModel : PageModel
{
    private const decimal SurplusDiscount = 0.30m;
    private const int BoxKg = 5;
    private const int ShelfLifeDays = 7;
    private const int LowStockKg = 50;


    public Surplus Surplus { get; } = new()
    {
      Product = "Cereza Picota",
      Producer = "Huerta Los Castaños",
      Village = "Cabezuela del Valle",
      HarvestDate = DateTime.Today.AddDays(-2),
      AvailableKg = 343,
      ReservedKg = 120,
      PricePerKg = 3.20m,
      Note = "<b>Recoger antes de las 14:00</b>"  
    };

    public DateTime BestBeforeDate { get; private set; }
    public int DaysLeft { get; private set; }
    public string? DaysCssClass { get; private set; }
    public decimal SurplusPrice { get; private set; }
    public int RemainingKg { get; private set; }
    public int FullBoxes { get; private set; }
    public int LooseKg { get; private set; }
    public decimal SurplusValue { get; private set; }
    public string StatusText { get; private set; } = "";
    public string ProducerNote { get; private set; } = "";
    public bool IsSoldOut { get; private set; }

    public void OnGet()
    {
        BestBeforeDate = Surplus.HarvestDate.AddDays(ShelfLifeDays);
        DaysLeft = (BestBeforeDate - DateTime.Today).Days;

        RemainingKg = Surplus.AvailableKg - Surplus.ReservedKg;
        IsSoldOut = RemainingKg <= 0;

        DaysCssClass = (DaysLeft <= 2 && RemainingKg > 0) ? "urgent" : null;

        SurplusPrice = Surplus.PricePerKg * (1 - SurplusDiscount);
        FullBoxes = RemainingKg / BoxKg;
        LooseKg = RemainingKg % BoxKg;
        SurplusValue = RemainingKg * SurplusPrice;

        StatusText = RemainingKg <= 0 ? "Agotado" : (RemainingKg < LowStockKg ? "¡Últimos kilos!" : "Disponible");
        ProducerNote = Surplus.Note ?? "Sin observaciones";
    }
}