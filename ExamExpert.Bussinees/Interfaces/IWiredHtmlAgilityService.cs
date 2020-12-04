using ExamExpert.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Bussinees.Interfaces
{
    public interface IWiredHtmlAgilityService
    {
        List<WiredTypingsListVM> GetLastFiveTypings();
    }
}
