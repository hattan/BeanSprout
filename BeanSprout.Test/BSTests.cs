using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BeanSprout.DataType;
using Xunit;

namespace BeanSprout.Test
{
    public class BSTests
    {
        /* Test Objects*/
        public interface IFoo
        {
            IEnumerable<Foo> GetFoos();
        }

        public class Foo
        {
            public int Id { get; set; }

            [FullName]
            public string Name { get; set; }

            [Static("Foo123")]
            public string Custom { get; set; }

            [Range(1, 30)]
            public int Age { get; set; }

            [Static(true)]
            public bool Active { get; set; }

            [Email]
            public string Email { get; set; }
        }

        [Fact]
        public void Sprout_CreatesAnObjectFromInterface()
        {
            //act
            var implementation = BS.Sprout<IFoo>();

            //assert
            Assert.NotNull(implementation);
        }

        [Fact]
        public void Sprout_CreatesAnObjectThatImplementsInterface()
        {
            //act
            var implementation = BS.Sprout<IFoo>();

            //assert
            IFoo test = implementation; // if not an IFoo will be null
            Assert.NotNull(test);
        }

        [Fact]
        public void Sprout_WhenCalledWithNoParams_ReturnsListWithOneItem()
        {
            //arrange
            var implementation = BS.Sprout<IFoo>();

            //act
            IEnumerable<Foo> data = implementation.GetFoos();

            //assert
            Assert.Single(data);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void Sprout_WhenCalledWithParams_ReturnsListWithSpecifiedSize(int size)
        {
            //arrange
            var implementation = BS.Sprout<IFoo>(size);

            //act
            IEnumerable<Foo> data = implementation.GetFoos();

            //assert
            Assert.Equal(size, data.Count());
        }

        [Fact]
        public void Sprout_CreatesImplementationThatReturnsCorrectModel()
        {
            //arrange
            var implementation = BS.Sprout<IFoo>();

            //act
            IEnumerable<Foo> data = implementation.GetFoos();
            Foo item = data.FirstOrDefault();

            //assert
            Assert.NotNull(item);
            Assert.IsType<Foo>(item);
        }


        [Fact]
        public void Sprout_WhenModelHasCustomData_ValueIsSetToCustomData()
        {
            //arrange
            var implementation = BS.Sprout<IFoo>();

            //act
            IEnumerable<Foo> data = implementation.GetFoos();
            Foo item = data.FirstOrDefault();

            //assert
            Assert.NotNull(item);
            Assert.Equal("Foo123", item.Custom);
        }

        [Fact]
        public void Sprout_WithModelRangeAttribute_GeneratesValueInRange()
        {
            //arrange
            var implementation = BS.Sprout<IFoo>();

            //act
            IEnumerable<Foo> data = implementation.GetFoos();
            Foo item = data.FirstOrDefault();

            //assert
            Assert.NotNull(item);
            Assert.True(item.Age >=1 && item.Age <= 30); //range specified above in test model
        }

        [Fact]
        public void Sprout_WithModelStaticAttributeOnBool_GeneratesExpectedBooleanValue()
        {
            //arrange
            var implementation = BS.Sprout<IFoo>();

            //act
            IEnumerable<Foo> data = implementation.GetFoos();
            Foo item = data.FirstOrDefault();

            //assert
            Assert.NotNull(item);
            Assert.True(item.Active); //range specified above in test model
        }

        [Fact]
        public void Sprout_WithModelEmailAttribute_GeneratesEmailFormatedValue()
        {
            //arrange
            var implementation = BS.Sprout<IFoo>();

            //act
            IEnumerable<Foo> data = implementation.GetFoos();
            Foo item = data.FirstOrDefault();

            //assert
            Assert.NotNull(item);

            bool isEmail = Regex.IsMatch(item.Email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase);

            Assert.True(isEmail); 
        }


      
    }


}
