using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Web.Models;

namespace SFA.DAS.EmployerDemand.Web.UnitTests.Models.AggregatedProviderCourseDemandDetailsViewModelTests
{
    public class WhenGettingCountDescription
    {
        [Test]
        public void And_1_Employer_Then_Singular_Wording()
        {
            var model = new AggregatedProviderCourseDemandDetailsViewModel
            {
                CourseDemandDetailsList = new List<ProviderCourseDemandDetailsViewModel>
                {
                    new ProviderCourseDemandDetailsViewModel()
                }
            };

            model.CountDescription.Should().StartWith("1 employer within");
        }

        [Test, AutoData]
        public void And_Multiple_Employers_Then_Plural_Wording(AggregatedProviderCourseDemandDetailsViewModel model)
        {
            model.CountDescription.Should().StartWith($"{model.CourseDemandDetailsList.Count} employers within");
        }

        [Test, AutoData]
        public void And_All_England_Then_States_England(AggregatedProviderCourseDemandDetailsViewModel model)
        {
            model.SelectedRadius = "1000";

            model.CountDescription.Should().EndWith(" within England.");
        }

        [Test, AutoData]
        public void And_Not_All_England_Then_States_Radius_And_Location(AggregatedProviderCourseDemandDetailsViewModel model)
        {
            model.SelectedRadius = "25";

            model.CountDescription.Should().EndWith($" within 25 miles of {model.Location}.");
        }
    }
}