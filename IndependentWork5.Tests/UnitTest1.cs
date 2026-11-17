
using System;
using System.Collections.Generic;
using IndependentWork7;
using Xunit;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_ShouldAddElement()
        {
            Cache<string, string> cache = new Cache<string, string>(3);

            cache.Add("1", "Apple");

            Assert.True(cache.ContainsKey("1"));
        }

        [Fact]
        public void Get_ShouldReturnValue()
        {
            Cache<int, string> cache = new Cache<int, string>(3);

            cache.Add(1, "Apple");

            Assert.Equal("Apple", cache.Get(1));
        }

        [Fact]
        public void Add_WhenCapacityExceeded_ShouldRemoveOldestElement()
        {
            Cache<int, string> cache = new Cache<int, string>(2);

            cache.Add(1, "Apple");
            cache.Add(2, "Banana");
            cache.Add(3, "Orange");

            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
        }

        [Fact]
        public void Get_WhenKeyDoesNotExist_ShouldThrowException()
        {
            Cache<int, string> cache = new Cache<int, string>(3);

            Assert.Throws<KeyNotFoundException>(() => cache.Get(10));
        }

        [Fact]
        public void ContainsKey_ShouldReturnCorrectResult()
        {
            Cache<int, string> cache = new Cache<int, string>(3);

            cache.Add(1, "Apple");

            Assert.True(cache.ContainsKey(1));
            Assert.False(cache.ContainsKey(2));
        }

        [Fact]
        public void Count_ShouldReturnNumberOfElements()
        {
            Cache<int, string> cache = new Cache<int, string>(3);

            cache.Add(1, "Apple");
            cache.Add(2, "Banana");

            Assert.Equal(2, cache.Count);
        }

        [Fact]
        public void Add_ExistingKey_ShouldUpdateValueWithoutChangingOrder()
        {
            Cache<int, string> cache = new Cache<int, string>(2);

            cache.Add(1, "Apple");
            cache.Add(2, "Banana");
            cache.Add(1, "Orange");
            cache.Add(3, "Grape");

            Assert.False(cache.ContainsKey(1));
            Assert.Equal("Banana", cache.Get(2));
            Assert.Equal("Grape", cache.Get(3));
        }
    }
}
