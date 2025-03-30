using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class RemoveAllUnstableTests
    {
        private readonly ITestOutputHelper _output;

        public RemoveAllUnstableTests(ITestOutputHelper output)
        {
            _output = output;
        }
        public static IEnumerable<object[]> RemovingMethods()
        {
            return (new (string, Action<IList<int>, Predicate<int>>)[] {
                ("List<T>", (IList<int> list, Predicate<int> predicate) => ListExtensions.RemoveAllUnstable<int>((List<int>)list, predicate)),
                ("IList<T>", (IList<int> list, Predicate<int> predicate) => ListExtensions.RemoveAllUnstable<int>(list, predicate)),
            }).Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
        }
        [Theory]
        [MemberData(nameof(RemovingMethods))]
        public void RemoveAllUnstable_EmptyList_NoChanges(string name, Action<IList<int>, Predicate<int>> action)
        {
            // Arrange
            var list = new List<int>();
            _output.WriteLine($"Testing with: {name}");

            // Act
            action(list, x => x % 2 == 0);

            // Assert
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(RemovingMethods))]
        public void RemoveAllUnstable_NoMatch_NoChanges(string name, Action<IList<int>, Predicate<int>> action)
        {
            // Arrange
            var list = new List<int> { 1, 3, 5 };
            _output.WriteLine($"Testing with: {name}");

            // Act
            action(list, x => x % 2 == 0);

            // Assert
            Assert.Equal(new List<int> { 1, 3, 5 }, list);
        }

        [Theory]
        [MemberData(nameof(RemovingMethods))]
        public void RemoveAllUnstable_AllMatch_ListIsEmpty(string name, Action<IList<int>, Predicate<int>> action)
        {
            // Arrange
            var list = new List<int> { 2, 4, 6 };
            _output.WriteLine($"Testing with: {name}");

            // Act
            action(list, x => x % 2 == 0);

            // Assert
            Assert.Empty(list);
        }

        [Theory]
        [MemberData(nameof(RemovingMethods))]
        public void RemoveAllUnstable_SomeMatch_RemovesCorrectElements(string name, Action<IList<int>, Predicate<int>> action)
        {
            // Arrange
            var list1 = new List<int> { 1, 2, 3, 4, 5 };
            var listEndsWithEven = new List<int> { 1, 2, 3, 4, 5, 6 };
            _output.WriteLine($"Testing with: {name}");

            // Act
            action(list1, x => x % 2 == 0);
            action(listEndsWithEven, x => x % 2 == 0);

            // Assert
            Assert.Equal(new List<int> { 1, 5, 3 }, list1);
            Assert.Equal(new List<int> { 1, 5, 3 }, listEndsWithEven);
        }

        [Theory]
        [MemberData(nameof(RemovingMethods))]
        public void RemoveAllUnstable_SomeMatch_RemovesCorrectSequentialElements(string name, Action<IList<int>, Predicate<int>> action)
        {
            // Arrange
            var list1 = new List<int> { 1, 2, 2, 3, 4, 4, 4, 4, 5 };
            var listEndsWithEven = new List<int> { 1, 2, 2, 3, 4, 4, 4, 4, 5, 6, 6, 6 };
            _output.WriteLine($"Testing with: {name}");

            // Act
            action(list1, x => x % 2 == 0);
            action(listEndsWithEven, x => x % 2 == 0);

            // Assert
            Assert.Equal(new List<int> { 1, 5, 3 }, list1);
            Assert.Equal(new List<int> { 1, 5, 3 }, listEndsWithEven);
        }
    }
}