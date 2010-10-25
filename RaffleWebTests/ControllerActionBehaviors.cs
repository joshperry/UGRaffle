using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using System.Web.Mvc;

namespace RaffleWebTests
{
    [Behaviors]
    public class Action_returns_default_view_result
    {
        protected static ViewResult result;

        It should_display_a_view = () => result.ShouldNotBeNull();

        It should_display_default_view = () => result.ViewName.ShouldEqual(string.Empty);
    }
}
