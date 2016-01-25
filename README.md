# BeanSprout ![alt tag](http://i.imgur.com/e8csqpf.png) 
A data mocking tool for C# application.

BeanSprout automatically creates a concrete instance of your repository interfaces and generates fake data. This is useful for generating design time data.


```
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
    }

 var implementation = BS.Sprout<IFoo>(size);
 IEnumerable<Foo> data = implementation.GetFoos();
 ```
 
## Installation
Install BeanSprout via Nuget https://www.nuget.org/packages/BeanSprout/
