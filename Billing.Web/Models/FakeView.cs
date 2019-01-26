using System;
using System.IO;
using System.Web.Mvc;

namespace Billing.Web.Models
{
    /// <summary>
    /// Defines the FakeView type
    /// </summary>
    public class FakeView : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }

    }
}