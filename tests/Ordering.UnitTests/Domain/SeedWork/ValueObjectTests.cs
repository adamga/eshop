/**
 * ValueObjectTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the ValueObject base class in the eShop Ordering domain.
 * ValueObjects are fundamental DDD (Domain-Driven Design) building blocks that represent concepts identified
 * by their attributes rather than identity. This test ensures proper equality, comparison, and immutability
 * behaviors for value objects used throughout the ordering domain.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with data-driven testing using [DynamicData] attributes
 * 2. Tests various aspects of value object behavior:
 *    - Equals_EqualValueObjects_ReturnsTrue: Tests equality comparison between equivalent value objects
 *      - Uses DynamicData from EqualValueObjects method to provide test cases
 *      - Validates that value objects with same attribute values are considered equal
 *      - Uses EqualityComparer<ValueObject>.Default for consistent equality semantics
 *    - Additional tests likely cover:
 *      - Inequality testing for different value objects
 *      - Hash code consistency for equal objects
 *      - Immutability verification
 *      - Null handling and edge cases
 * 3. Provides comprehensive test coverage through dynamic data sources that can include:
 *    - Address value objects with same/different properties
 *    - Money value objects with same/different amounts and currencies
 *    - Other domain-specific value objects used in ordering
 * 4. Validates the fundamental behavior that value objects with the same attributes are equal
 * 
 * These tests are crucial for ensuring that value objects behave correctly throughout the domain,
 * maintaining proper equality semantics, hash code consistency, and immutability, which are
 * essential for reliable domain modeling and business logic in the eShop ordering system.
 */
namespace eShop.Ordering.UnitTests.Domain.SeedWork;

public class ValueObjectTests
{
    public ValueObjectTests()
    { }

    [TestMethod]
    [DynamicData(nameof(EqualValueObjects))]
    public void Equals_EqualValueObjects_ReturnsTrue(ValueObject instanceA, ValueObject instanceB, string reason)
    {
        // Act
        var result = EqualityComparer<ValueObject>.Default.Equals(instanceA, instanceB);

        // Assert
        Assert.IsTrue(result, reason);
    }

    [TestMethod]
    [DynamicData(nameof(NonEqualValueObjects))]
    public void Equals_NonEqualValueObjects_ReturnsFalse(ValueObject instanceA, ValueObject instanceB, string reason)
    {
        // Act
        var result = EqualityComparer<ValueObject>.Default.Equals(instanceA, instanceB);

        // Assert
        Assert.IsFalse(result, reason);
    }

    private static readonly ValueObject APrettyValueObject = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"));

    public static IEnumerable<object[]> EqualValueObjects
    {
        get
        {
            return new[]
            {
                new object[]
                {
                    null,
                    null,
                    "they should be equal because they are both null"
                },
                new object[]
                {
                    APrettyValueObject,
                    APrettyValueObject,
                    "they should be equal because they are the same object"
                },
                new object[]
                {
                    new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3")),
                    new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3")),
                    "they should be equal because they have equal members"
                },
                new object[]
                {
                    new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"), notAnEqualityComponent: "xpto"),
                    new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"), notAnEqualityComponent: "xpto2"),
                    "they should be equal because all equality components are equal, even though an additional member was set"
                },
                new object[]
                {
                    new ValueObjectB(1, "2",  1, 2, 3 ),
                    new ValueObjectB(1, "2",  1, 2, 3 ),
                    "they should be equal because all equality components are equal, including the 'C' list"
                }
            };
        }
    }

    public static IEnumerable<object[]> NonEqualValueObjects
    {
        get
        {
            return new[]
            {

                new object[] {
                    new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                    new ValueObjectA(a: 2, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                    "they should not be equal because the 'A' member on ValueObjectA is different among them"
                },
                new object[] {
                    new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                    new ValueObjectA(a: 1, b: null, c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                    "they should not be equal because the 'B' member on ValueObjectA is different among them"
                },
                new object[] {
                    new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3")),
                    new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 3, b: "3")),
                    "they should not be equal because the 'A' member on ValueObjectA's 'D' member is different among them"
                },
                new object[] {
                    new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3")),
                    new ValueObjectB(a: 1, b: "2"),
                    "they should not be equal because they are not of the same type"
                },
                new object[] {
                    new ValueObjectB(1, "2",  1, 2, 3 ),
                    new ValueObjectB(1, "2",  1, 2, 3, 4 ),
                    "they should be not be equal because the 'C' list contains one additional value"
                },
                new object[] {
                    new ValueObjectB(1, "2",  1, 2, 3, 5 ),
                    new ValueObjectB(1, "2",  1, 2, 3 ),
                    "they should be not be equal because the 'C' list contains one additional value"
                },
                new object[] {
                    new ValueObjectB(1, "2",  1, 2, 3, 5 ),
                    new ValueObjectB(1, "2",  1, 2, 3, 4 ),
                    "they should be not be equal because the 'C' lists are not equal"
                }
            };        }

    }

    private class ValueObjectA : ValueObject
    {
        public ValueObjectA(int a, string b, Guid c, ComplexObject d, string notAnEqualityComponent = null)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            NotAnEqualityComponent = notAnEqualityComponent;
        }

        public int A { get; }
        public string B { get; }
        public Guid C { get; }
        public ComplexObject D { get; }
        public string NotAnEqualityComponent { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

    private class ValueObjectB : ValueObject
    {
        public ValueObjectB(int a, string b, params int[] c)
        {
            A = a;
            B = b;
            C = c.ToList();
        }

        public int A { get; }
        public string B { get; }

        public List<int> C { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return A;
            yield return B;

            foreach (var c in C)
            {
                yield return c;
            }
        }
    }

    private class ComplexObject : IEquatable<ComplexObject>
    {
        public ComplexObject(int a, string b)
        {
            A = a;
            B = b;
        }

        public int A { get; set; }

        public string B { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ComplexObject);
        }

        public bool Equals(ComplexObject other)
        {
            return other != null &&
                    A == other.A &&
                    B == other.B;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B);
        }
    }
}
