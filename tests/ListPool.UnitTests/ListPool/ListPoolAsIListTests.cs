using System;
using System.Collections;
using System.Linq;
using AutoFixture;
using Xunit;

namespace ListPool.UnitTests.ListPool
{
    public class ListPoolAsIListTests : ListPoolTestsBase
    {
        public override void Add_item_without_indicate_capacity_of_list()
        {
            int expectedItem = s_fixture.Create<int>();
            using var listPool = new ListPool<int> {expectedItem};
            IList sut = listPool;

            Assert.Equal(expectedItem, sut[0]);
        }


        public override void Add_items_when_capacity_is_full_then_buffer_autogrow()
        {
            using var listPool = new ListPool<int>(128);
            IList sut = listPool;
            var expectedItems = s_fixture.CreateMany<int>(listPool.Capacity * 2).ToList();

            foreach (int expectedItem in expectedItems)
            {
                sut.Add(expectedItem);
            }

            Assert.Equal(expectedItems.Count, sut.Count);
            Assert.True(expectedItems.All(expectedItem => sut.Contains(expectedItem)));
        }

        public override void Contains_empty_ListPool_without_indicating_capacity_returns_false()
        {
            int randomItem = s_fixture.Create<int>();
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            bool actual = sut.Contains(randomItem);

            Assert.False(actual);
        }


        public override void Contains_return_true_when_item_exists()
        {
            int expectedAt0 = s_fixture.Create<int>();
            int expectedAt1 = s_fixture.Create<int>();
            int expectedAt2 = s_fixture.Create<int>();
            int unexpected = s_fixture.Create<int>();

            using var listPool = new ListPool<int>(3) {expectedAt0, expectedAt1, expectedAt2};
            IList sut = listPool;

            Assert.True(sut.Contains(expectedAt0));
            Assert.True(sut.Contains(expectedAt1));
            Assert.True(sut.Contains(expectedAt2));
            Assert.False(sut.Contains(unexpected));
        }


        public override void CopyTo_copy_all_elements_to_target_array()
        {
            int expectedAt0 = s_fixture.Create<int>();
            int expectedAt1 = s_fixture.Create<int>();
            int expectedAt2 = s_fixture.Create<int>();
            using var listPool = new ListPool<int>(3) {expectedAt0, expectedAt1, expectedAt2};
            IList sut = listPool;
            int[] array = new int[3];

            sut.CopyTo(array, 0);

            Assert.Equal(sut.Count, array.Length);
            Assert.Contains(expectedAt0, array);
            Assert.Contains(expectedAt1, array);
            Assert.Contains(expectedAt2, array);
        }


        public override void Count_property_is_for_items_Added_count_not_capacity_of_buffer()
        {
            const int listCapacity = 10;
            const int expectedItemsCount = 3;

            using var listPool = new ListPool<int>(listCapacity) {1, 2, 3};
            IList sut = listPool;

            Assert.Equal(expectedItemsCount, sut.Count);
        }


        public override void Create_list_and_add_values()
        {
            int expectedAt0 = s_fixture.Create<int>();
            int expectedAt1 = s_fixture.Create<int>();
            int expectedAt2 = s_fixture.Create<int>();

            using var listPool = new ListPool<int>(3) {expectedAt0, expectedAt1, expectedAt2};
            IList sut = listPool;

            Assert.Equal(expectedAt0, sut[0]);
            Assert.Equal(expectedAt1, sut[1]);
            Assert.Equal(expectedAt2, sut[2]);
        }


        public override void Create_list_and_add_values_after_clear()
        {
            using var listPool =
                new ListPool<int>(3) {s_fixture.Create<int>(), s_fixture.Create<int>(), s_fixture.Create<int>()};
            IList sut = listPool;

            sut.Clear();

            Assert.Empty(sut);
        }


        public override void Create_without_parameters_should_add_and_get_items()
        {
            const int expectedItemsCount = 3;
            int expectedAt0 = s_fixture.Create<int>();
            int expectedAt1 = s_fixture.Create<int>();
            int expectedAt2 = s_fixture.Create<int>();

            using var listPool = new ListPool<int> {expectedAt0, expectedAt1, expectedAt2};
            IList sut = listPool;

            Assert.Equal(expectedAt0, sut[0]);
            Assert.Equal(expectedAt1, sut[1]);
            Assert.Equal(expectedAt2, sut[2]);
            Assert.Equal(expectedItemsCount, sut.Count);
        }


        public override void Enumerate_when_capacity_is_not_set_dont_throw_exception()
        {
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            foreach (int _ in sut)
            {
            }
        }


        public override void Get_item_with_index_above_itemsCount_throws_ArgumentOutOfRangeException()
        {
            const int index = 2;
            using var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;

            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut[index]);

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void Get_item_with_index_bellow_zero_throws_ArgumentOutOfRangeException()
        {
            int index = -1;
            var listPool = new ListPool<int>();
            IList sut = listPool;

            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut[index]);

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void IndexOf_empty_ListPool_without_indicating_capacity_returns_negative_one()
        {
            int randomItem = s_fixture.Create<int>();
            const int expected = -1;
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            int actual = sut.IndexOf(randomItem);

            Assert.Equal(expected, actual);
        }


        public override void IndexOf_returns_index_of_item()
        {
            int expectedAt0 = s_fixture.Create<int>();
            int expectedAt1 = s_fixture.Create<int>();
            int expectedAt2 = s_fixture.Create<int>();
            using var listPool = new ListPool<int>(3) {expectedAt0, expectedAt1, expectedAt2};
            IList sut = listPool;

            Assert.Equal(0, sut.IndexOf(expectedAt0));
            Assert.Equal(1, sut.IndexOf(expectedAt1));
            Assert.Equal(2, sut.IndexOf(expectedAt2));
        }


        public override void Insert_at_existing_index_move_items_up()
        {
            int[] expectedItems = s_fixture.CreateMany<int>(3).ToArray();
            int expectedItemAt1 = s_fixture.Create<int>();
            int expectedItemsCount = expectedItems.Length + 1;
            using var listPool = expectedItems.ToListPool();
            IList sut = listPool;

            sut.Insert(1, expectedItemAt1);

            Assert.Equal(expectedItemsCount, sut.Count);
            Assert.Equal(expectedItems[0], (int)sut[0]);
            Assert.Equal(expectedItemAt1, (int)sut[1]);
            Assert.Equal(expectedItems[1], (int)sut[2]);
            Assert.Equal(expectedItems[2], (int)sut[3]);
        }


        public override void Insert_at_the_end_add_new_item()
        {
            int expectedAt3 = s_fixture.Create<int>();
            using var listPool =
                new ListPool<int>(4) {s_fixture.Create<int>(), s_fixture.Create<int>(), s_fixture.Create<int>()};
            IList sut = listPool;

            sut.Insert(3, expectedAt3);

            Assert.Equal(4, sut.Count);
            Assert.Equal(expectedAt3, sut[3]);
        }


        public override void Insert_item_with_index_above_itemsCount_throws_ArgumentOutOfRangeException()
        {
            const int index = 2;
            using var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;
            int item = s_fixture.Create<int>();

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.Insert(index, item));

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void Insert_item_with_index_bellow_zero_throws_ArgumentOutOfRangeException()
        {
            const int index = -1;
            int item = s_fixture.Create<int>();
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.Insert(index, item));

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void Insert_items_when_capacity_is_full_then_buffer_autogrow()
        {
            using var listPool = new ListPool<int>(128);
            IList sut = listPool;
            var expectedItems = s_fixture.CreateMany<int>(listPool.Capacity * 2).ToList();
            int index = 0;

            foreach (int expectedItem in expectedItems)
            {
                sut.Insert(index++, expectedItem);
            }

            Assert.Equal(expectedItems.Count, sut.Count);
            Assert.True(expectedItems.All(expectedItem => sut.Contains(expectedItem)));
        }


        public override void Insert_without_indicating_capacity_of_list()
        {
            const int index = 0;
            int expectedItem = s_fixture.Create<int>();
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            sut.Insert(index, expectedItem);

            Assert.Equal(expectedItem, sut[0]);
        }


        public override void Readonly_property_is_always_false()
        {
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            Assert.False(sut.IsReadOnly);
        }


        public override void Remove_item_that_doesnt_exists_return_false()
        {
            string item = s_fixture.Create<string>();
            using var listPool = new ListPool<string> {s_fixture.Create<string>()};
            IList sut = listPool;

            sut.Remove(item);

            Assert.Single(sut);
        }


        public override void Remove_when_item_exists_remove_item_and_decrease_itemsCount()
        {
            const int expectedCountAfterRemove = 2;
            int expectedAt0 = s_fixture.Create<int>();
            using var listPool = new ListPool<int>(3) {expectedAt0, s_fixture.Create<int>(), s_fixture.Create<int>()};
            IList sut = listPool;

            sut.Remove(expectedAt0);

            Assert.False(sut.Contains(expectedAt0));
            Assert.Equal(expectedCountAfterRemove, sut.Count);
        }


        public override void Remove_when_item_is_null_return_false()
        {
            string item = null;
            using var listPool = new ListPool<string>();
            IList sut = listPool;

            sut.Remove(item);
        }


        public override void RemoveAt_when_item_exists_remove_item_and_decrease_itemsCount()
        {
            const int expectedCountAfterRemove = 2;
            int expectedAt1 = s_fixture.Create<int>();
            using var listPool = new ListPool<int>(3) {s_fixture.Create<int>(), expectedAt1, s_fixture.Create<int>()};
            IList sut = listPool;

            sut.RemoveAt(1);

            Assert.False(sut.Contains(expectedAt1));
            Assert.Equal(expectedCountAfterRemove, sut.Count);
        }


        public override void RemoveAt_with_index_above_itemsCount_throws_ArgumentOutOfRangeException()
        {
            const int index = 2;
            using var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.RemoveAt(index));

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void RemoveAt_with_index_bellow_zero_throws_ArgumentOutOfRangeException()
        {
            const int index = -1;
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.RemoveAt(index));

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void RemoveAt_with_index_zero_when_not_item_added_throws_ArgumentOutOfRangeException()
        {
            const int index = 0;
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.RemoveAt(index));

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void Set_at_existing_index_update_item()
        {
            const int expectedItemsCount = 3;
            int expectedItem = s_fixture.Create<int>();
            using var listPool =
                new ListPool<int>(3) {s_fixture.Create<int>(), s_fixture.Create<int>(), s_fixture.Create<int>()};
            IList sut = listPool;

            sut[2] = expectedItem;

            Assert.Equal(expectedItemsCount, sut.Count);
            Assert.Equal(expectedItem, sut[2]);
        }


        public override void Set_item_with_index_above_itemsCount_throws_ArgumentOutOfRangeException()
        {
            const int index = 2;
            using var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;
            int item = s_fixture.Create<int>();

            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut[index] = item);

            Assert.Equal(nameof(index), exception.ParamName);
        }


        public override void Set_item_with_index_bellow_zero_throws_ArgumentOutOfRangeException()
        {
            const int index = -1;
            int item = s_fixture.Create<int>();
            var listPool = new ListPool<int>();
            IList sut = listPool;

            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut[index] = item);

            Assert.Equal(nameof(index), exception.ParamName);
        }

        [Fact]
        public void Add_item_when_is_not_same_type_throw_ArgumentException()
        {
            using var listPool = new ListPool<int>();
            IList sut = listPool;
            string itemWithWrongType = s_fixture.Create<string>();

            ArgumentException actualException = Assert.Throws<ArgumentException>(() => sut.Add(itemWithWrongType));
            Assert.Equal("item", actualException.ParamName);
        }

        [Fact]
        public void Contains_item_with_another_type_throws_ArgumentException()
        {
            string itemWithWrongType = s_fixture.Create<string>();
            var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;

            ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.Contains(itemWithWrongType));

            Assert.Equal("item", exception.ParamName);
        }

        [Fact]
        public void IndexOf_item_with_another_type_throws_ArgumentException()
        {
            string itemWithWrongType = s_fixture.Create<string>();
            var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;

            ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.IndexOf(itemWithWrongType));

            Assert.Equal("item", exception.ParamName);
        }

        [Fact]
        public void Insert_item_when_is_not_same_type_throw_ArgumentException()
        {
            using var listPool = new ListPool<int>();
            IList sut = listPool;
            string itemWithWrongType = s_fixture.Create<string>();

            ArgumentException actualException =
                Assert.Throws<ArgumentException>(() => sut.Insert(0, itemWithWrongType));
            Assert.Equal("item", actualException.ParamName);
        }

        [Fact]
        public void IsFixedSize_always_return_false()
        {
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            Assert.False(sut.IsFixedSize);
        }

        [Fact]
        public void IsSynchronized_always_return_false()
        {
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            Assert.False(sut.IsSynchronized);
        }

        [Fact]
        public void Remove_item_with_another_type_throws_ArgumentException()
        {
            string itemWithWrongType = s_fixture.Create<string>();
            var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;

            ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.Remove(itemWithWrongType));

            Assert.Equal("item", exception.ParamName);
        }

        [Fact]
        public void Set_item_with_another_type_throws_ArgumentException()
        {
            const int index = 0;
            string itemWithWrongType = s_fixture.Create<string>();
            var listPool = new ListPool<int> {s_fixture.Create<int>()};
            IList sut = listPool;

            ArgumentException exception = Assert.Throws<ArgumentException>(() => sut[index] = itemWithWrongType);

            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void SyncRoot_never_is_null()
        {
            using var listPool = new ListPool<int>();
            IList sut = listPool;

            Assert.NotNull(sut.SyncRoot);
        }
    }
}