using System.Data;
using Contracts;
using Contracts.Repositories;
using Domain.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Services;

public class MenuImportService : IMenuImportService
{
    private readonly IRepositoryManager _repository;

    private ImportReport Report { get; } = new();

    public async Task<ImportReport> ImportMenuAsync(Guid kitchenId, IFormFile file)
    {
        if (!ValidateFile(file, out var reader))
            return Report;

        var table = reader.AsDataSet().Tables[0];
        var menu = new Menu(kitchenId);
        
        await ReadTable(table, menu);
        
        _repository.Menu.CreateMenu(menu);
        await _repository.SaveAsync();
        return Report;
    }

    private async Task ReadTable(DataTable table, Menu menu)
    {
        var mappedTableColumns = await ResolveColumns(table);

        for (var i = 1; i < table.Rows.Count; i++)
        {
            if (TryParseRow(table.Rows[i], i, mappedTableColumns, out var parsedResult))
            {
                if (parsedResult.Type == RowType.Group)
                {
                    var dishType = new DishType(parsedResult.Name);
                    var j = i;

                    while (true)
                    {
                        j++;
                        
                        var isSuccessParse = TryParseRow(table.Rows[j], j, mappedTableColumns,
                            out var parsedResultInternal);

                        if (!isSuccessParse)
                        {
                            j++;
                            break;
                        }

                        if (parsedResultInternal.Type != RowType.Dish)
                        {
                            j++;
                            break;
                        }
                        
                        decimal.TryParse(parsedResultInternal.Price, out var dishPrice);
                        menu.AddDish(
                            parsedResultInternal.Name,
                            dishPrice,
                            dishType);
                    }

                    i = j - 1;
                }

                if (parsedResult.Type == RowType.Dish)
                {
                    menu.AddDish(parsedResult.Name, decimal.Parse(parsedResult.Price ?? "0"));
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
                
        if (positionTypeColumnIndex.IsMapped && !row.IsNull(positionTypeColumnIndex.Index))
        {
            var typeValue = Convert.ToString(row[positionTypeColumnIndex.Index]);
            
            if (MenuPositionTypeMap.AllowedTokens[MenuPositionTypeMap.TokenName.Dish].Contains(typeValue))
                parsedResult.Type = RowType.Dish;
            
            if (MenuPositionTypeMap.AllowedTokens[MenuPositionTypeMap.TokenName.Group].Contains(typeValue))
                parsedResult.Type = RowType.Group;
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
        if (nameColumnIndex.IsMapped && !row.IsNull(nameColumnIndex.Index))
        {
            var nameValue = Convert.ToString(row[nameColumnIndex.Index]);
            parsedResult.Name = nameValue;
        }
        else
        {
            badRow.Errors.Add("не задано название позиции в строке");
            Report.AddBadRowError(badRow);
            return false;
        }

        #endregion

        #region Поиск цены позиции в записи

        var priceColumnIndex = mappedTableColumns[TableColumnsMap.ColumnTypes.Price];
        if (priceColumnIndex.IsMapped && !row.IsNull(priceColumnIndex.Index))
        {
            var priceValue = Convert.ToString(row[priceColumnIndex.Index]);
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
        if (unitColumnIndex.IsMapped && !row.IsNull(unitColumnIndex.Index))
        {
            var unitValue = Convert.ToString(row[unitColumnIndex.Index]);
            if (MenuUnitsMap.AllowedTokens.Contains(unitValue))
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
        if (vendorCodeColumnIndex.IsMapped && !row.IsNull(vendorCodeColumnIndex.Index))
        {
            var articleValue = Convert.ToString(row[vendorCodeColumnIndex.Index]);
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

        return true;
    }

    public MenuImportService(IRepositoryManager repository)
    {
        _repository = repository;
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
        { TokenName.Dish, new HashSet<string>() { "Товар", "Блюдо" } }
    };

    internal enum TokenName
    {
        Group,
        Dish
    }
}

internal static class MenuUnitsMap
{
    public static readonly HashSet<string> AllowedTokens =
        new () { "Ед", "Шт", "Ед.", "Шт.", "Порц", "Порц.", "Порция", "Единица товара", "Штука" };
    
    internal enum TokenName
    {
        Unit
    }
}

internal class TableColumnsMap
{
    private readonly Dictionary<ColumnTypes, ColumnTypeIndex> _map = new();    
    
    public ColumnTypeIndex this[ColumnTypes columnTypes] => 
        _map.GetValueOrDefault(columnTypes, new ColumnTypeIndex(false, -1));
    
    private static readonly Dictionary<ColumnTypes, HashSet<string>> ColumnsForSearch = new ()
    {
        { ColumnTypes.Name, new HashSet<string> { "Название", "Наименование" } },
        { ColumnTypes.VendorCode, new HashSet<string> { "Артикул", "Номер" } },
        { ColumnTypes.Price, new HashSet<string> { "Цена", "Стоимость" } },
        { ColumnTypes.PositionType, new HashSet<string> { "Тип" } },
        { ColumnTypes.Units, new HashSet<string> { "Единицы измерения", "Ед. измерения" } }
    };

    public TableColumnsMap(DataTable table)
    {
        foreach (var column in ColumnsForSearch)
        {
            foreach (var columnName in column.Value)
            {
                var index = table.Columns.IndexOf(columnName);
                if (index != -1)
                {
                    _map.Add(column.Key, new ColumnTypeIndex(true, index));
                    break;
                }
            }

            if (!_map.ContainsKey(column.Key))
            {
                _map.Add(column.Key, new ColumnTypeIndex(false, -1));
            }
        }
    }

    internal class ColumnTypeIndex
    {
        public bool IsMapped { get; }
        public int Index { get; }

        public ColumnTypeIndex(bool isMapped, int index)
        {
            IsMapped = isMapped;
            Index = index;
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