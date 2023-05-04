using System.Text.RegularExpressions;
using Contracts;
using Shared.DataTransferObjects.Menu;

namespace Services.Menu;

[Obsolete]
public class PlainTextParser : IPlainTextParser
{
    private readonly Regex containPrice = new(@"\w*[0-9]{0,3}₽");
    private readonly Regex matchLostString = new(@"^[а-я«».,-].+");
    private readonly Regex matchSentence = new(@"[А-Яа-я«»\.,-]{1}.*");
    private readonly Regex newLineMoreThenTwice = new(@"\n{2,}");
    private readonly Regex onlyText = new(@"[^A-Za-zА-ЯЁа-яё0-9\s\,«»()""-]");

    /// <summary>
    ///     Split the string from the body of the mail into
    ///     a list of strings, combine sentences if they are
    ///     separated.
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    public List<string> NormalizeMenu(string menu)
    {
        var tempString = menu.Replace("\r", "");

        tempString = tempString.Remove(tempString.LastIndexOf("₽") + 1);

        //Looking for multiple char '\n' in string, replace to one. 
        tempString = newLineMoreThenTwice.Replace(tempString, "\n");

        //Convert string to list, separating by '\n'. 
        var rawList = tempString.Split("\n").ToList();

        //Removing extra symbols
        for (var i = 0; i < rawList.Count; i++) rawList[i] = matchSentence.Match(rawList[i]).Value;

        var normalizedMenu = new List<string>();

        //Removing strings that dont any useful sense
        for (var i = 0; i < rawList.Count; i++)
            if (rawList[i].Contains("Обед"))
            {
                normalizedMenu.Add(rawList[i]);
            }
            else
            {
                //Check if next line is part of previous one
                if (i + 1 < rawList.Count)
                    if (matchLostString.IsMatch(rawList[i + 1]))
                    {
                        var fixedString = rawList[i] + " " + rawList[i + 1];
                        normalizedMenu.Add(fixedString.Replace("  ", " "));
                        i++;
                        continue;
                    }

                normalizedMenu.Add(rawList[i]);
            }

        return normalizedMenu;
    }

    /// <summary>
    ///     Converts the menu from a list of strings
    ///     to a data transfer model.
    /// </summary>
    /// <param name="menuRaw"></param>
    /// <returns></returns>
    public MenuForCreationDto ConvertMenu(List<string> menuRaw)
    {
        var menu = new MenuForCreationDto
            { LunchSets = new List<LunchSetForCreationDto>(), Options = new List<OptionForCreationDto>() };

        for (var i = 0; i < menuRaw.Count; i++)
        {
            //Looking for lunch set
            if (containPrice.IsMatch(menuRaw[i]) && menuRaw[i].Contains("Обед"))
            {
                //Extract the price of the lunch set
                var price = decimal.Parse(containPrice.Match(menuRaw[i]).Value.Replace("₽", ""));
                var lunchSet = new LunchSetForCreationDto { Price = price, LunchSetList = new List<string>() };
                var j = i + 1;

                //Looking for lunch set units
                while (!containPrice.IsMatch(menuRaw[j]))
                    if (menuRaw[j].Length == 0 || menuRaw[j] is null)
                    {
                        j++;
                    }
                    else
                    {
                        //Extract the lunch set unit
                        var lunchSetUnit = onlyText.Replace(menuRaw[j], "");
                        lunchSet.LunchSetList.Add(lunchSetUnit.Trim());
                        j++;
                    }

                menu.LunchSets.Add(lunchSet);

                //Jump to next lunch set
                i = j - 1;
            }

            //Looking for options
            if (containPrice.IsMatch(menuRaw[i]))
            {
                //Extract the price
                var price = decimal.Parse(containPrice.Match(menuRaw[i]).Value.Replace("₽", ""));

                //Delete price from the name
                var name = containPrice.Replace(menuRaw[i], "");
                //Delete unnecessary symbols
                name = onlyText.Replace(name, "").Trim();

                var option = new OptionForCreationDto { Price = price, Name = name };
                menu.Options.Add(option);
            }
        }

        return menu;
    }
}