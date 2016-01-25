using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Faker;
using ImpromptuInterface;

namespace BeanSprout
{
    public class BS
    {
        public static T Sprout<T>(int count = 1)
        {
            var target = typeof(T);
            var methods = target.GetMethods();
            var tOriginal = new ExpandoObject() as IDictionary<string, Object>;

            foreach (var method in methods)
            {
                string name = method.Name;
                Type returnType = method.ReturnType;
                tOriginal.Add(name, CreateMethod(returnType, count));
            }

            T tActsLike = tOriginal.ActLike();
            return tActsLike;
        }

        private static Func<object> CreateMethod(Type returnType, int count)
        {
            return () => returnType.IsInterface && returnType.Name.Contains("IEnumerable") ? CreateEnumerable(returnType, count) : null;
        }

        private static IList CreateEnumerable(Type returnType, int count)
        {
            Type modelType = returnType.GetGenericArguments()[0];

            var list = CreateList(modelType);
            for (var i = 0; i < count; i++)
            {
                var model = CreateModel(modelType);
                list.Add(model);
            }

            return list;
        }

        private static object CreateModel(Type modelType)
        {
            object model = Activator.CreateInstance(modelType);

            var propInfos = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propInfo in propInfos)
            {
                object value = GetPropertyValue(propInfo);

                PropertyInfo prop = modelType.GetProperty(propInfo.Name);
                prop.SetValue(model, value, null);
            }
            return model;
        }

        private static object GetPropertyValue(PropertyInfo propInfo)
        {
            object[] attrs = propInfo.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (attr.GetType() == typeof(DataType.IPv4))
                {
                    return Internet.IPv4Address();
                }

                if (attr.GetType() == typeof(DataType.IPv6))
                {
                    return Internet.IPv6Address();
                }

                if (attr.GetType() == typeof(DataType.Email))
                {
                    return Internet.Email();
                }

                if (attr.GetType() == typeof(DataType.PhoneNumber))
                {
                    return Phone.Number();
                }

                if (attr.GetType() == typeof(DataType.PhoneNumber))
                {
                    return Phone.Number();
                }

                if (attr.GetType() == typeof(DataType.FullName))
                {
                    return Name.FullName();
                }

                if (attr.GetType() == typeof(DataType.FirstName))
                {
                    return Name.First();
                }

                if (attr.GetType() == typeof(DataType.LastName))
                {
                    return Name.Last();
                }

                if (attr.GetType() == typeof(DataType.Static))
                {
                    var staticAttribute = attr as DataType.Static;
                    return staticAttribute != null ? staticAttribute.Value : "";
                }
            }

            var pt = propInfo.PropertyType;
            if (pt.Name.ToLower().Contains("string"))
            {
                return Company.Name();
            }

            if (pt.Name.ToLower().Contains("int"))
            {
                return RandomNumber.Next();
            }

            return null;
        }


        private static IList CreateList(Type model)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { model });
            var list = (IList)Activator.CreateInstance(listType);
            return list;
        }
    }
}