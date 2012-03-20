using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
        [Test()]
        public void TestThatBMWInitializes()
        {
            var carBMW = ObjectMother.BMW();
            String result = carBMW.Name;
            Assert.AreEqual(result, "BMW Volkswagen");
        }

        [Test()]
        public void TestThatCarDoesGetMileageFromTheDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int miles = 200;
            mockDatabase.Miles = miles;

            var car = new Car(135);
            car.Database = mockDatabase;
            int result = car.Mileage;
            Assert.AreEqual(result, miles);
        }

        [Test()]
        public void TestThatCarDoesGetCarLocationFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int carnum1 = 222;
            int carnum2 = 105;
            String carloc1 = "Seattle";
            String carloc2 = "Madison";
            using(mocks.Record())
            {
                mockDatabase.getCarLocation(carnum1);
                LastCall.Return(carloc1);
                mockDatabase.getCarLocation(carnum2);
                LastCall.Return(carloc2);
            }

            String result;
            Car car = new Car(3);
            car.Database = mockDatabase;
            result = car.getCarLocation(carnum1);
            Assert.AreEqual(result, carloc1);

            result = car.getCarLocation(carnum2);
            Assert.AreEqual(result, carloc2);
        }

		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}
	}
}
