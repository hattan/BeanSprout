using System;
namespace BeanSprout.DataType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IPv4 : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class IPv6 : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumber : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Address : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Email : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class FullName : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class FirstName : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class LastName : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Static : Attribute
    {

        public Static(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }

}
