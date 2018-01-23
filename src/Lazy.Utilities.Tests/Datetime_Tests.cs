using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using Lazy.Utilities.Extensions;
namespace Lazy.Utilities.Tests
{
    public class Datetime_Tests
    {
        [Fact]
        public void Tests()
        {
            var input1 = new DateTime(2018, 1, 23);
            input1.GetWeekFirstDay(DayOfWeek.Monday).ShouldBe(new DateTime(2018, 1, 22));
            input1.GetWeekFirstDay(DayOfWeek.Sunday).ShouldBe(new DateTime(2018, 1, 21));
            input1.GetWeekLastDay(DayOfWeek.Monday).ShouldBe(new DateTime(2018, 1, 28));
            input1.GetWeekLastDay(DayOfWeek.Sunday).ShouldBe(new DateTime(2018, 1, 27));

            input1.GetNextWeekLastDay(DayOfWeek.Monday).ShouldBe(new DateTime(2018, 2, 4));
            input1.GetNextWeekLastDay(DayOfWeek.Sunday).ShouldBe(new DateTime(2018, 2, 3));
            input1.GetNextWeekFisrtDay(DayOfWeek.Monday).ShouldBe(new DateTime(2018, 1, 29));
            input1.GetNextWeekFisrtDay(DayOfWeek.Sunday).ShouldBe(new DateTime(2018, 1, 28));


            input1.GetMonthFirstDay().ShouldBe(new DateTime(2018, 1, 1));
            input1.GetMonthLastDay().ShouldBe(new DateTime(2018, 1, 31));
            input1.GetNextMonthFirstDay().ShouldBe(new DateTime(2018, 2, 1));
            input1.GetNextMonthLastDay().ShouldBe(new DateTime(2018, 2, 28));


            input1.GetNextSpecificDayOfWork(DayOfWeek.Tuesday).ShouldBe(new DateTime(2018, 1, 30));
            input1.GetNextSpecificDayOfWorkContainSelf(DayOfWeek.Tuesday).ShouldBe(input1);

            input1.GetNextSpecificDayOfMonth(23).ShouldBe(new DateTime(2018, 2, 23));
            new DateTime(2018, 1, 31).GetNextSpecificDayOfMonth(30).ShouldBe(new DateTime(2018, 3, 30));
            input1.GetNextSpecificDayOfMonthContainSelf(23).ShouldBe(input1);

            var input2 = new DateTime(2018, 1, 23, 1, 1, 1);
            input1.IsSameDay(input2).ShouldBeTrue();
        }
    }
}
