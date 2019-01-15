# <img src="/OOBehave/Logo.png" alt="drawing" width="50" height="50"/> **OOBehave**
Every Enterprise application including web applications need a business layer. OOBehave is a framework for building object-oriented behavioral modeled business layer libraries in .NET. It reduces development time by providing the non-business logic needed to build business objects and collaborate them together. It is inspired by CSLA and built new from the ground up in .NET Standard 2.0.

It features:
- Validation Rules
- Authorization Rules
- Meta Properties
- Parent \ Child \ List collaboration
- CRUD Operation Structure
- N-Layer Architecture Portal
- Fully Async / Await
- Dependency Injection Architecture

I built OOBehave because in the modern development landscape there are dizzying number of tools available.  The challenge is how to learn, apply and combine the tools to build business object. OOBehave does this for you by bringing these powerful tools and patterns into one framework for your team to build maintainable business libraries. 

<img src="/OOBehave/ConnectingTheDots.png" alt="drawing" width="300" height="300"/>

## Getting Started

The project is in its infancy. There will be more examples to follow. Right now, the most examples are the various objects in the unit test library. Also, here’s a few examples.

#### ValidateBase

ValidateBase provides the rules engine RuleManager and the corresponding meta properties including IsValid and IsBusy. 
Here’s a simple example object:

```
    internal class SimpleValidateObject : ValidateBase<SimpleValidateObject>, ISimpleValidateObject
    {
        public SimpleValidateObject(IValidateBaseServices<SimpleValidateObject> services,
                                    IShortNameRule shortNameRule) : base(services)
        {
            RuleManager.AddRule(shortNameRule);
        }

        public Guid Id { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

    }

    public interface ISimpleValidateObject : IValidateBase
    {
        Guid Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string ShortName { get; set; }
    }
```

Some things to note:
-   ValidateBase<T> - Base class that provides the rules functionality and meta properties
-   Getter and Setter used for each property. This allows the base class to execute the rules.
-   Constructor Dependency Injection is used to supply everything both SimpleValidateObject and ValidateBase<> need
-   IShortNameRule is injected to allow for unit testing
-   Class is internal while interface is public. This is the pattern set forth to support unit testing. All 
    consumers should interact with the interface including lists and parent / child relationships.

Here's ShortNameRule:

```
    public interface IShortNameRule : IRule<ISimpleValidateObject> { }

    public class ShortNameRule : RuleBase<ISimpleValidateObject>, IShortNameRule
    {
        public ShortNameRule() : base()
        {
            TriggerProperties.Add(nameof(ISimpleValidateObject.FirstName));
            TriggerProperties.Add(nameof(ISimpleValidateObject.LastName));
        }

        public override IRuleResult Execute(ISimpleValidateObject target)
        {

            var result = RuleResult.Empty();

            if (string.IsNullOrWhiteSpace(target.FirstName))
            {
                result.AddPropertyError(nameof(ISimpleValidateObject.FirstName), $"{nameof(ISimpleValidateObject.FirstName)} is required.");
            }

            if (string.IsNullOrWhiteSpace(target.LastName))
            {
                result.AddPropertyError(nameof(ISimpleValidateObject.LastName), $"{nameof(ISimpleValidateObject.LastName)} is required.");
            }

            if (!result.IsError)
            {
                target.ShortName = $"{target.FirstName} {target.LastName}";
            }

            return result;
        }

    }
```

Each time FirstName or LastName are executed the rule is ran. If both FirstName and LastName have a value ShortName is updated. If not SimpleValidateObject.IsValid will be false.

Some things to note:
-   RuleBase<> is the base class for Validation Rules. There are more features of RuleBase than are shown here.
-   Multiple operations are allowed in the rule. In this case 2 validations (FirstName and LastName are not empty) and a modification (ShortName).
-   The interaction is direct with the target.
-   The target is sent in on execution not in the constructor. This is so one instance of the rule can be shared between multiple objects to reduce memory overhead.
-   One way of sharing a rule between different object types is using a common interface and implementing the rule on that interface. Also, there are many more options and techniques for sharing a rule.
-   Rules can be easily unit tested if an interface (ISimpleValidateObject) is the target. 


[Here is a link to SimpleValidateObject in the project] (OOBehave/OOBehave.UnitTest/Example/SimpleValidate)