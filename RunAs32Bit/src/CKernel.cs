using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunAs32Bit {
	public static class CKernel {
		[STAThread()] static int Main(string[] args) {
			try {
				Application.EnableVisualStyles(); Application.SetCompatibleTextRenderingDefault(false);
				var appFile = args[0]; var asm = Assembly.LoadFrom(appFile); const string MethodName = "Main";
				var bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				var cls = asm.GetTypes().FirstOrDefault(t => t.GetMethod(MethodName, bindingFlags) != null);
				var methodInfo = cls.GetMethod(MethodName, bindingFlags); var methodArgs = new object[methodInfo.GetParameters().Length];
				if (methodArgs.Length != 0) { methodArgs[0] = args.Skip(1).Where(arg => arg.Trim() != "").ToArray(); }
				var result = methodInfo.Invoke(cls, methodArgs); if (result is bool bResult) { result = bResult ? 0 : -1; }
				if (result is int iResult) { return iResult; }
				return 0;
			}
			catch (Exception ex) {
				while (ex?.InnerException != null) { ex = ex.InnerException; }
				try {
					Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.Message);
					Console.ForegroundColor = ConsoleColor.DarkRed; Console.WriteLine(ex.StackTrace);
					Console.ResetColor();
				}
				catch { }
				return -2;
			}
		}
	}
}
