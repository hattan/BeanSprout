using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
                var parameters = method.GetParameters();
                var paramCount = parameters.Count();
                switch (paramCount)
                {
                    case 0:
                        tOriginal.Add(name, CreateMethod(returnType, count));
                        break;
                    case 1:
                        tOriginal.Add(name, CreateMethodOne(returnType, count));
                        break;
                    case 2:
                          tOriginal.Add(name, CreateMethodTwo(returnType, count));
                        break;
                    case 3:
                          tOriginal.Add(name, CreateMethodThree(returnType, count));
                        break;
                    case 4:
                          tOriginal.Add(name, CreateMethodFour(returnType, count));
                        break;
                }
                
            }

            T tActsLike = tOriginal.ActLike();
            return tActsLike;
        }
        private static Func<object> CreateMethod(Type returnType, int count)
        {
            return () => returnType.GetTypeInfo().IsInterface && returnType.Name.Contains("IEnumerable") ? CreateEnumerable(returnType, count) : null;
        }
        private static Func<object,object> CreateMethodOne(Type returnType, int count)
        {
                return a => returnType.GetTypeInfo().IsInterface && returnType.Name.Contains("IEnumerable") ? CreateEnumerable(returnType, count) : null;
        }
        private static Func<object, object, object> CreateMethodTwo(Type returnType, int count)
        {
            return (a,b) => returnType.GetTypeInfo().IsInterface && returnType.Name.Contains("IEnumerable") ? CreateEnumerable(returnType, count) : null;
        }
        private static Func<object, object, object, object> CreateMethodThree(Type returnType, int count)
        {
            return (a, b,c) => returnType.GetTypeInfo().IsInterface && returnType.Name.Contains("IEnumerable") ? CreateEnumerable(returnType, count) : null;
        }
        private static Func<object, object, object, object,object> CreateMethodFour(Type returnType, int count)
        {
            return (a, b, c,d) => returnType.GetTypeInfo().IsInterface && returnType.Name.Contains("IEnumerable") ? CreateEnumerable(returnType, count) : null;
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
            var pt = propInfo.PropertyType;
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
                    if (pt == typeof(string))
                    {
                        return staticAttribute != null ? staticAttribute.Value : "";
                    }

                    if (pt == typeof (bool))
                    {
                        return staticAttribute != null && staticAttribute.BoolValue;
                    }

                    return null;
                }

                if (attr.GetType() == typeof(DataType.Range))
                {
                    var rangeAttribute = attr as DataType.Range;
                    if (rangeAttribute != null)
                    {
                        return RandomNumber.Next(rangeAttribute.Min, rangeAttribute.Max);
                    }
                    throw new Exception("Error Casting attribute to BeanSprout.DataType.Range");
                }
            }

          
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