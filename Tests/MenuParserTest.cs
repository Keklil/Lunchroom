using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using Services.MailService;
using Shared.DataTransferObjects.Menu;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class MenuPraserTest
{
    protected readonly ITestOutputHelper _output;

    [Fact]
    public void TestConverter()
    {
        var menuRaw = new List<string>
        {
            new("Доброе утро!☕️❤️"),
            new(""),
            new("Обед за 220₽"),
            new("📌 Салат «Крабик»"),
            new("📌 Суп «Щи зелёные со шпинатом, яйцом и курицей»"),
            new("📌 Котлета домашняя из свинины с гречкой в соусе"),
            new(""),
            new("Обед за 240₽☺️"),
            new("📌 Салат «Крабик»"),
            new("📌 Суп «Щи зелёные со шпинатом, яйцом и курицей»"),
            new("📌 Медальон из нежного куриного филе с макаронами "),
            new(""),
            new("Обед за 260₽🥩"),
            new("📌 Салат «Крабик»"),
            new("📌 Суп «Щи зелёные со шпинатом, яйцом и курицей»"),
            new("📌 Фунчоза с курицей и овощами в соевом соусе"),
            new(""),
            new("📌 Замена салата на «Цезарь» +10₽ "),
            new("📌 Замена салата на «Дипломат»+15₽"),
            new("📌Пицца «Ассорти» 65₽"),
            new("📌Пицца «Деревенская» 70₽"),
            new("📌Пирожок с картошкой 45₽"),
            new("📌Беляш с мясом 50₽"),
            new("🥤Морс 0,5 80₽"),
            new("🥤Компот из сухофруктов 80₽"),
            new(""),
            new("К каждому обеду прилагается хлеб.🍴"),
            new("Приятного аппетита и хорошего Вам дня!☺️"),
            new("С ❤️Ваша Столовая «Самоварчик»")
        };

        var menuPrepared = new MenuForCreationDto();
        var lunchSet = new LunchSetForCreationDto
        {
            Price = 220,
            LunchSetList = new List<string>
            {
                "Салат «Крабик»",
                "Суп «Щи зелёные со шпинатом, яйцом и курицей»",
                "Котлета домашняя из свинины с гречкой в соусе"
            }
        };

        var parser = new MailParser();

        var menu = parser.ConvertMenu(menuRaw);

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        var menuString = JsonSerializer.Serialize(menu, options);

        _output.WriteLine(menuString);
    }

    [Fact]
    public void TestNormalizer()
    {
        var menuRaw =
            "Доброе утро!☕️❤️\r\n\r\nОбед за 220₽\r\n📌 Салат «Свекла с сыром»\r\n📌 Суп «С домашней лапшой с курицей»\r\n📌 Котлета «Паровая» с макаронами\r\n\r\n\r\nОбед за 240₽☺️\r\n📌 Салат «Свекла с сыром»\r\n📌 Суп «С домашней лапшой с курицей»\r\n📌 Шашлычок из нежного куриного филе с гречей\r\n\r\nОбед за 260₽\U0001f969\r\n📌 Салат «Свекла с сыром»\r\n📌 Суп «С домашней лапшой с курицей»\r\n📌 Перец фаршированный с пюре на молоке и сливочном масле\r\n\r\n📌 Замена салата на\r\n«Цезарь» +10₽\r\n📌 Замена салата на «Полянка» +15₽\r\n📌Пицца «Ассорти» 65₽\r\n📌Пицца «Деревенская» 70₽\r\n📌Пирожок с картошкой 45₽\r\n📌Беляш с мясом 50₽\r\n\U0001f964Морс 0,5 80₽\r\n\U0001f964Компот из сухофруктов 80₽\r\n\r\nК каждому обеду прилагается хлеб, комплект одноразовых приборов, салфетки.🍴\r\n\r\nПриятного аппетита и хорошего Вам дня!☺️\r\nС ❤️Ваша Столовая «Самоварчик»\r\n";

        var normalizer = new MailParser();

        var normalizedMenu = normalizer.NormalizeMenu(menuRaw);

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        var menuString = JsonSerializer.Serialize(normalizedMenu, options);


        _output.WriteLine(menuString);
    }

    public MenuPraserTest(ITestOutputHelper testOutput)
    {
        _output = testOutput;
    }
}