# BeanSprout ![alt tag](http://i.imgur.com/e8csqpf.png) 
A data mocking tool for .NET applications.

BeanSprout automatically creates a concrete instance of your repository interfaces and generates fake data. This is useful for generating design time data.

```csharp
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
Currently BeanSprout works off of specific convetions.
* It assumes that you are passing in an interface and will also only mock interface methods that return IEnumerable<T>. 
* BeanSprout will do a shallow data mock. Complex nested model support will come in a future release.

###Future Enhancements

* Add support for multiple return types, not just IEnumerable.
* Support for Deep complex models.
* Support for RegEx DataTypes(model property attributes). 


## Installation
Install BeanSprout via Nuget https://www.nuget.org/packages/BeanSprout/
