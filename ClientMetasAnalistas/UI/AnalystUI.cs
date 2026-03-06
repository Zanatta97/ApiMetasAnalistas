using ClientMetasAnalistas.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.UI
{
    internal class AnalystUI
    {
        private readonly IAnalystService _analystService;

        public AnalystUI(IAnalystService analystService)
        {
            _analystService = analystService;
        }
    }
}
