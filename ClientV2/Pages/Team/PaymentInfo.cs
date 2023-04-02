using System.ComponentModel.DataAnnotations;

namespace ClientV2.Pages.Team;

public class PaymentInfo
{
    [Display(Name = "QR")] public IFormFile? FormFile { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    public string Link { get; set; }

    public string Description { get; set; }
}