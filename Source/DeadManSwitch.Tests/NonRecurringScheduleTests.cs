using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeadManSwitch.Schedule;

namespace DeadManSwitch.Tests
{
    [TestClass]
    public class NonRecurringScheduleTests
    {
        [TestMethod]
        public void TestCtor_ThrowsEx_WhenOffsetIsNegative()
        {
            //Arrange
            int fakeUserId = 500;
            TimeSpan negOffset = new TimeSpan(0, 0, -1);

            try
            {
                NonRecurringSchedule cut = new NonRecurringSchedule(fakeUserId, negOffset);

                Assert.Fail("OneTimeSchedule constructor should not allow negative offset");
            }
            catch (ArgumentException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void TestCtor_DoesNotThrowEx_WhenDateTimeKindIsUtcAndInFuture()
        {
            //Arrange
            int fakeUserId = 500;
            DateTime testDateTime = DateTime.UtcNow.AddHours(1);

            NonRecurringSchedule cut = new NonRecurringSchedule(fakeUserId, testDateTime);

            Assert.AreEqual(DateTimeKind.Utc, cut.NextCheckIn.Kind);
        }

        [TestMethod]
        public void TestCtor_ThrowsEx_WhenDateTimeKindIsNotUtc()
        {
            //Arrange
            int fakeUserId = 500;
            DateTime testDateTime = DateTime.Now;

            try
            {
                NonRecurringSchedule cut = new NonRecurringSchedule(fakeUserId, testDateTime);

                Assert.Fail("OneTimeSchedule constructor should not allow non-UTC DateTime.Kind");
            }
            catch (ArgumentException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void TestCtor_ThrowsEx_WhenDateTimePriorToUtcNow()
        {
            //Arrange
            int fakeUserId = 500;
            DateTime testDateTime = DateTime.UtcNow.AddSeconds(-5);

            try
            {
                NonRecurringSchedule cut = new NonRecurringSchedule(fakeUserId, testDateTime);

                Assert.Fail("OneTimeSchedule constructor should not allow DateTime that is prior to UtcNow");
            }
            catch (ArgumentException)
            {
                //Expected. Test passes.
            }
        }

        [TestMethod]
        public void TestCtor_ThrowsEx_WhenLocalCheckInDateTimeIsPriorToCurrentLocalDateTime()
        {
            //Arrange
            int fakeUserId = 500;
            DateTime localCurrentTime = DateTime.Now;
            DateTime localCheckInTime = localCurrentTime.AddMinutes(-5);

            try
            {
                NonRecurringSchedule cut = new NonRecurringSchedule(fakeUserId, localCheckInTime, localCurrentTime);

                Assert.Fail("OneTimeSchedule constructor should not allow localCheckInTime prior to localCurrentTime");
            }
            catch (ArgumentException)
            {
                //Expected. Test passes.
            }
        }

    }
}
