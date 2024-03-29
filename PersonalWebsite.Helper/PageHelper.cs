﻿using System.Text;
using System.Text.RegularExpressions;

namespace PersonalWebsite.Helper
{
    /// <summary>
    /// ** 描述：分页类
    /// ** 创始时间：2015-5-29
    /// ** 修改时间：-
    /// ** 作者：sunkaixuan
    public class PageString
    {
        /// <summary>
        /// 是否是英文      (默认：false)
        /// </summary>
        public bool SetIsEnglish { get; set; }
        /// <summary>
        /// 是否显示分页文字(默认：true)
        /// </summary>
        public bool SetIsShowText { get; set; }
        /// <summary>
        /// 样式            (默认:"pagination")
        /// </summary>
        public string SetClassName { get; set; }
        /// <summary>
        /// 分页参数名      (默认:"pageIndex")
        /// </summary>
        public string SetPageIndexName { get; set; }
        /// <summary>
        /// 是否是异步 同步 href='' 异步 onclick=ajaxPage()    (默认:false)
        /// </summary>
        public bool SetIsAjax { get; set; }

        /// <summary>
        /// 自定义文字
        /// string.Format("{0}{1}{2}","总记录数","当前页数","总页数")
        /// 默认值：《span class=\"pagetext\"》《strong》总共《/strong》:{0} 条 《strong》当前《/strong》:{1}/{2}《/span》
        /// </summary>
        public string SetTextFormat { get; set; }

        public PageString()
        {
            SetIsEnglish = false;
            SetIsShowText = true;
            SetTextFormat = "<span class=\"pagetext\"><strong>总共</strong>:{0} 条 <strong>当前</strong>:{1}/{2}</span> ";
            SetClassName = "pagination";
            SetPageIndexName = "pageIndex";
            SetIsAjax = false;
        }

        /*免费的样式
        .pagination .click {cursor:pointer}
        .pagination a{text-decoration: none;border: 1px solid #AAE;color: #15B;font-size: 13px;border-radius: 2px;}
        .pagination span{ color:#666;font-size:13px;display: inline-block;border: 1px solid #ccc;padding: 0.2em 0.6em;}
        .pagination span.pagetext{ border:none}
        .pagination a:hover{background: #26B;color: #fff;}
        .pagination a{display: inline-block;padding: 0.2em 0.6em;}
        .pagination .page_current{background: #26B;color: #fff;border: 1px solid #AAE;margin-right: 5px;}
        .pagination{margin-top: 20px;}
        .pagination .current.prev, .pagination .current.next{color: #999;border-color: #999;background: #fff;}
         * */

        /// <summary>
        /// 分页算法＜一＞共20页 首页 上一页  1  2  3  4  5  6  7  8  9  10  下一页  末页
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="query_string">Url参数</param>
        /// <returns></returns>
        public string ToString(int total, int pageSize, int pageIndex, string query_string)
        {

            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            StringBuilder pagestr = new StringBuilder();
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            pagestr.AppendFormat("<div class=\"{0}\" >", SetClassName);
            if (pageIndex < 1) { pageIndex = 1; }
            //计算总页数
            if (pageSize != 0)
            {
                allpage = (total / pageSize);
                allpage = ((total % pageSize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = pageIndex + 1;
            pre = pageIndex - 1;
            startcount = (pageIndex + 3) > allpage ? allpage - 4 : pageIndex - 2;//中间页起始序号
            //中间页终止序号
            endcount = pageIndex < 3 ? 5 : pageIndex + 2;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; }//页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内

            bool isFirst = pageIndex == 1;
            bool isLast = pageIndex == allpage;

            if (SetIsShowText)
                pagestr.AppendFormat(SetTextFormat, total, pageIndex, allpage);

            if (isFirst)
            {
                pagestr.Append("<span>首页</span> <span>上一页</span>");
            }
            else
            {
                pagestr.AppendFormat("<a href=\"{0}pageIndex=1\">首页</a>  <a href=\"{0}pageIndex={1}\">上一页</a>", query_string, pre);
            }
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                bool isCurent = pageIndex == i;
                if (isCurent)
                {
                    pagestr.Append("  <a class=\"page_current\">" + i + "</a>");
                }
                else
                {
                    pagestr.Append("   <a href=\"" + query_string + "pageIndex=" + i + "\">" + i + "</a>");
                }

            }
            if (isLast)
            {
                pagestr.Append("<span>下一页</span> <span>末页</span>");
            }
            else
            {
                pagestr.Append("  <a  href=\"" + query_string + "pageIndex=" + next + "\">下一页</a>  <a href=\"" + query_string + "pageIndex=" + allpage + "\">末页</a>");
            }
            pagestr.AppendFormat("</div>");
            return ConversionData(pagestr.ToString());
        }

        private string ConversionData(string page)
        {
            if (SetIsEnglish)
            {
                page = page.Replace("上一页", "Previous").Replace("下一页", "Next").Replace("总共", "total").Replace("当前", "Current").Replace("条", "records").Replace("首页", "First").Replace("末页", "Last");
            }
            if (SetIsAjax)
            {
                var matches = Regex.Matches(page, @"href\="".*?""", RegexOptions.Singleline);
                if (matches != null && matches.Count > 0)
                {
                    foreach (Match m in matches)
                    {
                        page = page.Replace(m.Value, string.Format("class=\"click\" onclick=\"ajaxPage('{0}')\"", Regex.Match(m.Value, string.Format(@"{0}\=(\d+)", SetPageIndexName)).Groups[1].Value));
                    }
                }
            }
            return page;

        }

    }

}
