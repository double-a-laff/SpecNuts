﻿using SpecFlow.Reporting.Json;
using SpecFlow.Reporting.Text;
using SpecFlow.Reporting.Xml;
using System;
using TechTalk.SpecFlow;

namespace SpecFlow.Reporting.Tests
{
	[Binding]
	public partial class Steps : ReportingStepDefinitions
	{
		[BeforeTestRun]
		public static void BeforeTestRun()
		{
			Reporter.FixedRunTime = DateTime.MinValue;
			TextReporter.Enabled = true;
			JsonReporter.Enabled = true;
			XmlReporter.Enabled = false;

			IntializeApprovalTests();
		}

		[Given(@"a scenario is specified")]
		public void GivenAScenarioIsSpecified()
		{
		}

		[When(@"the tests run")]
		public void WhenTheTestsRun()
		{
		}

		[Then(@"a report is generated")]
		public void ThenAReportIsGenerated()
		{
		}
	}
}