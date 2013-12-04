using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Trendings
{
    public class TrendingParser
    {
        public FormatedList Parser(object data)
        {
            FormatedList resultado = new FormatedList();

            resultado.name = "Alarmas";
            resultado.data = data;

            return resultado;
        }
    }

    public class FormatedList
    {
        public FormatedList()
        {
        }

        public string name { get; set; }
        public object data { get; set; }
    }
}