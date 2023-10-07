using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Claunia.PropertyList;
using Ionic.Zip;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Renci.SshNet;
using RestSharp;

// Token: 0x0200000D RID: 13
public partial class Form1 : Form
{
	// Token: 0x0600003D RID: 61 RVA: 0x0020D4F4 File Offset: 0x0020B6F4
	public Form1()
	{
		if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count<Process>() > 1)
		{
			MessageBox.Show("Broque Ramdisk is already running.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Process.GetCurrentProcess().Kill();
		}
		this.method_105<ComponentResourceManager, Label, Button, CheckBox, Panel, PictureBox, LinkLabel, ComboBox, TextBox, ProgressBar, object, EventArgs, bool, ProcessStartInfo, LinkLabelLinkClickedEventArgs, string, ComboBox.ObjectCollection, DialogResult, FormClosingEventArgs>();
		base.FormBorderStyle = FormBorderStyle.FixedSingle;
		foreach (Process process in Process.GetProcessesByName("iproxy"))
		{
			process.Kill();
		}
		this.label21.Text = "v" + this.string_0;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x0020D690 File Offset: 0x0020B890
	public T0 method_0<T0, T1, T2>()
	{
		T0 t2;
		using (T1 current = WindowsIdentity.GetCurrent())
		{
			T2 t = new WindowsPrincipal(current);
			t2 = t.IsInRole(WindowsBuiltInRole.Administrator);
		}
		return t2;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0020D6D8 File Offset: 0x0020B8D8
	public void method_1<T0, T1>()
	{
		T0 t = Form1.smethod_4();
		t.FileName = "explorer.exe";
		t.Arguments = string.Concat(new T1[]
		{
			"/select,\"",
			Environment.CurrentDirectory,
			"\\files\\BackUp\\",
			this.string_5,
			".zip\""
		});
		t.UseShellExecute = false;
		Process.Start(t);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x0020D740 File Offset: 0x0020B940
	public void method_2<T0, T1, T2, T3, T4>()
	{
		T0 t = new SQLiteConnection("URI=file:./files/BackUp/" + this.string_5 + "/iCloudInfo/Accounts3.sqlite");
		t.Open();
		T1 t2 = new SQLiteCommand(t);
		t2.CommandText = "SELECT ZVALUE FROM ZACCOUNTPROPERTY WHERE ZKEY = 'additionalInfo'";
		T2[] array = (byte[])t2.ExecuteScalar();
		T3 @string = Encoding.UTF8.GetString(array);
		t2.CommandText = "SELECT ZVALUE FROM ZACCOUNTPROPERTY WHERE ZKEY = 'appleId'";
		T2[] array2 = (byte[])t2.ExecuteScalar();
		T3 string2 = Encoding.UTF8.GetString(array2);
		t2.CommandText = "SELECT ZVALUE FROM ZACCOUNTPROPERTY WHERE ZKEY = 'FullUserName'";
		T2[] array3 = (byte[])t2.ExecuteScalar();
		T3 string3 = Encoding.UTF8.GetString(array3);
		t2.Dispose();
		t.Close();
		T3[] array4 = Regex.Matches(@string, "(\\d{3}[ -.]?\\d{3}[ -.]?\\d{4}|\\(\\d{3}\\) ?\\d{3}[ -.]?\\d{4})").Cast<T4>().Select(new Func<Match, string>(Form1.Class6.ab9.method_0<T3, T4>))
			.ToArray<T3>();
		T3[] array5 = Regex.Matches(string2, "([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})").Cast<T4>().Select(new Func<Match, string>(Form1.Class6.ab9.method_1<T3, T4>))
			.ToArray<T3>();
		T3[] array6 = Regex.Matches(string3, "([A-Z][a-z]+)\\s([A-Z][a-z]+)").Cast<T4>().Select(new Func<Match, string>(Form1.Class6.ab9.method_2<T3, T4>))
			.ToArray<T3>();
		T3 t3 = "./files/BackUp/" + this.string_5 + "/iCloudInfo/iCloudInfo.txt";
		File.WriteAllText(t3, string.Concat(new T3[]
		{
			"Email: ",
			string.Join(", ", array5),
			"\nPhone number: ",
			string.Join(", ", array4),
			"\nName: ",
			string.Join(", ", array6)
		}));
	}

	// Token: 0x06000041 RID: 65 RVA: 0x0020D914 File Offset: 0x0020BB14
	private static T4 smethod_0<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T4 gparam_0, T4 gparam_1, T4 gparam_2)
	{
		T0 t = new RestClient(string.Concat(new T4[] { "https://api.github.com/repos/", gparam_0, "/", gparam_1, "/contents/", gparam_2 }));
		T1 t2 = new RestRequest(Method.GET);
		t2.AddHeader("Accept", "application/vnd.github.v3+json");
		T2 t3 = t.Execute(t2);
		if (t3.StatusCode != HttpStatusCode.OK)
		{
			throw new Exception(string.Format("Failed to retrieve file content. Response code: {0}", t3.StatusCode));
		}
		T4 content = t3.Content;
		T5 t4 = JsonConvert.DeserializeObject(content);
		if (Form1.Class10.callSite_0 == null)
		{
			Form1.Class10.callSite_0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "content", typeof(Form1), new T6[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
		}
		T5 t5 = Form1.Class10.callSite_0.Target(Form1.Class10.callSite_0, t4);
		if (Form1.Class10.callSite_4 == null)
		{
			Form1.Class10.callSite_4 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Trim", null, typeof(Form1), new T6[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
		}
		Func<CallSite, object, object> target = Form1.Class10.callSite_4.Target;
		CallSite callSite_ = Form1.Class10.callSite_4;
		if (Form1.Class10.callSite_3 == null)
		{
			Form1.Class10.callSite_3 = CallSite<Func<CallSite, Encoding, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetString", null, typeof(Form1), new T6[]
			{
				CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
				CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
			}));
		}
		Func<CallSite, Encoding, object, object> target2 = Form1.Class10.callSite_3.Target;
		CallSite callSite_2 = Form1.Class10.callSite_3;
		Encoding utf = Encoding.UTF8;
		if (Form1.Class10.callSite_2 == null)
		{
			Form1.Class10.callSite_2 = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "FromBase64String", null, typeof(Form1), new T6[]
			{
				CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
				CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
			}));
		}
		Func<CallSite, Type, object, object> target3 = Form1.Class10.callSite_2.Target;
		CallSite callSite_3 = Form1.Class10.callSite_2;
		Type typeFromHandle = typeof(T7);
		if (Form1.Class10.callSite_1 == null)
		{
			Form1.Class10.callSite_1 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(Form1), new T6[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
		}
		T5 t6 = target(callSite_, target2(callSite_2, utf, target3(callSite_3, typeFromHandle, Form1.Class10.callSite_1.Target(Form1.Class10.callSite_1, t5))));
		if (Form1.Class10.callSite_5 == null)
		{
			Form1.Class10.callSite_5 = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(T4), typeof(Form1)));
		}
		return Form1.Class10.callSite_5.Target(Form1.Class10.callSite_5, t6);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0020DB9C File Offset: 0x0020BD9C
	public void method_3<T0, T1, T2>()
	{
		if (!this.checkBox3.Checked)
		{
			this.sshClient_0 = new SshClient("127.0.0.1", 2222, "root", "alpine");
			this.scpClient_0 = new ScpClient("127.0.0.1", 2222, "root", "alpine");
		}
		else
		{
			this.sshClient_0 = new SshClient("127.0.0.1", 2222, "root", "TgRam2022");
			this.scpClient_0 = new ScpClient("127.0.0.1", 2222, "root", "TgRam2022");
		}
		T0[] processesByName = Process.GetProcessesByName("iproxy");
		for (T2 t = 0; t < processesByName.Length; t++)
		{
			T0 t2 = processesByName[t];
			t2.Kill();
		}
		T0 t3 = Form1.smethod_5();
		t3.StartInfo.FileName = Environment.CurrentDirectory + "/files/iproxy.exe";
		t3.StartInfo.Arguments = ((this.checkBox2.Checked || this.checkBox5.Checked || this.checkBox9.Checked || this.checkBox10.Checked) ? "2222 44" : (this.checkBox3.Checked ? "2222 86" : "2222 22"));
		t3.StartInfo.UseShellExecute = false;
		t3.StartInfo.CreateNoWindow = true;
		t3.StartInfo.RedirectStandardError = true;
		t3.Start();
	}

	// Token: 0x06000043 RID: 67 RVA: 0x0020DD08 File Offset: 0x0020BF08
	[DebuggerStepThrough]
	public T0 method_4<T0>()
	{
		Form1.Class19 @class = new Form1.Class19();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder<bool>.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class19>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0020DD4C File Offset: 0x0020BF4C
	public T2 method_5<T0, T1, T2, T3>()
	{
		this.progressBar1.Value = 0;
		T0 t = 0;
		for (;;)
		{
			try
			{
				this.method_3<T3, T2, T0>();
				T1 t2 = "%USERPATH%\\.ssh\\known_hosts";
				T2 t3 = File.Exists(t2);
				if (t3 != null)
				{
					File.Delete(t2);
				}
				T2 t4 = !this.sshClient_0.IsConnected;
				if (t4 != null)
				{
					this.sshClient_0.Connect();
				}
				T2 t5 = !this.scpClient_0.IsConnected;
				if (t5 != null)
				{
					this.scpClient_0.Connect();
				}
				goto IL_9C;
			}
			catch
			{
				if (t != 19)
				{
					t++;
					goto IL_9C;
				}
				return 0;
			}
			goto IL_80;
			IL_91:
			bool flag;
			if (!flag)
			{
				break;
			}
			continue;
			IL_80:
			flag = !this.sshClient_0.IsConnected;
			goto IL_91;
			IL_9C:
			if (t < 20)
			{
				goto IL_80;
			}
			flag = false;
			goto IL_91;
		}
		return 1;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0020DE14 File Offset: 0x0020C014
	public void method_6<T0, T1, T2>(T1 gparam_0)
	{
		T0 t = Form1.smethod_5();
		t.StartInfo.FileName = "./files/drivers/libusbK/dpscat.exe";
		t.StartInfo.UseShellExecute = false;
		t.StartInfo.Arguments = ((gparam_0 != null) ? (Environment.Is64BitOperatingSystem ? ("/PATH  \"" + Path.Combine(new T2[]
		{
			Environment.CurrentDirectory,
			"files",
			"drivers",
			"usb",
			"x64"
		}) + "\"") : ("/PATH \"" + Path.Combine(new T2[]
		{
			Environment.CurrentDirectory,
			"files",
			"drivers",
			"usb",
			"x86"
		}) + "\"")) : ("/PATH \"" + Path.Combine(Environment.CurrentDirectory, "files", "drivers", "libusbK") + "\""));
		t.StartInfo.UseShellExecute = false;
		t.StartInfo.RedirectStandardOutput = true;
		t.Start();
		t.WaitForExit();
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0020DF30 File Offset: 0x0020C130
	public void method_7<T0, T1>()
	{
		T0 t = Form1.smethod_5();
		t.StartInfo.Arguments = "install --inf=\"" + Path.Combine(new T1[]
		{
			Environment.CurrentDirectory,
			"files",
			"drivers",
			"libusbK",
			"Apple_Mobile_Device_(DFU_Mode).inf"
		}) + "\"";
		t.StartInfo.FileName = Path.Combine(new T1[]
		{
			Environment.CurrentDirectory,
			"files",
			"drivers",
			"libusbK",
			"install-filter.exe"
		});
		t.StartInfo.UseShellExecute = false;
		t.StartInfo.RedirectStandardOutput = true;
		t.StartInfo.CreateNoWindow = true;
		t.Start();
		t.WaitForExit();
	}

	// Token: 0x06000047 RID: 71 RVA: 0x0020E000 File Offset: 0x0020C200
	public void method_8<T0, T1>()
	{
		T0 t = Form1.smethod_5();
		t.StartInfo.Arguments = (Environment.Is64BitOperatingSystem ? ("install --inf=\"" + Path.Combine(new T1[]
		{
			Environment.CurrentDirectory,
			"files",
			"drivers",
			"usb",
			"x64",
			"usbaapl64.inf"
		}) + "\"") : ("install --inf=\"" + Path.Combine(new T1[]
		{
			Environment.CurrentDirectory,
			"files",
			"drivers",
			"usb",
			"x86",
			"usbaapl64.inf"
		}) + "\""));
		t.StartInfo.FileName = Path.Combine(new T1[]
		{
			Environment.CurrentDirectory,
			"files",
			"drivers",
			"libusbK",
			"install-filter.exe"
		});
		t.StartInfo.UseShellExecute = false;
		t.StartInfo.RedirectStandardOutput = true;
		t.StartInfo.CreateNoWindow = true;
		t.Start();
		t.WaitForExit();
	}

	// Token: 0x06000048 RID: 72 RVA: 0x0020E12C File Offset: 0x0020C32C
	public void method_9<T0, T1, T2>()
	{
		T0 t = Form1.smethod_5();
		t.StartInfo.Arguments = "pwn";
		t.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "files", "gaster.exe");
		t.StartInfo.UseShellExecute = false;
		t.StartInfo.RedirectStandardOutput = true;
		t.StartInfo.CreateNoWindow = true;
		t.Start();
		T1 t2 = !t.WaitForExit(6000);
		if (t2 != null)
		{
			this.method_7<T0, T2>();
			T1 t3 = !t.WaitForExit(13000);
			if (t3 != null)
			{
				t.Kill();
				throw new Exception("Error: Failed to pwn DFU");
			}
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x0020E1D4 File Offset: 0x0020C3D4
	public void method_10<T0>()
	{
		using (T0 t = Form1.smethod_5())
		{
			t.StartInfo.FileName = Environment.CurrentDirectory + "/files/ideviceactivation.exe";
			t.StartInfo.Arguments = "deactivate";
			t.StartInfo.UseShellExecute = false;
			t.StartInfo.RedirectStandardOutput = true;
			t.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			t.StartInfo.CreateNoWindow = true;
			t.Start();
			t.StandardOutput.ReadToEnd();
			t.WaitForExit();
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x0020E278 File Offset: 0x0020C478
	public void method_11<T0, T1, T2>(T2 gparam_0)
	{
		using (T0 t = Form1.smethod_5())
		{
			t.StartInfo.FileName = Environment.CurrentDirectory + "/files/ideviceactivation.exe";
			if (!(gparam_0 == ""))
			{
				t.StartInfo.Arguments = "activate -s " + gparam_0;
			}
			else
			{
				t.StartInfo.Arguments = "activate";
			}
			t.StartInfo.UseShellExecute = false;
			t.StartInfo.RedirectStandardOutput = true;
			t.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			t.StartInfo.CreateNoWindow = true;
			t.Start();
			t.StandardOutput.ReadToEnd();
			t.WaitForExit();
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0020E344 File Offset: 0x0020C544
	public T1 method_12<T0, T1, T2, T3, T4>(T3 gparam_0)
	{
		T0 t;
		if (gparam_0 == "pair")
		{
			Process process = Form1.smethod_5();
			ProcessStartInfo processStartInfo = Form1.smethod_4();
			processStartInfo.FileName = Environment.CurrentDirectory + "/files/idevicepair.exe";
			processStartInfo.Arguments = "pair";
			processStartInfo.UseShellExecute = false;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.CreateNoWindow = true;
			process.StartInfo = processStartInfo;
			t = process;
		}
		else
		{
			Process process2 = Form1.smethod_5();
			ProcessStartInfo processStartInfo2 = Form1.smethod_4();
			processStartInfo2.FileName = Environment.CurrentDirectory + "/files/idevicepair.exe";
			processStartInfo2.Arguments = "validate";
			processStartInfo2.UseShellExecute = false;
			processStartInfo2.RedirectStandardOutput = true;
			processStartInfo2.CreateNoWindow = true;
			process2.StartInfo = processStartInfo2;
			t = process2;
		}
		try
		{
			t.Start();
			T2 standardOutput = t.StandardOutput;
			T3 t2 = standardOutput.ReadToEnd();
			Thread.Sleep(2000);
			try
			{
				t.Kill();
			}
			catch
			{
			}
			T1 t3 = t2.Contains("SUCCESS");
			if (t3 != null)
			{
				standardOutput.Dispose();
				return 1;
			}
			return 0;
		}
		catch
		{
		}
		return 0;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x0020E464 File Offset: 0x0020C664
	public void method_13<T0, T1, T2, T3, T4>()
	{
		while (this.method_12<T1, T0, T2, T3, T4>("pair") == 0)
		{
			MessageBox.Show("Unlock device and press Trust", "[TRUST THIS PC]", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x0020E49C File Offset: 0x0020C69C
	public T1 method_14<T0, T1, T2, T3>()
	{
		T0 t = WebRequest.CreateHttp("http://ahmedunllock.tech/windows/AIO_PHPP/GSM-2/gsm_activation_record.php?serial=" + this.string_6);
		t.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
		t.Timeout = 12000;
		T1 t4;
		using (T2 t2 = (T2)((object)t.GetResponse()))
		{
			using (T3 t3 = new StreamReader(t2.GetResponseStream()))
			{
				t4 = t3.ReadToEnd();
			}
		}
		return t4;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0020E52C File Offset: 0x0020C72C
	public T0 method_15<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		return gparam_0.Split(Environment.NewLine.ToCharArray(), gparam_1 + 1).Skip(gparam_1).FirstOrDefault<T0>();
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0020E55C File Offset: 0x0020C75C
	public T1 method_16<T0, T1, T2, T3>(T2 gparam_0, T1 gparam_1, T3 gparam_2 = 4)
	{
		T0 t;
		gparam_0.TryGetValue(gparam_1, out t);
		return this.method_15<T1, T3>(t.ToXmlPropertyList().ToString(), gparam_2).Replace("\n", "").Replace("\r", "")
			.Replace("</data>", "")
			.Replace("</plist>", "")
			.Replace("</string>", "")
			.Replace("<string>", "")
			.Trim();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x0020E5E8 File Offset: 0x0020C7E8
	public void method_17<T0, T1, T2, T3, T4, T5, T6>()
	{
		T0 t = this.method_14<T2, T0, T3, T4>();
		t = t.ToString().Replace("\n", "").Replace("\r", "")
			.Replace("\t", "");
		File.WriteAllText(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/act_rec.plist.tmp", t);
		T1 t2 = (T1)((object)PropertyListParser.Parse(new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/act_rec.plist.tmp")));
		this.string_17 = this.method_16<T5, T0, T1, T6>(t2, "WildcardTicketToRemove", 4);
		t2.Remove("WildcardTicketToRemove");
		PropertyListParser.SaveAsXml(t2, new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/activation_record.plist"));
	}

	// Token: 0x06000051 RID: 81 RVA: 0x0020E6C0 File Offset: 0x0020C8C0
	public void method_18<T0, T1, T2>(T2 gparam_0)
	{
		T0 t = Form1.smethod_5();
		T1 t2 = Form1.smethod_4();
		t2.WindowStyle = ProcessWindowStyle.Hidden;
		t2.FileName = Path.Combine(Environment.CurrentDirectory, "files", "win-plutil.exe");
		T2 t3 = string.Format("\"{0}\"", gparam_0);
		t2.Arguments = "-convert binary1 " + t3;
		t.StartInfo = t2;
		t.Start();
		t.WaitForExit();
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0020E72C File Offset: 0x0020C92C
	public void method_19<T0>()
	{
		T0 t = (T0)((object)PropertyListParser.Parse(new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/com.apple.commcenter.device_specific_nobackup.plist")));
		try
		{
			t.Remove("kPostponementTicket");
		}
		catch
		{
		}
		NSDictionary nsdictionary = t;
		string text = "kPostponementTicket";
		NSDictionary nsdictionary2 = Form1.smethod_6();
		nsdictionary2.Add("ActivationState", "Activated");
		nsdictionary2.Add("ActivationTicket", this.string_17);
		nsdictionary2.Add("ActivityURL", "https://albert.apple.com/deviceservices/activity");
		nsdictionary2.Add("PhoneNumberNotificationURL", "https://albert.apple.com/deviceservices/phoneHome");
		nsdictionary.Add(text, nsdictionary2);
		PropertyListParser.SaveAsXml(t, new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/com.apple.commcenter.device_specific_nobackup.plist"));
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0020E7FC File Offset: 0x0020C9FC
	public void method_20<T0>()
	{
		T0 t = (T0)((object)PropertyListParser.Parse(new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/data_ark.plist")));
		try
		{
			t.Remove("-UCRTOOBForbidden");
			t.Remove("ActivationState");
			t.Remove("-ActivationState");
			t.Add("ActivationState", "Activated");
		}
		catch (Exception)
		{
		}
		PropertyListParser.SaveAsXml(t, new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/data_ark.plist"));
	}

	// Token: 0x06000054 RID: 84 RVA: 0x0020E8A4 File Offset: 0x0020CAA4
	public void method_21<T0, T1, T2, T3>()
	{
		try
		{
			T0 enumerator = this.method_44<SshCommand, T1, StreamReader>("ls /System/Library/PrivateFrameworks/SystemStatusServer.framework").Split(new T3[] { 10 }).ToList<T1>()
				.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					T1 t = enumerator.Current;
					T2 t2 = t.Contains("lproj");
					if (t2 != null)
					{
						this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/broque"), "/System/Library/PrivateFrameworks/SystemStatusServer.framework/" + t + "/SystemStatusServer.strings");
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x0020E95C File Offset: 0x0020CB5C
	public void method_22<T0>()
	{
		this.ECIDValue.Text = "";
		this.DeviceTypeValue.Text = "";
		this.ECIDStatusValue.Text = "";
		this.ModelNameValue.Text = "";
		if (!this.checkBox5.Checked && !this.checkBox9.Checked && !this.checkBox10.Checked && !this.checkBox11.Checked && !this.checkBox12.Checked)
		{
			this.Anno.Text = "Please Connect your device in PwnDFU mode and click on Check Device.";
		}
		else
		{
			this.Anno.Text = "Please Connect your device in Normal mode and click on Check Device.";
		}
		this.actbutt.Text = "Check Device";
		this.feedbacktext.ForeColor = Color.Crimson;
		this.feedbacktext.Text = "No device Connected....";
		this.string_3 = "";
		this.bool_0 = false;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0020EA54 File Offset: 0x0020CC54
	public void method_23<T0, T1, T2, T3>(T3 gparam_0, T3 gparam_1)
	{
		using (T0 t = Ionic.Zip.ZipFile.Read(gparam_0))
		{
			t.Password = "iBoyRamdisk123!@#";
			foreach (T2 t2 in t)
			{
				t2.Extract(gparam_1, ExtractExistingFileAction.OverwriteSilently);
			}
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0020EAC8 File Offset: 0x0020CCC8
	public void method_24<T0>(T0 gparam_0, T0 gparam_1)
	{
		System.IO.Compression.ZipFile.CreateFromDirectory(gparam_0, gparam_1);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x0020EADC File Offset: 0x0020CCDC
	[DebuggerStepThrough]
	public T0 method_25<T0, T1>(T1 gparam_0)
	{
		Form1.Class12 @class = new Form1.Class12();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder<bool>.Create();
		@class.form1_0 = this;
		@class.string_0 = gparam_0;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class12>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0020EB28 File Offset: 0x0020CD28
	[DebuggerStepThrough]
	public T0 method_26<T0>()
	{
		Form1.Class13 @class = new Form1.Class13();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class13>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x0020EB6C File Offset: 0x0020CD6C
	[DebuggerStepThrough]
	public T0 method_27<T0>()
	{
		Form1.Class14 @class = new Form1.Class14();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder<bool>.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class14>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0020EBB0 File Offset: 0x0020CDB0
	public void method_28<T0, T1, T2, T3>(T0 gparam_0)
	{
		T0 t = File.ReadAllText("./files/BackUp/" + this.string_5 + "/" + gparam_0.ToString());
		T1 t2 = Form1.smethod_7();
		t2.LoadXml(t);
		T2 t3 = t2.SelectSingleNode("//key[text()='FairPlayKeyData']");
		T2 nextSibling = t3.NextSibling;
		T0 innerText = nextSibling.InnerText;
		T3[] array = Convert.FromBase64String(innerText);
		T0 @string = Encoding.ASCII.GetString(array);
		T0 t4 = "./files/BackUp/" + this.string_5 + "/FairPlay/iTunes_Control/iTunes/IC-Info.sisv";
		T3[] array2 = Convert.FromBase64String(@string.Replace("-----END CONTAINER-----", "").Replace("-----BEGIN CONTAINER-----", "").Replace("\n", "")
			.Replace("\r", ""));
		File.WriteAllBytes(t4, array2);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x0020EC84 File Offset: 0x0020CE84
	public void method_29<T0, T1, T2, T3, T4>()
	{
		T0 t = File.ReadAllText("./files/BackUp/" + this.string_5 + "/Wildcard.der");
		T1 t2 = Form1.smethod_7();
		t2.Load("./files/BackUp/" + this.string_5 + "/3");
		T2 t3 = t2.SelectSingleNode("/plist/dict/key[text()='kPostponementTicket']/following-sibling::dict/key[text()='WildcardTicket']/following-sibling::string");
		t3.InnerText = t;
		T3 t4 = new XmlSerializer(typeof(T1));
		using (T4 t5 = new StreamWriter("./files/BackUp/" + this.string_5 + "/3"))
		{
			t4.Serialize(t5, t2);
		}
		File.Delete("./files/BackUp/" + this.string_5 + "/Wildcard.der");
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0020ED50 File Offset: 0x0020CF50
	public T0 method_30<T0, T1, T2, T3>(T0 gparam_0)
	{
		T0 t = string.Concat(new T0[] { gparam_0, "?udid=", this.string_7, "&sn=", this.string_6, "&ucid=", this.string_8 });
		T1 t2 = WebRequest.CreateHttp(t);
		t2.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
		t2.Timeout = 12000;
		T0 t5;
		using (T2 t3 = (T2)((object)t2.GetResponse()))
		{
			using (T3 t4 = new StreamReader(t3.GetResponseStream()))
			{
				t5 = t4.ReadToEnd();
			}
		}
		return t5;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0020EE18 File Offset: 0x0020D018
	public void method_31<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
	{
		try
		{
			this.method_30<T3, T4, T5, T6>("https://bytem8.000webhostapp.com/iOS15/activate.php");
			using (T0 t = Form1.smethod_8())
			{
				t.DownloadFile("https://bytem8.000webhostapp.com/iOS15/NewActivation/" + this.string_7 + "/activation_record.plist", Directory.GetCurrentDirectory() + "/files/BackUp/" + this.string_5 + "/1");
			}
			this.progressBar1.Value = 50;
			File.Copy("./files/Tickets/com.apple.commcenter.device_specific_nobackup.plist", "./files/BackUp/" + this.string_5 + "/3", true);
			this.method_28<T3, T7, T8, T9>("1");
			this.method_24<T3>("./files/Backup/" + this.string_5, "./files/Backup/" + this.string_5 + ".zip");
			Directory.Delete("./files/Backup/" + this.string_5, true);
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "Files generated successfully....";
			this.button5.Text = "Close";
			this.label18.Visible = true;
			this.label18.Text = "Congrats! Activation files was generated\ud83c\udf89\ud83c\udf89";
			this.method_1<T10, T3>();
			this.progressBar2.Value = 100 - this.progressBar2.Value;
			MessageBox.Show("Activation files generated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception t2)
		{
			this.progressBar2.Value = 100 - this.progressBar2.Value;
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "Files not generated....Try again!";
			T2 t3 = Directory.Exists("./files/BackUp/" + this.string_5);
			if (t3 != null)
			{
				Directory.Delete("./files/BackUp/" + this.string_5, true);
			}
			MessageBox.Show(t2.Message);
			MessageBox.Show("There was an error generating files\n\nMake sure you are connected to the internet\n\nOr server might be down. try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x0020F034 File Offset: 0x0020D234
	public void method_32<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
	{
		try
		{
			this.method_30<T3, T4, T5, T6>("https://pentaboy.my.id/ios2/activation_pwn.php");
			using (T0 t = Form1.smethod_8())
			{
				t.DownloadFile("https://pentaboy.my.id/ios2/Devices/activation_records/" + this.string_6 + "/activation_record.plist", Directory.GetCurrentDirectory() + "/files/BackUp/" + this.string_5 + "/1");
				t.DownloadFile("https://pentaboy.my.id/ios2/Devices/activation_records/" + this.string_6 + "/Wildcard.der", Directory.GetCurrentDirectory() + "/files/BackUp/" + this.string_5 + "/Wildcard.der");
				t.DownloadFile("https://pentaboy.my.id/ios2/Devices/activation_records/" + this.string_6 + "/com.apple.commcenter.device_specific_nobackup.plist", Directory.GetCurrentDirectory() + "/files/BackUp/" + this.string_5 + "/3");
			}
			this.progressBar1.Value = 50;
			this.method_29<T3, T7, T8, T9, T10>();
			this.method_28<T3, T7, T8, T11>("1");
			this.method_24<T3>("./files/Backup/" + this.string_5, "./files/Backup/" + this.string_5 + ".zip");
			Directory.Delete("./files/Backup/" + this.string_5, true);
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "Files generated successfully....";
			this.button5.Text = "Close";
			this.label18.Visible = true;
			this.label18.Text = "Congrats! Activation files was generated\ud83c\udf89\ud83c\udf89";
			this.method_1<T12, T3>();
			this.progressBar2.Value = 100 - this.progressBar2.Value;
			MessageBox.Show("Activation files generated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception t2)
		{
			this.progressBar2.Value = 100 - this.progressBar2.Value;
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "Files not generated....Try again!";
			T2 t3 = Directory.Exists("./files/BackUp/" + this.string_5);
			if (t3 != null)
			{
				Directory.Delete("./files/BackUp/" + this.string_5, true);
			}
			MessageBox.Show(t2.Message);
			MessageBox.Show("There was an error generating files\n\nMake sure you are connected to the internet\n\nOr server might be down. try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x0020F2A0 File Offset: 0x0020D4A0
	public T2 method_33<T0, T1, T2>()
	{
		T0 length = new FileInfo(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + ".zip").Length;
		T1 t = length / 1024.0;
		T2 t2;
		if (t < 6.0)
		{
			t2 = 0;
		}
		else
		{
			t2 = 1;
		}
		return t2;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0020F2FC File Offset: 0x0020D4FC
	public static T2 smethod_1<T0, T1, T2, T3>(T2 gparam_0, T2 gparam_1, T1 gparam_2 = true, T1 gparam_3 = false)
	{
		Process process = Form1.smethod_5();
		ProcessStartInfo processStartInfo = Form1.smethod_4();
		processStartInfo.FileName = Environment.CurrentDirectory + "/files/irecover.exe";
		processStartInfo.Arguments = gparam_0;
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.CreateNoWindow = true;
		process.StartInfo = processStartInfo;
		T0 t = process;
		t.Start();
		T1 t2 = gparam_2 == 0;
		T2 t3;
		if (t2 != null)
		{
			t3 = "";
		}
		else
		{
			if (gparam_3 == null)
			{
				T2 t5;
				do
				{
					T1 t4 = (t5 = t.StandardOutput.ReadLine()) != null;
					if (t4 == null)
					{
						goto IL_B3;
					}
				}
				while (!t5.Contains(gparam_1));
				return t5;
			}
			if (t.WaitForExit(500) && t.StandardOutput.ReadToEnd().Contains(gparam_1))
			{
				return gparam_1;
			}
			IL_B3:
			t3 = "";
		}
		return t3;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x0020F3C8 File Offset: 0x0020D5C8
	[DebuggerStepThrough]
	public T0 method_34<T0>()
	{
		Form1.Class11 @class = new Form1.Class11();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder<bool>.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class11>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x0020F40C File Offset: 0x0020D60C
	public void method_35<T0>()
	{
		T0 t = Form1.smethod_5();
		t.StartInfo.FileName = Environment.CurrentDirectory + "/files/irecovery.exe";
		t.StartInfo.Arguments = "-n";
		t.StartInfo.UseShellExecute = false;
		t.StartInfo.RedirectStandardOutput = true;
		t.StartInfo.CreateNoWindow = true;
		t.StartInfo.RedirectStandardError = true;
		t.Start();
	}

	// Token: 0x06000064 RID: 100 RVA: 0x0020F480 File Offset: 0x0020D680
	[DebuggerStepThrough]
	public T0 method_36<T0>()
	{
		Form1.Class31 @class = new Form1.Class31();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder<bool>.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class31>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x0020F4C4 File Offset: 0x0020D6C4
	[DebuggerStepThrough]
	public T0 method_37<T0>()
	{
		Form1.Class32 @class = new Form1.Class32();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder<bool>.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class32>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0020F508 File Offset: 0x0020D708
	public T1 method_38<T0, T1, T2>()
	{
		T0 t = Form1.smethod_9();
		return string.Format("bgcolor {0} {1} {2}", t.Next(256), t.Next(256), t.Next(256));
	}

	// Token: 0x06000067 RID: 103 RVA: 0x0020F558 File Offset: 0x0020D758
	[DebuggerStepThrough]
	public T0 method_39<T0, T1>(T1 gparam_0)
	{
		Form1.Class15 @class = new Form1.Class15();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder.Create();
		@class.form1_0 = this;
		@class.list_0 = gparam_0;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class15>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0020F5A4 File Offset: 0x0020D7A4
	public void method_40()
	{
		this.startDFU.Enabled = true;
		this.pressDFU.Enabled = false;
		this.label20.Enabled = false;
		this.sideButton678Lab.Visible = false;
		this.volumeDown78XLab.Visible = false;
		this.sideButton678Lab.Enabled = true;
		this.homeButtLab.Enabled = true;
		this.volumeDown78XLab.Enabled = true;
		this.label22.Enabled = true;
		this.label22.Visible = false;
		this.homeButtLab.Visible = false;
		this.button9.Visible = true;
		this.button7.Text = "Start";
		this.label24.Text = "Time to put the device into DFU mode. Locate the buttons as marked below on\r\n";
		this.label23.Visible = true;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0020F670 File Offset: 0x0020D870
	[DebuggerStepThrough]
	public T0 method_41<T0>()
	{
		Form1.Class23 @class = new Form1.Class23();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class23>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0020F6B4 File Offset: 0x0020D8B4
	[DebuggerStepThrough]
	public T0 method_42<T0>()
	{
		Form1.Class22 @class = new Form1.Class22();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class22>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x0020F6F8 File Offset: 0x0020D8F8
	public void method_43<T0, T1, T2>(T1 gparam_0)
	{
		Form1.Class9 @class = new Form1.Class9();
		@class.timer_0 = Form1.smethod_10();
		if (gparam_0 != null && gparam_0 >= 0)
		{
			@class.timer_0.Interval = gparam_0;
			@class.timer_0.Enabled = true;
			@class.timer_0.Start();
			@class.timer_0.Tick += @class.method_0<object, EventArgs>;
			while (@class.timer_0.Enabled)
			{
				Application.DoEvents();
			}
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0020F774 File Offset: 0x0020D974
	public static T4 smethod_2<T0, T1, T2, T3, T4, T5, T6>(T4 gparam_0)
	{
		T0 t = Activator.CreateInstance(typeof(T0));
		T1[] byte_ = Class34.Byte_0;
		using (T2 t2 = new MemoryStream(byte_))
		{
			using (T3 t3 = new StreamReader(t2))
			{
				T4 t4;
				while ((t4 = t3.ReadLine()) != null)
				{
					T5 t5 = t4.StartsWith(gparam_0);
					if (t5 != null)
					{
						T4[] array = t4.Split(new T6[] { 9 }, StringSplitOptions.RemoveEmptyEntries);
						t.Add(array[1]);
					}
				}
			}
		}
		T4 t7;
		if (t.Count != 0)
		{
			T5 t6 = t.Count < 2 || t[0] == t[t.Count - 1];
			if (t6 != null)
			{
				t7 = t[0];
			}
			else
			{
				t7 = t[0] + "-" + t[t.Count - 1];
			}
		}
		else
		{
			t7 = "x.x.x";
		}
		return t7;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0020F890 File Offset: 0x0020DA90
	public T1 method_44<T0, T1, T2>(T1 gparam_0)
	{
		T0 t = this.sshClient_0.CreateCommand(gparam_0);
		T1 t3;
		try
		{
			t.BeginExecute();
			T2 t2 = new StreamReader(t.OutputStream);
			t3 = t2.ReadToEnd();
		}
		catch
		{
			t3 = "";
		}
		return t3;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0020F8E4 File Offset: 0x0020DAE4
	public void method_45<T0, T1, T2, T3, T4, T5>(T2 gparam_0)
	{
		try
		{
			this.sshClient_0.CreateCommand("chmod -R 777 /" + gparam_0 + "/mobile/Library/FairPlay/iTunes_Control/iTunes/IC-Info.sisv").Execute();
			this.sshClient_0.CreateCommand("chmod -R 777 /" + gparam_0 + "/mobile/Library/FairPlay").Execute();
			this.scpClient_0.Download("/" + gparam_0 + "/mobile/Library/FairPlay", new DirectoryInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/FairPlay/"));
			this.sshClient_0.CreateCommand("chmod 755 " + gparam_0 + "/mobile/Library/FairPlay").Execute();
		}
		catch
		{
			try
			{
				this.sshClient_0.CreateCommand("chmod 755 /" + gparam_0 + "/mobile/Library/FairPlay").Execute();
				this.sshClient_0.CreateCommand("chmod -R 777 /" + gparam_0 + "/mobile/Library/FairPlay").Execute();
				this.sshClient_0.CreateCommand("chmod -R 777 /" + gparam_0 + "/mobile/Library/FairPlay/iTunes_Control/iTunes/IC-Info.sisv").Execute();
				this.sshClient_0.CreateCommand("chmod +x /" + gparam_0 + "/mobile/Library/FairPlay").Execute();
				this.scpClient_0.Download("/" + gparam_0 + "/mobile/Library/FairPlay", new DirectoryInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/FairPlay/"));
				this.sshClient_0.CreateCommand("chmod 755 /" + gparam_0 + "/mobile/Library/FairPlay").Execute();
			}
			catch
			{
				T0 t = Form1.smethod_5();
				t.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "files", "plistutil.exe");
				t.StartInfo.Arguments = string.Concat(new T2[]
				{
					"-i \"",
					Path.Combine(new T2[]
					{
						Environment.CurrentDirectory,
						"files",
						"BackUp",
						this.string_5,
						"1"
					}),
					"\" -o \"",
					Path.Combine(new T2[]
					{
						Environment.CurrentDirectory,
						"files",
						"BackUp",
						this.string_5,
						"4"
					}),
					"\""
				});
				t.StartInfo.UseShellExecute = false;
				t.StartInfo.CreateNoWindow = true;
				t.Start();
				t.WaitForExit();
				this.method_28<T2, T3, T4, T5>("4");
				T1 t2 = File.Exists(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + "/4");
				if (t2 != null)
				{
					File.Delete(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + "/4");
				}
			}
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0020FBE8 File Offset: 0x0020DDE8
	public void method_46<T0>()
	{
		T0 t = Form1.smethod_5();
		t.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "files", "idevicediagnostics.exe");
		t.StartInfo.Arguments = "restart";
		t.StartInfo.UseShellExecute = false;
		t.StartInfo.CreateNoWindow = true;
		t.Start();
		t.WaitForExit();
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0020FC50 File Offset: 0x0020DE50
	public void method_47()
	{
		Directory.Delete(Environment.CurrentDirectory + "/tmp", true);
		Directory.CreateDirectory(Environment.CurrentDirectory + "/tmp");
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0020FC88 File Offset: 0x0020DE88
	public void method_48<T0, T1, T2, T3>()
	{
		T0 t = string.Concat(new T0[]
		{
			"https://hasnit3ch.com/Backup/Ramdisktool/Files/MDM68/OFFLINE.php?serial=", this.string_6, "&uuid=", this.string_7, "&type=", this.string_13, "&ver=", this.string_16, "&ime=", this.string_11,
			"&build=", this.string_19
		});
		T1 t2 = WebRequest.CreateHttp(t);
		t2.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
		t2.Timeout = 12000;
		using (T2 t3 = (T2)((object)t2.GetResponse()))
		{
			using (T3 t4 = new StreamReader(t3.GetResponseStream()))
			{
				t4.ReadToEnd();
			}
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0020FD7C File Offset: 0x0020DF7C
	public void method_49<T0, T1>()
	{
		T0 t = "https://hasnit3ch.com/Backup/Ramdisktool/Files/MDM68/MDM.zip";
		using (T1 t2 = Form1.smethod_8())
		{
			t2.DownloadFile(t, Environment.CurrentDirectory + "/tmp/MDM.zip");
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0020FDC8 File Offset: 0x0020DFC8
	public void method_50<T0, T1, T2, T3, T4, T5>()
	{
		T0[] byte_ = Class34.Byte_1;
		T1 t = Environment.CurrentDirectory + "/tmp/ffe2017db9c5071adfa1c23d3769970f7524a9d4";
		using (T2 t2 = new MemoryStream(byte_))
		{
			using (T3 t3 = new ZipArchive(t2))
			{
				foreach (T5 t4 in t3.Entries)
				{
					T1 t5 = Path.Combine(t, t4.FullName);
					Directory.CreateDirectory(Path.GetDirectoryName(t5));
					t4.ExtractToFile(t5, true);
				}
			}
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x0020FE90 File Offset: 0x0020E090
	public void method_51<T0>()
	{
		File.Delete(Environment.CurrentDirectory + "/tmp/ffe2017db9c5071adfa1c23d3769970f7524a9d4/Info.plist");
		File.Delete(Environment.CurrentDirectory + "/tmp/ffe2017db9c5071adfa1c23d3769970f7524a9d4/Manifest.plist");
		T0 t = string.Concat(new T0[]
		{
			"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\n<plist version=\"1.0\">\n<dict>\n\t<key>Build Version</key>\n\t<string>",
			this.string_19,
			"</string>\n\t<key>Device Name</key>\n\t<string>iDevice</string>\n\t<key>Display Name</key>\n\t<string>iDevice</string>\n\t<key>GUID</key>\n\t<string>FF2C7E0CDE08E1EF5B4EDF4770D46349</string>\n\t<key>ICCID</key>\n\t<string>89441000300416431124</string>\n\t<key>IMEI</key>\n\t<string>",
			this.string_11,
			"</string>\n\t<key>Installed Applications</key>\n\t<array>\n\t</array>\n\t<key>Last Backup Date</key>\n\t<date>2016-01-09T01:10:10Z</date>\n\t<key>Product Name</key>\n\t<string>iDevice</string>\n\t<key>Product Type</key>\n\t<string>",
			this.string_13,
			"</string>\n\t<key>Product Version</key>\n\t<string>",
			this.string_16,
			"</string>\n\t<key>Serial Number</key>\n\t<string>",
			this.string_6,
			"</string>\n\t<key>Target Identifier</key>\n\t<string>",
			this.string_7,
			"</string>\n\t<key>Target Type</key>\n\t<string>Device</string>\n\t<key>Unique Identifier</key>\n\t<string>",
			this.string_7.ToUpper(),
			"</string>\n\t<key>iTunes Files</key>\n\t<dict>\n\t\t<key>IC-Info.sidv</key>\n\t\t<data>\n\t\tAAEAAWwwhNpdjI/0+tuCaf3Lr/a0/jxxO6hTrdgMgG2hg+1TK4swDZuqZmlT\n\t\tO/pC6NiVwaNPASlv7d71w/GrriC4AoY4wME4BEGrLpERbf9QU+46j00isbzG\n\t\t2/Y2Nb6O+TQwCSSyIYtvyhd8J4DRD5Ny2q+Lp7R0UwIe1TRcYkMzG2kZIMQ8\n\t\tdLhfhNqQBogEu9Lch2f6344xPJU/qnarWEo/G7MzfS/Ldo5B0xcax0OKewe/\n\t\tVFZ/FyicA7ooch/wPTpw500xOmJkpNit/BZkHzZ7ypCrZ15e6zQVZynS7mc2\n\t\t+YAIZPJ/VQRukfCSUEZljHzhA5wBKKhHJVsF02p3xlP5kdqXCeJwugaUNWbA\n\t\t6swdykFRAntp2goem0pKjIfGZHKYPaBnHWAtZk0H8eb+0n25a+xF4V0EaCr8\n\t\tT5EYWGrKfSufBSRx7cMN+vMBaP2/WkRX5vEGCs2StmxfKFrC9UwmLZaOGK1u\n\t\tySjY5ucGUxf4NQ6GPe7oXZsxQlmHjY7h2blLWLUEK7BlN8pghlRrmZBESh8x\n\t\tjJgaEHzI86wA8CVAAcj2xtFUY3pIU8aISY5mgkKTkHZHrsbW56SphD81uzUc\n\t\t/QKwx4BaTra+MGG0KgwJolHnHhlKKXm5Rc0Wboi+/b0pDeiadNyLMievAgsu\n\t\tB2Q8imWKzpVX5xa7WXB8Nq/AxS6fWtKf6pIheuFGgj3vpoNe/GBN9L4WPAuk\n\t\tYDA+HReZGJ/3ywTkUODN8JwcJlYYV2zKgB+uUKJIhPlcHh2Fs4c6Bdlw0A/d\n\t\tAd0mcVGOCtKkR4AIpzNQWAvYGOTyzOcidZ+snpwxEk/sZIGR36tCBTlc6sw+\n\t\tGhUqJUrvdSB92gYgrYwfZnFEewb+A1h0tiQ8hnh9JIDIs5BLAIYDGkB899Fa\n\t\tYtj5mmtco8e6zUtnMO6Vk7E2szktGVqMrHY/wyLBW/Pr6ZVG26A8zQrq5Lpo\n\t\tRBIHMH6ZWjSORcqdosurTGzoiM0RHSEKUgRW4yFfZtQoLLNO5cy7ra39Ev9u\n\t\tM3vAb12SC1KiFKGeOezYX3SyFWgANhILat95lkDgxeGmg2LZ5X3F0r+TxYJB\n\t\tu1WlJybLya51uzWTje6Z+BiYTOT+VC7sRySNJ8HBr+ICQG5gICS1NTlNgvIz\n\t\tWh9F/An9y1UM5DCPrPL4CZjIgd0qxI+FSDK404//n2XZag3LlbtgU7IOm9H0\n\t\tsdjzyd5WlBriT77NSyuO+sVqxpu6vEKyZWn/+fvbfJ558gj95TXZ5cBTBX4J\n\t\tMd5M8qQZwIKyJjkHVKLTox7oHByT/xRDy+Rtr8eTAJ1vBvVrL/9hp34TIKFr\n\t\tZiiWVosADUij6zFHUq3GtUow7KtT8JqfUWreQ9++yOXPSJRmhr0LU3mvzXKk\n\t\tMRBEuo8QdenPUAH9FyrQbaul3fyJOctqDcdt/hs1PIi4NnHBBF78AZk+Blaa\n\t\tqyPVCFQQ7ouUPS7aSCanSG5vg0kk6be7t67GmLN/wZ5fdno7hHIR9cMX1Qjn\n\t\tuwBfMwo6VJ361Ue+jg0m\n\t\t</data>\n\t\t<key>VoiceMemos.plist</key>\n\t\t<data>\n\t\tPD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPCFET0NU\n\t\tWVBFIHBsaXN0IFBVQkxJQyAiLS8vQXBwbGUvL0RURCBQTElTVCAxLjAvL0VO\n\t\tIiAiaHR0cDovL3d3dy5hcHBsZS5jb20vRFREcy9Qcm9wZXJ0eUxpc3QtMS4w\n\t\tLmR0ZCI+CjxwbGlzdCB2ZXJzaW9uPSIxLjAiPgo8ZGljdC8+CjwvcGxpc3Q+\n\t\tCg==\n\t\t</data>\n\t\t<key>iTunesPrefs</key>\n\t\t<data>\n\t\tZnJwZAEAGAABAQABcM3kScC709TQ0F329vRlpgAAAAAAAAABAAAAAAAAAAAA\n\t\tAAAAAQAAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAD9TjewHrt4rwAB\n\t\tAAQAAAAAcM3kScC709QCAAAAAAAAAAAAAAAAAAAAAAABAAAAAAAAAAABAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAEBAAAAAAAAAAAAAAEAAQABAAEAAAAA\n\t\tAAAAAAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAEdFaG+OxBBX6WCavcHd8XWo\n\t\tJyfdAAAAAAAAAAAAAAAAAAAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAEBAAQAAAAABwAAAAAAAQABAAAAAQEBgAAAAAAAAQAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU2NyZXcgTG9vc2UAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFND\n\t\tUkVXTE9PU0UtUEMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU2NyZXcgTG9v\n\t\tc2UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAFNDUkVXTE9PU0UtUEMAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABTY3JldyBMb29zZQAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAU0NSRVdMT09TRS1QQwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAABTY3JldyBMb29zZQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU0NSRVdMT09TRS1QQwAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\n\t\tAAAAAAAAAAAAAAAAAAAAAAA=\n\t\t</data>\n\t\t<key>iTunesPrefs.plist</key>\n\t\t<data>\n\t\tPD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPCFET0NU\n\t\tWVBFIHBsaXN0IFBVQkxJQyAiLS8vQXBwbGUvL0RURCBQTElTVCAxLjAvL0VO\n\t\tIiAiaHR0cDovL3d3dy5hcHBsZS5jb20vRFREcy9Qcm9wZXJ0eUxpc3QtMS4w\n\t\tLmR0ZCI+CjxwbGlzdCB2ZXJzaW9uPSIxLjAiPgo8ZGljdD4KCTxrZXk+QXBw\n\t\tbGljYXRpb25JRHM8L2tleT4KCTxhcnJheS8+Cgk8a2V5PkF1ZGlvYm9va1Bs\n\t\tYXlsaXN0SURzPC9rZXk+Cgk8YXJyYXkvPgoJPGtleT5BdWRpb2Jvb2tUcmFj\n\t\ta0lEczwva2V5PgoJPGFycmF5Lz4KCTxrZXk+Qm9va1RyYWNrSURzPC9rZXk+\n\t\tCgk8YXJyYXkvPgoJPGtleT5MaWJyYXJ5Qm9va1RyYWNrSURzPC9rZXk+Cgk8\n\t\tYXJyYXkvPgoJPGtleT5Nb3ZpZVBsYXlsaXN0SURzPC9rZXk+Cgk8YXJyYXkv\n\t\tPgoJPGtleT5Nb3ZpZVRyYWNrSURzPC9rZXk+Cgk8YXJyYXkvPgoJPGtleT5N\n\t\tdXNpY0FsYnVtSURzPC9rZXk+Cgk8YXJyYXkvPgoJPGtleT5NdXNpY0FydGlz\n\t\tdElEczwva2V5PgoJPGFycmF5Lz4KCTxrZXk+TXVzaWNHZW5yZU5hbWVzPC9r\n\t\tZXk+Cgk8YXJyYXkvPgoJPGtleT5NdXNpY1BsYXlsaXN0SURzPC9rZXk+Cgk8\n\t\tYXJyYXkvPgoJPGtleT5NdXNpY1RyYWNrSURzPC9rZXk+Cgk8YXJyYXkvPgoJ\n\t\tPGtleT5Qb2RjYXN0Q2hhbm5lbElEczwva2V5PgoJPGFycmF5Lz4KCTxrZXk+\n\t\tUG9kY2FzdFBsYXlsaXN0SURzPC9rZXk+Cgk8YXJyYXkvPgoJPGtleT5Qb2Rj\n\t\tYXN0VHJhY2tJRHM8L2tleT4KCTxhcnJheS8+Cgk8a2V5PlJpbmd0b25lVHJh\n\t\tY2tJRHM8L2tleT4KCTxhcnJheS8+Cgk8a2V5PlRWU2hvd0FsYnVtSURzPC9r\n\t\tZXk+Cgk8YXJyYXkvPgoJPGtleT5UVlNob3dOYW1lczwva2V5PgoJPGFycmF5\n\t\tLz4KCTxrZXk+VFZTaG93UGxheWxpc3RJRHM8L2tleT4KCTxhcnJheS8+Cgk8\n\t\ta2V5PlRWU2hvd1RyYWNrSURzPC9rZXk+Cgk8YXJyYXkvPgoJPGtleT5pUG9k\n\t\tUHJlZnM8L2tleT4KCTxkYXRhPgoJWm5Kd1pBRUFHQUFCQVFBQmNNM2tTY0M3\n\t\tMDlUUTBGMzI5dlJscGdBQUFBQUFBQUFCQUFBQUFBQUFBQUFBQUFBQUFRQUEK\n\t\tCUFBQUNBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQVFBQUFBRDlUamV3\n\t\tSHJ0NHJ3QUJBQVFBQUFBQWNNM2tTY0M3CgkwOVFDQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFCQUFBQUFBQUFBQUFCQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQQoJQWdBQUFBRUJBQUFBQUFBQUFBQUFBQUVBQVFBQkFBRUFBQUFBQUFB\n\t\tQUFBQUFBQUlBQUFBQUFBQUFBQUFBQUFBQUFBQUEKCUFBQUFBRWRGYUcrT3hC\n\t\tQlg2V0NhdmNIZDhYV29KeWZkQUFBQUFBQUFBQUFBQUFBQUFBQUFBUDhBQUFB\n\t\tQUFBQUFBQUFBCglBQUFBQUFBQUFBQUFBQUFBQUFBQUFBRUJBQVFBQUFBQUJ3\n\t\tQUFBQUFBQVFBQkFBQUFBUUVCZ0FBQUFBQUFBUUFBQUFBQQoJQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUEKCUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQVUyTnlaWGNnVEc5dmMyVUFBQUFBQUFBQUFBQUFBQUFBCglBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUZORFVrVlhURTlQVTBVdAoJVUVNQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUEK\n\t\tCUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBCglBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQVUyTnlaWGNnVEc5dmMyVUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQQoJQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUZO\n\t\tRFVrVlhURTlQVTBVdFVFTUFBQUFBQUFBQUFBQUEKCUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUJUWTNK\n\t\tbGR5Qk1iMjl6CglaUUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQQoJQUFBQVUwTlNS\n\t\tVmRNVDA5VFJTMVFRd0FBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUEKCUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBCglBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQQoJQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUEK\n\t\tCUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBCglBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQQoJQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUJUWTNKbGR5Qk1iMjl6WlFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUEKCUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFVME5TUlZkTVQwOVRSUzFRUXdB\n\t\tQUFBQUFBQUFBCglBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFB\n\t\tQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQQoJQUFBQUFBQUFB\n\t\tQUE9Cgk8L2RhdGE+Cgk8a2V5PmlUdW5lc1VDaGFubmVsSURzPC9rZXk+Cgk8\n\t\tYXJyYXkvPgoJPGtleT5pVHVuZXNVUGxheWxpc3RJRHM8L2tleT4KCTxhcnJh\n\t\teS8+Cgk8a2V5PmlUdW5lc1VUcmFja0lEczwva2V5PgoJPGFycmF5Lz4KCTxr\n\t\tZXk+aVR1bmVzVmVyc2lvbjwva2V5PgoJPHN0cmluZz4xMi4xLjE8L3N0cmlu\n\t\tZz4KPC9kaWN0Pgo8L3BsaXN0Pgo=\n\t\t</data>\n\t</dict>\n\t<key>iTunes Settings</key>\n\t<dict>\n\t</dict>\n\t<key>iTunes Version</key>\n\t<string>12.1.1.4</string>\n</dict>\n\n</plist>\n"
		});
		T0 t2 = string.Concat(new T0[]
		{
			"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\n<plist version=\"1.0\">\n<dict>\n\t<key>SystemDomainsVersion</key>\n\t<string>20.0</string>\n\t<key>Applications</key>\n\t<dict>\n\t\t<key>com.apple.WebViewService</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.WebViewService</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/E4FCD42A-BC3F-4E1C-AC6C-6D9932E23A04/WebViewService.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>1.0</string>\n\t\t</dict>\n\t\t<key>com.apple.weather</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.weather</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/E33E843B-0B2B-4F9C-B1C8-5C239BCFA34F/Weather.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>1.0</string>\n\t\t</dict>\n\t\t<key>com.apple.datadetectors.DDActionsService</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.datadetectors.DDActionsService</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/798F78FC-74FD-48AF-8C7E-552CED6C6F09/DDActionsService.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>109.1</string>\n\t\t</dict>\n\t\t<key>com.apple.webapp</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.webapp</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/05B79610-7ECE-481C-A97F-86FD7A39924C/Web.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>1.0</string>\n\t\t</dict>\n\t\t<key>com.apple.mobilesafari</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.mobilesafari</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/AA4CF771-2B1C-4586-A4E8-0BC262FB4A64/MobileSafari.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>9537.53</string>\n\t\t</dict>\n\t\t<key>com.apple.mobilemail</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.mobilemail</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/4BA96C3F-A486-4C5F-A40A-8EC7B97185C7/MobileMail.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>53</string>\n\t\t</dict>\n\t\t<key>com.apple.WebContentFilter.remoteUI.WebContentAnalysisUI</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.WebContentFilter.remoteUI.WebContentAnalysisUI</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/2301C014-F368-49A4-B061-FB7C2EC5072F/WebContentAnalysisUI.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>1.0</string>\n\t\t</dict>\n\t\t<key>com.apple.calculator</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.calculator</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/5C23AE54-B1C1-4A36-91D4-F5EC51FA4C7A/Calculator.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>1.0</string>\n\t\t</dict>\n\t\t<key>com.apple.ios.StoreKitUIService</key>\n\t\t<dict>\n\t\t\t<key>CFBundleIdentifier</key>\n\t\t\t<string>com.apple.ios.StoreKitUIService</string>\n\t\t\t<key>Path</key>\n\t\t\t<string>/private/var/mobile/Applications/91AE728C-760A-4D79-ADB8-E0564AA50682/StoreKitUIService.app</string>\n\t\t\t<key>CFBundleVersion</key>\n\t\t\t<string>1.0</string>\n\t\t</dict>\n\t</dict>\n\t<key>Lockdown</key>\n\t<dict>\n\t\t<key>ProductVersion</key>\n\t\t<string>", this.string_16, "</string>\n\t\t<key>com.apple.TerminalFlashr</key>\n\t\t<dict>\n\t\t</dict>\n\t\t<key>com.apple.mobile.data_sync</key>\n\t\t<dict>\n\t\t</dict>\n\t\t<key>BuildVersion</key>\n\t\t<string>", this.string_19, "</string>\n\t\t<key>com.apple.mobile.iTunes.accessories</key>\n\t\t<dict>\n\t\t</dict>\n\t\t<key>com.apple.mobile.wireless_lockdown</key>\n\t\t<dict>\n\t\t</dict>\n\t\t<key>DeviceName</key>\n\t\t<string>iDevice</string>\n\t\t<key>com.apple.Accessibility</key>\n\t\t<dict>\n\t\t\t<key>MonoAudioEnabledByiTunes</key>\n\t\t\t<false/>\n\t\t\t<key>ZoomTouchEnabledByiTunes</key>\n\t\t\t<false/>\n\t\t\t<key>SpeakAutoCorrectionsEnabledByiTunes</key>\n\t\t\t<false/>\n\t\t\t<key>InvertDisplayEnabledByiTunes</key>\n\t\t\t<false/>\n\t\t\t<key>VoiceOverTouchEnabledByiTunes</key>\n\t\t\t<false/>\n\t\t\t<key>ClosedCaptioningEnabledByiTunes</key>\n\t\t\t<false/>\n\t\t</dict>\n\t\t<key>SerialNumber</key>\n\t\t<string>", this.string_6, "</string>\n\t\t<key>com.apple.MobileDeviceCrashCopy</key>\n\t\t<dict>\n\t\t</dict>\n\t\t<key>ProductType</key>\n\t\t<string>", this.string_13, "</string>\n\t\t<key>UniqueDeviceID</key>\n\t\t<string>", this.string_7,
			"</string>\n\t</dict>\n\t<key>Version</key>\n\t<string>9.1</string>\n\t<key>IsEncrypted</key>\n\t<false/>\n\t<key>BackupKeyBag</key>\n\t<data>\n\tVkVSUwAAAAQAAAADVFlQRQAAAAQAAAABVVVJRAAAABBk9LZRmWFFi6rkXZI+VyTkSE1D\n\tSwAAACgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAV1JBUAAA\n\tAAQAAAAAU0FMVAAAABRKcoazIFe+umUnatxygUAZgA77jklURVIAAAAEAAAnEFVVSUQA\n\tAAAQLFa0YJd/QveoMPBSrzr/EUNMQVMAAAAEAAAAC1dSQVAAAAAEAAAAA0tUWVAAAAAE\n\tAAAAAFdQS1kAAAAoS4ZC7AEiKRxD+ERiobPIy9XOZ8R29wnYiifRTtbBucuoc1M0P8sQ\n\tCFVVSUQAAAAQderx0B8pQg+qN0M5+t4iWENMQVMAAAAEAAAACldSQVAAAAAEAAAAA0tU\n\tWVAAAAAEAAAAAFdQS1kAAAAoiLojrem2fT8R6KuVu0zS6eZTvG93VtwuT/9rIr0judq0\n\to6ziW0traFVVSUQAAAAQ6SK0kBP2Qi2u+VWoUqFGkENMQVMAAAAEAAAACVdSQVAAAAAE\n\tAAAAA0tUWVAAAAAEAAAAAFdQS1kAAAAoNuxfETOAHV27ez3eQ62LvCFwjj4rtiS/a6hi\n\t7kQJgqL4P4uO/AUdTVVVSUQAAAAQt04hAtaETsig+4gnTtIx0kNMQVMAAAAEAAAACFdS\n\tQVAAAAAEAAAAA0tUWVAAAAAEAAAAAFdQS1kAAAAoJ9aNkWwfH3bi5a4X+glH1TsMzjPs\n\tyWM3btTpyeaMKfuX/Jc2GmvHR1VVSUQAAAAQaBjAejNCQ8unT62TexmRVENMQVMAAAAE\n\tAAAAB1dSQVAAAAAEAAAAA0tUWVAAAAAEAAAAAFdQS1kAAAAoxLTWAS4Iu7UOx6SLg43v\n\tnRbi5Q56EJD9dd4RDF5gvMLWeNy2dgoRt1VVSUQAAAAQCtspNy1BS5qcm6KB4iBaG0NM\n\tQVMAAAAEAAAABldSQVAAAAAEAAAAA0tUWVAAAAAEAAAAAFdQS1kAAAAoKV/y7Gz+HMvf\n\tO33L740P3RJ/Mq1vDgW1eTzF6EbbkUyW7kKqlsREelVVSUQAAAAQLzL0NDPdRvOaZCyw\n\tjpYkd0NMQVMAAAAEAAAABVdSQVAAAAAEAAAAA0tUWVAAAAAEAAAAAFdQS1kAAAAoUEgq\n\thReM+yxo+G1Ly9XTDzF/n9knFfpg1MKxx4UExXTyMI30sH38jlVVSUQAAAAQTf1U1IhX\n\tQleQignFuSbHeUNMQVMAAAAEAAAABFdSQVAAAAAEAAAAAktUWVAAAAAEAAAAAFdQS1kA\n\tAAAo9k+AlAkP/wLlupXwFALyJZbNa3nugp9nsC/UaVvuQdL3qt2kxL7HDlVVSUQAAAAQ\n\t+qgcAbbuSAGGzSStYBexDUNMQVMAAAAEAAAAA1dSQVAAAAAEAAAAAktUWVAAAAAEAAAA\n\tAFdQS1kAAAAoG95Y+8zthmowg+dyJbOaKjDfJSVtUute6yn2NiFzkgxeu/7vpQJCvlVV\n\tSUQAAAAQdjdjbrDST0OIAEekLfX6+0NMQVMAAAAEAAAAAldSQVAAAAAEAAAAAktUWVAA\n\tAAAEAAAAAFdQS1kAAAAoCo6pD1UUGFkv9J77rE76AqzKVEvYQ8zKSsKIg1JNV8aWRdXg\n\t20WmVlVVSUQAAAAQcuf0U9kyQcOJqJso2USjq0NMQVMAAAAEAAAAAVdSQVAAAAAEAAAA\n\tAktUWVAAAAAEAAAAAFdQS1kAAAAorSXlXwp+hi7dPsRZALMPbaR47hAXq03JKWPMjrzh\n\tyhkpUTjE0E3U2A==\n\t</data>\n\t<key>WasPasscodeSet</key>\n\t<false/>\n\t<key>Date</key>\n\t<date>2016-01-09T01:10:07Z</date>\n</dict>\n</plist>\n"
		});
		File.WriteAllText(Environment.CurrentDirectory + "/tmp/ffe2017db9c5071adfa1c23d3769970f7524a9d4/Info.plist", t);
		File.WriteAllText(Environment.CurrentDirectory + "/tmp/ffe2017db9c5071adfa1c23d3769970f7524a9d4/Manifest.plist", t2);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0020FFF4 File Offset: 0x0020E1F4
	public void method_52<T0, T1, T2, T3>()
	{
		T0 t = Form1.smethod_5();
		Process process = t;
		T2 t2 = Form1.smethod_4();
		t2.FileName = Environment.CurrentDirectory + "/files/idevicebackup2.exe";
		t2.Arguments = string.Concat(new T1[]
		{
			"restore -u ",
			this.string_7,
			" -s ffe2017db9c5071adfa1c23d3769970f7524a9d4 --system --reboot --settings \"",
			Path.Combine(Environment.CurrentDirectory, "tmp"),
			"\""
		});
		t2.UseShellExecute = false;
		t2.RedirectStandardOutput = true;
		t2.CreateNoWindow = true;
		process.StartInfo = t2;
		T0 t3 = t;
		t3.Start();
		T1 t4 = t3.StandardOutput.ReadToEnd();
		T3 t5 = t4.Contains("com.apple.mobilebackup2.");
		if (t5 != null)
		{
			throw new Exception("Device could not be activated, Maybe FMI is turned on.");
		}
		T3 t6 = t4.Contains("disabled");
		if (t6 != null)
		{
			throw new Exception("Please disable FMI before proceeding.");
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x002100D0 File Offset: 0x0020E2D0
	public async void method_53()
	{
		await Task.Run(new Action(this.method_106<Exception, bool, Process, StreamReader, string, ProcessStartInfo, int, System.Windows.Forms.Timer, byte, MemoryStream, ZipArchive, IEnumerator<ZipArchiveEntry>, ZipArchiveEntry>));
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0021010C File Offset: 0x0020E30C
	public void method_54<T0, T1, T2, T3>()
	{
		T0 @checked = this.checkBox3.Checked;
		if (@checked != null)
		{
			this.string_15 = "disk1";
		}
		else
		{
			this.string_15 = "disk0s1";
		}
		this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s1 /mnt1").Execute();
		T0 t = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s1").Trim() == "Preboot";
		if (t != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s1 /mnt6").Execute();
		}
		T0 t2 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s2").Trim() == "Preboot";
		if (t2 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s2 /mnt6").Execute();
		}
		T0 t3 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s3").Trim() == "Preboot";
		if (t3 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s3 /mnt6").Execute();
		}
		T0 t4 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s4").Trim() == "Preboot";
		if (t4 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s4 /mnt6").Execute();
		}
		T0 t5 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s5").Trim() == "Preboot";
		if (t5 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s5 /mnt6").Execute();
		}
		T0 t6 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s6").Trim() == "Preboot";
		if (t6 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s6 /mnt6").Execute();
		}
		T0 t7 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s7").Trim() == "Preboot";
		if (t7 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s7 /mnt6").Execute();
		}
		T0 t8 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s1").Trim() == "xART";
		if (t8 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s1 /mnt7").Execute();
		}
		T0 t9 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s2").Trim() == "xART";
		if (t9 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s2 /mnt7").Execute();
		}
		T0 t10 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s3").Trim() == "xART";
		if (t10 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s3 /mnt7").Execute();
		}
		T0 t11 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s4").Trim() == "xART";
		if (t11 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s4 /mnt7").Execute();
		}
		T0 t12 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s5").Trim() == "xART";
		if (t12 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s5 /mnt7").Execute();
		}
		T0 t13 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s6").Trim() == "xART";
		if (t13 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s6 /mnt7").Execute();
		}
		T0 t14 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s7").Trim() == "xART";
		if (t14 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s7 /mnt7").Execute();
		}
		T0 t15 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s1").Trim() == "Hardware";
		if (t15 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s1 /mnt5").Execute();
		}
		T0 t16 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s2").Trim() == "Hardware";
		if (t16 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s2 /mnt5").Execute();
		}
		T0 t17 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s3").Trim() == "Hardware";
		if (t17 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s3 /mnt5").Execute();
		}
		T0 t18 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s4").Trim() == "Hardware";
		if (t18 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s4 /mnt5").Execute();
		}
		T0 t19 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s5").Trim() == "Hardware";
		if (t19 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s5 /mnt5").Execute();
		}
		T0 t20 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s6").Trim() == "Hardware";
		if (t20 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s6 /mnt5").Execute();
		}
		T0 t21 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s7").Trim() == "Hardware";
		if (t21 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s7 /mnt5").Execute();
		}
		T0 t22 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s4").Trim() == "Baseband Data";
		if (t22 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s4 /mnt4").Execute();
		}
		T0 t23 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s5").Trim() == "Baseband Data";
		if (t23 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s5 /mnt4").Execute();
		}
		T0 t24 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s6").Trim() == "Baseband Data";
		if (t24 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s6 /mnt4").Execute();
		}
		T0 t25 = this.method_44<T1, T2, T3>("/System/Library/Filesystems/apfs.fs/apfs.util -p /dev/" + this.string_15 + "s7").Trim() == "Baseband Data";
		if (t25 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/" + this.string_15 + "s7 /mnt4").Execute();
		}
		T0 t26 = this.method_44<T1, T2, T3>("ls /mnt6") == "";
		if (t26 != null)
		{
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/disk1s1 /mnt1").Execute();
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/disk1s3 /mnt7").Execute();
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/disk1s5 /mnt5").Execute();
			this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/disk1s6 /mnt6").Execute();
			T0 t27 = !this.checkBox3.Checked;
			if (t27 != null)
			{
				this.sshClient_0.CreateCommand("/sbin/mount_apfs -R /dev/disk1s4 /mnt4").Execute();
			}
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00210A68 File Offset: 0x0020EC68
	public void method_55<T0, T1, T2, T3>()
	{
		this.sshClient_0.CreateCommand("if [ -e /usr/bin/mount_universal ]; then mount_universal; else mount_filesystems; fi").Execute();
		this.method_54<T0, T1, T2, T3>();
		if (!(this.string_4 == "0x7000") && !(this.string_4 == "0x7001"))
		{
			this.sshClient_0.CreateCommand("/usr/libexec/seputil --gigalocker-init").Execute();
			this.sshClient_0.CreateCommand("/usr/libexec/seputil --load /mnt6/*/usr/standalone/firmware/sep-firmware.img4 && /usr/libexec/seputil --load /mnt6/*/usr/standalone/firmware/sep-firmware.img4").Execute();
		}
		else
		{
			this.sshClient_0.CreateCommand("/usr/libexec/seputil --load /mnt1/usr/standalone/firmware/sep-firmware.img4 && /usr/libexec/seputil --load /mnt1/usr/standalone/firmware/sep-firmware.img4").Execute();
		}
		this.sshClient_0.CreateCommand("/sbin/mount_apfs /dev/disk1s2 /mnt2 && /sbin/mount_apfs /dev/disk1s2 /mnt2").Execute();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00210B14 File Offset: 0x0020ED14
	public void method_56<T0, T1, T2, T3>()
	{
		this.method_54<T0, T1, T2, T3>();
		this.sshClient_0.CreateCommand("/usr/libexec/seputil --gigalocker-init").Execute();
		this.sshClient_0.CreateCommand("/usr/sbin/nvram oblit-inprogress=1 rev=2").Execute();
		this.sshClient_0.CreateCommand("usbcfwflasher && usbcfwflasher").Execute();
		this.sshClient_0.CreateCommand("/usr/sbin/nvram -d oblit-inprogres && /usr/sbin/nvram -d oblit-inprogres && /usr/sbin/nvram -d oblit-inprogres").Execute();
		this.sshClient_0.CreateCommand("/usr/libexec/seputil --load /mnt6/*/usr/standalone/firmware/sep-firmware.img4 && /usr/libexec/seputil --load /mnt6/*/usr/standalone/firmware/sep-firmware.img4").Execute();
		this.sshClient_0.CreateCommand("/sbin/mount_apfs /dev/disk1s2 /mnt2 && /sbin/mount_apfs /dev/disk1s2 /mnt2").Execute();
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00210BAC File Offset: 0x0020EDAC
	public void method_57<T0, T1, T2, T3>()
	{
		this.method_54<T0, T1, T2, T3>();
		this.sshClient_0.CreateCommand("/usr/libexec/seputil --gigalocker-init").Execute();
		this.sshClient_0.CreateCommand("/usr/libexec/seputil --load /mnt6/*/usr/standalone/firmware/sep-firmware.img4 && /usr/libexec/seputil --load /mnt6/*/usr/standalone/firmware/sep-firmware.img4").Execute();
		this.sshClient_0.CreateCommand("/sbin/mount_apfs /dev/disk1s2 /mnt2 && /sbin/mount_apfs /dev/disk1s2 /mnt2").Execute();
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00210C04 File Offset: 0x0020EE04
	public async void method_58()
	{
		this.progressBar2.Value = 0;
		try
		{
			this.label11.ForeColor = Color.Black;
			if (Directory.Exists("./files/Backup/" + this.string_5))
			{
				Directory.Delete("./files/Backup/" + this.string_5, true);
			}
			this.label11.Text = "Backing Up...! Dont't disconnect iDevice.";
			Directory.CreateDirectory(Environment.CurrentDirectory + "/files/Backup/" + this.string_5);
			Directory.CreateDirectory(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/");
			Directory.CreateDirectory(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/FairPlay/");
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
			string text = this.method_44<SshCommand, string, StreamReader>("find /private/var/containers/Data/System/ -iname 'internal'").Replace("Library/internal", "").Replace("\n", "")
				.Replace("//", "/");
			this.scpClient_0.Download(text + "Library/activation_records/activation_record.plist", new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/1"));
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			this.scpClient_0.Download(text + "Library/internal/data_ark.plist", new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/2"));
			this.scpClient_0.Download("/private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist", new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/3"));
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			this.method_45<Process, bool, string, XmlDocument, XmlNode, byte>("private/var");
			try
			{
				this.label11.Text = "Trying to back up iCloud info....";
				this.scpClient_0.Download("/private/var/mobile/Library/Accounts/Accounts3.sqlite", new DirectoryInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/"));
				this.method_2<SQLiteConnection, SQLiteCommand, byte, string, Match>();
				File.Delete(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/Accounts3.sqlite");
			}
			catch
			{
				try
				{
					this.label11.Text = "Failed to backup iCloud info, skipping....";
					this.method_43<bool, int, System.Windows.Forms.Timer>(2000);
					Directory.Delete(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/", true);
				}
				catch
				{
				}
			}
			this.method_24<string>("./files/Backup/" + this.string_5, "./files/Backup/" + this.string_5 + ".zip");
			this.label11.Text = "Attempting to save BackUp online....";
			this.method_43<bool, int, System.Windows.Forms.Timer>(1000);
			bool flag = await this.method_4<Task<bool>>();
			if (!flag)
			{
				this.label11.Text = "Could not save BackUp online, skipping....";
			}
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			Directory.Delete("./files/Backup/" + this.string_5, true);
			this.method_1<ProcessStartInfo, string>();
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "BackUp Done...";
			this.label18.Visible = true;
			this.button5.Text = "Erase Device";
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(1000);
			MessageBox.Show("BackUp was Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			text = null;
		}
		catch (Exception ex)
		{
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "BackUp Error...";
			MessageBox.Show(ex.Message);
			MessageBox.Show("There was an error backing Up. \n\nMake sure iDevice is connected properly, then try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			if (Directory.Exists("./files/Backup/" + this.string_5))
			{
				Directory.Delete("./files/Backup/" + this.string_5, true);
			}
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00210C40 File Offset: 0x0020EE40
	public async void method_59()
	{
		this.progressBar2.Value = 0;
		try
		{
			this.label11.ForeColor = Color.Black;
			this.label11.Text = "Backing Up...! Dont't disconnect iDevice.";
			Directory.CreateDirectory(Environment.CurrentDirectory + "/files/Backup/" + this.string_5);
			Directory.CreateDirectory(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/");
			Directory.CreateDirectory(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/FairPlay/");
			this.label11.Text = "Backing Up...! If stuck here, update retain data.";
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			if (!this.checkBox3.Checked)
			{
				this.method_55<bool, SshCommand, string, StreamReader>();
			}
			else
			{
				this.method_56<bool, SshCommand, string, StreamReader>();
			}
			this.label11.Text = "Backing Up...! Dont't disconnect iDevice.";
			string text = this.method_44<SshCommand, string, StreamReader>("find /mnt2/containers/Data/System/ -iname 'internal'").Replace("Library/internal", "").Replace("\n", "")
				.Replace("//", "/");
			this.scpClient_0.Download(text + "Library/activation_records/activation_record.plist", new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/1"));
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			this.scpClient_0.Download(text + "Library/internal/data_ark.plist", new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/2"));
			this.scpClient_0.Download("/mnt2/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist", new FileInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/3"));
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			this.method_45<Process, bool, string, XmlDocument, XmlNode, byte>("mnt2");
			try
			{
				this.label11.Text = "Trying to back up iCloud info....";
				this.scpClient_0.Download("/mnt2/mobile/Library/Accounts/Accounts3.sqlite", new DirectoryInfo(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/"));
				this.method_2<SQLiteConnection, SQLiteCommand, byte, string, Match>();
				File.Delete(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/Accounts3.sqlite");
			}
			catch
			{
				try
				{
					this.label11.Text = "Failed to backup iCloud info, skipping....";
					this.method_43<bool, int, System.Windows.Forms.Timer>(2000);
					Directory.Delete(Environment.CurrentDirectory + "/files/Backup/" + this.string_5 + "/iCloudInfo/", true);
				}
				catch
				{
				}
			}
			this.method_24<string>("./files/Backup/" + this.string_5, "./files/Backup/" + this.string_5 + ".zip");
			this.label11.Text = "Attempting to save BackUp online....";
			this.method_43<bool, int, System.Windows.Forms.Timer>(1000);
			bool flag = await this.method_4<Task<bool>>();
			if (!flag)
			{
				this.label11.Text = "Could not save BackUp online, skipping....";
			}
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(3000);
			Directory.Delete("./files/Backup/" + this.string_5, true);
			this.method_1<ProcessStartInfo, string>();
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "BackUp Done...";
			this.button5.Text = "Erase Device";
			this.linkLabel3.Visible = false;
			this.label17.Text = "NOTICE: Erasing your device will delete all user data including activation \r\nfiles. Make sure FMI is off or you have activation files backed up.";
			this.button6.Visible = true;
			this.label18.Visible = true;
			this.progressBar2.Value += 20;
			this.method_43<bool, int, System.Windows.Forms.Timer>(1000);
			MessageBox.Show("BackUp was Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			text = null;
		}
		catch (Exception ex)
		{
			try
			{
				string text2 = this.method_44<SshCommand, string, StreamReader>("ls /mnt2");
				if (text2 == "")
				{
					MessageBox.Show("Could not mount filesystems \n\nTry a different ramdisk file \n\nOr update retain user data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				text2 = null;
			}
			catch
			{
				this.label11.ForeColor = Color.Crimson;
				this.label11.Text = "BackUp error...";
				MessageBox.Show(ex.Message);
				MessageBox.Show("There was an error backing Up. \n\nMake sure iDevice is connected properly, then try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				if (Directory.Exists("./files/Backup" + this.string_5))
				{
					Directory.Delete("./files/Backup/" + this.string_5, true);
				}
			}
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00210C7C File Offset: 0x0020EE7C
	public void method_60<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
	{
		this.label11.ForeColor = Color.Black;
		this.progressBar2.Value = 0;
		try
		{
			this.label11.Text = "Activating device...! Dont't disconnect iDevice.";
			this.method_23<T4, T5, T6, T0>("./files/Backup/" + this.string_5 + ".zip", "./tmp/" + this.string_5);
			T1 t = Directory.Exists("./tmp/" + this.string_5 + "/iCloudInfo");
			if (t != null)
			{
				Directory.Delete(Environment.CurrentDirectory + "/tmp/" + this.string_5 + "/iCloudInfo", true);
			}
			this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
			this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Media/Downloads/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Media/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("mkdir /private/var/mobile/Media/Downloads/" + this.string_5).Execute();
			T0 t2 = this.method_44<T7, T0, T8>("find /private/var/containers/Data/System/ -iname 'internal'").Replace("Library/internal", "").Replace("\n", "")
				.Replace("//", "/");
			this.scpClient_0.Upload(new DirectoryInfo(Environment.CurrentDirectory + "/tmp/" + this.string_5), "/private/var/mobile/Media/Downloads/" + this.string_5);
			this.sshClient_0.CreateCommand("mv -f /private/var/mobile/Media/Downloads/" + this.string_5 + " /private/var/mobile/Media/" + this.string_5).Execute();
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("/usr/sbin/chown -R mobile:mobile /private/var/mobile/Media/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("chmod -R 755 /private/var/mobile/Media/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("chmod 644 /private/var/mobile/Media/" + this.string_5 + "/1").Execute();
			this.sshClient_0.CreateCommand("chmod 644 /private/var/mobile/Media/" + this.string_5 + "/2").Execute();
			this.sshClient_0.CreateCommand("chmod 644 /private/var/mobile/Media/" + this.string_5 + "/3").Execute();
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("mv -f /private/var/mobile/Media/" + this.string_5 + "/FairPlay /private/var/mobile/Library/FairPlay").Execute();
			this.sshClient_0.CreateCommand("chmod 755 /private/var/mobile/Library/FairPlay").Execute();
			this.sshClient_0.CreateCommand("/usr/sbin/chown -R mobile:mobile /private/var/mobile/Library/FairPlay").Execute();
			this.sshClient_0.CreateCommand("chflags nouchg " + t2 + "Library/internal/data_ark.plist").Execute();
			this.sshClient_0.CreateCommand(string.Concat(new T0[] { "mv -f /private/var/mobile/Media/", this.string_5, "/2 ", t2, "Library/internal/data_ark.plist" })).Execute();
			this.sshClient_0.CreateCommand("chmod 755 " + t2 + "Library/internal/data_ark.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg " + t2 + "Library/internal/data_ark.plist").Execute();
			this.progressBar2.Value += 20;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("mkdir " + t2 + "Library/activation_records").Execute();
			this.sshClient_0.CreateCommand(string.Concat(new T0[] { "mv -f /private/var/mobile/Media/", this.string_5, "/1 ", t2, "Library/activation_records/activation_record.plist" })).Execute();
			this.sshClient_0.CreateCommand("chmod 755 " + t2 + "Library/activation_records/activation_record.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg " + t2 + "/Library/activation_records/activation_record.plist").Execute();
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("chflags nouchg /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("mv -f /private/var/mobile/Media/" + this.string_5 + "/3 /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("usr/sbin/chown root:mobile /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chmod 755 /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.progressBar2.Value += 20;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Library/Logs/*").Execute();
			this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Library/Logs/*").Execute();
			this.sshClient_0.CreateCommand("launchctl unload /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			T1 @checked = this.checkBox4.Checked;
			if (@checked != null)
			{
				try
				{
					this.label11.Text = "Skipping SetUp....";
					T2[] array = Convert.FromBase64String("YnBsaXN0MDDfECABAgMEBQYHCAkKCwwNDg8QERITFBUWFxgZGhscHR4fICEiIiIlIiciKSIrIiIuIjIiNCI2JTolJyI9IiIiIiIiXFNldHVwVmVyc2lvbl8QEVVzZXJDaG9zZUxhbmd1YWdlXxAXUEJEaWFnbm9zdGljczRQcmVzZW50ZWRfEBBQcml2YWN5UHJlc2VudGVkXxAfV2ViS2l0QWNjZWxlcmF0ZWREcmF3aW5nRW5hYmxlZF8QFFBheW1lbnRNaW5pQnVkZHk0UmFuXxArV2ViS2l0TG9jYWxTdG9yYWdlRGF0YWJhc2VQYXRoUHJlZmVyZW5jZUtleV5NZXNhMlByZXNlbnRlZFZMb2NhbGVfECFQaG9uZU51bWJlclBlcm1pc3Npb25QcmVzZW50ZWRLZXlfEBRzZXR1cE1pZ3JhdG9yVmVyc2lvbl8QGEhTQTJVcGdyYWRlTWluaUJ1ZGR5M1Jhbl8QFVNldHVwRmluaXNoZWRBbGxTdGVwc18QGWxhc3RQcmVwYXJlTGF1bmNoU2VudGluZWxfEBRBcHBsZUlEUEIxMFByZXNlbnRlZF8QFVByaXZhY3lDb250ZW50VmVyc2lvbl8QH1VzZXJJbnRlcmZhY2VTdHlsZU1vZGVQcmVzZW50ZWRYTGFuZ3VhZ2VfEBxIb21lQnV0dG9uQ3VzdG9taXplUHJlc2VudGVkWWNocm9uaWNsZV8QFUFuaW1hdGVMYW51Z2FnZUNob2ljZV1TZXR1cExhc3RFeGl0XxAQTWFnbmlmeVByZXNlbnRlZF8QFFdlYkRhdGFiYXNlRGlyZWN0b3J5XxAnV2ViS2l0T2ZmbGluZVdlYkFwcGxpY2F0aW9uQ2FjaGVFbmFibGVkWlNldHVwU3RhdGVfEBNBdXRvVXBkYXRlUHJlc2VudGVkXVJlc3RvcmVDaG9pY2VfEBNTY3JlZW5UaW1lUHJlc2VudGVkXxASUGFzc2NvZGU0UHJlc2VudGVkWVNldHVwRG9uZV8QIldlYktpdFNocmlua3NTdGFuZGFsb25lSW1hZ2VzVG9GaXQQCwkJCQgJXxAaL3Zhci9tb2JpbGUvTGlicmFyeS9DYWNoZXMJVWVzX0RPCRAKCQmiLzAzQcJgTHmPqfwQAAkQAglVZXMtRE8J0Tc4WGZlYXR1cmVzoAgzQcJgTHXyZncICV8QE1NldHVwVXNpbmdBc3Npc3RhbnQJCQkJCQkACABLAFgAbACGAJkAuwDSAQABDwEWAToBUQFsAYQBoAG3Ac8B8QH6AhkCIwI7AkkCXAJzAp0CqAK+AswC4gL3AwEDJgMoAykDKgMrAywDLQNKA0sDUQNSA1QDVQNWA1kDYgNkA2UDZwNoA24DbwNyA3sDfAN9A4YDhwOIA54DnwOgA6EDogOjAAAAAAAAAgEAAAAAAAAARAAAAAAAAAAAAAAAAAAAA6Q=");
					this.scpClient_0.Upload(new MemoryStream(array), "/private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist");
					this.sshClient_0.CreateCommand("chmod 600 /private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
					this.sshClient_0.CreateCommand("uicache --all && chflags uchg /private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
				}
				catch
				{
				}
			}
			this.sshClient_0.CreateCommand("launchctl load /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			try
			{
				this.sshClient_0.CreateCommand("shutdown -r now").Execute();
			}
			catch
			{
			}
			this.progressBar2.Value += 20;
			this.method_43<T1, T9, T10>(3000);
			Directory.Delete("./tmp/" + this.string_5, true);
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "Device has been activated...";
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.button5.Text = "Close";
			this.button6.Visible = false;
			this.label18.Text = "Congrats! Your iDevice has been activated\ud83c\udf89\ud83c\udf89";
			this.label18.Visible = true;
			MessageBox.Show("Your device has been sucessfully activated! \n\nYou can update, but don't restore or erase.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			this.sshClient_0.Disconnect();
			this.scpClient_0.Disconnect();
		}
		catch (Exception t3)
		{
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "There was an error activating device....";
			Directory.Delete("./tmp/" + this.string_5, true);
			MessageBox.Show(t3.Message);
			MessageBox.Show("There was an error activating device!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x002113FC File Offset: 0x0020F5FC
	public void method_61<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
	{
		this.label11.ForeColor = Color.Black;
		this.progressBar2.Value = 0;
		try
		{
			this.label11.Text = "Activating device...! Dont't disconnect iDevice.";
			this.method_23<T4, T5, T6, T0>("./files/Backup/" + this.string_5 + ".zip", "./tmp/" + this.string_5);
			T1 t = !File.Exists("./tmp/" + this.string_5 + "/2");
			if (t != null)
			{
				this.bool_2 = true;
				this.checkBox4.Checked = true;
			}
			T1 t2 = Directory.Exists("./tmp/" + this.string_5 + "/iCloudInfo");
			if (t2 != null)
			{
				Directory.Delete(Environment.CurrentDirectory + "/tmp/" + this.string_5 + "/iCloudInfo", true);
			}
			T1 @checked = this.checkBox3.Checked;
			if (@checked != null)
			{
				this.method_57<T1, T7, T0, T8>();
			}
			else
			{
				this.method_55<T1, T7, T0, T8>();
			}
			this.sshClient_0.CreateCommand("rm -rf /mnt2/mobile/Media/Downloads/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("rm -rf /mnt2/mobile/Media/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("mkdir /mnt2/mobile/Media/Downloads/" + this.string_5).Execute();
			T0 t3 = this.method_44<T7, T0, T8>("find /mnt2/containers/Data/System/ -iname 'internal'").Replace("Library/internal", "").Replace("\n", "")
				.Replace("//", "/");
			this.scpClient_0.Upload(new DirectoryInfo(Environment.CurrentDirectory + "/tmp/" + this.string_5), "/mnt2/mobile/Media/Downloads/" + this.string_5);
			this.sshClient_0.CreateCommand("mv -f /mnt2/mobile/Media/Downloads/" + this.string_5 + " /mnt2/mobile/Media/" + this.string_5).Execute();
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("/usr/sbin/chown -R mobile:mobile /mnt2/mobile/Media/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("chmod -R 755 /mnt2/mobile/Media/" + this.string_5).Execute();
			this.sshClient_0.CreateCommand("chmod 644 /mnt2/mobile/Media/" + this.string_5 + "/1").Execute();
			this.sshClient_0.CreateCommand("chmod 644 /mnt2/mobile/Media/" + this.string_5 + "/2").Execute();
			this.sshClient_0.CreateCommand("chmod 644 /mnt2/mobile/Media/" + this.string_5 + "/3").Execute();
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("mv -f /mnt2/mobile/Media/" + this.string_5 + "/FairPlay /mnt2/mobile/Library/FairPlay").Execute();
			this.sshClient_0.CreateCommand("chmod 755 /mnt2/mobile/Library/FairPlay").Execute();
			this.sshClient_0.CreateCommand("/usr/sbin/chown -R mobile:mobile /mnt2/mobile/Library/FairPlay").Execute();
			this.sshClient_0.CreateCommand("chflags nouchg " + t3 + "Library/internal/data_ark.plist").Execute();
			this.sshClient_0.CreateCommand(string.Concat(new T0[] { "mv -f /mnt2/mobile/Media/", this.string_5, "/2 ", t3, "Library/internal/data_ark.plist" })).Execute();
			this.sshClient_0.CreateCommand("chmod 755 " + t3 + "Library/internal/data_ark.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg " + t3 + "Library/internal/data_ark.plist").Execute();
			this.progressBar2.Value += 20;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("mkdir " + t3 + "Library/activation_records").Execute();
			this.sshClient_0.CreateCommand(string.Concat(new T0[] { "mv -f /mnt2/mobile/Media/", this.string_5, "/1 ", t3, "Library/activation_records/activation_record.plist" })).Execute();
			this.sshClient_0.CreateCommand("chmod 755 " + t3 + "Library/activation_records/activation_record.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg " + t3 + "/Library/activation_records/activation_record.plist").Execute();
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("chflags nouchg /mnt2/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("mv -f /mnt2/mobile/Media/" + this.string_5 + "/3 /mnt2/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("usr/sbin/chown root:mobile /mnt2/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chmod 755 /mnt2/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg /mnt2/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.progressBar2.Value += 20;
			this.method_43<T1, T9, T10>(3000);
			this.sshClient_0.CreateCommand("rm -rf /mnt2/mobile/Library/Logs/*").Execute();
			this.sshClient_0.CreateCommand("rm -rf /mnt2/mobile/Library/Logs/*").Execute();
			this.sshClient_0.CreateCommand("launchctl unload /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			T1 checked2 = this.checkBox4.Checked;
			if (checked2 != null)
			{
				try
				{
					this.label11.Text = "Skipping SetUp....";
					T2[] array = Convert.FromBase64String("YnBsaXN0MDDfECABAgMEBQYHCAkKCwwNDg8QERITFBUWFxgZGhscHR4fICEiIiIlIiciKSIrIiIuIjIiNCI2JTolJyI9IiIiIiIiXFNldHVwVmVyc2lvbl8QEVVzZXJDaG9zZUxhbmd1YWdlXxAXUEJEaWFnbm9zdGljczRQcmVzZW50ZWRfEBBQcml2YWN5UHJlc2VudGVkXxAfV2ViS2l0QWNjZWxlcmF0ZWREcmF3aW5nRW5hYmxlZF8QFFBheW1lbnRNaW5pQnVkZHk0UmFuXxArV2ViS2l0TG9jYWxTdG9yYWdlRGF0YWJhc2VQYXRoUHJlZmVyZW5jZUtleV5NZXNhMlByZXNlbnRlZFZMb2NhbGVfECFQaG9uZU51bWJlclBlcm1pc3Npb25QcmVzZW50ZWRLZXlfEBRzZXR1cE1pZ3JhdG9yVmVyc2lvbl8QGEhTQTJVcGdyYWRlTWluaUJ1ZGR5M1Jhbl8QFVNldHVwRmluaXNoZWRBbGxTdGVwc18QGWxhc3RQcmVwYXJlTGF1bmNoU2VudGluZWxfEBRBcHBsZUlEUEIxMFByZXNlbnRlZF8QFVByaXZhY3lDb250ZW50VmVyc2lvbl8QH1VzZXJJbnRlcmZhY2VTdHlsZU1vZGVQcmVzZW50ZWRYTGFuZ3VhZ2VfEBxIb21lQnV0dG9uQ3VzdG9taXplUHJlc2VudGVkWWNocm9uaWNsZV8QFUFuaW1hdGVMYW51Z2FnZUNob2ljZV1TZXR1cExhc3RFeGl0XxAQTWFnbmlmeVByZXNlbnRlZF8QFFdlYkRhdGFiYXNlRGlyZWN0b3J5XxAnV2ViS2l0T2ZmbGluZVdlYkFwcGxpY2F0aW9uQ2FjaGVFbmFibGVkWlNldHVwU3RhdGVfEBNBdXRvVXBkYXRlUHJlc2VudGVkXVJlc3RvcmVDaG9pY2VfEBNTY3JlZW5UaW1lUHJlc2VudGVkXxASUGFzc2NvZGU0UHJlc2VudGVkWVNldHVwRG9uZV8QIldlYktpdFNocmlua3NTdGFuZGFsb25lSW1hZ2VzVG9GaXQQCwkJCQgJXxAaL3Zhci9tb2JpbGUvTGlicmFyeS9DYWNoZXMJVWVzX0RPCRAKCQmiLzAzQcJgTHmPqfwQAAkQAglVZXMtRE8J0Tc4WGZlYXR1cmVzoAgzQcJgTHXyZncICV8QE1NldHVwVXNpbmdBc3Npc3RhbnQJCQkJCQkACABLAFgAbACGAJkAuwDSAQABDwEWAToBUQFsAYQBoAG3Ac8B8QH6AhkCIwI7AkkCXAJzAp0CqAK+AswC4gL3AwEDJgMoAykDKgMrAywDLQNKA0sDUQNSA1QDVQNWA1kDYgNkA2UDZwNoA24DbwNyA3sDfAN9A4YDhwOIA54DnwOgA6EDogOjAAAAAAAAAgEAAAAAAAAARAAAAAAAAAAAAAAAAAAAA6Q=");
					this.scpClient_0.Upload(new MemoryStream(array), "/mnt2/mobile/Library/Preferences/com.apple.purplebuddy.plist");
					this.sshClient_0.CreateCommand("chmod 600 /mnt2/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
					this.sshClient_0.CreateCommand("uicache --all && chflags uchg /mnt2/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
				}
				catch
				{
				}
			}
			this.sshClient_0.CreateCommand("launchctl load /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			T1 t4 = this.bool_2;
			if (t4 != null)
			{
				this.label11.Text = "Fixing iServices...";
				this.method_43<T1, T9, T10>(1000);
				this.sshClient_0.CreateCommand("chflags nouchg " + t3 + "Library/internal/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("mkdir -p -m 755 /mnt2/mobile/Media/iTunes_Control/iTunes/").Execute();
				this.sshClient_0.CreateCommand("cp -f /mnt2/mobile/Library/FairPlay/iTunes_Control/iTunes/IC-Info.sisv /mnt2/mobile/Media/iTunes_Control/iTunes/IC-Info.sidv").Execute();
				this.sshClient_0.CreateCommand("/sbin/chown -R mobile:mobile /mnt2/mobile/Media/iTunes_Control").Execute();
				this.sshClient_0.CreateCommand("chmod 0644 /mnt2/mobile/Media/iTunes_Control/iTunes/IC-Info.sidv").Execute();
				this.sshClient_0.CreateCommand("plutil -remove -\"-UCRTOOBForbidden\" " + t3 + "Library/internal/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("plutil -\"-BrickState\" -0 " + t3 + "Library/internal/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("plutil -remove -ActivationState " + t3 + "Library/internal/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("plutil -remove -\"-ActivationState\" " + t3 + "Library/internal/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("plutil -ActivationState -string Activated " + t3 + "Library/internal/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("cp -f " + t3 + "Library/internal/data_ark.plist /var/root/Library/Lockdown/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("chflags uchg /var/root/Library/Lockdown/data_ark.plist").Execute();
				this.sshClient_0.CreateCommand("chflags uchg " + t3 + "Library/internal/data_ark.plist").Execute();
				T1 t5 = !this.checkBox14.Checked;
				if (t5 != null)
				{
					this.sshClient_0.CreateCommand("chflags -R nouchg /mnt6/$(cat /mnt6/active)/usr/local/standalone/firmware/").Execute();
					this.sshClient_0.CreateCommand("/bin/mv -f /mnt6/$(cat /mnt6/active)/usr/local/standalone/firmware/Baseband /mnt6/$(cat /mnt6/active)/usr/local/standalone/firmware/Baseband2").Execute();
					this.label11.Text = "Enabling airplane mode....";
					this.sshClient_0.CreateCommand("rm -rf /mnt2/preferences/SystemConfiguration/com.apple.radios.plist").Execute();
					this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/radios"), "/mnt2/preferences/SystemConfiguration/com.apple.radios.plist");
					this.sshClient_0.CreateCommand("chflags uchg /mnt2/preferences/SystemConfiguration/com.apple.radios.plist").Execute();
				}
			}
			this.progressBar2.Value += 20;
			this.method_43<T1, T9, T10>(3000);
			Directory.Delete("./tmp/" + this.string_5, true);
			this.sshClient_0.CreateCommand("/sbin/reboot").Execute();
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "Device has been activated...";
			this.progressBar2.Value += 10;
			this.method_43<T1, T9, T10>(3000);
			this.button5.Text = "Close";
			this.button6.Visible = false;
			this.label18.Text = "Congrats! Your iDevice has been activated\ud83c\udf89\ud83c\udf89";
			this.label18.Visible = true;
			MessageBox.Show("Your device has been sucessfully activated! \n\nYou can update, but don't restore or erase.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			this.sshClient_0.Disconnect();
			this.scpClient_0.Disconnect();
		}
		catch (Exception t6)
		{
			try
			{
				T0 t7 = this.method_44<T7, T0, T8>("ls /mnt2");
				T1 t8 = t7 == "";
				if (t8 != null)
				{
					MessageBox.Show("Could not mount filesystems \n\nTry a different ramdisk file \n\nOr update retain user data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			catch
			{
				this.label11.ForeColor = Color.Crimson;
				this.label11.Text = "There was an error activating device....";
				Directory.Delete("./tmp/" + this.string_5, true);
				MessageBox.Show(t6.Message);
				MessageBox.Show("There was an error activating device!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00211E34 File Offset: 0x00210034
	public async void method_62()
	{
		this.progressBar2.Value = 0;
		try
		{
			await Task.Run(new Func<Task>(this.method_108<Task>));
			this.button5.Text = "Close";
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "Bypass done...";
			this.progressBar2.Value += 100 - this.progressBar2.Value;
			Directory.Delete(Environment.CurrentDirectory + "/files/Backup/" + this.string_5, true);
			this.label18.Visible = true;
			this.label18.Text = "Congrats! Your iDevice has been activated\ud83c\udf89\ud83c\udf89";
			MessageBox.Show("Your device has been sucessfully activated! \n\nDon't erase or update.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			this.button5.Enabled = true;
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "There was an error activating device....";
			Directory.Delete(Environment.CurrentDirectory + "/files/Backup/" + this.string_5, true);
			MessageBox.Show(ex.Message);
			MessageBox.Show("There was an error activating device!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00211E70 File Offset: 0x00210070
	public void method_63<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
	{
		this.method_13<T2, T3, T4, T5, T6>();
		try
		{
			this.label11.ForeColor = Color.Black;
			this.label11.Text = "Mounting filesystems...";
			this.progressBar2.Value = 0;
			this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
			this.sshClient_0.CreateCommand("cp -rp /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/FactoryActivation.pem /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/RaptorActivation.pem").Execute();
			this.sshClient_0.CreateCommand("launchctl stop com.apple.mobileactivationd && launchctl start com.apple.mobileactivationd && killall mobileactivationd").Execute();
			this.label11.Text = "Preparing to fix iServices...";
			this.progressBar2.Value += 30;
			this.method_43<T2, T7, T8>(1000);
			this.sshClient_0.CreateCommand("launchctl unload /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			this.sshClient_0.CreateCommand("launchctl load /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			this.sshClient_0.CreateCommand("rm -rf /tmp/Backup && mkdir -p /tmp/Backup && chown -R mobile:mobile /tmp/Backup && chmod -R 755 /tmp/Backup").Execute();
			this.sshClient_0.CreateCommand("chflags -R uchg /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			T0[] array = Convert.FromBase64String("PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPCFET0NUWVBFIHBsaXN0IFBVQkxJQyAiLS8vQXBwbGUvL0RURCBQTElTVCAxLjAvL0VOIiAiaHR0cDovL3d3dy5hcHBsZS5jb20vRFREcy9Qcm9wZXJ0eUxpc3QtMS4wLmR0ZCI+CjxwbGlzdCB2ZXJzaW9uPSIxLjAiPgo8ZGljdD4KCTxrZXk+a1Bvc3Rwb25lbWVudFRpY2tldDwva2V5PgoJPGRpY3Q+CgkJPGtleT5BY3Rpdml0eVVSTDwva2V5PgoJCTxzdHJpbmc+aHR0cHM6Ly9hbGJlcnQuYXBwbGUuY29tL2RldmljZXNlcnZpY2VzL2FjdGl2aXR5PC9zdHJpbmc+CgkJPGtleT5BY3RpdmF0aW9uVGlja2V0PC9rZXk+CgkJPHN0cmluZz5QRDk0Yld3Z2RtVnljMmx2YmowaU1TNHdJaUJsYm1OdlpHbHVaejBpVlZSR0xUZ2lQejRLUENGRVQwTlVXVkJGSUhCc2FYTjBJRkJWUWt4SlF5QWlMUzh2UVhCd2JHVXZMMFJVUkNCUVRFbFRWQ0F4TGpBdkwwVk9JaUFpYUhSMGNEb3ZMM2QzZHk1aGNIQnNaUzVqYjIwdlJGUkVjeTlRY205d1pYSjBlVXhwYzNRdE1TNHdMbVIwWkNJK0NqeHdiR2x6ZENCMlpYSnphVzl1UFNJeExqQWlQZ284WkdsamRENEtDVHhyWlhrK1FXTjBhWFpoZEdsdmJsSmxjWFZsYzNSSmJtWnZQQzlyWlhrK0NnazhaR2xqZEQ0S0NRazhhMlY1UGtGamRHbDJZWFJwYjI1U1lXNWtiMjF1WlhOelBDOXJaWGsrQ2drSlBITjBjbWx1Wno0ek1HSTJNR1prTUMwMk5qYzBMVFEzTnpndFltSXhOQzFtTkdaaE9UUTBNV1EwWXpnOEwzTjBjbWx1Wno0S0NRazhhMlY1UGtGamRHbDJZWFJwYjI1VGRHRjBaVHd2YTJWNVBnb0pDVHh6ZEhKcGJtYytWVzVoWTNScGRtRjBaV1E4TDNOMGNtbHVaejRLQ1FrOGEyVjVQa1pOYVZCQlkyTnZkVzUwUlhocGMzUnpQQzlyWlhrK0Nna0pQSFJ5ZFdVdlBnb0pQQzlrYVdOMFBnb0pQR3RsZVQ1Q1lYTmxZbUZ1WkZKbGNYVmxjM1JKYm1adlBDOXJaWGsrQ2drOFpHbGpkRDRLQ1FrOGEyVjVQa0ZqZEdsMllYUnBiMjVTWlhGMWFYSmxjMEZqZEdsMllYUnBiMjVVYVdOclpYUThMMnRsZVQ0S0NRazhkSEoxWlM4K0Nna0pQR3RsZVQ1Q1lYTmxZbUZ1WkVGamRHbDJZWFJwYjI1VWFXTnJaWFJXWlhKemFXOXVQQzlyWlhrK0Nna0pQSE4wY21sdVp6NVdNand2YzNSeWFXNW5QZ29KQ1R4clpYaytRbUZ6WldKaGJtUkRhR2x3U1VROEwydGxlVDRLQ1FrOGFXNTBaV2RsY2o0eE1qTTBOVFkzUEM5cGJuUmxaMlZ5UGdvSkNUeHJaWGsrUW1GelpXSmhibVJOWVhOMFpYSkxaWGxJWVhOb1BDOXJaWGsrQ2drSlBITjBjbWx1Wno0NFEwSXhNRGN3UkRrMVFqbERSVVUwUXpnd01EQXdOVVV5TVRrNVFrSTRSa0l4T0ROQ01ESTNNVE5CTlRKRVFqVkZOelZEUVRKQk5qRTFOVE0yTVRneVBDOXpkSEpwYm1jK0Nna0pQR3RsZVQ1Q1lYTmxZbUZ1WkZObGNtbGhiRTUxYldKbGNqd3ZhMlY1UGdvSkNUeGtZWFJoUGdvSkNVVm5hR1JEZHowOUNna0pQQzlrWVhSaFBnb0pDVHhyWlhrK1NXNTBaWEp1WVhScGIyNWhiRTF2WW1sc1pVVnhkV2x3YldWdWRFbGtaVzUwYVhSNVBDOXJaWGsrQ2drSlBITjBjbWx1Wno0eE1qTTBOVFkzT0RreE1qTTBOVFk4TDNOMGNtbHVaejRLQ1FrOGEyVjVQazF2WW1sc1pVVnhkV2x3YldWdWRFbGtaVzUwYVdacFpYSThMMnRsZVQ0S0NRazhjM1J5YVc1blBqRXlNelExTmpjNE9URXlNelExUEM5emRISnBibWMrQ2drSlBHdGxlVDVUU1UxVGRHRjBkWE04TDJ0bGVUNEtDUWs4YzNSeWFXNW5QbXREVkZOSlRWTjFjSEJ2Y25SVFNVMVRkR0YwZFhOT2IzUkpibk5sY25SbFpEd3ZjM1J5YVc1blBnb0pDVHhyWlhrK1UzVndjRzl5ZEhOUWIzTjBjRzl1WlcxbGJuUThMMnRsZVQ0S0NRazhkSEoxWlM4K0Nna0pQR3RsZVQ1clExUlFiM04wY0c5dVpXMWxiblJKYm1adlVGSk1UbUZ0WlR3dmEyVjVQZ29KQ1R4cGJuUmxaMlZ5UGpBOEwybHVkR1ZuWlhJK0Nna0pQR3RsZVQ1clExUlFiM04wY0c5dVpXMWxiblJKYm1adlUyVnlkbWxqWlZCeWIzWnBjMmx2Ym1sdVoxTjBZWFJsUEM5clpYaytDZ2tKUEdaaGJITmxMejRLQ1R3dlpHbGpkRDRLQ1R4clpYaytSR1YyYVdObFEyVnlkRkpsY1hWbGMzUThMMnRsZVQ0S0NUeGtZWFJoUGdvSlRGTXdkRXhUTVVOU1ZXUktWR2xDUkZKV1NsVlRWVnBLVVRCR1ZWSlRRbE5TVmtaV1VsWk9WVXhUTUhSTVV6QkxWRlZzU2xGdWFFVlJNRTVDVlhwQ1JGRldSa0lLQ1dReVpGcFVXR2hOVmtWR2VWRnRaRTlXYTBwQ1ZGWlNTMUpWYTNwVWJYUlNUVVUxUmxKVVZrMVdWbXQ2VkdwQ1VtUkZOVVpWVkVKT1lWUkJNRlZXVmt0Ulp6QkxDZ2xVUmxKeVpVWktjVmRyVmxOU1JXdzFWV3RXUzFJd05YRlNWWGhPVVZkMFNGRlVSbFpTVlVwdlZGVk9WMVpyTVRSUk0zQkNVMnRLYmxSc1drTlJWMlJWVVZkMFR3b0pZV3BhZUZOVmJIUlVibXhYVTIxV01VNXNUVEpWYWs0MFVWY3hUMVJYTldGalJFcEhURE5vUlZOSVJqVmlWbXhWVDFab1QxSkZkekJqUmxKYVlqRm5NbUY2UW1zS0NWRnJNVk5UV0dSR1VWWnNSVlpzUmxKVFFUQkxVbGhrYzFKSFVsbFJiWGhxWW14S2QxbHRNRFJsUlZZMlVWWktRMW93TlZkUmEwWjJWa1ZPY2xKdVpHcFNNMmh6Q2dsVFZWWnpaRlpzTlU1SWFFVmxhMFpQVVcxa1QxWnJTa0pqTVZKRFlsZDRVbGxWWXpWa1VUQkxWMnhTUkZGdE5UWlJWVFZEV2pKMGVHRkhkSEJTZW13elRVVktRZ29KVVZVeFFrMUZaRVJWTTBaSVZUQnNhVTB3VWxKU1ZVcERWVlpXUWxGVVVraFJhMFpFVERKNGVXSkhWbEpVYW1SM1VWRXdTMDB5YUVoV1Zsa3dVMFpzVTFsWGRIWUtDV0ZyYXpSUFYzZDRZVVpLZG1ScVFsUk9SRUpQVFVoQmVVMVVhSEpVVjI5NVlrUkdUMkV6VVhkV1dFSnhWMnM1UlU1V1ZsZGxWR1JEVDBWc1QxRnJTbTFSTW14TkNnbE5aekJMV25rNGRreDVkSHBhVlZab1ZqRmpNR0ZFV1hkVU0wcE9aRzVLYkZGV1FUQk5SMHBzVlRKYVVGbHVjRTFXUjNoWVV6SkdWMk5YUm5KTlYxSkdWR3BTU2dvSlRrZDRUVlJZYUhCbFZGVnlZak53U1ZwcVdtbFdkekJMVkdsMGJsZEZTbFZOTWpsNFdraFdSRkY2UmxkV2VsWktWMjVhTWxwRlVsTldSV3gzWVVab05tRXlSVXNLQ1ZWVlZrZFJWVVpRVVcxd1VsRllaRzVYVjNSRVdqRnNSbEZZU2xWaE1WcEZaREJHVjAxSWJIUlphelZXVW0xNE1FMHllRXhqTUhSQ1drRXdTMkp1WXpCVFJscFBDZ2xhTUVaMVVraG9hV1JSTUV0UlZVcFhWMVZTTWxOR2FFSk5SRVpOVjBaT1MxRjVkSFJrYW1kNVZGWlNTV1F5U2s1T1JWRjJWMnhKY2xKRmFGcFJWMWt5V1hsek5Rb0pZVmMxVGxKdFVrOVBSMnhhVjBoU1NXRkZPWGRqVjNNd1lWZDRUbFIzTUV0Wk1uUnVXV3RzTmxNd2IzbE9XRkpQV1RGS1ZXTXdPWGRXVlU1Q1pEQldRbEZYUmtJS0NVeFRNSFJNVXpGR1ZHdFJaMUV3VmxOV1JXeEhVMVZPUWxaRlZXZFZhMVpTVmxWV1ZGWkRNSFJNVXpCMENnazhMMlJoZEdFK0NnazhhMlY1UGtSbGRtbGpaVWxFUEM5clpYaytDZ2s4WkdsamRENEtDUWs4YTJWNVBsTmxjbWxoYkU1MWJXSmxjand2YTJWNVBnb0pDVHh6ZEhKcGJtYytSbEl4VURKSFNEaEtPRmhJUEM5emRISnBibWMrQ2drSlBHdGxlVDVWYm1seGRXVkVaWFpwWTJWSlJEd3ZhMlY1UGdvSkNUeHpkSEpwYm1jK1pEazRPVEl3T1RaalpqTTBNVEZsWVRnM1pEQXdNalF5WVdNeE16QXdNRE5tTXpReE1XVTBNand2YzNSeWFXNW5QZ29KUEM5a2FXTjBQZ29KUEd0bGVUNUVaWFpwWTJWSmJtWnZQQzlyWlhrK0NnazhaR2xqZEQ0S0NRazhhMlY1UGtKMWFXeGtWbVZ5YzJsdmJqd3ZhMlY1UGdvSkNUeHpkSEpwYm1jK01UaEdNREE4TDNOMGNtbHVaejRLQ1FrOGEyVjVQa1JsZG1salpVTnNZWE56UEM5clpYaytDZ2tKUEhOMGNtbHVaejVwVUdodmJtVThMM04wY21sdVp6NEtDUWs4YTJWNVBrUmxkbWxqWlZaaGNtbGhiblE4TDJ0bGVUNEtDUWs4YzNSeWFXNW5Qa0k4TDNOMGNtbHVaejRLQ1FrOGEyVjVQazF2WkdWc1RuVnRZbVZ5UEM5clpYaytDZ2tKUEhOMGNtbHVaejVOVEV4T01qd3ZjM1J5YVc1blBnb0pDVHhyWlhrK1QxTlVlWEJsUEM5clpYaytDZ2tKUEhOMGNtbHVaejVwVUdodmJtVWdUMU04TDNOMGNtbHVaejRLQ1FrOGEyVjVQbEJ5YjJSMVkzUlVlWEJsUEM5clpYaytDZ2tKUEhOMGNtbHVaejVwVUdodmJtVXdMREE4TDNOMGNtbHVaejRLQ1FrOGEyVjVQbEJ5YjJSMVkzUldaWEp6YVc5dVBDOXJaWGsrQ2drSlBITjBjbWx1Wno0eE5DNHdMakE4TDNOMGNtbHVaejRLQ1FrOGEyVjVQbEpsWjJsdmJrTnZaR1U4TDJ0bGVUNEtDUWs4YzNSeWFXNW5Qa3hNUEM5emRISnBibWMrQ2drSlBHdGxlVDVTWldkcGIyNUpibVp2UEM5clpYaytDZ2tKUEhOMGNtbHVaejVNVEM5QlBDOXpkSEpwYm1jK0Nna0pQR3RsZVQ1U1pXZDFiR0YwYjNKNVRXOWtaV3hPZFcxaVpYSThMMnRsZVQ0S0NRazhjM1J5YVc1blBrRXhNak0wUEM5emRISnBibWMrQ2drSlBHdGxlVDVUYVdkdWFXNW5SblZ6WlR3dmEyVjVQZ29KQ1R4MGNuVmxMejRLQ1FrOGEyVjVQbFZ1YVhGMVpVTm9hWEJKUkR3dmEyVjVQZ29KQ1R4cGJuUmxaMlZ5UGpFeU16UTFOamM0T1RFeU16UThMMmx1ZEdWblpYSStDZ2s4TDJScFkzUStDZ2s4YTJWNVBsSmxaM1ZzWVhSdmNubEpiV0ZuWlhNOEwydGxlVDRLQ1R4a2FXTjBQZ29KQ1R4clpYaytSR1YyYVdObFZtRnlhV0Z1ZER3dmEyVjVQZ29KQ1R4emRISnBibWMrUWp3dmMzUnlhVzVuUGdvSlBDOWthV04wUGdvSlBHdGxlVDVUYjJaMGQyRnlaVlZ3WkdGMFpWSmxjWFZsYzNSSmJtWnZQQzlyWlhrK0NnazhaR2xqZEQ0S0NRazhhMlY1UGtWdVlXSnNaV1E4TDJ0bGVUNEtDUWs4ZEhKMVpTOCtDZ2s4TDJScFkzUStDZ2s4YTJWNVBsVkpTME5sY25ScFptbGpZWFJwYjI0OEwydGxlVDRLQ1R4a2FXTjBQZ29KQ1R4clpYaytRbXgxWlhSdmIzUm9RV1JrY21WemN6d3ZhMlY1UGdvSkNUeHpkSEpwYm1jK1ptWTZabVk2Wm1ZNlptWTZabVk2Wm1ZOEwzTjBjbWx1Wno0S0NRazhhMlY1UGtKdllYSmtTV1E4TDJ0bGVUNEtDUWs4YVc1MFpXZGxjajR5UEM5cGJuUmxaMlZ5UGdvSkNUeHJaWGsrUTJocGNFbEVQQzlyWlhrK0Nna0pQR2x1ZEdWblpYSStNekkzTmpnOEwybHVkR1ZuWlhJK0Nna0pQR3RsZVQ1RmRHaGxjbTVsZEUxaFkwRmtaSEpsYzNNOEwydGxlVDRLQ1FrOGMzUnlhVzVuUG1abU9tWm1PbVptT21abU9tWm1PbVptUEM5emRISnBibWMrQ2drSlBHdGxlVDVWU1V0RFpYSjBhV1pwWTJGMGFXOXVQQzlyWlhrK0Nna0pQR1JoZEdFK0Nna0pUVWxKUkRCM1NVSkJha05EUVRoM1JVbFFORU16YzNGUmRGQXhVekpvZDBKYWVrTnZTR056YjBneWVFNTFOV01yWVRSUk5EVnZTakZOUzBZekNna0pRa1ZGUlRKbE9UTmxiMVpQZUhWbU1HVkxVRlZ4VGtWbk5ucE5iRUp6VG5FcmFuSXJVbkZOUVhoVGFGWkJMMk5VTlc5dWEzSXdkQ3RGTUVoTENna0pibE5rZGtoTk1pOUdaWFJ5VDNGcFQwazBSSFpJVUVsRVZ6QkVNblZCVVZFemFXOWlVSGRoUVd4R2JGaElVRmR5T0UxS0x5dDNVVkZIVkd4dUNna0pSVmhQTVRaT2RESnJWVVVyZHk4dlFteEhkMVE0VjNoU1pYa3ZTVTQxU1cxTmJHdFplbHBzU25wYWNrODNkVmwwYkhCbFozazJLM2hKYVd3eUNna0pRakpZYkhrMGFVZDRVbHBwVWxkNU5YTkxjRkZ2TWxsNmIwcEZiVzFYVTI1bWFuVXdZMVV5TDNKaU9VWkNkblZXYVM5clYxTkdia0ZyZERSNUNna0pjVkYzTkdzd2FXSjBjRFZYSzFsVlEwTnZabTh6ZVdWdWFrMHlWV013Yml0NVNFeHlVMjB3UlRsUFVETndkRXhVTjNaSGNuSm1hM0l6V0ZKcENna0pkSGRFY0dSQ1QzTnpLMWg2U0VGUldFdDFjRzg1V0dreFVXMU9iR3AxVkdveGFrcFpielpOYzFreU9VUllPVVZhY0ZkRWRtcEpjMGw1VEhkNENna0pRalJqYlVsVFZXWTRRbTV5VWxGSE9VUkJNMDFsWXpaaWFGUmtVRUpqZFV0WFpIQkNibTVETWxZNFYzQm1UWEJ3VlVRMlUyUm5kVzVwZWpaNkNna0pURWN3Tm1OR1IzZHZVWFp1V1hoUmExVnJhMnBrV1dSNk5HODVlWE01TDNaeFEySnhabkJ1TkhSalpFa3lNV001WjI5TmQweG9SSE5vWW1zMUNna0pVRU5hUW5Ob05VWTBVMUpTYVdkQlYzSkJVME5CZWprNE1rSTNiemh3UTBOYUwycFpLM2xhUTNwQmIzSjZTRzV6UjJaMmQwdHBTbEJCVFdwcENna0paVEEwUnpScVNrMDRjRXBSVVU1dVdtRmhVQ3QwUm1Wc1pHaEVSMUZ1YnpBMGRtWktSRmt6T0VaR1RTdGhaVU4zZWxKeVF5OURVR0pyWlZwUkNna0pOWFI1TlRkTVNYTnpNVWh5VW1VelNURmpLMFpNTlhCdVptd3ZhRXN4UWpGMVFUUkhSRFJXYkZreFUweE1NWGsxYWpSSGRVWlVNMWhUZUhwaUNna0pXbEl2Wm1KRWExVjVWSE5VTTNJMmVHZG9XblJOTkVKWVNXOWhOakphUkVNelNWQnRUMko0UzJKb2JHRkxRVFJ0U3pKek0xRkNORlpqTmxNdkNna0piVFoxWVRaUWFrd3ZRakUxUXpCalRHcHlNVU5OYjB4MEx6YzRURlZSVjIxR1JYVjVTa2hrZG5SVE5uaEliV3RNUkc5RlpXMXRNSGxEY0dKcUNna0pNbWhyUm10MmQzZElTbGcyU0RGaVVtMUtXUzlIVW1ZMVVYVklXRFZLZGxrM1pHaE9ZMll6TkVObWFWRXhSSGR3WjJWS1VrdzVlVE4yU0cwdkNna0paa0ZTVjBKeFdETmtXalYxVlVwWGNVTnpNa2x2TUZkSVJHZHFNVGgzY1c1dlVFdzJRblJIY2pWaFdFSkZlR0YzV2twR1QxWk9jVlpqVjJsUENna0pPRTlMTXpodVNERkthR2N4Vms0NFVVUkJlbGhtVEVwalEydzBVRU42TW01c1ZscFNNRGwxV25GME5scFBhWEZqVlVOeVozaFpiVGRJUWt0YUNna0pPUzlCUm1JeVZteExVRlJaVFROdWVYQkRlR2g1TW1OTVFub3dLM1JDSzBWNlYwaFRiamx6VTNGTWVsTjFlRkJPZEdJeFkyMUZNbm81T0ZOb0Nna0pNazFVVnpKYVdrNDJOV2R2WWt4clNVNXdiemRVYjFSQk1tNTBjSFkxWmpCcWRsaHBWblpJVjFWMWRtaFVTVmxMWkc0dkt6QTBjek5KUTBWTENna0pRVmxKUTBOUE5qZ3Zha3h1Y0RWUVVFUnVSbVZzUTNaMWQwZEZSVEZrYjBsTU56WjZVbGxOT1dscldUSkhSVkI1Tlc1WGRXMXlkWHA0VTJSQ0Nna0pNVVJCTm5OT2VVeFFhbk4yUW5CbllVVm5XbUkwT1VwWFNqbEVSVTV2WVdaS2VHUTRkbEJvUm5wT1JIWkVMME5SS3pVNFZHdENZbVl3V0VWTENna0phMnhJUnpkek9GWTBTa1JzWVM5ak1UQmpTRGN5V1M4d0wwbE9VaTlrVVZrMVYzRlNhSE5pU0VWRmFsQlZla2REVEdOVlBRb0pDVHd2WkdGMFlUNEtDUWs4YTJWNVBsZHBabWxCWkdSeVpYTnpQQzlyWlhrK0Nna0pQSE4wY21sdVp6NW1aanBtWmpwbVpqcG1aanBtWmpwbVpqd3ZjM1J5YVc1blBnb0pQQzlrYVdOMFBnbzhMMlJwWTNRK0Nqd3ZjR3hwYzNRKzwvc3RyaW5nPgoJCTxrZXk+UGhvbmVOdW1iZXJOb3RpZmljYXRpb25VUkw8L2tleT4KCQk8c3RyaW5nPmh0dHBzOi8vYWxiZXJ0LmFwcGxlLmNvbS9kZXZpY2VzZXJ2aWNlcy9waG9uZUhvbWU8L3N0cmluZz4KCQk8a2V5PkFjdGl2YXRpb25TdGF0ZTwva2V5PgoJCTxzdHJpbmc+QWN0aXZhdGVkPC9zdHJpbmc+Cgk8L2RpY3Q+CjwvZGljdD4KPC9wbGlzdD4=");
			this.scpClient_0.Upload(new MemoryStream(array), "/tmp/Backup/com.apple.commcenter.device_specific_nobackup.plist");
			this.sshClient_0.CreateCommand("rm /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.label11.Text = "Fixing iServices...";
			this.progressBar2.Value += 40;
			this.method_43<T2, T7, T8>(1000);
			this.sshClient_0.CreateCommand("mv -f /tmp/Backup/com.apple.commcenter.device_specific_nobackup.plist /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/lib/mon5"), "/af2ccdService.dylib");
			this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/lib/mon6"), "/af2ccdService.plist");
			this.sshClient_0.CreateCommand("cd / ; mv af2ccdService.dylib /Library/MobileSubstrate/DynamicLibraries/af2ccdService.dylib").Execute();
			this.sshClient_0.CreateCommand("cd / ; mv af2ccdService.plist /Library/MobileSubstrate/DynamicLibraries/af2ccdService.plist").Execute();
			this.progressBar2.Value += 30;
			try
			{
				this.sshClient_0.CreateCommand("chmod +x /usr/bin/ldrestart && /usr/bin/ldrestart").Execute();
			}
			catch
			{
			}
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "iServices fixed, respringing...";
			this.label18.Visible = true;
			this.button5.Text = "Close";
			this.label18.Text = "Congrats! iServices has been fixed\ud83c\udf89\ud83c\udf89";
			MessageBox.Show("iServices has been fixed, respringing!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception t)
		{
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "There was an error fixing iServices....";
			MessageBox.Show(t.Message);
			MessageBox.Show("There was an error fixing iServices!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00212184 File Offset: 0x00210384
	public void method_64<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
	{
		try
		{
			this.label11.ForeColor = Color.Black;
			this.label11.Text = "Mounting filesystems...";
			this.progressBar2.Value = 0;
			this.method_23<T4, T5, T6, T0>("./files/Backup/" + this.string_5 + ".zip", "./tmp/" + this.string_5);
			this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
			this.label11.Text = "Pre-activation stuff...";
			this.progressBar2.Value += 10;
			this.method_43<T2, T7, T8>(3000);
			T0 t = this.method_44<T9, T0, T10>("find /private/var/containers/Data/System/ -iname 'internal'").Replace("Library/internal", "").Replace("\n", "")
				.Replace("//", "/");
			this.sshClient_0.CreateCommand("chflags nouchg " + t + "Library/internal/data_ark.plist").Execute();
			this.sshClient_0.CreateCommand("chflags nouchg " + t + "Library/activation_records/activation_record.plist").Execute();
			this.sshClient_0.CreateCommand("chflags nouchg /var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
			this.sshClient_0.CreateCommand("chflags nouchg /var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chflags nouchg /var/root/Library/Lockdown/data_ark.plist").Execute();
			this.sshClient_0.CreateCommand("rm " + t + "Library/internal/data_ark.plist").Execute();
			this.sshClient_0.CreateCommand("chmod -R 755 /var/containers/Data/System").Execute();
			this.sshClient_0.CreateCommand("mkdir -p " + t + "Library/activation_records").Execute();
			this.sshClient_0.CreateCommand("mkdir /var/mobile/Media/Downloads/" + this.string_5).Execute();
			this.scpClient_0.Upload(new DirectoryInfo(Environment.CurrentDirectory + "/tmp/" + this.string_5), "/var/mobile/Media/Downloads/" + this.string_5);
			this.label11.Text = "Preparing activation files...(stucked? Use another Palen1x version)";
			this.progressBar2.Value += 20;
			this.method_43<T2, T7, T8>(3000);
			this.sshClient_0.CreateCommand("cp -r /var/mobile/Media/Downloads" + this.string_5 + "/3 /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("usr/sbin/chown root:mobile /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chmod 755 /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.sshClient_0.CreateCommand("chflags uchg /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
			this.label11.Text = "Activating device...";
			this.progressBar2.Value += 10;
			this.method_43<T2, T7, T8>(3000);
			this.sshClient_0.CreateCommand(string.Concat(new T0[] { "cp -r /var/mobile/Media/Downloads/", this.string_5, "/1 ", t, "/Library/activation_records/activation_record.plist" })).Execute();
			this.sshClient_0.CreateCommand("chflags -R nouchg /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates").Execute();
			this.sshClient_0.CreateCommand("cp -rp /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/FactoryActivation.pem /System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/RaptorActivation.pem").Execute();
			this.sshClient_0.CreateCommand("launchctl unload /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			this.sshClient_0.CreateCommand("launchctl load /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
			this.label11.Text = "Fixing iCloud Sign-In and stuff...";
			this.progressBar2.Value += 20;
			this.method_43<T2, T7, T8>(3000);
			T0 t2 = this.method_44<T9, T0, T10>("grep -Rhl 'com.apple.fairplayd.H2' /private/var/containers/Data/System");
			this.sshClient_0.CreateCommand("mkdir -p " + t2 + "/Documents/Media/iTunes_Control/iTunes/../../Library/FairPlay/iTunes_Control/iTunes").Execute();
			this.sshClient_0.CreateCommand(string.Concat(new T0[] { "cp -rp /private/var/mobile/Media/Downloads/", this.string_5, "/FairPlay/iTunes_Control/iTunes/IC-Info.sisv ", t2, "/Documents/Media/iTunes_Control/iTunes/IC-Info.sidv" })).Execute();
			this.sshClient_0.CreateCommand(string.Concat(new T0[] { "cp -rp /private/var/mobile/Media/Downloads/", this.string_5, "/FairPlay/iTunes_Control/iTunes/IC-Info.sisv ", t2, "/Documents/Library/FairPlay/iTunes_Control/iTunes/IC-Info.sisv" })).Execute();
			this.sshClient_0.CreateCommand("mkdir -m 755 -p /private/var/mobile/Library/FairPlay/iTunes_Control/iTunes/").Execute();
			this.sshClient_0.CreateCommand("cp -rp /private/var/mobile/Media/Downloads/" + this.string_5 + "/FairPlay/iTunes_Control/iTunes/IC-Info.sisv /private/var/mobile/Library/FairPlay/iTunes_Control/iTunes/IC-Info.sisv").Execute();
			this.sshClient_0.CreateCommand("chmod 0644 /private/var/mobile/Library/FairPlay/iTunes_Control/iTunes/*").Execute();
			this.sshClient_0.CreateCommand("chown -R mobile:mobile /private/var/mobile/Library/FairPlay/").Execute();
			this.label11.Text = "Skipping Set-Up...";
			this.progressBar2.Value += 20;
			this.method_43<T2, T7, T8>(3000);
			T1[] array = Convert.FromBase64String("YnBsaXN0MDDfECABAgMEBQYHCAkKCwwNDg8QERITFBUWFxgZGhscHR4fICEiIiIlIiciKSIrIiIuIjIiNCI2JTolJyI9IiIiIiIiXFNldHVwVmVyc2lvbl8QEVVzZXJDaG9zZUxhbmd1YWdlXxAXUEJEaWFnbm9zdGljczRQcmVzZW50ZWRfEBBQcml2YWN5UHJlc2VudGVkXxAfV2ViS2l0QWNjZWxlcmF0ZWREcmF3aW5nRW5hYmxlZF8QFFBheW1lbnRNaW5pQnVkZHk0UmFuXxArV2ViS2l0TG9jYWxTdG9yYWdlRGF0YWJhc2VQYXRoUHJlZmVyZW5jZUtleV5NZXNhMlByZXNlbnRlZFZMb2NhbGVfECFQaG9uZU51bWJlclBlcm1pc3Npb25QcmVzZW50ZWRLZXlfEBRzZXR1cE1pZ3JhdG9yVmVyc2lvbl8QGEhTQTJVcGdyYWRlTWluaUJ1ZGR5M1Jhbl8QFVNldHVwRmluaXNoZWRBbGxTdGVwc18QGWxhc3RQcmVwYXJlTGF1bmNoU2VudGluZWxfEBRBcHBsZUlEUEIxMFByZXNlbnRlZF8QFVByaXZhY3lDb250ZW50VmVyc2lvbl8QH1VzZXJJbnRlcmZhY2VTdHlsZU1vZGVQcmVzZW50ZWRYTGFuZ3VhZ2VfEBxIb21lQnV0dG9uQ3VzdG9taXplUHJlc2VudGVkWWNocm9uaWNsZV8QFUFuaW1hdGVMYW51Z2FnZUNob2ljZV1TZXR1cExhc3RFeGl0XxAQTWFnbmlmeVByZXNlbnRlZF8QFFdlYkRhdGFiYXNlRGlyZWN0b3J5XxAnV2ViS2l0T2ZmbGluZVdlYkFwcGxpY2F0aW9uQ2FjaGVFbmFibGVkWlNldHVwU3RhdGVfEBNBdXRvVXBkYXRlUHJlc2VudGVkXVJlc3RvcmVDaG9pY2VfEBNTY3JlZW5UaW1lUHJlc2VudGVkXxASUGFzc2NvZGU0UHJlc2VudGVkWVNldHVwRG9uZV8QIldlYktpdFNocmlua3NTdGFuZGFsb25lSW1hZ2VzVG9GaXQQCwkJCQgJXxAaL3Zhci9tb2JpbGUvTGlicmFyeS9DYWNoZXMJVWVzX0RPCRAKCQmiLzAzQcJgTHmPqfwQAAkQAglVZXMtRE8J0Tc4WGZlYXR1cmVzoAgzQcJgTHXyZncICV8QE1NldHVwVXNpbmdBc3Npc3RhbnQJCQkJCQkACABLAFgAbACGAJkAuwDSAQABDwEWAToBUQFsAYQBoAG3Ac8B8QH6AhkCIwI7AkkCXAJzAp0CqAK+AswC4gL3AwEDJgMoAykDKgMrAywDLQNKA0sDUQNSA1QDVQNWA1kDYgNkA2UDZwNoA24DbwNyA3sDfAN9A4YDhwOIA54DnwOgA6EDogOjAAAAAAAAAgEAAAAAAAAARAAAAAAAAAAAAAAAAAAAA6Q=");
			this.scpClient_0.Upload(new MemoryStream(array), "/var/mobile/Library/Preferences/com.apple.purplebuddy.plist");
			this.sshClient_0.CreateCommand("chmod 600 /var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
			this.label11.Text = "Post-activation clean-ups...";
			this.progressBar2.Value += 10;
			this.method_43<T2, T7, T8>(3000);
			this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Media/Downloads/" + this.string_5).Execute();
			T2 t3 = !this.checkBox14.Checked;
			if (t3 != null)
			{
				this.sshClient_0.CreateCommand("mount -o rw,union,update /private/preboot").Execute();
				this.sshClient_0.CreateCommand("mv -f /private/preboot/$(cat /private/preboot/active)/usr/local/standalone/firmware/Baseband /private/preboot/$(cat /private/preboot/active)/usr/local/standalone/firmware/Baseband2").Execute();
			}
			try
			{
				this.sshClient_0.CreateCommand("shutdown -r now").Execute();
			}
			catch
			{
			}
			this.label11.ForeColor = Color.MediumSeaGreen;
			this.label11.Text = "Bypass done...";
			this.progressBar2.Value += 10;
			Directory.Delete("./tmp/" + this.string_5, true);
			this.label18.Visible = true;
			this.button5.Text = "Close";
			this.label18.Text = "Congrats! Your iDevice has been activated\ud83c\udf89\ud83c\udf89";
			MessageBox.Show("Your device has been sucessfully activated! \n\nDon't restore or erase or set screen-lock.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception t4)
		{
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "There was an error activating device....";
			Directory.Delete("./tmp/" + this.string_5, true);
			MessageBox.Show(t4.Message);
			MessageBox.Show("There was an error activating device!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00212868 File Offset: 0x00210A68
	private void method_65<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.jailbreakPanel.Visible = true;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00212884 File Offset: 0x00210A84
	private void method_66<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		T0 t = this.checkBox2.Checked || this.checkBox5.Checked || this.checkBox9.Checked || this.checkBox10.Checked || this.checkBox11.Checked || this.checkBox12.Checked;
		if (t != null)
		{
			this.Anno.Text = "Please Connect your device in Normal mode and click on Check Device.\r\n";
		}
		else
		{
			this.Anno.Text = "Please Connect your device in PwnDFU mode and click on Check Device.\r\n";
		}
		this.jailbreakPanel.Visible = false;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00212914 File Offset: 0x00210B14
	private async void method_67<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.feedbacktext.ForeColor = Color.Black;
		this.feedbacktext.Text = "Checking Device....";
		TaskAwaiter<bool> taskAwaiter2;
		if (!this.checkBox5.Checked && !this.checkBox9.Checked && !this.checkBox10.Checked && !this.checkBox11.Checked && !this.checkBox12.Checked)
		{
			if (this.checkBox2.Checked)
			{
				TaskAwaiter<bool> taskAwaiter = this.method_37<Task<bool>>().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				taskAwaiter.GetResult();
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter3 = this.method_34<Task<bool>>().GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				taskAwaiter3.GetResult();
			}
		}
		else
		{
			await this.method_36<Task<bool>>();
		}
		if (this.string_3 == "DFU")
		{
			this.feedbacktext.ForeColor = Color.MediumSeaGreen;
			this.feedbacktext.Text = "Device Connected....";
			this.Anno.Text = string.Concat(new string[]
			{
				this.string_2,
				" with ECID ",
				this.ECIDValue.Text,
				" connected in ",
				this.string_3,
				" mode"
			});
			if (!(this.actbutt.Text == "Start"))
			{
				this.actbutt.Text = "Start";
				MessageBox.Show("Device connected Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter4 = this.method_34<Task<bool>>().GetAwaiter();
				if (!taskAwaiter4.IsCompleted)
				{
					await taskAwaiter4;
					taskAwaiter4 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter4.GetResult())
				{
					this.actbutt.Text = "Check Device";
					MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else if (!this.bool_1)
				{
					MessageBox.Show("Your ECID is not registered! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					ProcessStartInfo processStartInfo = Form1.Class25.smethod_0();
					processStartInfo.FileName = this.string_18 + "/";
					processStartInfo.UseShellExecute = true;
					Process.Start(processStartInfo);
				}
				else
				{
					this.jailbreakPanel.Visible = true;
					this.bootPanel.Visible = true;
					if (!this.checkBox6.Checked)
					{
						this.button3.Text = "Jailbreak Device";
						this.jbtext.Text = "Click \"Jailbreak Device\" to start.";
						this.label9.Text = "NOTICE: Make sure you have already downloaded the ramdisk for your \r\ndevice before starting the jailbreak.";
						this.label5.Text = "Great! Almost there. Now click on \"Jailbreak Device\" to Jailbreak your device \r\nand enable SSH connection.";
					}
					else
					{
						this.button3.Text = "Boot purple";
						this.jbtext.Text = "Click \"Boot purple\" to start.";
						this.label9.Text = "NOTICE: Make sure you have already downloaded the ramdisk for your \r\ndevice before starting purple boot.";
						this.label5.Text = "Great! Almost there. Now click on \"Boot purple\" to put your device \r\ninto purple mode and enable port communication.";
					}
				}
			}
		}
		else if (!(this.string_3 == "Recovery"))
		{
			if (!(this.string_3 == "Normal"))
			{
				this.feedbacktext.ForeColor = Color.Crimson;
				this.feedbacktext.Text = "No device Connected....";
				this.method_22<bool>();
				MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				this.feedbacktext.ForeColor = Color.MediumSeaGreen;
				this.feedbacktext.Text = "Device Connected....";
				this.Anno.Text = string.Concat(new string[]
				{
					this.string_2,
					" with ECID ",
					this.ECIDValue.Text,
					" connected in ",
					this.string_3,
					" mode"
				});
				if (this.actbutt.Text == "Start")
				{
					TaskAwaiter<bool> taskAwaiter5 = this.method_36<Task<bool>>().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						await taskAwaiter5;
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					bool result = taskAwaiter5.GetResult();
					if (result)
					{
						if (!this.bool_1)
						{
							ProcessStartInfo processStartInfo2 = Form1.Class25.smethod_0();
							processStartInfo2.FileName = this.string_18 + "/";
							processStartInfo2.UseShellExecute = true;
							Process.Start(processStartInfo2);
							MessageBox.Show("Your ECID is not registered! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						else if (this.checkBox5.Checked)
						{
							this.jailbreakPanel.Visible = true;
							this.bootPanel.Visible = true;
							this.backUpPanel.Visible = true;
							this.button5.Text = "Generate files";
							this.linkLabel3.Visible = false;
							this.label7.Text = "Great! Now keep your device connected with an active internet connection, \nlet's generate activation files for your device.";
							this.label11.Text = "Click on \"Generate files\" to start.";
							this.label17.Text = "NOTICE: Activating your device with these generated activation files will disable \nthe function of calls and mobile data usage. However after successfully generating \nthe files, you can activate your device like normal passcode bypass.";
						}
						else if (this.checkBox2.Checked)
						{
							if (!File.Exists("./files/Backup/" + this.string_5 + ".zip"))
							{
								this.jailbreakPanel.Visible = true;
								this.bootPanel.Visible = true;
								this.backUpPanel.Visible = true;
							}
							else
							{
								this.label7.Text = "Great!  We found a backUp for your device, lets retore the BackUp and\r\nactivate your device.";
								this.label11.Text = "Click on \"Activate\" to start.";
								this.label17.Text = "NOTICE: Do not disonnect the device until Activation is done. If however\r\nyou encountered an error. Click on start afresh and start the bypass again.\r\n";
								this.button5.Text = "Activate";
								this.jailbreakPanel.Visible = true;
								this.bootPanel.Visible = true;
								this.backUpPanel.Visible = true;
								this.linkLabel3.Visible = false;
							}
						}
						else if (!this.checkBox12.Checked)
						{
							if (this.checkBox9.Checked)
							{
								this.jailbreakPanel.Visible = true;
								this.bootPanel.Visible = true;
								this.backUpPanel.Visible = true;
								this.linkLabel3.Visible = false;
								if (!(this.string_9 != "Unactivated"))
								{
									this.button5.Text = "Bypass Hello";
									this.label7.Text = "Great! Now make sure you have jailbroken your device with Palen1x.";
									this.label11.Text = "Click on \"Bypass\" to start.";
									this.label17.Text = "NOTICE: Activating your device with this bypass method will disable the function \nof calls and mobile data usage. However after successfully bypassing your device \nyou can use your device like you normally do, and its fully untethered.";
								}
								else
								{
									this.button5.Text = "Fix iServices";
									this.label7.Text = "Great! We noticed that your device is already activated. ";
									this.label11.Text = "Click on \"Fix iServices\" to start.";
									this.label17.Text = "NOTICE: This fix is for Checkra1n supported devices only, starting from ios 12-14, \nit will attempt to fix errors when using apple services such as siri, facetime,\niMessage, iCloud sign in etc";
								}
							}
							else if (!this.checkBox10.Checked)
							{
								if (this.checkBox11.Checked)
								{
									this.jailbreakPanel.Visible = true;
									this.bootPanel.Visible = true;
									this.backUpPanel.Visible = true;
									this.button5.Text = "Generate token";
									this.linkLabel3.Visible = false;
									this.label7.Text = "Great! Now make sure you have connected your device in Normal mode.";
									this.label11.Text = "Click on \"Generate Token\" to start.";
									this.label17.Text = "NOTICE: This method does not turn FMI off on your device, however you can send \nthe generaeted file to me on telegram @iosnemes1s with a fee and I will turn \nFMI off for you, this method works on all devices including iPads.";
								}
							}
							else
							{
								this.jailbreakPanel.Visible = true;
								this.bootPanel.Visible = true;
								this.backUpPanel.Visible = true;
								this.button5.Text = "Remove ID";
								this.linkLabel3.Visible = false;
								this.label7.Text = "Great! Now make sure you have jailbroken your device with Checkra1n or Palen1x.";
								this.label11.Text = "Click on \"Remove ID\" to start.";
								this.label17.Text = "NOTICE: This method hides the apple ID from your device, however it doesnt turn \nFMI off. So if you erase your device it might be iCloud locked. Use with caution \nand make sure you know exactly what you are doing.";
							}
						}
						else
						{
							this.jailbreakPanel.Visible = true;
							this.bootPanel.Visible = true;
							this.backUpPanel.Visible = true;
							this.button5.Text = "Bypass MDM";
							this.linkLabel3.Visible = false;
							this.label7.Text = "Great! Now make sure you have restored your device with the latest firmware.";
							this.label11.Text = "Click on \"Bypass MDM\" to start.";
							this.label17.Text = "NOTICE: This is a temporal MDM bypass, do not restore or update your device, if \nyour device becomes MDM locked again, you can still bypass again with this tool.";
						}
					}
					else
					{
						this.actbutt.Text = "Check Device";
						MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				else
				{
					this.actbutt.Text = "Start";
					MessageBox.Show("Device connected Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}
		else
		{
			this.feedbacktext.ForeColor = Color.MediumSeaGreen;
			this.Anno.Text = string.Concat(new string[]
			{
				this.string_2,
				" ( iOS ",
				Form1.smethod_2<List<string>, byte, MemoryStream, StreamReader, string, bool, char>(Form1.smethod_1<Process, bool, string, ProcessStartInfo>("-q", "iBoot", true, false).Replace("iBoot: ", "")),
				" ) connected in ",
				this.string_3,
				" mode"
			});
			this.feedbacktext.Text = "Connected in recovery mode....";
			if (!(this.actbutt.Text == "Start"))
			{
				this.actbutt.Text = "Start";
				MessageBox.Show("Device connected Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter6 = this.method_34<Task<bool>>().GetAwaiter();
				if (!taskAwaiter6.IsCompleted)
				{
					await taskAwaiter6;
					taskAwaiter6 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				bool result2 = taskAwaiter6.GetResult();
				if (result2)
				{
					if (!this.bool_1)
					{
						MessageBox.Show("Your ECID is not registered! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						ProcessStartInfo processStartInfo3 = Form1.Class25.smethod_0();
						processStartInfo3.FileName = this.string_18 + "/";
						processStartInfo3.UseShellExecute = true;
						Process.Start(processStartInfo3);
					}
					else
					{
						this.devicePicture.Visible = true;
						if (!(this.string_13 == "iPhone10,3") && !(this.string_13 == "iPhone10,6"))
						{
							this.devicePicture.Image = Class34.Bitmap_0;
							this.label22.Visible = true;
						}
						else
						{
							this.sideButton678Lab.Visible = true;
							this.devicePicture.Image = Class34.Bitmap_1;
						}
						int num = 0;
						string[] array = this.string_13.Split(new char[] { 'e', ',' });
						foreach (string text in array)
						{
							if (int.TryParse(text, out num))
							{
								break;
							}
							text = null;
						}
						string[] array2 = null;
						if (num >= 9)
						{
							this.volumeDown78XLab.Visible = true;
						}
						else
						{
							this.pressDFU.Text = "2. Press and hold the Side \r\nand Home buttons \r\ntogether (4)\r\n";
							this.label20.Text = "3. Release the Side button \r\nBUT KEEP HOLDING the \r\nHome button (10)\r\n";
							this.homeButtLab.Visible = true;
						}
						this.jailbreakPanel.Visible = true;
						this.bootPanel.Visible = true;
						this.backUpPanel.Visible = true;
						this.dfuPanel.Visible = true;
						array = null;
					}
				}
				else
				{
					this.actbutt.Text = "Check Device";
					MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0021295C File Offset: 0x00210B5C
	private void method_68<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.ECIDValue.ForeColor = Color.DodgerBlue;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x0021297C File Offset: 0x00210B7C
	private void method_69<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.ECIDValue.ForeColor = Color.Black;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x0021299C File Offset: 0x00210B9C
	private void method_70<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		Clipboard.SetText(this.ECIDValue.Text);
		MessageBox.Show("Device ECID successfully copied!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x002129CC File Offset: 0x00210BCC
	private async void method_71<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		if (!File.Exists("C:\\Windows\\System32\\msvcr120.dll"))
		{
			File.Copy(Environment.CurrentDirectory + "/files/msvcr120.dll", "C:\\Windows\\System32\\msvcr120.dll");
		}
		this.feedbacktext.ForeColor = Color.Black;
		this.feedbacktext.Text = "Checking Device....";
		await this.method_34<Task<bool>>();
		if (!(this.string_3 == "Recovery"))
		{
			if (!(this.string_3 == "DFU"))
			{
				this.feedbacktext.ForeColor = Color.Crimson;
				this.feedbacktext.Text = "No device Connected....";
			}
			else
			{
				this.feedbacktext.ForeColor = Color.MediumSeaGreen;
				this.feedbacktext.Text = "Connected in " + this.string_3 + " mode....";
				this.Anno.Text = string.Concat(new string[]
				{
					this.string_2,
					" with ECID ",
					this.ECIDValue.Text,
					" connected in ",
					this.string_3,
					" mode."
				});
				this.actbutt.Text = "Start";
			}
		}
		else
		{
			this.feedbacktext.ForeColor = Color.MediumSeaGreen;
			this.feedbacktext.Text = "Connected in " + this.string_3 + " mode....";
			this.actbutt.Text = "Start";
			this.Anno.Text = string.Concat(new string[]
			{
				this.string_2,
				" ( iOS ",
				Form1.smethod_2<List<string>, byte, MemoryStream, StreamReader, string, bool, char>(Form1.smethod_1<Process, bool, string, ProcessStartInfo>("-q", "iBoot", true, false).Replace("iBoot: ", "")),
				" ) connected in ",
				this.string_3,
				" mode"
			});
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00212A14 File Offset: 0x00210C14
	private void method_72<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		T0 t = !this.checkBox1.Checked;
		if (t != null)
		{
			this.actbutt.Enabled = false;
		}
		else
		{
			this.actbutt.Enabled = true;
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00212A50 File Offset: 0x00210C50
	private void method_73<T0, T1, T2, T3, T4>(T2 gparam_0, T3 gparam_1)
	{
		T0 t = MessageBox.Show("Do you want to Exit!", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
		T1 t2 = t == 7;
		if (t2 != null)
		{
			gparam_1.Cancel = true;
		}
		else
		{
			ProcessStartInfo processStartInfo = Form1.smethod_4();
			processStartInfo.FileName = "https://www.youtube.com/@iosnemes1s";
			processStartInfo.UseShellExecute = true;
			Process.Start(processStartInfo);
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00212AA0 File Offset: 0x00210CA0
	private void method_74<T0, T1, T2>(T0 gparam_0, T1 gparam_1)
	{
		ProcessStartInfo processStartInfo = Form1.smethod_4();
		processStartInfo.FileName = "https://mega.nz/folder/EmoykbTb#QLKZ9bdS3O2mSkNgw1ITKw";
		processStartInfo.UseShellExecute = true;
		Process.Start(processStartInfo);
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00212ACC File Offset: 0x00210CCC
	public void method_75<T0, T1, T2>()
	{
		this.button5.Enabled = false;
		this.comboBox1.Items.Clear();
		T0[] portNames = SerialPort.GetPortNames();
		T1 items = this.comboBox1.Items;
		T2[] array = portNames;
		T2[] array2 = array;
		items.AddRange(array2);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00212B14 File Offset: 0x00210D14
	private async void method_76<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		if (!(this.button3.Text == "Next"))
		{
			this.button3.Enabled = false;
			this.button4.Enabled = false;
			this.button11.Enabled = false;
			this.jbtext.ForeColor = Color.Black;
			TaskAwaiter<bool> taskAwaiter = this.method_34<Task<bool>>().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			bool result = taskAwaiter.GetResult();
			if (result)
			{
				try
				{
					if (this.bool_0)
					{
						if (Directory.Exists("./tmp/" + this.string_1))
						{
							Directory.Delete("./tmp/" + this.string_1, true);
						}
						this.method_23<Ionic.Zip.ZipFile, IEnumerator<ZipEntry>, ZipEntry, string>(string.Concat(new string[] { "./ramdisk/", this.string_14, "/", this.string_1, ".zip" }), "./tmp/" + this.string_1);
						if (this.checkBox6.Checked)
						{
							TaskAwaiter taskAwaiter3 = this.method_42<Task>().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
							}
							taskAwaiter3.GetResult();
						}
						else
						{
							TaskAwaiter taskAwaiter5 = this.method_41<Task>().GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								await taskAwaiter5;
								TaskAwaiter taskAwaiter4;
								taskAwaiter5 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
							}
							taskAwaiter5.GetResult();
						}
						this.button3.Enabled = true;
						this.button11.Enabled = true;
						if (Directory.Exists("./tmp/" + this.string_1))
						{
							Directory.Delete("./tmp/" + this.string_1, true);
						}
					}
					else
					{
						try
						{
							await Task.Run(new Action(this.method_109<Process, bool, string, int, System.Windows.Forms.Timer>));
						}
						catch
						{
						}
						await this.method_34<Task<bool>>();
						if (!this.bool_0)
						{
							this.button4.Enabled = true;
							this.button3.Enabled = true;
							this.button11.Enabled = true;
							this.jbtext.ForeColor = Color.Crimson;
							this.jbtext.Text = "Couldn't exploit device....";
							MessageBox.Show("Failed to install exploit on device using gaster\n\nTry again or exploit it manually!");
						}
						else
						{
							this.jbtext.Text = "Device exploited successfully...";
							this.method_43<bool, int, System.Windows.Forms.Timer>(1000);
							if (Directory.Exists("./tmp/" + this.string_1))
							{
								Directory.Delete("./tmp/" + this.string_1, true);
							}
							this.method_23<Ionic.Zip.ZipFile, IEnumerator<ZipEntry>, ZipEntry, string>(string.Concat(new string[] { "./ramdisk/", this.string_14, "/", this.string_1, ".zip" }), "./tmp/" + this.string_1);
							if (this.checkBox6.Checked)
							{
								TaskAwaiter taskAwaiter6 = this.method_42<Task>().GetAwaiter();
								if (!taskAwaiter6.IsCompleted)
								{
									await taskAwaiter6;
									TaskAwaiter taskAwaiter4;
									taskAwaiter6 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter);
								}
								taskAwaiter6.GetResult();
							}
							else
							{
								TaskAwaiter taskAwaiter7 = this.method_41<Task>().GetAwaiter();
								if (!taskAwaiter7.IsCompleted)
								{
									await taskAwaiter7;
									TaskAwaiter taskAwaiter4;
									taskAwaiter7 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter);
								}
								taskAwaiter7.GetResult();
							}
							this.button3.Enabled = true;
							this.button11.Enabled = true;
							if (Directory.Exists("./tmp/" + this.string_1))
							{
								Directory.Delete("./tmp/" + this.string_1, true);
							}
						}
					}
					return;
				}
				catch (FileNotFoundException)
				{
					this.button4.Enabled = true;
					this.button3.Enabled = true;
					this.button11.Enabled = true;
					MessageBox.Show("No Ramdisk found for this iDevice!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					return;
				}
			}
			this.jailbreakPanel.Visible = false;
			this.bootPanel.Visible = false;
			MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (!this.checkBox6.Checked)
		{
			if (!File.Exists("./files/Backup/" + this.string_5 + ".zip"))
			{
				this.jailbreakPanel.Visible = true;
				this.bootPanel.Visible = true;
				this.backUpPanel.Visible = true;
			}
			else
			{
				this.label7.Text = "Great!  We found a backUp for your device, lets retore the BackUp and\r\nactivate your device.";
				this.label11.Text = "Click on \"Activate\" to start.";
				this.label17.Text = "NOTICE: Do not disonnect the device until Activation is done. If however\r\nyou encountered an error. Click on start afresh and start the bypass again.\r\n";
				this.button5.Text = "Activate";
				this.jailbreakPanel.Visible = true;
				this.bootPanel.Visible = true;
				this.comboBox1.Visible = false;
				this.progressBar2.Visible = true;
				this.backUpPanel.Visible = true;
				this.linkLabel3.Visible = false;
				this.button5.Enabled = true;
			}
		}
		else
		{
			this.label7.Text = "Great!  Now select the COM port for your device amd click on Write SN\nTo change your serial Number.";
			this.label11.Text = "Click on \"Write SN\" to start.";
			this.label17.Text = "NOTICE: Ensure you know what you are doing, using this tool will alter your\n iPhones diagnostic information and change it's serial number permanently.\nEnsure you have a backup of your original Serial Number.";
			this.button5.Enabled = this.comboBox1.SelectedIndex != -1;
			this.button5.Text = "Write SN";
			this.button6.Text = "Refresh";
			this.method_75<string, ComboBox.ObjectCollection, object>();
			this.button6.Visible = true;
			this.progressBar2.Visible = false;
			this.comboBox1.Visible = true;
			this.textBox1.Visible = true;
			this.jailbreakPanel.Visible = true;
			this.bootPanel.Visible = true;
			this.backUpPanel.Visible = true;
			this.linkLabel3.Visible = false;
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00212B5C File Offset: 0x00210D5C
	private async void method_77<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		await this.method_34<Task<bool>>();
		this.jbtext.ForeColor = Color.Black;
		this.jbtext.Text = "Click \"Jailbreak Device\" to start.";
		this.progressBar1.Value = 0;
		this.jailbreakPanel.Visible = false;
		this.bootPanel.Visible = false;
		this.button4.Enabled = true;
		this.button3.Enabled = true;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00212BA4 File Offset: 0x00210DA4
	private void method_78<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		if (!this.checkBox2.Checked)
		{
			this.checkBox5.Enabled = true;
			this.checkBox6.Enabled = true;
			this.checkBox9.Enabled = true;
			this.checkBox10.Enabled = true;
			this.checkBox11.Enabled = true;
			this.checkBox12.Enabled = true;
		}
		else
		{
			this.checkBox5.Enabled = false;
			this.checkBox6.Enabled = false;
			this.checkBox9.Enabled = false;
			this.checkBox10.Enabled = false;
			this.checkBox11.Enabled = false;
			this.checkBox12.Enabled = false;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00212C54 File Offset: 0x00210E54
	public T0 method_79<T0>()
	{
		T0 t = !this.serialPort_0.IsOpen;
		if (t != null)
		{
			try
			{
				this.serialPort_0.PortName = this.comboBox1.SelectedItem.ToString();
				this.serialPort_0.BaudRate = 115200;
				this.serialPort_0.DataBits = 8;
				this.serialPort_0.Open();
			}
			catch
			{
				return 0;
			}
		}
		return 1;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00212CD0 File Offset: 0x00210ED0
	public void method_80()
	{
		this.serialPort_0.Close();
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00212CE8 File Offset: 0x00210EE8
	private async void method_81<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.button5.Enabled = false;
		this.label11.ForeColor = Color.Black;
		if (!(this.button5.Text == "Back Up"))
		{
			if (this.button5.Text == "Write SN")
			{
				try
				{
					if (!this.method_79<bool>())
					{
						MessageBox.Show("Couldn't open Serial port to Write SN", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
					else
					{
						DialogResult dialogResult = MessageBox.Show("Are you sure you want to change SN?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
						if (dialogResult == DialogResult.Yes)
						{
							this.serialPort_0.WriteLine("Syscfg add SrNm " + this.textBox1.Text);
							this.serialPort_0.WriteLine("reset");
							this.label18.Visible = true;
							this.label18.Text = "Congrats! Serial Number has been changed\ud83c\udf89\ud83c\udf89";
							this.button5.Text = "Close";
							this.button6.Visible = false;
							MessageBox.Show("Successfully wriiten " + this.textBox1.Text + " as SN!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						this.method_80();
					}
					goto IL_17B8;
				}
				catch (Exception ex)
				{
					Exception ex2 = ex;
					MessageBox.Show(ex2.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					MessageBox.Show("Oooops, an error occured changing SN", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					goto IL_17B8;
				}
			}
			if (this.button5.Text == "Fix iServices")
			{
				TaskAwaiter<bool> taskAwaiter = this.method_36<Task<bool>>().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else if (!this.method_5<int, string, bool, Process>())
				{
					MessageBox.Show("Device has not been jailbroken!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else if (!this.string_16.StartsWith("16.") && !this.string_16.StartsWith("15."))
				{
					this.method_63<byte, Exception, bool, Process, StreamReader, string, ProcessStartInfo, int, System.Windows.Forms.Timer>();
				}
				else
				{
					MessageBox.Show("Sorry, iOS " + this.string_16 + " is not supported yet!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			else if (this.button5.Text == "Bypass Hello")
			{
				TaskAwaiter<bool> taskAwaiter3 = this.method_36<Task<bool>>().GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter3.GetResult())
				{
					MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					this.checkBox3.Checked = false;
					if (this.method_5<int, string, bool, Process>())
					{
						if (File.Exists(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + ".zip"))
						{
							if (this.method_33<long, double, bool>())
							{
								this.method_64<string, byte, bool, Exception, Ionic.Zip.ZipFile, IEnumerator<ZipEntry>, ZipEntry, int, System.Windows.Forms.Timer, SshCommand, StreamReader>();
							}
							else
							{
								MessageBox.Show("Sorry, Activation file is invalid \n\nBypass will not be successful!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							}
						}
						else
						{
							MessageBox.Show("Please generate activation files before!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
					}
					else
					{
						MessageBox.Show("Jailbreak the device with Palen1x or Checkra1n!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter2;
				TaskAwaiter<bool> taskAwaiter5;
				if (this.button5.Text == "Generate token")
				{
					TaskAwaiter<bool> taskAwaiter4 = this.method_36<Task<bool>>().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						await taskAwaiter4;
						taskAwaiter4 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					bool result = taskAwaiter4.GetResult();
					if (!result)
					{
						MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
					if (!(this.string_9 != "Activated"))
					{
						MessageBox.Show("Device is already activated!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
					if (File.Exists(Environment.CurrentDirectory + "/files/Tokens/" + this.string_5 + ".zip"))
					{
						MessageBox.Show("Token already exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
					try
					{
						this.progressBar2.Value = 0;
						this.label11.Text = "Generating token...";
						this.method_11<Process, bool, string>("https://unlockbd.xyz/UBTools/PHP/Token/token.php");
						using (HttpClient httpClient = Form1.Class28.smethod_0())
						{
							Directory.CreateDirectory("./files/Tokens/" + this.string_5);
							HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Concat(new string[] { "https://unlockbd.xyz/UBTools/PHP/Token/TokensGenerated/", this.string_8, "/", this.string_8, "_Info.plist" }));
							HttpResponseMessage httpResponseMessage2 = httpResponseMessage;
							httpResponseMessage = null;
							try
							{
								using (FileStream fileStream = new FileStream(string.Concat(new string[] { "./files/Tokens/", this.string_5, "/", this.string_8, "-act-req.token" }), FileMode.Create))
								{
									await httpResponseMessage2.Content.CopyToAsync(fileStream);
								}
								FileStream fileStream = null;
							}
							finally
							{
								if (httpResponseMessage2 != null)
								{
									((IDisposable)httpResponseMessage2).Dispose();
								}
							}
							httpResponseMessage2 = null;
							File.WriteAllText("./files/Tokens/" + this.string_5 + "/Info.txt", string.Concat(new string[]
							{
								"UDID: ",
								this.string_7,
								"\nSerial Number: ",
								this.string_6,
								"\nHardware: ",
								this.string_10,
								"\nIMEI: ",
								this.string_11,
								"\nModel: ",
								(this.string_2 == "Unsupported") ? this.string_13 : this.string_2
							}));
						}
						HttpClient httpClient = null;
						this.method_24<string>("./files/Tokens/" + this.string_5, "./files/Tokens/" + this.string_5 + ".zip");
						Directory.Delete("./files/Tokens/" + this.string_5, true);
						ProcessStartInfo processStartInfo = Form1.Class28.smethod_1();
						processStartInfo.FileName = "explorer.exe";
						processStartInfo.Arguments = string.Concat(new string[]
						{
							"/select,\"",
							Environment.CurrentDirectory,
							"\\files\\Tokens\\",
							this.string_5,
							".zip\""
						});
						processStartInfo.UseShellExecute = false;
						Process.Start(processStartInfo);
						this.label11.ForeColor = Color.MediumSeaGreen;
						this.label11.Text = "Token generated...";
						this.progressBar2.Value = 100;
						this.method_43<bool, int, System.Windows.Forms.Timer>(2000);
						this.label18.Visible = true;
						this.label18.Text = "Congrats! FMI Token was generated\ud83c\udf89\ud83c\udf89";
						MessageBox.Show("FMI token has been generated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
					catch (Exception ex)
					{
						Exception ex3 = ex;
						if (Directory.Exists("./files/Tokens/" + this.string_5))
						{
							Directory.Delete("./files/Tokens/" + this.string_5, true);
						}
						this.label11.ForeColor = Color.Crimson;
						this.label11.Text = "There was an error generating token....";
						MessageBox.Show(ex3.Message);
						MessageBox.Show("Token could not be generated \n\nCheck cable or internet connection!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
				}
				else if (this.button5.Text == "Remove ID")
				{
					taskAwaiter5 = this.method_36<Task<bool>>().GetAwaiter();
					if (taskAwaiter5.IsCompleted)
					{
						goto IL_118F;
					}
					await taskAwaiter5;
				}
				else if (!(this.button5.Text == "Generate files"))
				{
					if (!(this.button5.Text == "Activate"))
					{
						if (this.button5.Text == "Bypass MDM")
						{
							TaskAwaiter<bool> taskAwaiter6 = this.method_36<Task<bool>>().GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								await taskAwaiter6;
								taskAwaiter6 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							bool result2 = taskAwaiter6.GetResult();
							if (result2)
							{
								this.method_53();
								goto IL_17B8;
							}
							MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							goto IL_17B8;
						}
						else
						{
							if (!(this.button5.Text == "Bypass"))
							{
								if (!(this.button5.Text == "Close"))
								{
									if (!(this.button5.Text == "Erase Device"))
									{
										goto IL_17B8;
									}
									this.label11.ForeColor = Color.Black;
									try
									{
										if (this.method_5<int, string, bool, Process>())
										{
											if (this.checkBox2.Checked)
											{
												DialogResult dialogResult2 = MessageBox.Show("Do you want to erase device?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
												if (dialogResult2 == DialogResult.Yes)
												{
													this.label11.Text = "iDevice is being erased......";
													this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
													if (this.string_16.StartsWith("12"))
													{
														this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/restore"), "/bin/erase");
														this.sshClient_0.CreateCommand("cd /bin/erase").Execute();
														this.sshClient_0.CreateCommand("chmod 755 /bin/erase").Execute();
														this.button5.Text = "Close";
														this.label18.Text = "Congrats! Your iDevice has been erased\ud83c\udf89\ud83c\udf89";
														this.label18.Visible = true;
														this.label11.Text = "Device erased......";
														this.sshClient_0.CreateCommand("/bin/erase -command 8a5fbd968b4f16624ecb5713744028fefabe8a20de10dfc58c4ede37212ac3da").Execute();
													}
													else
													{
														this.sshClient_0.CreateCommand("snappy -f / -r orig-fs -x").Execute();
														this.sshClient_0.CreateCommand("launchctl unload -F /System/Library/LaunchDaemons/*").Execute();
														this.sshClient_0.CreateCommand("launchctl load -w -F /System/Library/LaunchDaemons/com.apple.CommCenterRootHelper.plist").Execute();
														this.sshClient_0.CreateCommand("launchctl load -w -F /System/Library/LaunchDaemons/com.apple.CommCenterMobileHelper.plist").Execute();
														this.sshClient_0.CreateCommand("launchctl load -w -F /System/Library/LaunchDaemons/com.apple.mobile.obliteration.plist").Execute();
														this.sshClient_0.CreateCommand("launchctl load -w -F /System/Library/LaunchDaemons/com.apple.devicedataresetd.plist").Execute();
														this.sshClient_0.CreateCommand("launchctl load -w -F /System/Library/LaunchDaemons/com.apple.backboardd.plist").Execute();
														this.sshClient_0.CreateCommand("launchctl load -w -F /System/Library/LaunchDaemons/com.apple.runningboardd.plist").Execute();
														this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
														this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/restore2"), "/bin/erase");
														this.sshClient_0.CreateCommand("chmod 755 /usr/bin/erase").Execute();
														this.sshClient_0.CreateCommand("/usr/bin/erase -command da7e6b6d2c20eb316c093 && rm -rf /usr/bin/erase").Execute();
														this.button5.Text = "Close";
														this.label18.Text = "Congrats! Your iDevice has been erased\ud83c\udf89\ud83c\udf89";
														this.label18.Visible = true;
														this.label11.Text = "Device erased......";
													}
												}
											}
											else
											{
												DialogResult dialogResult3 = MessageBox.Show("Do you want to erase device?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
												if (dialogResult3 == DialogResult.Yes)
												{
													this.label11.Text = "Device is being erased......";
													if (!this.checkBox3.Checked)
													{
														this.method_55<bool, SshCommand, string, StreamReader>();
													}
													else
													{
														this.method_56<bool, SshCommand, string, StreamReader>();
													}
													this.sshClient_0.CreateCommand("/usr/sbin/nvram oblit-inprogress=5 sync").Execute();
													this.sshClient_0.CreateCommand("/sbin/reboot").Execute();
													this.button5.Text = "Close";
													this.label18.Text = "Congrats! Your iDevice has been Erased\ud83c\udf89\ud83c\udf89";
													this.label18.Visible = true;
													this.label11.Text = "Device erased......";
												}
											}
										}
										goto IL_17B8;
									}
									catch
									{
										MessageBox.Show("No SSH Connection found, Device may be unplugged!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
										goto IL_17B8;
									}
								}
								Application.Exit();
								goto IL_17B8;
							}
							TaskAwaiter<bool> taskAwaiter7 = this.method_36<Task<bool>>().GetAwaiter();
							if (!taskAwaiter7.IsCompleted)
							{
								await taskAwaiter7;
								taskAwaiter7 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							bool result3 = taskAwaiter7.GetResult();
							if (!result3)
							{
								MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								goto IL_17B8;
							}
							if (!(this.string_9 != "Activated"))
							{
								MessageBox.Show("Device is already activated!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								goto IL_17B8;
							}
							this.progressBar2.Value = 0;
							this.label11.Text = "Checking SSH...";
							this.method_43<bool, int, System.Windows.Forms.Timer>(1000);
							if (!this.method_5<int, string, bool, Process>())
							{
								this.label11.ForeColor = Color.Crimson;
								this.label11.Text = "Please jailbreak your device...";
								MessageBox.Show("Device has not been jailbroken!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								goto IL_17B8;
							}
							try
							{
								await Task.Run(new Action(this.method_110<string, byte, bool, XmlDocument, XmlNode, Ionic.Zip.ZipFile, IEnumerator<ZipEntry>, ZipEntry>));
							}
							catch (Exception ex)
							{
								Exception ex4 = ex;
								this.label11.ForeColor = Color.Crimson;
								this.label11.Text = "Couldn't activate device...";
								MessageBox.Show(ex4.Message);
								MessageBox.Show("An error occured while attempting to activate iDevice", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							}
							goto IL_17B8;
						}
					}
					else
					{
						if (!this.method_5<int, string, bool, Process>())
						{
							MessageBox.Show("Device has not been jailbroken!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							goto IL_17B8;
						}
						if (this.checkBox2.Checked)
						{
							this.method_60<string, bool, byte, Exception, Ionic.Zip.ZipFile, IEnumerator<ZipEntry>, ZipEntry, SshCommand, StreamReader, int, System.Windows.Forms.Timer>();
							goto IL_17B8;
						}
						if (this.method_33<long, double, bool>())
						{
							this.method_61<string, bool, byte, Exception, Ionic.Zip.ZipFile, IEnumerator<ZipEntry>, ZipEntry, SshCommand, StreamReader, int, System.Windows.Forms.Timer>();
							goto IL_17B8;
						}
						MessageBox.Show("Sorry, Activation file is invalid \n\nBypass will not be successful!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
				}
				else
				{
					TaskAwaiter<bool> taskAwaiter8 = this.method_36<Task<bool>>().GetAwaiter();
					if (!taskAwaiter8.IsCompleted)
					{
						await taskAwaiter8;
						taskAwaiter8 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter8.GetResult())
					{
						MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
					if (File.Exists(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + ".zip"))
					{
						MessageBox.Show("BackUp for this device already exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						goto IL_17B8;
					}
					this.label11.ForeColor = Color.Black;
					this.label11.Text = "Generating files....";
					Directory.CreateDirectory(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5);
					Directory.CreateDirectory(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + "/FairPlay/iTunes_Control/iTunes/");
					if (!this.checkBox13.Checked)
					{
						this.method_31<WebClient, Exception, bool, string, HttpWebRequest, HttpWebResponse, StreamReader, XmlDocument, XmlNode, byte, ProcessStartInfo>();
						goto IL_17B8;
					}
					this.method_32<WebClient, Exception, bool, string, HttpWebRequest, HttpWebResponse, StreamReader, XmlDocument, XmlNode, XmlSerializer, StreamWriter, byte, ProcessStartInfo>();
					goto IL_17B8;
				}
				taskAwaiter5 = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
				IL_118F:
				bool result4 = taskAwaiter5.GetResult();
				if (result4)
				{
					if (this.method_5<int, string, bool, Process>())
					{
						try
						{
							this.label11.Text = "Mounting filesystems...";
							this.progressBar2.Value = 0;
							this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
							this.label11.Text = "Removing account...";
							this.progressBar2.Value += 50;
							this.method_43<bool, int, System.Windows.Forms.Timer>(2000);
							this.sshClient_0.CreateCommand("rm -rf /var/mobile/Library/Accounts/").Execute();
							this.label11.Text = "Cleanups...";
							this.progressBar2.Value += 20;
							this.method_43<bool, int, System.Windows.Forms.Timer>(2000);
							this.sshClient_0.CreateCommand("cd /private/var/containers/Data/System/*/Library/internal/../ && chflags -R uchg activation_records/ internal/").Execute();
							this.sshClient_0.CreateCommand("cd /private/var/containers/Data/System/*/Library/internal/../ && chflags -R uchg activation_records/* internal/*").Execute();
							this.sshClient_0.CreateCommand("chflags -R uchg /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
							this.sshClient_0.CreateCommand("chflags -R uchg /private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
							this.sshClient_0.CreateCommand("launchctl load -w -F /System/Library/LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
							try
							{
								this.sshClient_0.CreateCommand("shutdown -r now").Execute();
							}
							catch
							{
							}
							this.label11.ForeColor = Color.MediumSeaGreen;
							this.label11.Text = "Removal Done...";
							this.progressBar2.Value += 30;
							this.method_43<bool, int, System.Windows.Forms.Timer>(2000);
							this.label18.Visible = true;
							this.label18.Text = "Congrats! Apple ID has been removed\ud83c\udf89\ud83c\udf89";
							MessageBox.Show("Apple ID has been removed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							goto IL_17B8;
						}
						catch (Exception ex)
						{
							Exception ex5 = ex;
							this.label11.ForeColor = Color.Crimson;
							this.label11.Text = "There was an error removing account....";
							MessageBox.Show(ex5.Message);
							MessageBox.Show("There was an error removing account!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							goto IL_17B8;
						}
					}
					MessageBox.Show("Jailbreak the device with Checkra1n or Palen1x!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					MessageBox.Show("No Device Detected, Please check Drivers/Cable!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}
		else if (!this.method_5<int, string, bool, Process>())
		{
			MessageBox.Show("Device has not been jailbroken!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (this.checkBox2.Checked)
		{
			this.method_58();
		}
		else
		{
			this.method_59();
		}
		IL_17B8:
		this.button5.Enabled = true;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00212D30 File Offset: 0x00210F30
	private void method_82<T0, T1, T2, T3, T4>(T1 gparam_0, T2 gparam_1)
	{
		if (!(this.button6.Text == "Refresh"))
		{
			try
			{
				this.button5.Text = "Close";
				this.button6.Visible = false;
				this.sshClient_0.CreateCommand("/sbin/reboot").Execute();
				return;
			}
			catch
			{
				MessageBox.Show("No SSH Connection found, Device may be unplugged!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
		}
		this.method_75<T3, T4, T1>();
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00212DB8 File Offset: 0x00210FB8
	private void method_83<T0, T1, T2>(T0 gparam_0, T1 gparam_1)
	{
		this.jbtext.Text = "Click \"Jailbreak Device\" to start.";
		this.jbtext.ForeColor = Color.Black;
		this.progressBar1.Value = 0;
		this.progressBar2.Value = 0;
		this.checkBox2.Checked = false;
		this.jailbreakPanel.Visible = false;
		this.bootPanel.Visible = false;
		this.backUpPanel.Visible = false;
		this.button3.Text = "Jailbreak Device";
		this.actbutt.Text = "Check Device";
		this.label18.Visible = false;
		this.label11.Text = "Click on \"Back Up\" to start.";
		this.label11.ForeColor = Color.Black;
		this.button6.Enabled = true;
		this.button6.Text = "Reboot";
		this.button5.Text = "Back Up";
		this.button4.Enabled = true;
		this.checkBox2.Checked = false;
		this.checkBox3.Checked = false;
		this.checkBox5.Checked = false;
		this.checkBox6.Checked = false;
		this.checkBox9.Checked = false;
		this.checkBox10.Checked = false;
		this.checkBox11.Checked = false;
		this.checkBox12.Checked = false;
		this.checkBox13.Checked = false;
		this.checkBox14.Checked = false;
		this.method_22<T2>();
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00212F30 File Offset: 0x00211130
	private void method_84<T0, T1, T2, T3>(T1 gparam_0, T2 gparam_1)
	{
		T0 t = this.ECIDStatusValue.Text != "Registered";
		if (t != null)
		{
			ProcessStartInfo processStartInfo = Form1.smethod_4();
			processStartInfo.FileName = this.string_18 + "/";
			processStartInfo.UseShellExecute = true;
			Process.Start(processStartInfo);
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00212F80 File Offset: 0x00211180
	private async void method_85<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		try
		{
			string text = Form1.smethod_0<RestClient, RestRequest, IRestResponse, bool, string, object, CSharpArgumentInfo, Convert, HttpStatusCode>("Broquess", "Broque_Ramdisk", "web.txt");
			this.string_18 = text;
			text = null;
		}
		catch
		{
			MessageBox.Show("Please connect to the internet!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Process.GetCurrentProcess().Kill();
		}
		if (!this.method_0<bool, WindowsIdentity, WindowsPrincipal>())
		{
			MessageBox.Show("Please run this tool as administrator", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Process.GetCurrentProcess().Kill();
		}
		bool flag = await this.method_27<Task<bool>>();
		if (!flag)
		{
			TaskAwaiter taskAwaiter = this.method_26<Task>().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter);
			}
			taskAwaiter.GetResult();
			ProcessStartInfo processStartInfo = Form1.Class17.smethod_0();
			processStartInfo.FileName = "https://mega.nz/folder/c7ABCKaS#f4-s4aW97g7Y3rFPujDDZQ";
			processStartInfo.UseShellExecute = true;
			Process.Start(processStartInfo);
			Process.GetCurrentProcess().Kill();
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00212FC8 File Offset: 0x002111C8
	private void method_86<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		T0 @checked = this.checkBox3.Checked;
		if (@checked != null)
		{
			this.string_14 = "iOS16";
			this.checkBox6.Enabled = false;
		}
		else
		{
			this.string_14 = "iOS15";
			this.checkBox6.Enabled = true;
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00213014 File Offset: 0x00211214
	private void method_87<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		if (!this.checkBox5.Checked)
		{
			this.checkBox2.Enabled = true;
			this.checkBox6.Enabled = true;
			this.checkBox9.Enabled = true;
			this.checkBox10.Enabled = true;
			this.checkBox11.Enabled = true;
			this.checkBox12.Enabled = true;
			this.checkBox13.Visible = false;
		}
		else
		{
			this.checkBox2.Enabled = false;
			this.checkBox6.Enabled = false;
			this.checkBox9.Enabled = false;
			this.checkBox10.Enabled = false;
			this.checkBox11.Enabled = false;
			this.checkBox12.Enabled = false;
			this.checkBox13.Visible = true;
			this.checkBox13.Checked = false;
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x002130E8 File Offset: 0x002112E8
	private void method_88<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		this.label11.Text = "You can erase device now....";
		this.button5.Text = "Erase Device";
		this.label7.Text = "Great!  You just skipped to erasing your device with just a click";
		this.label11.Text = "Click on \"Erase Device\" to start.";
		this.label17.Text = "NOTICE: Erasing your device will delete all user data including activation \r\nfiles. Make sure FMI is off or you have activation files backed up.";
		this.label18.Visible = false;
		this.linkLabel3.Visible = false;
		T0 t = !this.checkBox2.Checked;
		if (t != null)
		{
			this.button6.Visible = true;
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0021317C File Offset: 0x0021137C
	private void method_89<T0, T1, T2>(T0 gparam_0, T1 gparam_1)
	{
		ProcessStartInfo processStartInfo = Form1.smethod_4();
		processStartInfo.FileName = "https://twitter.com/iosnemes1s";
		processStartInfo.UseShellExecute = true;
		Process.Start(processStartInfo);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x002131A8 File Offset: 0x002113A8
	private void method_90<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.jailbreakPanel.Visible = false;
		this.bootPanel.Visible = false;
		this.backUpPanel.Visible = false;
		this.dfuPanel.Visible = false;
		this.method_40();
	}

	// Token: 0x0600009C RID: 156 RVA: 0x002131EC File Offset: 0x002113EC
	private void method_91<T0, T1, T2, T3, T4, T5, T6>()
	{
		base.Invoke(new Action(this.method_111));
		T0 t = 5;
		T1 now = DateTime.Now;
		T2 t2 = new System.Timers.Timer(2000.0);
		t2.Elapsed += this.method_112<object, ElapsedEventArgs, Process, T2>;
		t2.AutoReset = false;
		t2.Start();
		for (;;)
		{
			Form1.Class7 @class = new Form1.Class7();
			@class.form1_0 = this;
			T3 t3 = DateTime.Now - now;
			T5 t4 = t3.TotalSeconds >= 5.0;
			if (t4 != null)
			{
				break;
			}
			t--;
			T0 t5 = 0;
			T4[] array = this.string_13.Split(new T6[] { 101, 44 });
			T4[] array2 = array;
			for (T0 t6 = 0; t6 < array2.Length; t6++)
			{
				T4 t7 = array2[t6];
				if (int.TryParse(t7, out t5))
				{
					break;
				}
			}
			if (t5 < 9)
			{
				@class.string_0 = string.Format("2. Press and hold the Side \r\nand Home buttons \r\ntogether ({0})\r\n", t);
			}
			else
			{
				@class.string_0 = string.Format("2. Press and hold the Side \r\nand Volume down buttons \r\ntogether ({0})\r\n", t);
			}
			base.Invoke(new Action(@class.method_0));
			Thread.Sleep(1000);
		}
		this.method_92<T0, T1, T3, T4, T5, T6>();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00213330 File Offset: 0x00211530
	private void method_92<T0, T1, T2, T3, T4, T5>()
	{
		base.Invoke(new Action(this.method_113));
		T0 t = 11;
		T1 now = DateTime.Now;
		for (;;)
		{
			Form1.Class8 @class = new Form1.Class8();
			@class.form1_0 = this;
			T2 t2 = DateTime.Now - now;
			T4 t3 = t2.TotalSeconds >= 11.0;
			if (t3 != null)
			{
				break;
			}
			t--;
			T0 t4 = 0;
			T3[] array = this.string_13.Split(new T5[] { 101, 44 });
			T3[] array2 = array;
			for (T0 t5 = 0; t5 < array2.Length; t5++)
			{
				T3 t6 = array2[t5];
				if (int.TryParse(t6, out t4))
				{
					break;
				}
			}
			T4 t7 = t4 >= 9;
			if (t7 != null)
			{
				@class.string_0 = string.Format("3. Release the Side button \r\nBUT KEEP HOLDING the \r\nVolume down button ({0})\r\n", t);
			}
			else
			{
				@class.string_0 = string.Format("3. Release the Side button \r\nBUT KEEP HOLDING the \r\nHome button ({0})\r\n", t);
			}
			base.Invoke(new Action(@class.method_0));
			Thread.Sleep(1000);
		}
		base.Invoke(new Action(this.method_114));
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00213454 File Offset: 0x00211654
	private async void method_93<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.button7.Enabled = false;
		this.button9.Enabled = false;
		if (!(this.button7.Text == "Next"))
		{
			this.homeButtLab.Enabled = true;
			this.label22.Enabled = true;
			this.sideButton678Lab.Enabled = true;
			this.volumeDown78XLab.Enabled = true;
			TaskAwaiter taskAwaiter = Task.Run(new Action(this.method_115<int, DateTime, System.Timers.Timer, TimeSpan, string, bool, char>)).GetAwaiter();
			TaskAwaiter taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter);
			}
			taskAwaiter.GetResult();
			await this.method_34<Task<bool>>();
			if (!(this.string_3 == "DFU"))
			{
				if (!(this.string_3 == "Recovery"))
				{
					TaskAwaiter taskAwaiter3 = Task.Run(new Action(this.method_116<Process, bool, string>)).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						await taskAwaiter3;
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
					}
					taskAwaiter3.GetResult();
					await this.method_34<Task<bool>>();
					if (!(this.string_3 == "DFU"))
					{
						this.homeButtLab.Enabled = true;
						this.label22.Enabled = true;
						this.sideButton678Lab.Enabled = true;
						this.volumeDown78XLab.Enabled = true;
						this.button7.Text = "Retry";
						this.label24.Text = "Whoops, the device didn't enter DFU mode. Click Retry to try again.";
						this.label23.Visible = false;
					}
					else
					{
						this.button7.Text = "Next";
						this.button9.Visible = false;
						this.label24.Text = "Device entered DFU mode successfully.";
						this.label23.Visible = false;
					}
				}
				else
				{
					this.homeButtLab.Enabled = true;
					this.label22.Enabled = true;
					this.sideButton678Lab.Enabled = true;
					this.volumeDown78XLab.Enabled = true;
					this.button7.Text = "Retry";
					this.label24.Text = "Whoops, the device didn't enter DFU mode. Click Retry to try again.";
					this.label23.Visible = false;
				}
			}
			else
			{
				this.button7.Text = "Next";
				this.button9.Visible = false;
				this.label24.Text = "Device entered DFU mode successfully.";
				this.label23.Visible = false;
			}
		}
		else
		{
			this.bootPanel.Visible = false;
			this.backUpPanel.Visible = false;
			this.dfuPanel.Visible = false;
			this.method_40();
			this.jailbreakPanel.Visible = true;
			this.bootPanel.Visible = true;
			if (!this.checkBox6.Checked)
			{
				this.button3.Text = "Jailbreak Device";
				this.jbtext.Text = "Click \"Jailbreak Device\" to start.";
				this.label9.Text = "NOTICE: Make sure you have already downloaded the ramdisk for your \r\ndevice before starting the jailbreak.";
				this.label5.Text = "Great! Almost there. Now click on \"Jailbreak Device\" to Jailbreak your device \r\nand enable SSH connection.";
			}
			else
			{
				this.button3.Text = "Boot purple";
				this.jbtext.Text = "Click \"Boot purple\" to start.";
				this.label9.Text = "NOTICE: Make sure you have already downloaded the ramdisk for your \r\ndevice before starting purple boot.";
				this.label5.Text = "Great! Almost there. Now click on \"Boot purple\" to put your device \r\ninto purple mode and enable port communication.";
			}
		}
		this.button7.Enabled = true;
		this.button9.Enabled = true;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0021349C File Offset: 0x0021169C
	private void method_94<T0, T1, T2>(T0 gparam_0, T1 gparam_1)
	{
		ProcessStartInfo processStartInfo = Form1.smethod_4();
		processStartInfo.FileName = "https://wa.me/233541652492/";
		processStartInfo.UseShellExecute = true;
		Process.Start(processStartInfo);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x002134C8 File Offset: 0x002116C8
	private void method_95<T0, T1, T2>(T0 gparam_0, T1 gparam_1)
	{
		ProcessStartInfo processStartInfo = Form1.smethod_4();
		processStartInfo.FileName = "https://t.me/iosnemes1s";
		processStartInfo.UseShellExecute = true;
		Process.Start(processStartInfo);
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x002134F4 File Offset: 0x002116F4
	private void method_96<T0, T1, T2>(T0 gparam_0, T1 gparam_1)
	{
		ProcessStartInfo processStartInfo = Form1.smethod_4();
		processStartInfo.FileName = this.string_18 + "/donate";
		processStartInfo.UseShellExecute = true;
		Process.Start(processStartInfo);
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0021352C File Offset: 0x0021172C
	private void method_97<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		T0 @checked = this.checkBox9.Checked;
		if (@checked != null)
		{
			this.checkBox2.Enabled = false;
			this.checkBox5.Enabled = false;
			this.checkBox6.Enabled = false;
			this.checkBox10.Enabled = false;
			this.checkBox11.Enabled = false;
			this.checkBox12.Enabled = false;
		}
		else
		{
			this.checkBox2.Enabled = true;
			this.checkBox5.Enabled = true;
			this.checkBox6.Enabled = true;
			this.checkBox10.Enabled = true;
			this.checkBox11.Enabled = true;
			this.checkBox12.Enabled = true;
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x002135DC File Offset: 0x002117DC
	private void method_98<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		if (!this.checkBox10.Checked)
		{
			this.checkBox2.Enabled = true;
			this.checkBox5.Enabled = true;
			this.checkBox6.Enabled = true;
			this.checkBox9.Enabled = true;
			this.checkBox11.Enabled = true;
			this.checkBox12.Enabled = true;
		}
		else
		{
			this.checkBox2.Enabled = false;
			this.checkBox5.Enabled = false;
			this.checkBox6.Enabled = false;
			this.checkBox9.Enabled = false;
			this.checkBox11.Enabled = false;
			this.checkBox12.Enabled = false;
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0021368C File Offset: 0x0021188C
	private void method_99<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		T0 @checked = this.checkBox11.Checked;
		if (@checked != null)
		{
			this.checkBox2.Enabled = false;
			this.checkBox5.Enabled = false;
			this.checkBox6.Enabled = false;
			this.checkBox9.Enabled = false;
			this.checkBox10.Enabled = false;
			this.checkBox12.Enabled = false;
		}
		else
		{
			this.checkBox2.Enabled = true;
			this.checkBox5.Enabled = true;
			this.checkBox6.Enabled = true;
			this.checkBox9.Enabled = true;
			this.checkBox10.Enabled = true;
			this.checkBox12.Enabled = true;
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0021373C File Offset: 0x0021193C
	private void method_100<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		T0 @checked = this.checkBox12.Checked;
		if (@checked != null)
		{
			this.checkBox2.Enabled = false;
			this.checkBox5.Enabled = false;
			this.checkBox6.Enabled = false;
			this.checkBox9.Enabled = false;
			this.checkBox10.Enabled = false;
			this.checkBox11.Enabled = false;
		}
		else
		{
			this.checkBox2.Enabled = true;
			this.checkBox5.Enabled = true;
			this.checkBox6.Enabled = true;
			this.checkBox9.Enabled = true;
			this.checkBox10.Enabled = true;
			this.checkBox11.Enabled = true;
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x002137EC File Offset: 0x002119EC
	private void method_101<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		T0 @checked = this.checkBox14.Checked;
		if (@checked != null)
		{
			MessageBox.Show("Checking this option will keep your baseband after bypass, however you need a locked SIM card to make bypass untethered. (Check this on hello bypass only)", "NOTICE", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0021381C File Offset: 0x00211A1C
	private async void method_102<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.jbtext.ForeColor = Color.Black;
		this.progressBar1.Value = 0;
		this.button3.Enabled = false;
		this.button4.Enabled = false;
		this.button11.Enabled = false;
		try
		{
			await Task.Run(new Action(this.method_117<Process, bool, string, int, System.Windows.Forms.Timer>));
			MessageBox.Show("Please remove cable and re-plug!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch
		{
			this.jbtext.ForeColor = Color.Crimson;
			this.jbtext.Text = "Could not install drivers...";
		}
		this.button3.Enabled = true;
		this.button4.Enabled = true;
		this.button11.Enabled = true;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00213864 File Offset: 0x00211A64
	private void method_103<T0, T1, T2>(T1 gparam_0, T2 gparam_1)
	{
		if (!this.checkBox6.Checked)
		{
			this.checkBox12.Enabled = true;
			this.checkBox2.Enabled = true;
			this.checkBox5.Enabled = true;
			this.checkBox9.Enabled = true;
			this.checkBox10.Enabled = true;
			this.checkBox11.Enabled = true;
			this.checkBox3.Enabled = true;
			this.string_14 = "iOS15";
		}
		else
		{
			this.checkBox3.Enabled = false;
			this.checkBox12.Enabled = false;
			this.checkBox2.Enabled = false;
			this.checkBox5.Enabled = false;
			this.checkBox9.Enabled = false;
			this.checkBox10.Enabled = false;
			this.checkBox11.Enabled = false;
			this.string_14 = "purple";
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00213940 File Offset: 0x00211B40
	private void method_104<T0, T1>(T0 gparam_0, T1 gparam_1)
	{
		this.button5.Enabled = this.comboBox1.SelectedIndex != -1;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0021396C File Offset: 0x00211B6C
	protected virtual void System.Windows.Forms.Form.Dispose(bool disposing)
	{
		if (disposing && this.icontainer_0 != null)
		{
			this.icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0021399C File Offset: 0x00211B9C
	private void method_105<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>()
	{
		T0 t = new ComponentResourceManager(typeof(Form1));
		this.Title = Form1.smethod_11();
		this.Anno = Form1.smethod_11();
		this.label3 = Form1.smethod_11();
		this.ECID = Form1.smethod_11();
		this.ECIDValue = Form1.smethod_11();
		this.DeviceType = Form1.smethod_11();
		this.DeviceTypeValue = Form1.smethod_11();
		this.ECIDStatus = Form1.smethod_11();
		this.ECIDStatusValue = Form1.smethod_11();
		this.ModelName = Form1.smethod_11();
		this.ModelNameValue = Form1.smethod_11();
		this.label12 = Form1.smethod_11();
		this.button2 = Form1.smethod_12();
		this.actbutt = Form1.smethod_12();
		this.checkBox1 = Form1.smethod_13();
		this.label14 = Form1.smethod_11();
		this.label15 = Form1.smethod_11();
		this.feedbacktext = Form1.smethod_11();
		this.label13 = Form1.smethod_11();
		this.label19 = Form1.smethod_11();
		this.button1 = Form1.smethod_12();
		this.label2 = Form1.smethod_11();
		this.label4 = Form1.smethod_11();
		this.checkBox2 = Form1.smethod_13();
		this.checkBox3 = Form1.smethod_13();
		this.checkBox4 = Form1.smethod_13();
		this.checkBox5 = Form1.smethod_13();
		this.bootPanel = Form1.smethod_14();
		this.backUpPanel = Form1.smethod_14();
		this.dfuPanel = Form1.smethod_14();
		this.label22 = Form1.smethod_11();
		this.homeButtLab = Form1.smethod_11();
		this.sideButton678Lab = Form1.smethod_11();
		this.volumeDown78XLab = Form1.smethod_11();
		this.devicePicture = Form1.smethod_15();
		this.label1 = Form1.smethod_11();
		this.label20 = Form1.smethod_11();
		this.pressDFU = Form1.smethod_11();
		this.startDFU = Form1.smethod_11();
		this.button7 = Form1.smethod_12();
		this.button9 = Form1.smethod_12();
		this.label23 = Form1.smethod_11();
		this.label24 = Form1.smethod_11();
		this.linkLabel3 = Form1.smethod_16();
		this.label18 = Form1.smethod_11();
		this.linkLabel2 = Form1.smethod_16();
		this.label17 = Form1.smethod_11();
		this.label16 = Form1.smethod_11();
		this.label11 = Form1.smethod_11();
		this.button5 = Form1.smethod_12();
		this.label10 = Form1.smethod_11();
		this.label7 = Form1.smethod_11();
		this.checkBox13 = Form1.smethod_13();
		this.button6 = Form1.smethod_12();
		this.comboBox1 = Form1.smethod_17();
		this.textBox1 = Form1.smethod_18();
		this.progressBar2 = Form1.smethod_19();
		this.button4 = Form1.smethod_12();
		this.linkLabel1 = Form1.smethod_16();
		this.button3 = Form1.smethod_12();
		this.label9 = Form1.smethod_11();
		this.label8 = Form1.smethod_11();
		this.progressBar1 = Form1.smethod_19();
		this.jbtext = Form1.smethod_11();
		this.label6 = Form1.smethod_11();
		this.label5 = Form1.smethod_11();
		this.button11 = Form1.smethod_12();
		this.jailbreakPanel = Form1.smethod_14();
		this.button10 = Form1.smethod_12();
		this.label21 = Form1.smethod_11();
		this.linkLabel4 = Form1.smethod_16();
		this.linkLabel5 = Form1.smethod_16();
		this.linkLabel6 = Form1.smethod_16();
		this.checkBox8 = Form1.smethod_13();
		this.checkBox9 = Form1.smethod_13();
		this.checkBox10 = Form1.smethod_13();
		this.checkBox11 = Form1.smethod_13();
		this.checkBox12 = Form1.smethod_13();
		this.checkBox14 = Form1.smethod_13();
		this.checkBox6 = Form1.smethod_13();
		this.pictureBox1 = Form1.smethod_15();
		this.bootPanel.SuspendLayout();
		this.backUpPanel.SuspendLayout();
		this.dfuPanel.SuspendLayout();
		((ISupportInitialize)this.devicePicture).BeginInit();
		this.jailbreakPanel.SuspendLayout();
		((ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		t.ApplyResources(this.Title, "Title");
		this.Title.Name = "Title";
		t.ApplyResources(this.Anno, "Anno");
		this.Anno.Name = "Anno";
		t.ApplyResources(this.label3, "label3");
		this.label3.BackColor = Color.Transparent;
		this.label3.ForeColor = Color.Silver;
		this.label3.Name = "label3";
		t.ApplyResources(this.ECID, "ECID");
		this.ECID.Name = "ECID";
		t.ApplyResources(this.ECIDValue, "ECIDValue");
		this.ECIDValue.Cursor = Cursors.Hand;
		this.ECIDValue.Name = "ECIDValue";
		this.ECIDValue.Click += new EventHandler(this.method_70<T10, T11>);
		this.ECIDValue.MouseLeave += new EventHandler(this.method_69<T10, T11>);
		this.ECIDValue.MouseHover += new EventHandler(this.method_68<T10, T11>);
		t.ApplyResources(this.DeviceType, "DeviceType");
		this.DeviceType.Name = "DeviceType";
		t.ApplyResources(this.DeviceTypeValue, "DeviceTypeValue");
		this.DeviceTypeValue.Name = "DeviceTypeValue";
		t.ApplyResources(this.ECIDStatus, "ECIDStatus");
		this.ECIDStatus.Name = "ECIDStatus";
		t.ApplyResources(this.ECIDStatusValue, "ECIDStatusValue");
		this.ECIDStatusValue.Cursor = Cursors.Hand;
		this.ECIDStatusValue.Name = "ECIDStatusValue";
		this.ECIDStatusValue.Click += new EventHandler(this.method_84<T12, T10, T11, T13>);
		t.ApplyResources(this.ModelName, "ModelName");
		this.ModelName.Name = "ModelName";
		t.ApplyResources(this.ModelNameValue, "ModelNameValue");
		this.ModelNameValue.Name = "ModelNameValue";
		t.ApplyResources(this.label12, "label12");
		this.label12.BackColor = Color.Transparent;
		this.label12.ForeColor = Color.Silver;
		this.label12.Name = "label12";
		this.button2.Cursor = Cursors.Hand;
		t.ApplyResources(this.button2, "button2");
		this.button2.Name = "button2";
		this.button2.TabStop = false;
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new EventHandler(this.method_65<T10, T11>);
		this.actbutt.Cursor = Cursors.Hand;
		t.ApplyResources(this.actbutt, "actbutt");
		this.actbutt.Name = "actbutt";
		this.actbutt.TabStop = false;
		this.actbutt.UseVisualStyleBackColor = true;
		this.actbutt.Click += new EventHandler(this.method_67<T10, T11>);
		t.ApplyResources(this.checkBox1, "checkBox1");
		this.checkBox1.Checked = true;
		this.checkBox1.CheckState = CheckState.Checked;
		this.checkBox1.Cursor = Cursors.Hand;
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.CheckStateChanged += new EventHandler(this.method_72<T12, T10, T11>);
		t.ApplyResources(this.label14, "label14");
		this.label14.Name = "label14";
		t.ApplyResources(this.label15, "label15");
		this.label15.Name = "label15";
		t.ApplyResources(this.feedbacktext, "feedbacktext");
		this.feedbacktext.Name = "feedbacktext";
		t.ApplyResources(this.label13, "label13");
		this.label13.Name = "label13";
		t.ApplyResources(this.label19, "label19");
		this.label19.Name = "label19";
		this.button1.Cursor = Cursors.Hand;
		t.ApplyResources(this.button1, "button1");
		this.button1.Name = "button1";
		this.button1.TabStop = false;
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new EventHandler(this.method_66<T12, T10, T11>);
		t.ApplyResources(this.label2, "label2");
		this.label2.Name = "label2";
		t.ApplyResources(this.label4, "label4");
		this.label4.BackColor = Color.Transparent;
		this.label4.ForeColor = Color.Silver;
		this.label4.Name = "label4";
		t.ApplyResources(this.checkBox2, "checkBox2");
		this.checkBox2.Cursor = Cursors.Hand;
		this.checkBox2.Name = "checkBox2";
		this.checkBox2.UseVisualStyleBackColor = true;
		this.checkBox2.CheckStateChanged += new EventHandler(this.method_78<T12, T10, T11>);
		t.ApplyResources(this.checkBox3, "checkBox3");
		this.checkBox3.Cursor = Cursors.Hand;
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.UseVisualStyleBackColor = true;
		this.checkBox3.CheckStateChanged += new EventHandler(this.method_86<T12, T10, T11>);
		t.ApplyResources(this.checkBox4, "checkBox4");
		this.checkBox4.Checked = true;
		this.checkBox4.CheckState = CheckState.Checked;
		this.checkBox4.Cursor = Cursors.Hand;
		this.checkBox4.Name = "checkBox4";
		this.checkBox4.UseVisualStyleBackColor = true;
		t.ApplyResources(this.checkBox5, "checkBox5");
		this.checkBox5.Cursor = Cursors.Hand;
		this.checkBox5.Name = "checkBox5";
		this.checkBox5.UseVisualStyleBackColor = true;
		this.checkBox5.CheckedChanged += new EventHandler(this.method_87<T12, T10, T11>);
		this.bootPanel.Controls.Add(this.backUpPanel);
		this.bootPanel.Controls.Add(this.button4);
		this.bootPanel.Controls.Add(this.linkLabel1);
		this.bootPanel.Controls.Add(this.button3);
		this.bootPanel.Controls.Add(this.label9);
		this.bootPanel.Controls.Add(this.label8);
		this.bootPanel.Controls.Add(this.progressBar1);
		this.bootPanel.Controls.Add(this.jbtext);
		this.bootPanel.Controls.Add(this.label6);
		this.bootPanel.Controls.Add(this.label5);
		this.bootPanel.Controls.Add(this.button11);
		t.ApplyResources(this.bootPanel, "bootPanel");
		this.bootPanel.Name = "bootPanel";
		this.backUpPanel.Controls.Add(this.dfuPanel);
		this.backUpPanel.Controls.Add(this.linkLabel3);
		this.backUpPanel.Controls.Add(this.label18);
		this.backUpPanel.Controls.Add(this.linkLabel2);
		this.backUpPanel.Controls.Add(this.label17);
		this.backUpPanel.Controls.Add(this.label16);
		this.backUpPanel.Controls.Add(this.label11);
		this.backUpPanel.Controls.Add(this.button5);
		this.backUpPanel.Controls.Add(this.label10);
		this.backUpPanel.Controls.Add(this.label7);
		this.backUpPanel.Controls.Add(this.checkBox13);
		this.backUpPanel.Controls.Add(this.button6);
		this.backUpPanel.Controls.Add(this.comboBox1);
		this.backUpPanel.Controls.Add(this.textBox1);
		this.backUpPanel.Controls.Add(this.progressBar2);
		t.ApplyResources(this.backUpPanel, "backUpPanel");
		this.backUpPanel.Name = "backUpPanel";
		this.dfuPanel.Controls.Add(this.label22);
		this.dfuPanel.Controls.Add(this.homeButtLab);
		this.dfuPanel.Controls.Add(this.sideButton678Lab);
		this.dfuPanel.Controls.Add(this.volumeDown78XLab);
		this.dfuPanel.Controls.Add(this.devicePicture);
		this.dfuPanel.Controls.Add(this.label1);
		this.dfuPanel.Controls.Add(this.label20);
		this.dfuPanel.Controls.Add(this.pressDFU);
		this.dfuPanel.Controls.Add(this.startDFU);
		this.dfuPanel.Controls.Add(this.button7);
		this.dfuPanel.Controls.Add(this.button9);
		this.dfuPanel.Controls.Add(this.label23);
		this.dfuPanel.Controls.Add(this.label24);
		t.ApplyResources(this.dfuPanel, "dfuPanel");
		this.dfuPanel.Name = "dfuPanel";
		t.ApplyResources(this.label22, "label22");
		this.label22.BackColor = Color.Transparent;
		this.label22.Name = "label22";
		t.ApplyResources(this.homeButtLab, "homeButtLab");
		this.homeButtLab.BackColor = Color.Transparent;
		this.homeButtLab.ForeColor = Color.Black;
		this.homeButtLab.Name = "homeButtLab";
		t.ApplyResources(this.sideButton678Lab, "sideButton678Lab");
		this.sideButton678Lab.BackColor = Color.Transparent;
		this.sideButton678Lab.Name = "sideButton678Lab";
		t.ApplyResources(this.volumeDown78XLab, "volumeDown78XLab");
		this.volumeDown78XLab.Name = "volumeDown78XLab";
		this.devicePicture.BackColor = SystemColors.Control;
		t.ApplyResources(this.devicePicture, "devicePicture");
		this.devicePicture.Name = "devicePicture";
		this.devicePicture.TabStop = false;
		t.ApplyResources(this.label1, "label1");
		this.label1.Name = "label1";
		t.ApplyResources(this.label20, "label20");
		this.label20.Name = "label20";
		t.ApplyResources(this.pressDFU, "pressDFU");
		this.pressDFU.Name = "pressDFU";
		t.ApplyResources(this.startDFU, "startDFU");
		this.startDFU.Name = "startDFU";
		this.button7.Cursor = Cursors.Hand;
		t.ApplyResources(this.button7, "button7");
		this.button7.Name = "button7";
		this.button7.UseVisualStyleBackColor = true;
		this.button7.Click += new EventHandler(this.method_93<T10, T11>);
		this.button9.Cursor = Cursors.Hand;
		t.ApplyResources(this.button9, "button9");
		this.button9.Name = "button9";
		this.button9.UseVisualStyleBackColor = true;
		this.button9.Click += new EventHandler(this.method_90<T10, T11>);
		t.ApplyResources(this.label23, "label23");
		this.label23.Name = "label23";
		t.ApplyResources(this.label24, "label24");
		this.label24.Name = "label24";
		t.ApplyResources(this.linkLabel3, "linkLabel3");
		this.linkLabel3.Name = "linkLabel3";
		this.linkLabel3.TabStop = true;
		this.linkLabel3.LinkClicked += new LinkLabelLinkClickedEventHandler(this.method_88<T12, T10, T14>);
		t.ApplyResources(this.label18, "label18");
		this.label18.Name = "label18";
		t.ApplyResources(this.linkLabel2, "linkLabel2");
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.TabStop = true;
		this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.method_83<T10, T14, T12>);
		t.ApplyResources(this.label17, "label17");
		this.label17.Name = "label17";
		t.ApplyResources(this.label16, "label16");
		this.label16.BackColor = Color.Transparent;
		this.label16.ForeColor = Color.Silver;
		this.label16.Name = "label16";
		t.ApplyResources(this.label11, "label11");
		this.label11.Name = "label11";
		this.button5.Cursor = Cursors.Hand;
		t.ApplyResources(this.button5, "button5");
		this.button5.Name = "button5";
		this.button5.TabStop = false;
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new EventHandler(this.method_81<T10, T11>);
		t.ApplyResources(this.label10, "label10");
		this.label10.BackColor = Color.Transparent;
		this.label10.ForeColor = Color.Silver;
		this.label10.Name = "label10";
		t.ApplyResources(this.label7, "label7");
		this.label7.Name = "label7";
		t.ApplyResources(this.checkBox13, "checkBox13");
		this.checkBox13.Cursor = Cursors.Hand;
		this.checkBox13.Name = "checkBox13";
		this.checkBox13.UseVisualStyleBackColor = true;
		this.button6.Cursor = Cursors.Hand;
		t.ApplyResources(this.button6, "button6");
		this.button6.Name = "button6";
		this.button6.TabStop = false;
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new EventHandler(this.method_82<T12, T10, T11, T15, T16>);
		this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
		this.comboBox1.FormattingEnabled = true;
		t.ApplyResources(this.comboBox1, "comboBox1");
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.SelectedIndexChanged += new EventHandler(this.method_104<T10, T11>);
		t.ApplyResources(this.textBox1, "textBox1");
		this.textBox1.Name = "textBox1";
		t.ApplyResources(this.progressBar2, "progressBar2");
		this.progressBar2.Name = "progressBar2";
		this.button4.Cursor = Cursors.Hand;
		t.ApplyResources(this.button4, "button4");
		this.button4.Name = "button4";
		this.button4.TabStop = false;
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new EventHandler(this.method_77<T10, T11>);
		t.ApplyResources(this.linkLabel1, "linkLabel1");
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.TabStop = true;
		this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.method_74<T10, T14, T13>);
		this.button3.Cursor = Cursors.Hand;
		t.ApplyResources(this.button3, "button3");
		this.button3.Name = "button3";
		this.button3.TabStop = false;
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new EventHandler(this.method_76<T10, T11>);
		t.ApplyResources(this.label9, "label9");
		this.label9.Name = "label9";
		t.ApplyResources(this.label8, "label8");
		this.label8.BackColor = Color.Transparent;
		this.label8.ForeColor = Color.Silver;
		this.label8.Name = "label8";
		t.ApplyResources(this.progressBar1, "progressBar1");
		this.progressBar1.Name = "progressBar1";
		t.ApplyResources(this.jbtext, "jbtext");
		this.jbtext.Name = "jbtext";
		t.ApplyResources(this.label6, "label6");
		this.label6.BackColor = Color.Transparent;
		this.label6.ForeColor = Color.Silver;
		this.label6.Name = "label6";
		t.ApplyResources(this.label5, "label5");
		this.label5.Name = "label5";
		this.button11.Cursor = Cursors.Hand;
		t.ApplyResources(this.button11, "button11");
		this.button11.Name = "button11";
		this.button11.TabStop = false;
		this.button11.UseVisualStyleBackColor = true;
		this.button11.Click += new EventHandler(this.method_102<T10, T11>);
		this.jailbreakPanel.Controls.Add(this.bootPanel);
		this.jailbreakPanel.Controls.Add(this.checkBox5);
		this.jailbreakPanel.Controls.Add(this.checkBox4);
		this.jailbreakPanel.Controls.Add(this.checkBox3);
		this.jailbreakPanel.Controls.Add(this.checkBox2);
		this.jailbreakPanel.Controls.Add(this.label4);
		this.jailbreakPanel.Controls.Add(this.label2);
		this.jailbreakPanel.Controls.Add(this.button1);
		this.jailbreakPanel.Controls.Add(this.button10);
		this.jailbreakPanel.Controls.Add(this.label21);
		this.jailbreakPanel.Controls.Add(this.linkLabel4);
		this.jailbreakPanel.Controls.Add(this.linkLabel5);
		this.jailbreakPanel.Controls.Add(this.linkLabel6);
		this.jailbreakPanel.Controls.Add(this.label19);
		this.jailbreakPanel.Controls.Add(this.checkBox8);
		this.jailbreakPanel.Controls.Add(this.checkBox9);
		this.jailbreakPanel.Controls.Add(this.checkBox10);
		this.jailbreakPanel.Controls.Add(this.checkBox11);
		this.jailbreakPanel.Controls.Add(this.checkBox12);
		this.jailbreakPanel.Controls.Add(this.checkBox14);
		this.jailbreakPanel.Controls.Add(this.checkBox6);
		t.ApplyResources(this.jailbreakPanel, "jailbreakPanel");
		this.jailbreakPanel.Name = "jailbreakPanel";
		this.button10.Cursor = Cursors.Hand;
		t.ApplyResources(this.button10, "button10");
		this.button10.ForeColor = SystemColors.HotTrack;
		this.button10.Name = "button10";
		this.button10.UseVisualStyleBackColor = true;
		this.button10.Click += new EventHandler(this.method_96<T10, T11, T13>);
		t.ApplyResources(this.label21, "label21");
		this.label21.ForeColor = SystemColors.ControlText;
		this.label21.Name = "label21";
		t.ApplyResources(this.linkLabel4, "linkLabel4");
		this.linkLabel4.Name = "linkLabel4";
		this.linkLabel4.TabStop = true;
		this.linkLabel4.LinkClicked += new LinkLabelLinkClickedEventHandler(this.method_89<T10, T14, T13>);
		t.ApplyResources(this.linkLabel5, "linkLabel5");
		this.linkLabel5.Name = "linkLabel5";
		this.linkLabel5.TabStop = true;
		this.linkLabel5.LinkClicked += new LinkLabelLinkClickedEventHandler(this.method_94<T10, T14, T13>);
		t.ApplyResources(this.linkLabel6, "linkLabel6");
		this.linkLabel6.Name = "linkLabel6";
		this.linkLabel6.TabStop = true;
		this.linkLabel6.LinkClicked += new LinkLabelLinkClickedEventHandler(this.method_95<T10, T14, T13>);
		t.ApplyResources(this.checkBox8, "checkBox8");
		this.checkBox8.Cursor = Cursors.Hand;
		this.checkBox8.Name = "checkBox8";
		this.checkBox8.UseVisualStyleBackColor = true;
		t.ApplyResources(this.checkBox9, "checkBox9");
		this.checkBox9.Cursor = Cursors.Hand;
		this.checkBox9.Name = "checkBox9";
		this.checkBox9.UseVisualStyleBackColor = true;
		this.checkBox9.CheckStateChanged += new EventHandler(this.method_97<T12, T10, T11>);
		t.ApplyResources(this.checkBox10, "checkBox10");
		this.checkBox10.Cursor = Cursors.Hand;
		this.checkBox10.Name = "checkBox10";
		this.checkBox10.UseVisualStyleBackColor = true;
		this.checkBox10.CheckStateChanged += new EventHandler(this.method_98<T12, T10, T11>);
		t.ApplyResources(this.checkBox11, "checkBox11");
		this.checkBox11.Cursor = Cursors.Hand;
		this.checkBox11.Name = "checkBox11";
		this.checkBox11.UseVisualStyleBackColor = true;
		this.checkBox11.CheckStateChanged += new EventHandler(this.method_99<T12, T10, T11>);
		t.ApplyResources(this.checkBox12, "checkBox12");
		this.checkBox12.Cursor = Cursors.Hand;
		this.checkBox12.Name = "checkBox12";
		this.checkBox12.UseVisualStyleBackColor = true;
		this.checkBox12.CheckStateChanged += new EventHandler(this.method_100<T12, T10, T11>);
		t.ApplyResources(this.checkBox14, "checkBox14");
		this.checkBox14.Cursor = Cursors.Hand;
		this.checkBox14.Name = "checkBox14";
		this.checkBox14.UseVisualStyleBackColor = true;
		this.checkBox14.CheckStateChanged += new EventHandler(this.method_101<T12, T10, T11>);
		t.ApplyResources(this.checkBox6, "checkBox6");
		this.checkBox6.Cursor = Cursors.Hand;
		this.checkBox6.Name = "checkBox6";
		this.checkBox6.UseVisualStyleBackColor = true;
		this.checkBox6.CheckStateChanged += new EventHandler(this.method_103<T12, T10, T11>);
		t.ApplyResources(this.pictureBox1, "pictureBox1");
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.TabStop = false;
		t.ApplyResources(this, "$this");
		base.AutoScaleMode = AutoScaleMode.Font;
		base.Controls.Add(this.jailbreakPanel);
		base.Controls.Add(this.feedbacktext);
		base.Controls.Add(this.label15);
		base.Controls.Add(this.label14);
		base.Controls.Add(this.checkBox1);
		base.Controls.Add(this.actbutt);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.label12);
		base.Controls.Add(this.ModelNameValue);
		base.Controls.Add(this.ModelName);
		base.Controls.Add(this.ECIDStatusValue);
		base.Controls.Add(this.ECIDStatus);
		base.Controls.Add(this.DeviceTypeValue);
		base.Controls.Add(this.DeviceType);
		base.Controls.Add(this.ECIDValue);
		base.Controls.Add(this.ECID);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.Anno);
		base.Controls.Add(this.Title);
		base.Controls.Add(this.label13);
		base.MaximizeBox = false;
		base.Name = "Form1";
		base.FormClosing += new FormClosingEventHandler(this.method_73<T17, T12, T10, T18, T13>);
		base.Load += new EventHandler(this.method_71<T10, T11>);
		base.Shown += new EventHandler(this.method_85<T10, T11>);
		this.bootPanel.ResumeLayout(false);
		this.bootPanel.PerformLayout();
		this.backUpPanel.ResumeLayout(false);
		this.backUpPanel.PerformLayout();
		this.dfuPanel.ResumeLayout(false);
		this.dfuPanel.PerformLayout();
		((ISupportInitialize)this.devicePicture).EndInit();
		this.jailbreakPanel.ResumeLayout(false);
		this.jailbreakPanel.PerformLayout();
		((ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0021573C File Offset: 0x0021393C
	// Note: this type is marked as 'beforefieldinit'.
	static Form1()
	{
		object obj = Activator.CreateInstance(typeof(Dictionary<string, string>));
		obj.Add("iPhone7,1", "iPhone 6 Plus");
		obj.Add("iPhone7,2", "iPhone 6");
		obj.Add("iPhone8,1", "iPhone 6s");
		obj.Add("iPhone8,2", "iPhone 6s Plus");
		obj.Add("iPhone8,4", "iPhone SE (1st gen)");
		obj.Add("iPhone9,1", "iPhone 7 (Global)");
		obj.Add("iPhone9,2", "iPhone 7 Plus (Global)");
		obj.Add("iPhone9,3", "iPhone 7 (GSM)");
		obj.Add("iPhone9,4", "iPhone 7 Plus (GSM)");
		obj.Add("iPhone10,1", "iPhone 8 (Global)");
		obj.Add("iPhone10,2", "iPhone 8 Plus (Global)");
		obj.Add("iPhone10,3", "iPhone X (Global)");
		obj.Add("iPhone10,4", "iPhone 8 (GSM)");
		obj.Add("iPhone10,5", "iPhone 8 Plus (GSM)");
		obj.Add("iPhone10,6", "iPhone X (GSM)");
		obj.Add("iPod9,1", "iPod Touch (7th gen)");
		obj.Add("iPad5,1", "iPad mini 4 (WiFi)");
		obj.Add("iPad5,2", "iPad mini 4 (Cellular)");
		obj.Add("iPad5,3", "iPad Air 2 (WiFi)");
		obj.Add("iPad5,4", "iPad Air 2 (Cellular)");
		obj.Add("iPad6,3", "iPad Pro 9.7-inch (WiFi)");
		obj.Add("iPad6,4", "iPad Pro 9.7-inch (Cellular)");
		obj.Add("iPad6,7", "iPad Pro 12.9-inch (1st gen, WiFi)");
		obj.Add("iPad6,8", "iPad Pro 12.9-inch (1st gen, Cellular)");
		obj.Add("iPad6,11", "iPad (5th gen, WiFi)");
		obj.Add("iPad6,12", "iPad (5th gen, Cellular)");
		obj.Add("iPad7,1", "iPad Pro 12.9-inch (2nd gen, WiFi)");
		obj.Add("iPad7,2", "iPad Pro 12.9-inch (2nd gen, Cellular)");
		obj.Add("iPad7,3", "iPad Pro 10.5-inch (WiFi)");
		obj.Add("iPad7,4", "iPad Pro 10.5-inch (Cellular)");
		obj.Add("iPad7,5", "iPad (6th gen, WiFi)");
		obj.Add("iPad7,6", "iPad (6th gen, Cellular)");
		obj.Add("iPad7,11", "iPad (7th gen, WiFi)");
		obj.Add("iPad7,12", "iPad (7th gen, Cellular)");
		Form1.dictionary_0 = obj;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00215980 File Offset: 0x00213B80
	[CompilerGenerated]
	private void method_106<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
	{
		base.Invoke(new Action(this.method_107));
		try
		{
			this.progressBar2.Value = 0;
			this.method_13<T1, T2, T3, T4, T5>();
			this.label11.Text = "Preparing device...";
			this.progressBar2.Value += 10;
			this.method_43<T1, T6, T7>(1000);
			this.method_47();
			this.label11.Text = "Preparing device...";
			this.progressBar2.Value += 10;
			this.method_13<T1, T2, T3, T4, T5>();
			this.label11.Text = "Extracting bundles...";
			this.progressBar2.Value += 10;
			this.method_50<T8, T4, T9, T10, T11, T12>();
			this.label11.Text = "Patching bundles...";
			this.progressBar2.Value += 20;
			this.method_51<T4>();
			this.label11.Text = "Activating device...";
			this.progressBar2.Value += 20;
			this.method_11<T2, T1, T4>("");
			this.label11.Text = "Restoring...";
			this.progressBar2.Value += 20;
			this.method_52<T2, T4, T5, T1>();
			this.progressBar2.Value += 10;
			this.label11.Text = "MDM Bypassed!...";
			this.method_47();
			this.button5.Enabled = true;
			this.button5.Text = "Close";
		}
		catch (Exception t)
		{
			this.button5.Enabled = true;
			this.label11.ForeColor = Color.Crimson;
			this.label11.Text = "There was an error bypassing MDM....";
			MessageBox.Show(t.Message);
			MessageBox.Show("There was an error bypassing MDM!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00215B70 File Offset: 0x00213D70
	[CompilerGenerated]
	private void method_107()
	{
		this.button5.Enabled = false;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00215B8C File Offset: 0x00213D8C
	[CompilerGenerated]
	[DebuggerStepThrough]
	private T0 method_108<T0>()
	{
		Form1.Class5 @class = new Form1.Class5();
		@class.asyncTaskMethodBuilder_0 = AsyncTaskMethodBuilder.Create();
		@class.form1_0 = this;
		@class.int_0 = -1;
		@class.asyncTaskMethodBuilder_0.Start<Form1.Class5>(ref @class);
		return @class.asyncTaskMethodBuilder_0.Task;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00215BD0 File Offset: 0x00213DD0
	[CompilerGenerated]
	private void method_109<T0, T1, T2, T3, T4>()
	{
		this.jbtext.Text = "Checking if device is ready....";
		this.progressBar1.Value = 20;
		this.method_6<T0, T1, T2>(0);
		this.method_6<T0, T1, T2>(1);
		this.jbtext.Text = "Right before trigger (this is the real bug setup)...";
		this.progressBar1.Value += 20;
		this.method_7<T0, T2>();
		this.jbtext.Text = "Exploiting device (this is checkm8)";
		this.progressBar1.Value += 20;
		this.method_9<T0, T1, T2>();
		this.jbtext.Text = "Post exploitation clean-ups (This might take a while)...";
		this.progressBar1.Value += 20;
		this.method_8<T0, T2>();
		this.progressBar1.Value += 20;
		this.jbtext.Text = "Checking if device is exploited...";
		this.method_43<T1, T3, T4>(1000);
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00215CB8 File Offset: 0x00213EB8
	[CompilerGenerated]
	private void method_110<T0, T1, T2, T3, T4, T5, T6, T7>()
	{
		this.label11.Text = "Making activation record...";
		T2 t = File.Exists(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + ".zip");
		if (t != null)
		{
			File.Delete(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + ".zip");
		}
		Directory.CreateDirectory(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5);
		Directory.CreateDirectory(Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + "/FairPlay/iTunes_Control/iTunes/");
		T0 t2 = "MIICogIBATALBgkqhkiG9w0BAQsxaJ8/BCIpOuefQAThUH8An0sU8u7ynkFtjU5iK7LCVPXvf1IsCsqfh20HNWFRCSOYRJ+XPQwAAAAA7u7u7u7u7u+flz4EAAAAAJ+XPwQBAAAAn5dABAEAAACfl0EEAQAAAJ+XTAQAAAAABIIBADqStNCOV64BLCKVls72U5Bwh8qTJHwaQtkPjUj/wh3RbtC45BoDNebydW4RmSefowABaXRYFfGFhuyXHxfQyxre5gDMh6CftLMQdSuE0tLHw+Kki0me5xFxBFHtwQdt/fgd1VRnNUI8zokLGfjm4N8V3A6oMvnDwZLlZMci7jPhDOk7OW2P6XD0RCirK6kaYMQEgJdPr5lCUJRv2ywc0URrGMWNvU759pObUPjHgIvqNXY+7MeLi3vKqRpft7beOwDohoo1e1+GVQVGYP7qYYmNBMJlLFO75h8bDaSMc3a5MfDgwDekbZn7Q0ZiQ2TPHB/FQSsbfphSRWfnmr9b3/mjggEgMAsGCSqGSIb3DQEBAQOCAQ8AMIIBCgKCAQEArJFPRdnc/E7Vgatg/AHbKnGEudR+ug8WZghxMOlPad3fL42hHAXReVRcBE5liQXEyaP0ojy3s3QJhuNEXwLMYOLCKJNAj4SrE6dZqJ9CQamouvEnZjdC/gLBG5jSuAI4zF+hjObe8OZnV6YGcooEbRkA51dj+x5zmY+vT0va/w+EOdAiTWi6xiWdVFQTXCpCTUzA9qcax58XUi04+dcVSEwVO9U3ZeyoIUrJD/FmoDjjZOidCHDgsCGlnLfQP/gLKOMpOfzw4dWFIW1IiDvs9Uy+U3YhyyE4HPDVx2oAf8ojhBMzsdqXGVV148H0mZSkR4+ulZVlR4E/mxB2ZdP7HQIDAQAB";
		T0 t3 = Environment.CurrentDirectory + "/files/BackUp/" + this.string_5 + "/1";
		T0 t4 = string.Concat(new T0[]
		{
			"{\n\t\"InternationalMobileEquipmentIdentity\" = \"", this.string_11, "\";\n\t\"ActivityURL\" = \"https//albertapplecom/deviceservices/activity\";\n\t\"SerialNumber\" = \"", this.string_6, "\";\n\t\"ProductType\" = \"", this.string_13, "\";\n\t\"MobileEquipmentIdentifier\" = \"", this.string_12, "\";\n\t\"UniqueDeviceID\" = \"", this.string_7,
			"\";\n\t\"WildcardTicket\" = \"", t2, "\";\n\t\"ActivationRandomness\" = \"1A0CC786-CE38-4D31-BDFD-1FB4483AE4F8\";\n\t\"CertificateURL\" = \"https//albertapplecom/deviceservices/certifyMe\";\n\t\"PhoneNumberNotificationURL\" = \"https//albertapplecom/deviceservices/phoneHome\";\n}"
		});
		T2 t5 = this.string_12 == "";
		if (t5 != null)
		{
			t4 = t4.Replace("\n\t\"MobileEquipmentIdentifier\" = \"" + this.string_12 + "\";", "");
		}
		T1[] bytes = Encoding.UTF8.GetBytes(t4);
		T0 t6 = Convert.ToBase64String(bytes);
		T0 t7 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\n<plist version=\"1.0\">\n<dict>\n\t<key>AccountToken</key>\n\t<data>\n\t" + t6 + "\n\t</data>\n\t<key>AccountTokenCertificate</key>\n\t<data>\n\tLS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSURaekNDQWsrZ0F3SUJBZ0lCQWpB\n\tTkJna3Foa2lHOXcwQkFRVUZBREI1TVFzd0NRWURWUVFHRXdKVlV6RVQKTUJFR0ExVUVD\n\taE1LUVhCd2JHVWdTVzVqTGpFbU1DUUdBMVVFQ3hNZFFYQndiR1VnUTJWeWRHbG1hV05o\n\tZEdsdgpiaUJCZFhSb2IzSnBkSGt4TFRBckJnTlZCQU1USkVGd2NHeGxJR2xRYUc5dVpT\n\tQkRaWEowYVdacFkyRjBhVzl1CklFRjFkR2h2Y21sMGVUQWVGdzB3TnpBME1UWXlNalUx\n\tTURKYUZ3MHhOREEwTVRZeU1qVTFNREphTUZzeEN6QUoKQmdOVkJBWVRBbFZUTVJNd0VR\n\tWURWUVFLRXdwQmNIQnNaU0JKYm1NdU1SVXdFd1lEVlFRTEV3eEJjSEJzWlNCcApVR2h2\n\tYm1VeElEQWVCZ05WQkFNVEYwRndjR3hsSUdsUWFHOXVaU0JCWTNScGRtRjBhVzl1TUlH\n\tZk1BMEdDU3FHClNJYjNEUUVCQVFVQUE0R05BRENCaVFLQmdRREZBWHpSSW1Bcm1vaUhm\n\tYlMyb1BjcUFmYkV2MGQxams3R2JuWDcKKzRZVWx5SWZwcnpCVmRsbXoySkhZdjErMDRJ\n\tekp0TDdjTDk3VUk3ZmswaTBPTVkwYWw4YStKUFFhNFVnNjExVApicUV0K25qQW1Ba2dl\n\tM0hYV0RCZEFYRDlNaGtDN1QvOW83N3pPUTFvbGk0Y1VkemxuWVdmem1XMFBkdU94dXZl\n\tCkFlWVk0d0lEQVFBQm80R2JNSUdZTUE0R0ExVWREd0VCL3dRRUF3SUhnREFNQmdOVkhS\n\tTUJBZjhFQWpBQU1CMEcKQTFVZERnUVdCQlNob05MK3Q3UnovcHNVYXEvTlBYTlBIKy9X\n\tbERBZkJnTlZIU01FR0RBV2dCVG5OQ291SXQ0NQpZR3UwbE01M2cyRXZNYUI4TlRBNEJn\n\tTlZIUjhFTVRBdk1DMmdLNkFwaGlkb2RIUndPaTh2ZDNkM0xtRndjR3hsCkxtTnZiUzlo\n\tY0hCc1pXTmhMMmx3YUc5dVpTNWpjbXd3RFFZSktvWklodmNOQVFFRkJRQURnZ0VCQUY5\n\tcW1yVU4KZEErRlJPWUdQN3BXY1lUQUsrcEx5T2Y5ek9hRTdhZVZJODg1VjhZL0JLSGhs\n\td0FvK3pFa2lPVTNGYkVQQ1M5Vgp0UzE4WkJjd0QvK2Q1WlFUTUZrbmhjVUp3ZFBxcWpu\n\tbTlMcVRmSC94NHB3OE9OSFJEenhIZHA5NmdPVjNBNCs4CmFia29BU2ZjWXF2SVJ5cFhu\n\tYnVyM2JSUmhUekFzNFZJTFM2alR5Rll5bVplU2V3dEJ1Ym1taWdvMWtDUWlaR2MKNzZj\n\tNWZlREF5SGIyYnpFcXR2eDNXcHJsanRTNDZRVDVDUjZZZWxpblpuaW8zMmpBelJZVHh0\n\tUzZyM0pzdlpEaQpKMDcrRUhjbWZHZHB4d2dPKzdidFcxcEZhcjBaakY5L2pZS0tuT1lO\n\teXZDcndzemhhZmJTWXd6QUc1RUpvWEZCCjRkK3BpV0hVRGNQeHRjYz0KLS0tLS1FTkQg\n\tQ0VSVElGSUNBVEUtLS0tLQo=\n\t</data>\n\t<key>AccountTokenSignature</key>\n\t<data>\n\traD2OkSpiKa05ol4Af0pmP8R/g6ISCErqNxRJyi/yxShUZ7e0MFRFe7lsILfRxmJD+8E\n\tA2ztZSQ+euGLMD7sjyzwlJsY1C6yHAyh+mq1cmLrYas+e/muxO5DyoAGqsb5jTLeM0jS\n\temD9aTA4r8aUbPvScjMIYHqHVMzLyZflZZ4=\n\t</data>\n\t<key>DeviceCertificate</key>\n\t<data>\n\tLS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUM4akNDQWx1Z0F3SUJBZ0lKVTlE\n\teVdEQUlrV0pjTUEwR0NTcUdTSWIzRFFFQkJRVUFNRm94Q3pBSkJnTlYKQkFZVEFsVlRN\n\tUk13RVFZRFZRUUtFd3BCY0hCc1pTQkpibU11TVJVd0V3WURWUVFMRXd4QmNIQnNaU0Jw\n\tVUdodgpibVV4SHpBZEJnTlZCQU1URmtGd2NHeGxJR2xRYUc5dVpTQkVaWFpwWTJVZ1Ew\n\tRXdIaGNOTWpBd016STVNRGd6Ck9UVXpXaGNOTWpNd016STVNRGd6T1RVeldqQ0JnekV0\n\tTUNzR0ExVUVBeFlrUVRnelJFUkROakV0TUVFME5DMDAKUkRNeExVRXlSREl0TmtaRE5V\n\tSXlPVGhCUWtRMU1Rc3dDUVlEVlFRR0V3SlZVekVMTUFrR0ExVUVDQk1DUTBFeApFakFR\n\tQmdOVkJBY1RDVU4xY0dWeWRHbHViekVUTUJFR0ExVUVDaE1LUVhCd2JHVWdTVzVqTGpF\n\tUE1BMEdBMVVFCkN4TUdhVkJvYjI1bE1JR2ZNQTBHQ1NxR1NJYjNEUUVCQVFVQUE0R05B\n\tRENCaVFLQmdRRG5sbS96RitETUhQMUQKOEt3VlFKYkVYajhTU0NjWnhSREsrU1NSdytl\n\tUnVramd5VGZBd3pqa0poVjJ0YVNWbWdnVDVKWUVBWUdlaXFCcQovajdTV1lWL0ZVK3oz\n\tc3lEQTRhMlBMRjdSNFZyVXluOW9xRzg5ajdRS2pON2hpZnp3Y1lFWmhlWmV3bzZoTkVO\n\tClRsVFlFK2RGZDVhOXgwbHhSQjFELzVXcnJtSGxaUUlEQVFBQm80R1ZNSUdTTUI4R0Ex\n\tVWRJd1FZTUJhQUZMTCsKSVNORWhwVnFlZFdCSm81ekVOaW5USTUwTUIwR0ExVWREZ1FX\n\tQkJTSE42R2FNQlIxeGVPYmc4U2pVNWU0T3FRYwpUakFNQmdOVkhSTUJBZjhFQWpBQU1B\n\tNEdBMVVkRHdFQi93UUVBd0lGb0RBZ0JnTlZIU1VCQWY4RUZqQVVCZ2dyCkJnRUZCUWNE\n\tQVFZSUt3WUJCUVVIQXdJd0VBWUtLb1pJaHZkalpBWUtBZ1FDQlFBd0RRWUpLb1pJaHZj\n\tTkFRRUYKQlFBRGdZRUE1QlI3aktnNFlBUm1GM3ZXVWR1NWRnTnhjd1RoU0hiYU9PNmdQ\n\tM25IeWhNU1B1NnIwYmxDcE0vdwpVZkNZZWZVV1Q0WFBXVXZwU2tsaW5QR1JiN01nSG5E\n\tWDJRVEM3REFYTTJiOHJiK2E5bTRYQkFFcmZyUVRlbkJJClFWdjI0ajRQSHdpNUd6L0I5\n\tQWJ6ZXEyTEw1blMvdmNKMDJ6VzJoUHhqb1lNQjVaU2UrYz0KLS0tLS1FTkQgQ0VSVElG\n\tSUNBVEUtLS0tLQo=\n\t</data>\n\t<key>DeviceConfigurationFlags</key>\n\t<string>0</string>\n\t<key>FairPlayKeyData</key>\n\t<data>\n\tLS0tLS1CRUdJTiBDT05UQUlORVItLS0tLQpBQUVBQVRRVHdOS1loa1B3UDVDVlF5Ly9k\n\tYWpqMUxlMm81Q2MzV0hUSlhKdmZNVWR0bG5yOGl3T29KMVFKZkpHClpJaWF5L2dib0t2\n\tYzc4dzZTMTh6Q0FnMVpqdENCelhubGtmNjN0Z0wwOTF3bzA4SjFYTzY1Z0VwbDNURUxq\n\tV2EKcldMSnNCbm96ZHc0RGtHZHgybzhIRi95UTFvUTljVkxRZC8xL1I2d29oUVdNRFVq\n\tS3IrWnRMUUY5Z2lsUEdzdAptQ0NGTW5nRytWZEIzdGNsVVNWUGFoSmpDZmdqSjU1eVZO\n\tZkRJVVdRSXBwVUR3OG13U1kzWlYxb1ZKY3pGWEVYCnJNUnN6LzBwMVVGRWpmRDdCci9w\n\tQkdGUUxIRVV3cjVxRmw0anM3WUxxUjQ0WVdFSmtoalo3UUF0NXAxWWVsWDIKVS95SERY\n\tZWNLUiswdDk3UUFSS1JGTEpRbGZNNnREVW9mUXlhL0o5Y0tqYTNyYncyS3VERVpjS1J6\n\tcDlHR0wyZgpqWGFjTEt6MDRIdGpmb3VBRWE5OW9uZTdrUTJ4YTFQcGxzTWdMY2VwMVhW\n\tL0tHaExvL2I0MWNrYm8wTXhkSzVtCm9sOXpBdHRyb2pwRkJUZXFMeGFxbGRUQVVxUjJM\n\tdVhBRzUrUTIzYXl5MzR0MEJmWS9NRFc4L3BvamlqZmhFRGMKekR4NWtlVmkxZ0NHSHlV\n\tb1BMa1FvYXh2aHBLdFFJOG5jMGYya1Q5VGtiaENkeTJ2N2dEMXV2U1o3SEw2aEZBVwph\n\tcFB4UWxKZjlDclh1SHZhZUMxTGZmNlpXQitlSmxVcG1DTzFNV1lnL0xlMGQrMTRpSnU0\n\tb3pmb3pMWlB1WmJTCkZISkM0c1Vrbk50Vmd4OVFIS2dTdWkzNWh1UFlhSFM3OHV1NExy\n\tZVZFZHFjdE42R0I2UW01a0k3UGdZMGlNdUgKcW9HUGhNL3F0Vms2NS83TVNMQTkvck84\n\tWnRKa2xlWkpiQnNUYmJzS2dFd1NKdU5EaGt5TVVzdWlReGllOEJMMwo0N242UnkvcWJH\n\teFJUQ05oY3VIT05nRVlxekUwaVZTQ2FXb0JzN0Q1Tm11Q2ZEVFdMNjNhQzQ4a2FLeFNY\n\tVXJIClhHd3ZqTC9DOFBENkdDckQzU2dpMDJjMU9CNDhCUHZwWmRKY0pQbXYwSWNRUURU\n\tOUZ5UzYvSDMzQzlsWm42Y24KY1h1N0tOcXc2ZGRwNFErN0ZnQ0RWY0xucmk5TE4rczVM\n\tTGszSDJ1d3F6dGw0dkxTV3FkZG9jZkY4SDI1UTc5ZAp2VDNtRmRtOWJwS3B3YUZ3UGZn\n\tZCtabVJNWTViQmI2SXp2OTQ5Ymx6enRvd0pRam5DeHhFMVVWU0FoQlhSeG80Ckg3UjNu\n\tbjU5RjJwTHQ3N0F3d2dheEpJUVNVa3preUg0d05FMDNyMFFma1JKaHBrTUlxQkwzeTBK\n\teU5pK0p2TkIKTFhiSlRLWHN0QnZ1RmxnNFhHL3puMityMHJKUmZINHFnT25ib0FXT0lZ\n\tNDhPNEZ6NjdpNHFHdjRCV1U2SktkSApDRWxQMEJwMklZM0MzVGFzMkc5UmxTNnYwY2Q5\n\tV3pFRkJ1bFY5WEt6TXdpMjZNOGMvU0xwcTl3aEIwZXdwVmxTClhXS241V09oM2gySGhQ\n\tSUJpTVBvaE01blBpUUhUaDJyMGFyaXczYXNkMUVoaDA3RTdxanBxUmFuQmhuMnIwUTAK\n\tUzVFeHJBVldjYnYzVnVYWHVyL2RjWHByNjI3dHNUbi9VanlFZEd6a2NyeFllZUZPTW04\n\tNTdwSXlhRkc3SG50MQpJdUtmTnM0NlFOQURidHR1QzFGRm5RKzY2UzRWejQ0dTJ3OENE\n\tb2RoSGF3YWN2VmlTQktTNWVvbER5NWhjb1BECnBoUk1qem5jc2UzdVh6djgwNDNpc2E2\n\tamZLdEEyNUx2L1lYSzlKMFdkbmZvNUcyK0NDWk5BYUxzUFJHY0g2U1IKYk02Wk5YNlNz\n\tZkZJckU3SGMwMDdIcUJxTzJKUXZhbjdpRWJ0N3ordGtHbWJmaUhPCi0tLS0tRU5EIENP\n\tTlRBSU5FUi0tLS0tCg==\n\t</data>\n\t<key>unbrick</key>\n\t<true/>\n</dict>\n</plist>";
		File.WriteAllText(t3, t7);
		this.method_28<T0, T3, T4, T1>("1");
		File.Copy("./files/Tickets/com.apple.commcenter.device_specific_nobackup.plist", "./files/BackUp/" + this.string_5 + "/3", true);
		this.method_24<T0>("./files/Backup/" + this.string_5, "./files/Backup/" + this.string_5 + ".zip");
		Directory.Delete("./files/Backup/" + this.string_5, true);
		this.label11.Text = "Mounting filesystem...";
		this.progressBar2.Value += 10;
		this.sshClient_0.CreateCommand("mount -o rw,union,update /").Execute();
		this.sshClient_0.CreateCommand("snappy -f / -r `snappy -f / -l | sed -n 2p` -t orig-fs").Execute();
		this.sshClient_0.CreateCommand("rm -rf /usr/libexec/substrate /usr/libexec/substrated /usr/bin/cycc /usr/bin/cynject /./Library/Frameworks/* /usr/lib/cycript0.9 /usr/lib/libsubstrate.dylib /Library/MobileSubstrate /usr/include/substrate.h /usr/lib/substrate").Execute();
		this.sshClient_0.CreateCommand("cp -rp /./System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/FactoryActivation.pem /./System/Library/PrivateFrameworks/MobileActivation.framework/Support/Certificates/RaptorActivation.pem").Execute();
		this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Library/Preferences/*purplebuddy*").Execute();
		this.sshClient_0.CreateCommand("find /private/var/containers/Data/System/ -iname 'internal' >>/guid").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && chflags -R nouchg $GUI/../*").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && rm -rf $GUI/../activation_records").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && rm -rf $GUI && rm /guid").Execute();
		this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/lzma"), "/./usr/bin/lzma");
		this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/plutil"), "/./usr/bin/plutil");
		this.sshClient_0.CreateCommand("chmod -R 777 /usr/bin").Execute();
		this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/boot"), "/./boot.tar.lzma");
		this.sshClient_0.CreateCommand("lzma -d -v /./boot.tar.lzma").Execute();
		this.sshClient_0.CreateCommand("tar -xvf /./boot.tar -C /./").Execute();
		this.sshClient_0.CreateCommand("chmod -R 755 /usr/bin").Execute();
		this.sshClient_0.CreateCommand("BasebandON && rm /./boot.tar").Execute();
		this.sshClient_0.CreateCommand("chmod 777 /usr/libexec/subs*").Execute();
		this.sshClient_0.CreateCommand("/usr/libexec/substrate").Execute();
		this.sshClient_0.CreateCommand("/usr/libexec/substrated").Execute();
		this.sshClient_0.CreateCommand("res=$(uicache --respring && killall backboardd); if test -z $res; then killall HUD SpringBoard; else echo '' ;fi").Execute();
		this.label11.Text = "Patching some files...";
		this.progressBar2.Value += 20;
		this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/untethered"), "/./Library/MobileSubstrate/DynamicLibraries/untethered.dylib");
		this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/untetheredplist"), "/./Library/MobileSubstrate/DynamicLibraries/untethered.plist");
		this.sshClient_0.CreateCommand("chmod 777 /./Library/MobileSubstrate/DynamicLibraries/untethered.dylib").Execute();
		this.sshClient_0.CreateCommand("cd /System/Library && launchctl unload LaunchDaemons/com.apple.mobile.lockdown.plist && launchctl unload LaunchDaemons/com.apple.mobileactivationd.plist && launchctl load LaunchDaemons/com.apple.mobile.lockdown.plist && launchctl load LaunchDaemons/com.apple.mobileactivationd.plist").Execute();
		this.sshClient_0.CreateCommand("key=$(uicache --respring && killall SpringBoard mobileactivationd); if test -z $key; then killall HUD SpringBoard mobileactivationd; else echo '' >>/dev/nul; fi").Execute();
		this.sshClient_0.CreateCommand("find /private/var/containers/Data/System/ -iname 'internal' >>/guid").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System/ -iname 'internal'); chflags -R nouchg $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System/ -iname 'internal'); plutil -\"-BootSessionRTCResetCount\" -remove $key/data_ark.plist").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System/ -iname 'internal'); plutil -\"-BootSessionUUID\" -remove $key/data_ark.plist").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System/ -iname 'internal'); plutil -\"-BrickState\" -remove $key/data_ark.plist").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System/ -iname 'internal'); plutil -\"-TotalRTCResetCount\" -remove $key/data_ark.plist").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System/ -iname 'internal'); plutil -\"-UIKLegacyMigrationCompleted\" -remove $key/data_ark.plist").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System/ -iname 'internal'); plutil -\"-ActivationState\" -remove $key/data_ark.plist && rm /guid").Execute();
		this.label11.Text = "Activating device...";
		this.progressBar2.Value += 20;
		this.sshClient_0.CreateCommand("find /private/var/containers/Data/System/ -iname 'internal' >>/guid").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && cd $GUI/../ && mkdir -p activation_records").Execute();
		this.method_23<T5, T6, T7, T0>("./files/Backup/" + this.string_5 + ".zip", "./tmp/" + this.string_5);
		this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Media/Downloads/" + this.string_5).Execute();
		this.sshClient_0.CreateCommand("rm -rf /private/var/mobile/Media/" + this.string_5).Execute();
		this.sshClient_0.CreateCommand("mkdir /private/var/mobile/Media/Downloads/" + this.string_5).Execute();
		this.scpClient_0.Upload(new DirectoryInfo(Environment.CurrentDirectory + "/tmp/" + this.string_5), "/private/var/mobile/Media/Downloads/" + this.string_5);
		this.sshClient_0.CreateCommand("mv -f /private/var/mobile/Media/Downloads/" + this.string_5 + " /private/var/mobile/Media/" + this.string_5).Execute();
		this.sshClient_0.CreateCommand("/usr/sbin/chown -R mobile:mobile /private/var/mobile/Media/" + this.string_5).Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && chflags nouchg $GUI/../activation_records").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && mv /private/var/mobile/Media/" + this.string_5 + "/1 $GUI/../activation_records/activation_record.plist").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && chmod -R 666 $GUI/../activation_records").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && chown -R mobile $GUI/../activation_records").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && plutil -binary $GUI/../activation_records/activation_record.plist").Execute();
		this.sshClient_0.CreateCommand("GUI=$(cat /guid) && chflags -R uchg $GUI/../activation_records").Execute();
		this.sshClient_0.CreateCommand("chown -R mobile:mobile /private/var/mobile/Media/" + this.string_5).Execute();
		this.sshClient_0.CreateCommand("mv -f /private/var/mobile/Media/" + this.string_5 + "/FairPlay /private/var/mobile/Library/FairPlay").Execute();
		this.sshClient_0.CreateCommand("chmod 755 /private/var/mobile/Library/FairPlay").Execute();
		this.sshClient_0.CreateCommand("chown -R mobile:mobile /private/var/mobile/Library/FairPlay").Execute();
		this.sshClient_0.CreateCommand("rm /guid && rm -rf /private/var/mobile/Media/" + this.string_5).Execute();
		this.label11.Text = "Making bypass untethered...";
		this.progressBar2.Value += 20;
		this.sshClient_0.CreateCommand("rm -f /./Library/MobileSubstrate/DynamicLibraries/*").Execute();
		this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/iuntethered"), "/./Library/MobileSubstrate/DynamicLibraries/iuntethered.dylib");
		this.scpClient_0.Upload(new FileInfo(Environment.CurrentDirectory + "/files/Tickets/iuntetheredplist"), "/./Library/MobileSubstrate/DynamicLibraries/iuntethered.plist");
		this.sshClient_0.CreateCommand("chmod 755 /./Library/MobileSubstrate/DynamicLibraries/iuntethered.dylib").Execute();
		this.sshClient_0.CreateCommand("cd /System/Library && launchctl unload -w -F LaunchDaemons*").Execute();
		this.sshClient_0.CreateCommand("cp -r /var/mobile/Media/Downloads" + this.string_5 + "/3 /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
		this.sshClient_0.CreateCommand("usr/sbin/chown root:mobile /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
		this.sshClient_0.CreateCommand("chmod 755 /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
		this.sshClient_0.CreateCommand("chflags uchg /private/var/wireless/Library/Preferences/com.apple.commcenter.device_specific_nobackup.plist").Execute();
		this.sshClient_0.CreateCommand("chflags nouchg /./private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
		this.label11.Text = "Skipping setup...";
		this.progressBar2.Value += 10;
		T1[] array = Convert.FromBase64String("YnBsaXN0MDDfECABAgMEBQYHCAkKCwwNDg8QERITFBUWFxgZGhscHR4fICEiIiIlIiciKSIrIiIuIjIiNCI2JTolJyI9IiIiIiIiXFNldHVwVmVyc2lvbl8QEVVzZXJDaG9zZUxhbmd1YWdlXxAXUEJEaWFnbm9zdGljczRQcmVzZW50ZWRfEBBQcml2YWN5UHJlc2VudGVkXxAfV2ViS2l0QWNjZWxlcmF0ZWREcmF3aW5nRW5hYmxlZF8QFFBheW1lbnRNaW5pQnVkZHk0UmFuXxArV2ViS2l0TG9jYWxTdG9yYWdlRGF0YWJhc2VQYXRoUHJlZmVyZW5jZUtleV5NZXNhMlByZXNlbnRlZFZMb2NhbGVfECFQaG9uZU51bWJlclBlcm1pc3Npb25QcmVzZW50ZWRLZXlfEBRzZXR1cE1pZ3JhdG9yVmVyc2lvbl8QGEhTQTJVcGdyYWRlTWluaUJ1ZGR5M1Jhbl8QFVNldHVwRmluaXNoZWRBbGxTdGVwc18QGWxhc3RQcmVwYXJlTGF1bmNoU2VudGluZWxfEBRBcHBsZUlEUEIxMFByZXNlbnRlZF8QFVByaXZhY3lDb250ZW50VmVyc2lvbl8QH1VzZXJJbnRlcmZhY2VTdHlsZU1vZGVQcmVzZW50ZWRYTGFuZ3VhZ2VfEBxIb21lQnV0dG9uQ3VzdG9taXplUHJlc2VudGVkWWNocm9uaWNsZV8QFUFuaW1hdGVMYW51Z2FnZUNob2ljZV1TZXR1cExhc3RFeGl0XxAQTWFnbmlmeVByZXNlbnRlZF8QFFdlYkRhdGFiYXNlRGlyZWN0b3J5XxAnV2ViS2l0T2ZmbGluZVdlYkFwcGxpY2F0aW9uQ2FjaGVFbmFibGVkWlNldHVwU3RhdGVfEBNBdXRvVXBkYXRlUHJlc2VudGVkXVJlc3RvcmVDaG9pY2VfEBNTY3JlZW5UaW1lUHJlc2VudGVkXxASUGFzc2NvZGU0UHJlc2VudGVkWVNldHVwRG9uZV8QIldlYktpdFNocmlua3NTdGFuZGFsb25lSW1hZ2VzVG9GaXQQCwkJCQgJXxAaL3Zhci9tb2JpbGUvTGlicmFyeS9DYWNoZXMJVWVzX0RPCRAKCQmiLzAzQcJgTHmPqfwQAAkQAglVZXMtRE8J0Tc4WGZlYXR1cmVzoAgzQcJgTHXyZncICV8QE1NldHVwVXNpbmdBc3Npc3RhbnQJCQkJCQkACABLAFgAbACGAJkAuwDSAQABDwEWAToBUQFsAYQBoAG3Ac8B8QH6AhkCIwI7AkkCXAJzAp0CqAK+AswC4gL3AwEDJgMoAykDKgMrAywDLQNKA0sDUQNSA1QDVQNWA1kDYgNkA2UDZwNoA24DbwNyA3sDfAN9A4YDhwOIA54DnwOgA6EDogOjAAAAAAAAAgEAAAAAAAAARAAAAAAAAAAAAAAAAAAAA6Q=");
		this.scpClient_0.Upload(new MemoryStream(array), "/./private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist");
		this.sshClient_0.CreateCommand("chown mobile /./private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
		this.sshClient_0.CreateCommand("chmod 600 /./private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
		this.sshClient_0.CreateCommand("uicache --all").Execute();
		this.label11.Text = "Finishing up...";
		this.progressBar2.Value += 10;
		this.sshClient_0.CreateCommand("chflags uchg /./private/var/mobile/Library/Preferences/com.apple.purplebuddy.plist").Execute();
		this.sshClient_0.CreateCommand("cd /System/Library && launchctl load -w -F LaunchDaemons*").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'data_ark.plist'); build=$(ls /private/var/root/Library/Caches/com.apple.coresymbolicationd); plutil -\"-BuildVersion\" -string $build $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'data_ark.plist'); build=$(ls /private/var/root/Library/Caches/com.apple.coresymbolicationd); plutil -\"-LastActivated\" -string $build $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'data_ark.plist'); build=$(ls /private/var/root/Library/Caches/com.apple.coresymbolicationd); plutil -\"-ActivationState\" -remove $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'data_ark.plist'); build=$(ls /private/var/root/Library/Caches/com.apple.coresymbolicationd); plutil -\"-BrickState\" -remove $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'data_ark.plist'); build=$(ls /private/var/root/Library/Caches/com.apple.coresymbolicationd); plutil -\"-ActivationState\" -string Activated $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'data_ark.plist'); build=$(ls /private/var/root/Library/Caches/com.apple.coresymbolicationd); plutil -\"-BrickState\" -0 -false $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'data_ark.plist'); plutil -binary $key").Execute();
		this.sshClient_0.CreateCommand("key=$(find /private/var/containers/Data/System -iname 'internal'); chflags -R uchg $key").Execute();
		this.sshClient_0.CreateCommand("rm -f /usr/libexec/substrate*").Execute();
		this.sshClient_0.CreateCommand("rm /usr/bin/cy*").Execute();
		this.sshClient_0.CreateCommand("rm -rf /./Library/Frameworks/*").Execute();
		this.sshClient_0.CreateCommand("rm -rf /usr/lib/cycript0.9").Execute();
		this.sshClient_0.CreateCommand("rm /usr/lib/libsubstrate.dylib").Execute();
		this.sshClient_0.CreateCommand("rm -rf /./Library/MobileSubstrate").Execute();
		this.sshClient_0.CreateCommand("rm /usr/include/substrate.h").Execute();
		this.sshClient_0.CreateCommand("rm -rf /usr/lib/substrate").Execute();
		this.sshClient_0.CreateCommand("BasebandOFF").Execute();
		this.sshClient_0.CreateCommand("uicache --all && killall backboardd").Execute();
		this.label18.Visible = true;
		this.label18.Text = "Congrats! Your iDevice has been activated\ud83c\udf89\ud83c\udf89";
		this.label11.ForeColor = Color.MediumSeaGreen;
		this.label11.Text = "Device has been acivated...";
		Directory.Delete("./tmp/" + this.string_5, true);
		this.progressBar2.Value += 10;
		MessageBox.Show("Your device has been sucessfully activated! \n\nDon't update or restore your device.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x002168D0 File Offset: 0x00214AD0
	[CompilerGenerated]
	private void method_111()
	{
		this.startDFU.Enabled = false;
		this.pressDFU.Enabled = true;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x002168F8 File Offset: 0x00214AF8
	[CompilerGenerated]
	private void method_112<T0, T1, T2, T3>(T0 gparam_0, T1 gparam_1)
	{
		this.method_35<T2>();
		((T3)((object)gparam_0)).Stop();
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00216918 File Offset: 0x00214B18
	[CompilerGenerated]
	private void method_113()
	{
		this.sideButton678Lab.Enabled = false;
		this.label22.Enabled = false;
		this.pressDFU.Enabled = false;
		this.label20.Enabled = true;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00216958 File Offset: 0x00214B58
	[CompilerGenerated]
	private void method_114()
	{
		this.label20.Enabled = false;
		this.homeButtLab.Enabled = false;
		this.volumeDown78XLab.Enabled = false;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0021698C File Offset: 0x00214B8C
	[CompilerGenerated]
	private void method_115<T0, T1, T2, T3, T4, T5, T6>()
	{
		this.method_91<T0, T1, T2, T3, T4, T5, T6>();
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x002169A0 File Offset: 0x00214BA0
	[CompilerGenerated]
	private void method_116<T0, T1, T2>()
	{
		this.method_6<T0, T1, T2>(1);
		this.method_8<T0, T2>();
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x002169BC File Offset: 0x00214BBC
	[CompilerGenerated]
	private void method_117<T0, T1, T2, T3, T4>()
	{
		this.jbtext.Text = "Installing drivers (This might take a while)...";
		this.method_6<T0, T1, T2>(1);
		this.progressBar1.Value += 60;
		this.method_43<T1, T3, T4>(1000);
		this.method_8<T0, T2>();
		this.jbtext.ForeColor = Color.MediumSeaGreen;
		this.jbtext.Text = "Drivers has been updated...";
		this.progressBar1.Value += 40;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00216A3C File Offset: 0x00214C3C
	static SerialPort smethod_3()
	{
		return Activator.CreateInstance(typeof(SerialPort));
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00216A58 File Offset: 0x00214C58
	static ProcessStartInfo smethod_4()
	{
		return Activator.CreateInstance(typeof(ProcessStartInfo));
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00216A74 File Offset: 0x00214C74
	static Process smethod_5()
	{
		return Activator.CreateInstance(typeof(Process));
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00216A90 File Offset: 0x00214C90
	static NSDictionary smethod_6()
	{
		return Activator.CreateInstance(typeof(NSDictionary));
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00216AAC File Offset: 0x00214CAC
	static XmlDocument smethod_7()
	{
		return Activator.CreateInstance(typeof(XmlDocument));
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00216AC8 File Offset: 0x00214CC8
	static WebClient smethod_8()
	{
		return Activator.CreateInstance(typeof(WebClient));
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00216AE4 File Offset: 0x00214CE4
	static Random smethod_9()
	{
		return Activator.CreateInstance(typeof(Random));
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00216B00 File Offset: 0x00214D00
	static System.Windows.Forms.Timer smethod_10()
	{
		return Activator.CreateInstance(typeof(System.Windows.Forms.Timer));
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00216B1C File Offset: 0x00214D1C
	static Label smethod_11()
	{
		return Activator.CreateInstance(typeof(Label));
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00216B38 File Offset: 0x00214D38
	static Button smethod_12()
	{
		return Activator.CreateInstance(typeof(Button));
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00216B54 File Offset: 0x00214D54
	static CheckBox smethod_13()
	{
		return Activator.CreateInstance(typeof(CheckBox));
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00216B70 File Offset: 0x00214D70
	static Panel smethod_14()
	{
		return Activator.CreateInstance(typeof(Panel));
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00216B8C File Offset: 0x00214D8C
	static PictureBox smethod_15()
	{
		return Activator.CreateInstance(typeof(PictureBox));
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00216BA8 File Offset: 0x00214DA8
	static LinkLabel smethod_16()
	{
		return Activator.CreateInstance(typeof(LinkLabel));
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00216BC4 File Offset: 0x00214DC4
	static ComboBox smethod_17()
	{
		return Activator.CreateInstance(typeof(ComboBox));
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00216BE0 File Offset: 0x00214DE0
	static TextBox smethod_18()
	{
		return Activator.CreateInstance(typeof(TextBox));
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00216BFC File Offset: 0x00214DFC
	static ProgressBar smethod_19()
	{
		return Activator.CreateInstance(typeof(ProgressBar));
	}

	// Token: 0x0400002E RID: 46
	public string string_0 = "2.3.5";

	// Token: 0x0400002F RID: 47
	public SshClient sshClient_0;

	// Token: 0x04000030 RID: 48
	public ScpClient scpClient_0;

	// Token: 0x04000031 RID: 49
	public SerialPort serialPort_0 = Form1.smethod_3();

	// Token: 0x04000032 RID: 50
	public string string_1 = "";

	// Token: 0x04000033 RID: 51
	public string string_2 = "";

	// Token: 0x04000034 RID: 52
	public string string_3 = "";

	// Token: 0x04000035 RID: 53
	public string string_4 = "";

	// Token: 0x04000036 RID: 54
	public string string_5 = "";

	// Token: 0x04000037 RID: 55
	public bool bool_0 = false;

	// Token: 0x04000038 RID: 56
	public bool bool_1 = false;

	// Token: 0x04000039 RID: 57
	public string string_6 = "";

	// Token: 0x0400003A RID: 58
	public string string_7 = "";

	// Token: 0x0400003B RID: 59
	public string string_8 = "";

	// Token: 0x0400003C RID: 60
	public bool bool_2 = false;

	// Token: 0x0400003D RID: 61
	public string string_9 = "";

	// Token: 0x0400003E RID: 62
	public string string_10 = "";

	// Token: 0x0400003F RID: 63
	public string string_11 = "";

	// Token: 0x04000040 RID: 64
	public string string_12 = "";

	// Token: 0x04000041 RID: 65
	public string string_13 = "";

	// Token: 0x04000042 RID: 66
	public string string_14 = "iOS15";

	// Token: 0x04000043 RID: 67
	public string string_15 = "";

	// Token: 0x04000044 RID: 68
	public string string_16 = "";

	// Token: 0x04000045 RID: 69
	public string string_17 = "";

	// Token: 0x04000046 RID: 70
	public string string_18 = "";

	// Token: 0x04000047 RID: 71
	public string string_19 = "";

	// Token: 0x04000048 RID: 72
	private static Dictionary<string, string> dictionary_0;

	// Token: 0x04000049 RID: 73
	private IContainer icontainer_0 = null;

	// Token: 0x0400004A RID: 74
	private Label Title;

	// Token: 0x0400004B RID: 75
	private Label Anno;

	// Token: 0x0400004C RID: 76
	private Label label3;

	// Token: 0x0400004D RID: 77
	private Label ECID;

	// Token: 0x0400004E RID: 78
	private Label ECIDValue;

	// Token: 0x0400004F RID: 79
	private Label DeviceType;

	// Token: 0x04000050 RID: 80
	private Label DeviceTypeValue;

	// Token: 0x04000051 RID: 81
	private Label ECIDStatus;

	// Token: 0x04000052 RID: 82
	private Label ECIDStatusValue;

	// Token: 0x04000053 RID: 83
	private Label ModelName;

	// Token: 0x04000054 RID: 84
	private Label ModelNameValue;

	// Token: 0x04000055 RID: 85
	private Label label12;

	// Token: 0x04000056 RID: 86
	private Button button2;

	// Token: 0x04000057 RID: 87
	private Button actbutt;

	// Token: 0x04000058 RID: 88
	private CheckBox checkBox1;

	// Token: 0x04000059 RID: 89
	private Label label14;

	// Token: 0x0400005A RID: 90
	private Label label15;

	// Token: 0x0400005B RID: 91
	private Label feedbacktext;

	// Token: 0x0400005C RID: 92
	private Label label13;

	// Token: 0x0400005D RID: 93
	private Label label19;

	// Token: 0x0400005E RID: 94
	private Button button1;

	// Token: 0x0400005F RID: 95
	private Label label2;

	// Token: 0x04000060 RID: 96
	private Label label4;

	// Token: 0x04000061 RID: 97
	private CheckBox checkBox2;

	// Token: 0x04000062 RID: 98
	private CheckBox checkBox3;

	// Token: 0x04000063 RID: 99
	private CheckBox checkBox4;

	// Token: 0x04000064 RID: 100
	private CheckBox checkBox5;

	// Token: 0x04000065 RID: 101
	private Panel bootPanel;

	// Token: 0x04000066 RID: 102
	private Panel backUpPanel;

	// Token: 0x04000067 RID: 103
	private LinkLabel linkLabel3;

	// Token: 0x04000068 RID: 104
	private Label label18;

	// Token: 0x04000069 RID: 105
	private Button button6;

	// Token: 0x0400006A RID: 106
	private LinkLabel linkLabel2;

	// Token: 0x0400006B RID: 107
	private Label label17;

	// Token: 0x0400006C RID: 108
	private Label label16;

	// Token: 0x0400006D RID: 109
	private ProgressBar progressBar2;

	// Token: 0x0400006E RID: 110
	private Label label11;

	// Token: 0x0400006F RID: 111
	private Button button5;

	// Token: 0x04000070 RID: 112
	private Label label10;

	// Token: 0x04000071 RID: 113
	private Label label7;

	// Token: 0x04000072 RID: 114
	private Button button4;

	// Token: 0x04000073 RID: 115
	private LinkLabel linkLabel1;

	// Token: 0x04000074 RID: 116
	private Button button3;

	// Token: 0x04000075 RID: 117
	private Label label9;

	// Token: 0x04000076 RID: 118
	private Label label8;

	// Token: 0x04000077 RID: 119
	private ProgressBar progressBar1;

	// Token: 0x04000078 RID: 120
	private Label jbtext;

	// Token: 0x04000079 RID: 121
	private Label label6;

	// Token: 0x0400007A RID: 122
	private Label label5;

	// Token: 0x0400007B RID: 123
	private Panel jailbreakPanel;

	// Token: 0x0400007C RID: 124
	private Label label21;

	// Token: 0x0400007D RID: 125
	private Panel dfuPanel;

	// Token: 0x0400007E RID: 126
	private Label homeButtLab;

	// Token: 0x0400007F RID: 127
	private Label sideButton678Lab;

	// Token: 0x04000080 RID: 128
	private Label volumeDown78XLab;

	// Token: 0x04000081 RID: 129
	private PictureBox devicePicture;

	// Token: 0x04000082 RID: 130
	private Label label1;

	// Token: 0x04000083 RID: 131
	private Label label20;

	// Token: 0x04000084 RID: 132
	private Label pressDFU;

	// Token: 0x04000085 RID: 133
	private Label startDFU;

	// Token: 0x04000086 RID: 134
	private Button button7;

	// Token: 0x04000087 RID: 135
	private Button button9;

	// Token: 0x04000088 RID: 136
	private Label label23;

	// Token: 0x04000089 RID: 137
	private Label label24;

	// Token: 0x0400008A RID: 138
	private Label label22;

	// Token: 0x0400008B RID: 139
	private LinkLabel linkLabel6;

	// Token: 0x0400008C RID: 140
	private LinkLabel linkLabel5;

	// Token: 0x0400008D RID: 141
	private LinkLabel linkLabel4;

	// Token: 0x0400008E RID: 142
	private PictureBox pictureBox1;

	// Token: 0x0400008F RID: 143
	private Button button10;

	// Token: 0x04000090 RID: 144
	private CheckBox checkBox8;

	// Token: 0x04000091 RID: 145
	private CheckBox checkBox9;

	// Token: 0x04000092 RID: 146
	private CheckBox checkBox10;

	// Token: 0x04000093 RID: 147
	private CheckBox checkBox11;

	// Token: 0x04000094 RID: 148
	private CheckBox checkBox12;

	// Token: 0x04000095 RID: 149
	private CheckBox checkBox13;

	// Token: 0x04000096 RID: 150
	private CheckBox checkBox14;

	// Token: 0x04000097 RID: 151
	private Button button11;

	// Token: 0x04000098 RID: 152
	private ComboBox comboBox1;

	// Token: 0x04000099 RID: 153
	private CheckBox checkBox6;

	// Token: 0x0400009A RID: 154
	private TextBox textBox1;

	// Token: 0x0200000F RID: 15
	[CompilerGenerated]
	[Serializable]
	private sealed class Class6
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00217B8C File Offset: 0x00215D8C
		internal T0 method_0<T0, T1>(T1 gparam_0)
		{
			return gparam_0.Value;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00217B8C File Offset: 0x00215D8C
		internal T0 method_1<T0, T1>(T1 gparam_0)
		{
			return gparam_0.Value;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00217B8C File Offset: 0x00215D8C
		internal T0 method_2<T0, T1>(T1 gparam_0)
		{
			return gparam_0.Value;
		}

		// Token: 0x040000A2 RID: 162
		public static readonly Form1.Class6 ab9 = new Form1.Class6();

		// Token: 0x040000A3 RID: 163
		public static Func<Match, string> ab9__29_0;

		// Token: 0x040000A4 RID: 164
		public static Func<Match, string> ab9__29_1;

		// Token: 0x040000A5 RID: 165
		public static Func<Match, string> ab9__29_2;
	}

	// Token: 0x02000010 RID: 16
	[CompilerGenerated]
	private sealed class Class7
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00217BA0 File Offset: 0x00215DA0
		internal void method_0()
		{
			this.form1_0.pressDFU.Text = this.string_0;
		}

		// Token: 0x040000A6 RID: 166
		public string string_0;

		// Token: 0x040000A7 RID: 167
		public Form1 form1_0;
	}

	// Token: 0x02000011 RID: 17
	[CompilerGenerated]
	private sealed class Class8
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00217BC4 File Offset: 0x00215DC4
		internal void method_0()
		{
			this.form1_0.label20.Text = this.string_0;
		}

		// Token: 0x040000A8 RID: 168
		public string string_0;

		// Token: 0x040000A9 RID: 169
		public Form1 form1_0;
	}

	// Token: 0x02000012 RID: 18
	[CompilerGenerated]
	private sealed class Class9
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00217BE8 File Offset: 0x00215DE8
		internal void method_0<T0, T1>(T0 gparam_0, T1 gparam_1)
		{
			this.timer_0.Enabled = false;
			this.timer_0.Stop();
		}

		// Token: 0x040000AA RID: 170
		public System.Windows.Forms.Timer timer_0;
	}

	// Token: 0x02000013 RID: 19
	[CompilerGenerated]
	private static class Class10
	{
		// Token: 0x040000AB RID: 171
		public static CallSite<Func<CallSite, object, object>> callSite_0;

		// Token: 0x040000AC RID: 172
		public static CallSite<Func<CallSite, object, object>> callSite_1;

		// Token: 0x040000AD RID: 173
		public static CallSite<Func<CallSite, Type, object, object>> callSite_2;

		// Token: 0x040000AE RID: 174
		public static CallSite<Func<CallSite, Encoding, object, object>> callSite_3;

		// Token: 0x040000AF RID: 175
		public static CallSite<Func<CallSite, object, object>> callSite_4;

		// Token: 0x040000B0 RID: 176
		public static CallSite<Func<CallSite, object, string>> callSite_5;
	}
}
