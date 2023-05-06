using System.Data;
using ExcelDataReader;

namespace Services.Menu
{
    public class MergeCellsReader : IExcelDataReader
    {
        public MergeCellsReader(IExcelDataReader baseReader)
        {
            BaseReader = baseReader ?? throw new ArgumentNullException(nameof(baseReader));
            MergeCellsTopLeft = new Dictionary<CellRange, Cell>();
        }

        public double GetColumnWidth(int i)
        {
            return BaseReader.GetColumnWidth(i);
        }

        public string Name => BaseReader.Name;

        public string CodeName => BaseReader.CodeName;

        public string VisibleState => BaseReader.VisibleState;

        public HeaderFooter HeaderFooter => BaseReader.HeaderFooter;

        public CellRange[] MergeCells => BaseReader.MergeCells;

        public int ResultsCount => BaseReader.ResultsCount;
        public int RowCount => BaseReader.RowCount;

        public double RowHeight => BaseReader.RowHeight;

        public int Depth => BaseReader.Depth;

        public bool IsClosed => BaseReader.IsClosed;

        public int RecordsAffected => BaseReader.RecordsAffected;

        public int FieldCount => BaseReader.FieldCount;

        private IExcelDataReader BaseReader { get; }

        private Dictionary<CellRange, Cell> MergeCellsTopLeft { get; set; }

        private Cell[] RowCells { get; set; }

        public object this[int i] => GetValue(i);

        public object this[string name] => throw new NotSupportedException();

        public bool GetBoolean(int i) => (bool)GetValue(i);

        public byte GetByte(int i) => (byte)GetValue(i);

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            => throw new NotSupportedException();

        public char GetChar(int i) => (char)GetValue(i);

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
             => throw new NotSupportedException();

        public IDataReader GetData(int i) => throw new NotSupportedException();

        public string GetDataTypeName(int i) => throw new NotSupportedException();

        public DateTime GetDateTime(int i) => (DateTime)GetValue(i);

        public decimal GetDecimal(int i) => (decimal)GetValue(i);

        public double GetDouble(int i) => (double)GetValue(i);

        public Type GetFieldType(int i) => GetValue(i)?.GetType();

        public float GetFloat(int i) => (float)GetValue(i);

        public Guid GetGuid(int i) => (Guid)GetValue(i);

        public short GetInt16(int i) => (short)GetValue(i);

        public int GetInt32(int i) => (int)GetValue(i);

        public long GetInt64(int i) => (long)GetValue(i);

        public string GetName(int i) => throw new NotSupportedException();

        public int GetOrdinal(string name) => throw new NotSupportedException();

        /// <inheritdoc />
        public DataTable GetSchemaTable() => throw new NotSupportedException();

        public string GetString(int i) => (string)GetValue(i);

        public object GetValue(int i)
        {
            if (RowCells == null)
                throw new InvalidOperationException("No data exists for the row/column.");
            return RowCells[i]?.Value;
        }

        public int GetValues(object[] values) => throw new NotSupportedException();

        public bool IsDBNull(int i) => GetValue(i) == null;

        public string GetNumberFormatString(int i)
        {
            if (RowCells == null)
                throw new InvalidOperationException("No data exists for the row/column.");
            if (RowCells[i] == null)
                return null;
            return RowCells[i].NumberFormatString;
        }

        public int GetNumberFormatIndex(int i)
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            MergeCellsTopLeft = new Dictionary<CellRange, Cell>();
            return BaseReader.NextResult();
        }

        public bool Read()
        {
            if (!BaseReader.Read())
                return false;

            RowCells = new Cell[BaseReader.FieldCount];

            for (var i = 0; i < BaseReader.FieldCount; i++)
            {
                var merged = false;
                foreach (var mergeCell in BaseReader.MergeCells)
                {
                    if (IsTopLeft(mergeCell, BaseReader.Depth, i))
                    {
                        var value = GetCellValue(i);
                        RowCells[i] = value;
                        MergeCellsTopLeft.Add(mergeCell, value);
                        merged = true;
                        break;
                    }
                    else if (IsInCellRange(mergeCell, BaseReader.Depth, i))
                    {
                        MergeCellsTopLeft.TryGetValue(mergeCell, out var value);
                        RowCells[i] = value;
                        merged = true;
                        break;
                    }
                }

                if (!merged)
                {
                    var value = GetCellValue(i);
                    RowCells[i] = value;
                }
            }

            return true;
        }

        public void Reset()
        {
            MergeCellsTopLeft = new Dictionary<CellRange, Cell>();
            BaseReader.Reset();
        }

        public void Close()
        {
            BaseReader.Close();
        }

        public void Dispose()
        {
            MergeCellsTopLeft.Clear();
            BaseReader.Dispose();
        }

        private static bool IsInCellRange(CellRange range, int row, int column)
        {
            return row >= range.FromRow && row <= range.ToRow && column >= range.FromColumn && column <= range.ToColumn;
        }

        private static bool IsTopLeft(CellRange range, int row, int column)
        {
            return row == range.FromRow && column == range.FromColumn;
        }

        private Cell GetCellValue(int columnIndex)
        {
            return new Cell
            {
                Value = BaseReader.GetValue(columnIndex),
                NumberFormatString = BaseReader.GetNumberFormatString(columnIndex)
            };
        }

        private class Cell
        {
            public object Value { get; set; }

            public string NumberFormatString { get; set; }
        }
    }
}