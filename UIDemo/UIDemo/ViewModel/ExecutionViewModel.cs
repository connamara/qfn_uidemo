using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.Model;
using System.Collections.ObjectModel;
using UIDemo.Util;
using System.Diagnostics;
using System.Windows.Threading;

namespace UIDemo.ViewModel
{
    public class ExecutionViewModel : ViewModelBase
    {
        private QFApp _qfapp = null;

        private Object _executionsLock = new Object();
        public ObservableCollection<ExecutionRecord> Executions { get; set; }

        public ExecutionViewModel(QFApp app)
        {
            _qfapp = app;
            Executions = new ObservableCollection<ExecutionRecord>();

            _qfapp.Fix42ExecReportEvent += new Action<QuickFix.FIX42.ExecutionReport>(HandleExecutionReport);
        }

        public void HandleExecutionReport(QuickFix.FIX42.ExecutionReport msg)
        {
            try
            {
                string execId = msg.ExecID.Obj;
                string transType = FixEnumTranslator.Translate(msg.ExecTransType);
                string execType = FixEnumTranslator.Translate(msg.ExecType);

                Trace.WriteLine("EVM: Handling ExecutionReport: " + execId + " / " + transType + " / " + execType);

                ExecutionRecord exRec = new ExecutionRecord(
                    msg.ExecID.Obj,
                    msg.OrderID.Obj,
                    transType,
                    execType,
                    msg.Symbol.Obj,
                    FixEnumTranslator.Translate(msg.Side));

                SmartDispatcher.Invoke(new Action<ExecutionRecord>(AddExecution), exRec);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void AddExecution(ExecutionRecord r)
        {
            try
            {
                Trace.WriteLine("add execution");
                Executions.Add(r);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
