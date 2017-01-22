using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Tests
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void Should_return_zero_price()
		{
            var sku = "";
            var total = Total(sku);
            Assert.AreEqual(0, total);
		}

        [Test]
        public void Should_sum_prices_when_no_offers_available()
        {
            var sku = "ABCD";
            var total = Total(sku);
            Assert.AreEqual(115, total);
        }

        [Test]
        public void Should_sum_prices_when_offer_applicable()
        {
            var sku = "AAA";
            var total = Total(sku);
            Assert.AreEqual(130, total);
        }

        public int Total(string sku)
        {
            var total = 0;
            var prices = new Dictionary<char, int>
            {
                {'A', 50},
                {'B', 30},
                {'C', 20},
                {'D', 15}
            };
            var offers = new Dictionary<char, Tuple<int, int>>
            {
                {'A', new Tuple<int, int>(3, -20)},
                {'B', new Tuple<int, int>(2, -15)}
            };
            var items = new Dictionary<char, int>();

            foreach (var item in sku)
            {
                int price;
                if (!prices.TryGetValue(item, out price))
                {
                    throw new NotSupportedException($"SKU={item} is not supported");
                }
                total += price;

                if (items.ContainsKey(item))
                {
                    items[item]++;
                }
                else
                {
                    items[item] = 1;
                }

                Tuple<int, int> offer;
                if (offers.TryGetValue(item, out offer))
                {
                    if (items[item] % offer.Item1 == 0)
                    {
                        total = total + offer.Item2;
                    }
                }
            }
            return total;
        }
	}
}
