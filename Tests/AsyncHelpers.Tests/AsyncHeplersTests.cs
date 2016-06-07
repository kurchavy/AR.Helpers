using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using AR.Helpers;

namespace AsyncHelpers.Tests
{
    [TestFixture]
    public class AsyncHeplersTests
    {
        #region WithTimeout
        [Test]
        public void ThrowsOnTimeout()
        {
            var t1 = TestTasks.GetResult(TimeSpan.FromSeconds(5), 0);
            Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await t1.WithTimeout(TimeSpan.FromSeconds(2)));
        }

        [Test]
        public void NotThrowsBeforeTimeout()
        {
            var t1 = TestTasks.GetResult(TimeSpan.FromSeconds(2), 0);
            Assert.DoesNotThrowAsync(async () =>
                await t1.WithTimeout(TimeSpan.FromSeconds(5)));
        }

        [Test]
        public void ThrowsRightExceptionBeforeTimeout()
        {
            var t1 = TestTasks.GetResult(TimeSpan.FromSeconds(2), 1);
            Assert.ThrowsAsync<ArgumentException>(async () =>
               await t1.WithTimeout(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void ThrowsTimeoutExceptionBeforeArgument()
        {
            var t1 = TestTasks.GetResult(TimeSpan.FromSeconds(5), 2);
            Assert.ThrowsAsync<TaskCanceledException>(async () =>
               await t1.WithTimeout(TimeSpan.FromSeconds(2)));
        }

        [Test]
        public void ThrowsOnTimeoutNonGeneric()
        {
            var t1 = TestTasks.ProcessTask(TimeSpan.FromSeconds(5), 0);
            Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await t1.WithTimeout(TimeSpan.FromSeconds(2)));
        }

        [Test]
        public void NotThrowsBeforeTimeoutNonGeneric()
        {
            var t1 = TestTasks.ProcessTask(TimeSpan.FromSeconds(2), 0);
            Assert.DoesNotThrowAsync(async () =>
                await t1.WithTimeout(TimeSpan.FromSeconds(5)));
        }

        #endregion

        #region InCompletionOrder
        [Test]
        [TestCase(3000, 2000, 1000)]
        [TestCase(-3000, 2000, 1000)]
        public async Task CompletedInRightOrderNaive(int tm1, int tm2, int tm3)
        {
            var t1 = TestTasks.GetInt(tm1);
            var t2 = TestTasks.GetInt(tm2);
            var t3 = TestTasks.GetInt(tm3);
            var tasks = new Task<int>[] { t1, t2, t3 };

            var resultArray = new int[3];
            int idx = 0;

            foreach (var t in tasks)
            {
                try
                {
                    var res = await t;
                    resultArray[idx++] = res;
                }
                catch (Exception)
                {
                    resultArray[idx++] = -1;
                }
            }

            if (tm1 > 0)
                Assert.AreEqual(tm1, resultArray[0]);
            else
                Assert.AreEqual(-1, resultArray[0]);

            if (tm2 > 0)
                Assert.AreEqual(tm2, resultArray[1]);
            else
                Assert.AreEqual(-1, resultArray[1]);

            if (tm3 > 0)
                Assert.AreEqual(tm3, resultArray[2]);
            else
                Assert.AreEqual(-1, resultArray[2]);
        }

        [Test]
        [TestCase(3000, 2000, 1000)]
        [TestCase(-3000, 2000, 1000)]
        public async Task CompletedInRightOrder(int tm1, int tm2, int tm3)
        {
            var t1 = TestTasks.GetInt(tm1);
            var t2 = TestTasks.GetInt(tm2);
            var t3 = TestTasks.GetInt(tm3);
            var tasks = new Task<int>[] { t1, t2, t3 };

            var testArray = new int[] { tm1, tm2, tm3 };
            testArray = testArray.OrderBy(i => Math.Abs(i)).ToArray();

            var resultArray = new int[3];
            int idx = 0;

            foreach (var t in tasks.InCompletionOrder())
            {
                try
                {
                    var res = await t;
                    resultArray[idx++] = res;
                }
                catch (Exception)
                {
                    resultArray[idx++] = -1;
                }
            }

            for (int i = 0; i< 3; i++)
            {
                if (testArray[i] > 0)
                    Assert.AreEqual(testArray[i], resultArray[i]);
                else
                    Assert.AreEqual(-1, resultArray[i]);
            }
        }
        #endregion
    }
}
