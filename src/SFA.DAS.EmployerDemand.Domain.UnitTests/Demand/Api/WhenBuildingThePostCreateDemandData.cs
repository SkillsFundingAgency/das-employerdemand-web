using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Domain.Demand;
using SFA.DAS.EmployerDemand.Domain.Demand.Api.Requests;

namespace SFA.DAS.EmployerDemand.Domain.UnitTests.Demand.Api
{
    public class WhenBuildingThePostCreateDemandData
    {
        [Test, AutoData]
        public void Then_If_The_Number_Of_Apprentices_Is_Not_Known_Then_Set_To_Zero(CourseDemandRequest source)
        {
            source.NumberOfApprenticesKnown = false;
            
            var actual = new PostCreateDemandData(source);

            actual.Id.Should().Be(source.Id);
            actual.OrganisationName.Should().Be(source.OrganisationName);
            actual.ContactEmailAddress.Should().Be(source.ContactEmailAddress);
            actual.NumberOfApprentices.Should().Be(0);
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_From_CreateDemandRequest(CourseDemandRequest source)
        {
            source.NumberOfApprentices = "10";
            source.NumberOfApprenticesKnown = true;
            
            var actual = new PostCreateDemandData(source);

            actual.Id.Should().Be(source.Id);
            actual.OrganisationName.Should().Be(source.OrganisationName);
            actual.ContactEmailAddress.Should().Be(source.ContactEmailAddress);
            actual.NumberOfApprentices.Should().Be(Convert.ToInt32(source.NumberOfApprentices));
            actual.EntryPoint.Should().Be(source.EntryPoint);
        }
        
        [Test, AutoData]
        public void Then_The_Location_Is_Mapped(CourseDemandRequest source)
        {
            source.NumberOfApprentices = "10";
            
            var actual = new PostCreateDemandData(source);
            
            actual.LocationItem.Name.Should().BeEquivalentTo(source.LocationItem.Name);
            actual.LocationItem.LocationPoint.GeoPoint.Should().BeEquivalentTo(source.LocationItem.LocationPoint);
        }
        
        [Test, AutoData]
        public void Then_The_TrainingCourse_Is_Mapped(CourseDemandRequest source)
        {
            source.NumberOfApprentices = "10";
            
            var actual = new PostCreateDemandData(source);
            
            actual.TrainingCourse.Should().BeEquivalentTo(source.Course, options => options.Excluding(c => c.LastStartDate));
        }
    }
}