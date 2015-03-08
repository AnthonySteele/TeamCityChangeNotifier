using NUnit.Framework;

using TeamCityChangeNotifier.Args;

namespace TeamCityChangeNotifier.Tests
{
	[TestFixture]
	public class TeamCityOperationsTests
	{
		[Test]
		public void CanCreate()
		{
			var request = new Request
				{
					InitialBuildId = 1234,
					TeamCityUser = "fred",
					TeamCityPassword = "hunter2"
				};

			var operations = new TeamCityOperations(new FakeTeamCityReader("foo"), request);

			Assert.That(operations, Is.Not.Null);
		}
	}
}
