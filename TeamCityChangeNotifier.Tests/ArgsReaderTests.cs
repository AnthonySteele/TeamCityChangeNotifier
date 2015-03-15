using System.Collections.Generic;

using NUnit.Framework;

using TeamCityChangeNotifier.Args;

namespace TeamCityChangeNotifier.Tests
{
	[TestFixture]
	public class ArgsReaderTests
	{
		private readonly ArgsReader _reader = new ArgsReader();

		[Test]
		public void ReadEmptyArgsAsError()
		{
			var settings = _reader.ReadArgs(new List<string>());

			Assert.That(settings.ArgsError, Is.True);
			Assert.That(settings.ArgsErrorMessage, Is.EqualTo("Please supply a teamcity build number"));
		}
	
		[Test]
		public void ReadsABuildId()
		{
			var args = new List<string>
				{
					"id:12345"
				};

			var settings = _reader.ReadArgs(args);

			Assert.That(settings.ArgsError, Is.False);
			Assert.That(settings.InitialBuildId, Is.EqualTo(12345));
		}

		[Test]
		public void DoesNotReadANonNumericBuildId()
		{
			var args = new List<string>
				{
					"id:fred"
				};

			var settings = _reader.ReadArgs(args);

			Assert.That(settings.ArgsError, Is.True);
		}

		[Test]
		public void ReadsAnUnprefixNumberAsBuildId()
		{
			var args = new List<string>
				{
					"12345"
				};

			var settings = _reader.ReadArgs(args);

			Assert.That(settings.ArgsError, Is.False);
			Assert.That(settings.InitialBuildId, Is.EqualTo(12345));
		}

		[Test]
		public void ReadsABuildIdAndPassword()
		{
			var args = new List<string>
				{
					"id:12345",
					"pw:hunter2"
				};

			var settings = _reader.ReadArgs(args);

			Assert.That(settings.ArgsError, Is.False);
			Assert.That(settings.InitialBuildId, Is.EqualTo(12345));
			Assert.That(settings.TeamCityPassword, Is.EqualTo("hunter2"));
		}

		[Test]
		public void ReadsAnUnprefixNumberAndPassword()
		{
			var args = new List<string>
				{
					"12345",
					"pw:hunter2"
				};

			var settings = _reader.ReadArgs(args);

			Assert.That(settings.ArgsError, Is.False);
			Assert.That(settings.InitialBuildId, Is.EqualTo(12345));
			Assert.That(settings.TeamCityPassword, Is.EqualTo("hunter2"));
		}
		
		[Test]
		public void ReadsABuildIdAndUser()
		{
			var args = new List<string>
				{
					"id:12345",
					"u:fred",
					"pw:hunter2"
				};

			var settings = _reader.ReadArgs(args);

			Assert.That(settings.ArgsError, Is.False);
			Assert.That(settings.InitialBuildId, Is.EqualTo(12345));
			Assert.That(settings.TeamCityUser, Is.EqualTo("fred"));
			Assert.That(settings.TeamCityPassword, Is.EqualTo("hunter2"));
		}

		[Test]
		public void ErrorsOnUnknownArgs()
		{
			var args = new List<string>
				{
					"id:12345",
					"zzz:foo"
				};

			var settings = _reader.ReadArgs(args);

			Assert.That(settings.ArgsError, Is.True);
			Assert.That(settings.ArgsErrorMessage, Is.EqualTo("Unknown argument zzz:foo"));
		}
	}
}
