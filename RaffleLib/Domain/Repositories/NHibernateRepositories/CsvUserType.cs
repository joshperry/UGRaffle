using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.UserTypes;
using NHibernate.SqlTypes;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class CsvUserType : IUserType
    {
        private const string SEPARATOR = ",";

        bool IUserType.Equals(object x, object y)
        {
            var xl = x as IList<string>;
            var yl = y as IList<string>;

            return (xl != null || yl != null)
                && xl.Count == yl.Count
                && xl.Except(yl).Count() == 0;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            var list = value as IList<string>;
            return list.ToList();
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public bool IsMutable
        {
            get { return true; }
        }

        public object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner)
        {
            IList<string> result = null;

            var value = rs[names[0]] as string;
            if (!string.IsNullOrEmpty(value))
                result = value.Split(SEPARATOR.ToCharArray()).ToList();

            return result ?? new List<string>();
        }

        public void NullSafeSet(System.Data.IDbCommand cmd, object value, int index)
        {
            var svalue = value as string;
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
            }
            else
            {
                var colValue = string.Join(SEPARATOR, value as IList<string>);
                NHibernateUtil.String.Set(cmd, colValue, index);
            }
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType
        {
            get { return typeof(IList<string>); }
        }

        public NHibernate.SqlTypes.SqlType[] SqlTypes
        {
            get { return new SqlType[] { NHibernateUtil.String.SqlType }; }
        }
    }
}
