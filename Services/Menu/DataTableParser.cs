using System.Data;
using Contracts;
using Domain.Exceptions;
using Domain.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Services.Menu;

public class DataTableParser : IDataTableParser
{
    private ImportReport Report { get; } = new();

    public async Task<ImportReport> ImportMenuAsync(Guid kitchenId, IFormFile file)
    {
        if (!ValidateFile(file, out var reader))
            return Report;
        
        var table = reader.AsDataSet().Tables[0];

        Report.Menu = new Domain.Models.Menu(kitchenId);
        
        await ReadTable(table, Report.Menu);
        
        return Report;
    }

    private async Task ReadTable(DataTable table, Domain.Models.Menu menu)
    {
        var mappedTableColumns = await ResolveColumns(table);
        var parseResults = new List<ParseResult>();

        for (var i = mappedTableColumns.TitleRowIndex + 1; i <= table.Rows.Count - 1; i++)
        {
            if (TryParseRow(table.Rows[i], i, mappedTableColumns, out var parsedResult))
            {
                if (parsedResult.Type == RowType.Group)
                {
                    parseResults.Add(parsedResult);
                    var dishType = new DishType(parsedResult.Name);
                    var j = i + 1;

                    while (j <= table.Rows.Count - 2)
                    {
                        var isSuccessParse = TryParseRow(table.Rows[j], j, mappedTableColumns,
                            out var parsedResultInternal);

                        if (!isSuccessParse)
                        {
                            j++;
                            continue;
                        }

                        if (parsedResultInternal.Type != RowType.Dish)
                        {
                            break;
                        }
                        
                        decimal.TryParse(parsedResultInternal.Price, out var dishPrice);
                        menu.AddDish(
                            parsedResultInternal.Name,
                            dishPrice,
                            dishType);
                        parseResults.Add(parsedResultInternal);

                        j++;
                    }

                    i = j - 1;
                }

                if (parsedResult.Type == RowType.Dish)
                {
                    menu.AddDish(parsedResult.Name, decimal.Parse(parsedResult.Price ?? "0"));
                    parseResults.Add(parsedResult);
                }

                if (parsedResult.Type == RowType.LunchSet)
                {
                    var lunchSetDishes = new List<Dish>();
                    var j = i + 1;

                    while (j <= table.Rows.Count - 2)
                    {
                        var isSuccessParse = TryParseRow(table.Rows[j], j, mappedTableColumns,
                            out var parsedResultInternal);

                        if (!isSuccessParse)
                        {
                            j++;
                            continue;
                        }

                        if (parsedResultInternal.Type != RowType.Dish)
                        {
                            break;
                        }

                        var parsedDish = parseResults.FirstOrDefault(x => x.VendorCode == parsedResultInternal.VendorCode);
                        if (parsedDish != null)
                        {
                            var dishEntity = menu.Dishes.FirstOrDefault(x => x.Name.Contains(parsedDish.Name));
                            if (dishEntity != null)
                                lunchSetDishes.Add(dishEntity);
                        }
                        
                        j++;
                    }

                    menu.AddLunchSet(lunchSetDishes, decimal.Parse(parsedResult.Price ?? "0"), parsedResult.Name);
                    i = j - 1;
                }
            }
        }
    }

    private bool TryParseRow(DataRow row, int rowNumber, TableColumnsMap mappedTableColumns, out ParseResult parsedResult)
    {
        parsedResult = new ParseResult();
        var badRow = new ImportReport.BadRow { Number = rowNumber + 1 };

        #region Поиск типа позиции в записи

        var positionTypeColumnIndex = mappedTableColumns[TableColumnsMap.ColumnTypes.PositionType];
                
        if (positionTypeColumnIndex.IsMapped)
        {
            if (TryGetValueFromCellsRange(row, positionTypeColumnIndex, out var typeValue))
            {
                if (MenuPositionTypeMap.AllowedTokens[MenuPositionTypeMap.TokenName.Dish].Contains(typeValue))
                    parsedResult.Type = RowType.Dish;
                            
                if (MenuPositionTypeMap.AllowedTokens[MenuPositionTypeMap.TokenName.Group].Contains(typeValue))
                    parsedResult.Type = RowType.Group;
                
                if (MenuPositionTypeMap.AllowedTokens[MenuPositionTypeMap.TokenName.LunchSet].Contains(typeValue))
                    parsedResult.Type = RowType.LunchSet;
                
                if (MenuPositionTypeMap.AllowedTokens[MenuPositionTypeMap.TokenName.Option].Contains(typeValue))
                    parsedResult.Type = RowType.Option;
            }
            
        }
        else
        {
            badRow.Errors.Add("не задан тип позиции в строке");
            Report.AddBadRowError(badRow);
            return false;
        }

        #endregion

        #region Поиск названия позиции в записи

        var nameColumnIndex = mappedTableColumns[TableColumnsMap.ColumnTypes.Name];
        if (nameColumnIndex.IsMapped)
        {
            if (TryGetValueFromCellsRange(row, nameColumnIndex, out var nameValue))
                if (nameValue != null)
                    parsedResult.Name = nameValue;
        }
        else
        {
            badRow.Errors.Add("не задано название позиции в строке");
            Report.AddBadRowError(badRow);
        }

        #endregion

        #region Поиск цены позиции в записи

        var priceColumnIndex = mappedTableColumns[TableColumnsMap.ColumnTypes.Price];
        if (priceColumnIndex.IsMapped)
        {
            if (TryGetValueFromCellsRange(row, priceColumnIndex, out var priceValue))
                    parsedResult.Price = priceValue;
        }
        else
        {
            badRow.Errors.Add($"не задана цена позиции в строке");
            Report.AddBadRowError(badRow);
        }

        #endregion

        #region Поиск единиц измерения позиции в записи 

        var unitColumnIndex = mappedTableColumns[TableColumnsMap.ColumnTypes.Units];
        if (unitColumnIndex.IsMapped)
        {
            if (TryGetValueFromCellsRange(row, unitColumnIndex, out var unitValue))
                if (unitValue != null && MenuUnitsMap.AllowedTokens.Contains(unitValue))
                    parsedResult.Units = unitValue;
        }
        else
        {
            badRow.Errors.Add("не задана единица измерения в строке");
            Report.AddBadRowError(badRow);
        }

        #endregion

        #region Поиск артикула позиции в записи

        var vendorCodeColumnIndex = mappedTableColumns[TableColumnsMap.ColumnTypes.VendorCode];
        if (vendorCodeColumnIndex.IsMapped)
        {
            if (TryGetValueFromCellsRange(row, vendorCodeColumnIndex, out var articleValue))
                parsedResult.VendorCode = articleValue;
        }
        else
        {
            badRow.Errors.Add("не задан артикул позиции в строке");
            Report.AddBadRowError(badRow);
        }

        #endregion
        
        return true;
    }

    private bool TryGetValueFromCellsRange(DataRow row, TableColumnsMap.ColumnTypeIndex columnTypeIndex, out string? result)
    {
        result = null;
        for (var i = columnTypeIndex.StartIndex; i <= columnTypeIndex.EndIndex; i++)
        {
            var valueIsNotNull = !row.IsNull(i);
            if (valueIsNotNull)
            {
                result = Convert.ToString(row[i]);
                return valueIsNotNull;
            }
        }
        return false;
    }

    private Task<TableColumnsMap> ResolveColumns(DataTable table)
    {
        return Task.FromResult(new TableColumnsMap(table));
    }
    
    private bool ValidateFile(IFormFile file, out IExcelDataReader? reader)
    {
        reader = null;
        
        if (file is null)
        {
            Report.ErrorsWhileImport = "Пустой файл";
            return false;
        }

        if (file.Length > 10971520)
        {
            Report.ErrorsWhileImport = "Файл слишком большого размера";
            return false;
        }

        var fileStream = file.OpenReadStream();
        if (!fileStream.CanRead)
        {
            Report.ErrorsWhileImport = "Файл не может быть прочитан";
            return false;
        }
        
        if (file.FileName.EndsWith(".xls"))
            reader = ExcelReaderFactory.CreateBinaryReader(fileStream);
        else if (file.FileName.EndsWith(".xlsx"))
            reader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        else
        {
            Report.ErrorsWhileImport = "Этот формат файлов не поддерживается";
            return false;
        }

        reader = new MergeCellsReader(reader);
        
        return true;
    }

    class ParseResult
    {
        public RowType Type { get; set; }
        public string Name { get; set; } = null!;
        public string? VendorCode { get; set; }
        public string? Price { get; set; }
        public string? Units { get; set; }
    }

    private enum RowType
    {
        Dish,
        Group,
        LunchSet,
        Option
    }
}

internal static class MenuPositionTypeMap
{
    public static readonly Dictionary<TokenName, HashSet<string>> AllowedTokens = new()
    {
        { TokenName.Group, new HashSet<string>() { "Группа" } },
        { TokenName.Dish, new HashSet<string>() { "Товар", "Блюдо" } },
        { TokenName.LunchSet, new HashSet<string>() { "Комбо-набор", "Ланч" } },
        { TokenName.Option, new HashSet<string>() { "Опция" } }
    };

    internal enum TokenName
    {
        Group,
        Dish,
        LunchSet,
        Option
    }
}

internal static class MenuUnitsMap
{
    public static readonly HashSet<string> AllowedTokens =
        new () { "Ед", "Шт", "Ед.", "Шт.", "Порц", "Порц.", "Порция", "Единица товара", "Штука" };
}

internal class TableColumnsMap
{
    private readonly Dictionary<ColumnTypes, ColumnTypeIndex> _map = new();    
    
    public ColumnTypeIndex this[ColumnTypes columnTypes] => 
        _map.GetValueOrDefault(columnTypes, new ColumnTypeIndex(false, -1, -1));

    public int TitleRowIndex { get; set; } = -1;
    
    private static readonly Dictionary<ColumnTypes, HashSet<string>> ColumnsForSearch = new ()
    {
        { ColumnTypes.Name, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Название", "Наименование" } },
        { ColumnTypes.VendorCode, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Артикул", "Номер" } },
        { ColumnTypes.Price, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Цена", "Стоимость" } },
        { ColumnTypes.PositionType, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Тип" } },
        { ColumnTypes.Units, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Единицы измерения", "Ед. измерения" } }
    };

    public TableColumnsMap(DataTable table)
    {
        const int searchDepth = 4;
        for (var i = 0; i <= searchDepth; i++)
        {
            foreach (var column in ColumnsForSearch)
            {
                var startIndex = -1;
                var endIndex = -1;
                for (var j = 0; j <= table.Columns.Count - 1; j++)
                {
                    var value = table.Rows[i][j] as string;
                    if (value is null) continue;
                    
                    if (column.Value.Contains(value) || column.Value.Any(x => value.Contains(x)))
                    {
                        if (startIndex == -1)
                        {
                            startIndex = j;
                            endIndex = j;
                        }
                        else
                        {
                            endIndex = j;
                        }
                    }
                    else
                    {
                        if (startIndex != -1)
                            break;
                    }
                }
                if (startIndex != -1 && endIndex != -1)
                    _map.Add(column.Key, new ColumnTypeIndex(true, startIndex, endIndex));
            }

            if (_map.Any())
            {
                TitleRowIndex = i;
                break;
            }
        }
        
        foreach (var column in ColumnsForSearch)
        {
            if (!_map.ContainsKey(column.Key))
            {
                _map.Add(column.Key, new ColumnTypeIndex(false, -1, -1));
            }
        }
        
        if (!this[ColumnTypes.PositionType].IsMapped && !this[ColumnTypes.Name].IsMapped)
            throw new DomainException("Не удалось прочитать таблицу: столбцы \"Тип\" или \"Название\" не найдены");
    }

    internal class ColumnTypeIndex
    {
        public bool IsMapped { get; }
        public int StartIndex { get; }
        public int EndIndex { get;}

        public ColumnTypeIndex(bool isMapped, int startIndex, int endIndex)
        {
            IsMapped = isMapped;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }

    internal enum ColumnTypes
    {
        Name,
        VendorCode,
        Price,
        PositionType,
        Units
    }
}