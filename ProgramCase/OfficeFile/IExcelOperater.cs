using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using NPOI.SS.UserModel;

namespace ProgramCase
{
    /// <summary>
    /// Excel文件操作类
    /// </summary>
    public interface IExcelOperater
    {
        /// <summary>
        /// 将实体类数据导入到制定的Sheet页
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        IExcelOperater Import(string sheetName,object data);

        /// <summary>
        /// 设置默认的单元格样式
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IExcelOperater SetCellStyle(Action<ICellStyle> action);

        /// <summary>
        /// 自定义字段的格式化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        IExcelOperater Format<T>(Expression<Func<T,object>> expression, IColumnFormatter formatter);

        /// <summary>
        /// 设置导入的Header信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="headName"></param>
        /// <returns></returns>
        IExcelOperater SetHeader<T>(Expression<Func<T, object>> expression,string headName);

        /// <summary>
        /// 自定义操作Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IExcelOperater Set(string sheetName, Action<OperaterContext> action);

        /// <summary>
        /// 生成流
        /// </summary>
        /// <returns></returns>
        Stream Create();
    }
}
