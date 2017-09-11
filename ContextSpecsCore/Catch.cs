using System;
using System.Diagnostics;
using System.Threading.Tasks;
#if NET20
using Actionx = NUnit.Specifications.Action2;
#else
using Funcx = System.Func<System.Threading.Tasks.Task>;
using Actionx = System.Action;

#endif

namespace ContextSpecsCore
{
    [DebuggerStepThrough]
    public static class Catch
    {
        public static async Task<Exception> Exception(Funcx action)
        {
            Exception exception = null;

            try
            {
                await action();
            }
            catch (Exception e)
            {
                exception = e;
            }

            return exception;
        }

	    public static Exception Exception(Actionx action)
	    {
		    Exception exception = null;

		    try
		    {
			    action();
		    }
		    catch (Exception e)
		    {
			    exception = e;
		    }

		    return exception;
	    }
	}
}