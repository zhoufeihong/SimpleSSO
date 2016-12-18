using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure.Utilities
{
    public class Guard
    {
        private const string AgainstMessage = "Assertion evaluation failed with 'false'.";
        private const string ImplementsMessage = "Type '{0}' must implement type '{1}'.";
        private const string InheritsFromMessage = "Type '{0}' must inherit from type '{1}'.";
        private const string IsTypeOfMessage = "Type '{0}' must be of type '{1}'.";
        private const string IsEqualMessage = "Compared objects must be equal.";
        private const string IsPositiveMessage = "Argument '{0}' must be a positive value. Value: '{1}'.";
        private const string IsTrueMessage = "True expected for '{0}' but the condition was False.";
        private const string NotNegativeMessage = "Argument '{0}' cannot be a negative value. Value: '{1}'.";

        private Guard()
        {
        }

        /// <summary>
        /// Throws proper exception if the class reference is null.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value">Class reference to check.</param>
        /// <exception cref="InvalidOperationException">If class reference is null.</exception>
        [DebuggerStepThrough]
        public static void NotNull<TValue>(Func<TValue> value)
        {
            if (value() == null)
                throw new InvalidOperationException(string.Format("'{0}' cannot be null.", (value)));
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull(object arg, string argName)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(Func<T> arg)
        {
            if (arg() == null)
                throw new ArgumentNullException(GetParamName(arg));
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2>(Func<T1> arg1, Func<T2> arg2)
        {
            if (arg1() == null)
                throw new ArgumentNullException(GetParamName(arg1));

            if (arg2() == null)
                throw new ArgumentNullException(GetParamName(arg2));
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3)
        {
            if (arg1() == null)
                throw new ArgumentNullException(GetParamName(arg1));

            if (arg2() == null)
                throw new ArgumentNullException(GetParamName(arg2));

            if (arg3() == null)
                throw new ArgumentNullException(GetParamName(arg3));
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3, T4>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4)
        {
            if (arg1() == null)
                throw new ArgumentNullException(GetParamName(arg1));

            if (arg2() == null)
                throw new ArgumentNullException(GetParamName(arg2));

            if (arg3() == null)
                throw new ArgumentNullException(GetParamName(arg3));

            if (arg4() == null)
                throw new ArgumentNullException(GetParamName(arg4));
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3, T4, T5>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4, Func<T5> arg5)
        {
            if (arg1() == null)
                throw new ArgumentNullException(GetParamName(arg1));

            if (arg2() == null)
                throw new ArgumentNullException(GetParamName(arg2));

            if (arg3() == null)
                throw new ArgumentNullException(GetParamName(arg3));

            if (arg4() == null)
                throw new ArgumentNullException(GetParamName(arg4));

            if (arg5() == null)
                throw new ArgumentNullException(GetParamName(arg5));
        }


        [DebuggerStepThrough]
        public static string GetParamName<T>(Expression<Func<T>> expression)
        {
            string name = string.Empty;
            MemberExpression body = expression.Body as MemberExpression;

            if (body != null)
            {
                name = body.Member.Name;
            }

            return name;
        }

        [DebuggerStepThrough]
        public static string GetParamName<T>(Func<T> expression)
        {
            return expression.Method.Name;
        }
    }
}
