﻿using System.Collections.Generic;
using System.Reflection;
using TechTalk.SpecFlow;

namespace SpecFlow.Reporting
{
	public static partial class Reporters
	{
		[BeforeTestRun]
		internal static void BeforeTestRun()
		{
			testrunIsFirstFeature = true;
			testrunStarttime = CurrentRunTime;
		}

		[BeforeFeature]
		internal static void BeforeFeature()
		{
			var starttime = CurrentRunTime;

			// Init reports when the first feature runs. This is intentionally
			// not done in BeforeTestRun(), to make sure other
			// [BeforeTestRun] annotated methods can perform initialization
			// before the reports are created.
			if (testrunIsFirstFeature)
			{
				foreach (var reporter in reporters)
				{
					reporter.Report = CreateReport();
					reporter.Report.Generator = reporter.Name;
					reporter.Report.StartTime = starttime;

					RaiseEvent(StartingReport, reporter);
				}

				testrunIsFirstFeature = false;
			}

			foreach (var reporter in reporters)
			{
				var feature = CreateFeature();
				feature.StartTime = starttime;
				feature.Title = FeatureContext.Current.FeatureInfo.Title;
				feature.Description = FeatureContext.Current.FeatureInfo.Description;
				feature.Tags.AddRange(FeatureContext.Current.FeatureInfo.Tags);

				reporter.Report.Features.Add(feature);
				reporter.CurrentFeature = feature;

				RaiseEvent(StartingFeature, reporter);
			}
		}

		[BeforeScenario]
		internal static void BeforeScenario()
		{
			var starttime = CurrentRunTime;

			foreach (var reporter in reporters)
			{
				var scenario = CreateScenario();
				scenario.StartTime = starttime;
				scenario.Title = ScenarioContext.Current.ScenarioInfo.Title;
				scenario.Tags.AddRange(ScenarioContext.Current.ScenarioInfo.Tags);

				reporter.CurrentFeature.Scenarios.Add(scenario);
				reporter.CurrentScenario = scenario;

				RaiseEvent(StartingScenario, reporter);
			}
		}

		[BeforeScenarioBlock]
		internal static void BeforeScenarioBlock()
		{
			var starttime = CurrentRunTime;

			foreach (var reporter in reporters)
			{
				switch (ScenarioContext.Current.CurrentScenarioBlock)
				{
					case TechTalk.SpecFlow.ScenarioBlock.Given: reporter.CurrentScenarioBlock = reporter.CurrentScenario.Given; break;
					case TechTalk.SpecFlow.ScenarioBlock.Then: reporter.CurrentScenarioBlock = reporter.CurrentScenario.Then; break;
					case TechTalk.SpecFlow.ScenarioBlock.When: reporter.CurrentScenarioBlock = reporter.CurrentScenario.When; break;
					default:
						break;
				}

				reporter.CurrentScenarioBlock.StartTime = starttime;
				RaiseEvent(StartingScenarioBlock, reporter);
			}
		}

		[BeforeStep]
		internal static void BeforeStep()
		{
		}

		[AfterStep]
		internal static void AfterStep()
		{
			
		}

		[AfterScenarioBlock]
		internal static void AfterScenarioBlock()
		{
			var endtime = CurrentRunTime;
			foreach (var reporter in reporters)
			{
				var scenarioblock = reporter.CurrentScenarioBlock;
				scenarioblock.EndTime = endtime;
				RaiseEvent(FinishedScenarioBlock, reporter);
				reporter.CurrentScenarioBlock = null;
			}
		}

		[AfterScenario]
		internal static void AfterScenario()
		{
			foreach (var reporter in reporters.ToArray())
			{
				var scenario = reporter.CurrentScenario;
				scenario.EndTime = CurrentRunTime;
				RaiseEvent(FinishedScenario, reporter);
				reporter.CurrentScenario = null;
			}
		}

		[AfterFeature]
		internal static void AfterFeature()
		{
			foreach (var reporter in reporters)
			{
				var feature = reporter.CurrentFeature;
				feature.EndTime = CurrentRunTime;
				RaiseEvent(FinishedFeature, reporter);
				reporter.CurrentFeature = null;
			}
		}

		[AfterTestRun]
		internal static void AfterTestRun()
		{
			foreach (var reporter in reporters)
			{
				reporter.Report.EndTime = CurrentRunTime;
				RaiseEvent(FinishedReport, reporter);
			}
		}
	}
}