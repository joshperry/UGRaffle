using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace RaffleLib
{
    public static class Ensure
    {
        public static ArgumentEnsure<T> Argument<T>(Expression<Func<T>> param)
        {
            var body = (MemberExpression)param.Body;
            return new ArgumentEnsure<T>(body.Member.Name, (T)((FieldInfo)body.Member).GetValue(((ConstantExpression)body.Expression).Value));
        }

    }

    public class ArgumentEnsure<T>
    {
        T _value;
        string _name;
        public ArgumentEnsure(string name, T value)
        {
            _value = value;
            _name = name;
        }

        public ArgumentEnsure<T> IsNotNull()
        {
            if (_value == null)
                throw new ArgumentNullException(_name);

            return this;
        }

        public ArgumentEnsure<T> IsNotEmpty()
        {
            var sparam = _value as string;
            if (sparam == string.Empty)
                throw new ArgumentException("cannot be empty", _name);

            return this;
        }
    }
}
