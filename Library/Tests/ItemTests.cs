using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Library;
using Library.Exceptions;

namespace Library.Tests
{
    public class ItemTests
    {
        [Fact]
        public void GetNextAvailableItem_ReturnsItemWhenAvailable()
        {
            Item.ResetItemList();

            var item = Item.GetNextAvailableItem();

            Assert.NotNull(item);
            Assert.False(item.IsTaken);
        }

        [Fact]
        public void GetNextAvailableItem_ThrowsNoMoreItemsExceptionWhenNoneAvailable()
        {
            foreach (var item in Item.AllItems)
            {
                item.IsTaken = true;
            }
            Assert.Throws<NoMoreItemsException>(() => Item.GetNextAvailableItem());
        }
    }
}
