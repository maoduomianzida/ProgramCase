using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ProgramCase
{
    public class ExcelFileOperater : IExcelOperater
    {
        private Dictionary<PropertyInfo, string> _headerMap = new Dictionary<PropertyInfo, string>();
        private Dictionary<PropertyInfo, IColumnFormatter> _formatMap = new Dictionary<PropertyInfo, IColumnFormatter>();
        private Dictionary<string, int> _rowIndexDic = new Dictionary<string, int>();
        private HSSFWorkbook _workbook;
        private ICellStyle _defaultCellStyle;
        internal string Name { get; set; }

        public ExcelFileOperater()
        {
            _workbook = new HSSFWorkbook();
        }

        private int GetRowIndex(string sheetName)
        {
            if (_rowIndexDic.ContainsKey(sheetName))
            {
                return _rowIndexDic[sheetName];
            }

            return 0;
        }

        private bool IsList(object data, out Type listType)
        {
            Contract.Assert(data != null);
            Type[] typeArr = data.GetType().GetInterfaces();
            listType = typeArr.FirstOrDefault(type => type.IsGenericType && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()));

            return listType != null;
        }

        private void ProcessSingle(ISheet sheet, object data)
        {
            Dictionary<string, PropertyHelper> dic = PropertyHelper.GetPropertys(data.GetType());
            dic = OrderProperyDic(dic);
            int rowIndex = GetRowIndex(sheet.SheetName);
            SetHeader(sheet.CreateRow(rowIndex++), dic);
            ProcessItem(sheet.CreateRow(rowIndex++), data, dic);
        }

        private void ProcessItem(IRow row, object data, Dictionary<string, PropertyHelper> dic)
        {
            Contract.Assert(data != null);
            int index = 0;
            foreach (KeyValuePair<string, PropertyHelper> item in dic)
            {
                PropertyInfo property = item.Value.Property;
                ICell cell = row.CreateCell(index++);
                object itemValue = item.Value.GetValue(data);
                if (_formatMap.ContainsKey(property))
                {
                    itemValue = _formatMap[property].Format(item.Key, itemValue);
                }
                else
                {
                    ColumnFormatAttribute attribute = property.GetCustomAttribute<ColumnFormatAttribute>(true);
                    if (attribute != null)
                    {
                        itemValue = attribute.Format(item.Key, itemValue);
                    }
                }
                if (itemValue != null)
                {
                    Type itemType = itemValue.GetType();
                    if (itemType == typeof(string))
                    {
                        cell.SetCellValue((string)itemValue);
                    }
                    else if (itemType == typeof(double))
                    {
                        cell.SetCellValue((double)itemValue);
                    }
                    else if (itemType == typeof(DateTime))
                    {
                        cell.SetCellValue((DateTime)itemValue);
                    }
                    else if (itemType == typeof(bool))
                    {
                        cell.SetCellValue((bool)itemValue);
                    }
                    else
                    {
                        cell.SetCellValue(itemValue.ToString());
                    }
                }
            }
        }

        private Dictionary<string, PropertyHelper> OrderProperyDic(Dictionary<string, PropertyHelper> dic)
        {
            return dic.OrderBy(item =>
            {
                ColumnOrderAttribute orderAttr = item.Value.Property.GetCustomAttribute<ColumnOrderAttribute>(true);
                if (orderAttr != null)
                {
                    return orderAttr.Order;
                }

                return 0;
            }).ToDictionary(item => item.Key, item => item.Value);
        }

        /// <summary>
        /// 设置Header信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="dic"></param>
        private void SetHeader(IRow row, Dictionary<string, PropertyHelper> dic)
        {
            int index = 0;
            foreach (KeyValuePair<string, PropertyHelper> item in dic)
            {
                PropertyInfo property = item.Value.Property;
                string name = property.Name;
                int width = 0;
                if (_headerMap.ContainsKey(property))
                {
                    name = _headerMap[property];
                }
                else
                {
                    ColumnAttribute attr = property.GetCustomAttribute<ColumnAttribute>(true);
                    if (attr != null)
                    {
                        name = attr.Name;
                        width = attr.Width;
                    }
                }
                ICell cell = row.CreateCell(index);
                cell.SetCellValue(name);
                if (_defaultCellStyle != null)
                {
                    cell.Sheet.SetDefaultColumnStyle(index, _defaultCellStyle);
                }
                if (width != 0)
                {
                    cell.Sheet.SetColumnWidth(index, width * 256);
                }
                index++;
            }
        }

        private void ProcessCollection(ISheet sheet, object data, Type listType)
        {
            Contract.Assert(data != null);
            Type elementType = listType.GetGenericArguments()[0];
            Type dataType = data.GetType();
            Func<object, int, object> getIndexEleFunc = ExpressionCreater.GetIndexElementFunc(dataType);
            Func<object, int> getLengthFunc = ExpressionCreater.GetListLengthFunc(dataType);
            int length = getLengthFunc(data);
            bool isFirst = true;
            int rowIndex = GetRowIndex(sheet.SheetName);
            Dictionary<string, PropertyHelper> dic = PropertyHelper.GetPropertys(elementType);
            dic = OrderProperyDic(dic);
            for (int i = 0; i < length; i++)
            {
                object value = getIndexEleFunc(data, i);
                if (value != null)
                {
                    if (isFirst)
                    {
                        SetHeader(sheet.CreateRow(rowIndex++), dic);
                        isFirst = false;
                    }
                    ProcessItem(sheet.CreateRow(rowIndex++), value, dic);
                }
            }
            _rowIndexDic[sheet.SheetName] = rowIndex;
        }

        private ISheet GetOrCreateSheet(string sheetName)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(sheetName));
            ISheet sheet = _workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                sheet = _workbook.CreateSheet(sheetName);
                _rowIndexDic.Add(sheetName, 0);
            }

            return sheet;
        }

        public IExcelOperater Import(string sheetName, object data)
        {
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new ArgumentNullException(nameof(sheetName));
            }
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            ISheet sheet = GetOrCreateSheet(sheetName);
            Type listType;
            if (IsList(data, out listType))
            {
                ProcessCollection(sheet, data, listType);
            }
            else
            {
                ProcessSingle(sheet, data);
            }

            return this;
        }

        public IExcelOperater Format<T>(Expression<Func<T, object>> expression, IColumnFormatter formatter)
        {
            if (expression == null)
            {
                throw new ArgumentException(nameof(expression));
            }
            if (formatter == null)
            {
                throw new ArgumentException(nameof(formatter));
            }
            PropertyInfo propertyInfo = ExpressionParser.ParserProperty(expression);
            if (!_formatMap.ContainsKey(propertyInfo))
            {
                _formatMap.Add(propertyInfo, formatter);
            }

            return this;
        }

        public IExcelOperater SetHeader<T>(Expression<Func<T, object>> expression, string headName)
        {
            if (expression == null)
            {
                throw new ArgumentException(nameof(expression));
            }
            if (string.IsNullOrWhiteSpace(headName))
            {
                throw new ArgumentException(nameof(headName));
            }
            PropertyInfo propertyInfo = ExpressionParser.ParserProperty(expression);
            if (!_headerMap.ContainsKey(propertyInfo))
            {
                _headerMap.Add(propertyInfo, headName);
            }

            return this;
        }

        public IExcelOperater Set(string sheetName, Action<OperaterContext> action)
        {
            if (action == null)
            {
                throw new ArgumentException(nameof(action));
            }
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new ArgumentException(nameof(sheetName));
            }
            ISheet sheet = GetOrCreateSheet(sheetName);
            int rowIndex = GetRowIndex(sheetName);
            OperaterContext context = new OperaterContext(sheet, rowIndex);
            action(context);
            _rowIndexDic[sheetName] = context.RowIndex;

            return this;
        }

        private void ClearData()
        {
            _workbook = new HSSFWorkbook();
            _formatMap.Clear();
            _headerMap.Clear();
            _rowIndexDic.Clear();
            _defaultCellStyle = null;
        }

        public Stream Create()
        {
            MemoryStream stream = null;
            if (_workbook != null)
            {
                stream = new MemoryStream();
                _workbook.Write(stream);
                stream.Seek(0, SeekOrigin.Begin);
            }
            ClearData();

            return stream;
        }

        public IExcelOperater SetCellStyle(Action<ICellStyle> action)
        {
            if (action != null)
            {
                _defaultCellStyle = _workbook.CreateCellStyle();
                action(_defaultCellStyle);
            }

            return this;
        }
    }
}
