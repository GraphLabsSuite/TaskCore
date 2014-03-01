using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GraphLabs.Utils
{
    /// <summary> Вспомогательные методы для выражений </summary>
    public static class ExpressionUtility
    {
        #region NameForMember

        /// <summary>
        /// Возвращает имя свойства по выражению
        /// <example>
        /// var myVar = new MyClass();
        /// ExpressionUtility.NameForProperty( () => myVar.MyProperty )
        /// </example>
        /// </summary>
        public static string NameForMember<T>(Expression<Func<T>> expression)
        {
            return NameForMemberExprImpl(expression);
        }

        /// <summary>
        /// Возвращает имя свойства по выражению
        /// <example>
        /// ExpressionUtility.NameForProperty( ( MyClass m ) => m.MyProperty )
        /// </example>
        /// </summary>
        public static string NameForMember<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            return NameForMemberExprImpl(expression);
        }

        private static string NameForMemberExprImpl(LambdaExpression expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }

        #endregion


        #region NameForMethod

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<TObject, TReturn>(Expression<Func<TObject, Func<TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<TObject, T, TReturn>(Expression<Func<TObject, Func<T, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<TObject, T1, T2, TReturn>(Expression<Func<TObject, Func<T1, T2, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<TObject, T1, T2, T3, TReturn>(Expression<Func<TObject, Func<T1, T2, T3, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<TObject, T1, T2, T3, T4, TReturn>(Expression<Func<TObject, Func<T1, T2, T3, T4, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<TObject, T1, T2, T3, T4, T5, TReturn>(Expression<Func<TObject, Func<T1, T2, T3, T4, T5, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }




        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<TReturn>(Expression<Func<Func<TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<T, TReturn>(Expression<Func<Func<T, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<T1, T2, TReturn>(Expression<Func<Func<T1, T2, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<T1, T2, T3, TReturn>(Expression<Func<Func<T1, T2, T3, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<T1, T2, T3, T4, TReturn>(Expression<Func<Func<T1, T2, T3, T4, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForMethod<T1, T2, T3, T4, T5, TReturn>(Expression<Func<Func<T1, T2, T3, T4, T5, TReturn>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        #endregion


        #region NameForAction

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<TObject>(Expression<Func<TObject, Action>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<TObject, T>(Expression<Func<TObject, Action<T>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<TObject, T1, T2>(Expression<Func<TObject, Action<T1, T2>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<TObject, T1, T2, T3>(Expression<Func<TObject, Action<T1, T2, T3>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<TObject, T1, T2, T3, T4>(Expression<Func<TObject, Action<T1, T2, T3, T4>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<TObject, T1, T2, T3, T4, T5>(Expression<Func<TObject, Action<T1, T2, T3, T4, T5>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction(Expression<Func<Action>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<T>(Expression<Func<Action<T>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<T1, T2>(Expression<Func<Action<T1, T2>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<T1, T2, T3>(Expression<Func<Action<T1, T2, T3>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<T1, T2, T3, T4>(Expression<Func<Action<T1, T2, T3, T4>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        /// <summary> Возвращает имя метода по выражению </summary>
        public static string NameForAction<T1, T2, T3, T4, T5>(Expression<Func<Action<T1, T2, T3, T4, T5>>> expression)
        {
            return GetMethodOrActionNameImpl(expression);
        }

        #endregion


        private static string GetMethodOrActionNameImpl(LambdaExpression expression)
        {
            var operand = ((UnaryExpression)expression.Body).Operand;
            var methodCall = ((MethodCallExpression)operand).Object;
            var methodInfo = ((ConstantExpression)methodCall).Value;

            return ((MethodInfo)methodInfo).Name;
        }
    }
}