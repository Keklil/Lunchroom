﻿using System.Text.Json;
using NSwag.Examples;
using Shared.DataTransferObjects.Menu;

namespace LunchRoom.Controllers.Infrastructure.Examples;

public class UploadMenuExamples : IExampleProvider<RawMenuDto>
{
    public RawMenuDto GetExample()
    {
        var menu = new RawMenuDto
        {
            GroupId = Guid.Parse("2b974f1e-618d-4aef-962e-713d1db8d2c6"),
            Menu = new List<string>()
        };

        var menuString = JsonSerializer.Deserialize<List<string>>(
            """["Обед за 220₽🤩", "📌 Салат «Пестрый крабик» (болгарский перец, яйцо, огурцы свежие, морковь, рис, кукуруза, крабовое мясо, майонез)", "📌 Суп «Борщ наваристый с курицей и сметаной»", "📌Шницель мясной с гречей в томатном соусе", "", "Обед за 240₽☺️", "📌 Салат «Пестрый крабик»", "📌 Суп «Борщ наваристый с курицей и сметаной»", "📌 Плов восточный с традиционными специями с курицей", "", "Обед за 260₽ 😍", "📌 Салат «Пестрый крабик»", "📌 Суп «Борщ наваристый с курочкой и сметаной»", "📌 Филе гриль в маринаде со спагетти", "", "📌 Замена салата на ", "«Цезарь» +10₽", "📌 Замена салата на «Русская красавица» +15₽ (томаты, огурец, яйцо, ветчина, курочка копчёная, горошек, майонез)", "", "📌Пицца «Ассорти» 65₽", "📌Пицца «Деревенская» 70₽", "📌Пирожок с картошкой 45₽", "📌Беляш с мясом 50₽", "🥤Морс 0,5 80₽", "🥤Компот из сухофруктов 80₽", "", "К каждому обеду прилагается хлеб, комплект одноразовых приборов, салфетки.🍴", "", "Приятного аппетита и хорошего Вам дня!☺️", "С ❤️Ваша Столовая «Самоварчик»"]""");

        menu.Menu.AddRange(menuString);

        return menu;
    }
}